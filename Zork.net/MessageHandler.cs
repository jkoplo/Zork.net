﻿using System;

namespace Zork.Core
{
    public static class MessageHandler
    {
        public static void rspeak_(Game game, ObjectIds objectId) => MessageHandler.Speak(game, (int)objectId);
        public static void rspeak_(Game game, int messageNumber) => MessageHandler.Speak(game, messageNumber);
        public static void Speak(ObjectIds objectId, Game game) => MessageHandler.Speak(game, (int)objectId);
        public static void Speak(Game game, int messageNumber) => MessageHandler.Speak(messageNumber, game);
        public static void Speak(int messageNumber, Game game) => MessageHandler.rspsb2nl_(messageNumber, 0, 0, true, game);

        public static void more_output(Game game, string output) => game.WriteOutput(output);
        public static void more_input() { }

        /// <summary>
        /// OUTPUT RANDOM MESSAGE WITH SUBSTITUTABLE ARGUMENT
        /// </summary>
        /// <param name="messageNumber"></param>
        /// <param name="s1"></param>
        /// <param name="game"></param>
        public static void rspsub_(Game game, int messageNumber, int s1 = 0) => rspsub_(messageNumber, s1, game);
        public static void rspsub_(int messageNumber, int s1, Game game) => MessageHandler.rspsb2nl_(messageNumber, s1, 0, true, game);
        public static void rspsub_(ObjectIds messageNumber, int s1, Game game) => MessageHandler.rspsb2nl_((int)messageNumber, s1, 0, true, game);
        /// <summary>
        /// rspsb2_ - Output random message with up to two substitutable arguments.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="game"></param>
        public static void rspsb2_(Game game, int n, int s1, int s2) => rspsb2_(n, s1, s2, game);
        public static void rspsb2_(int n, int s1, int s2, Game game) => rspsb2nl_(n, s1, s2, true, game);

        /// <summary>
        /// Display a substitutable message with an optional newline
        /// </summary>
        private static void rspsb2nl_(int messageNumber, int y, int z, bool newLine, Game game)
        {
            string zkey = "IanLanceTaylorJr";
            int x = messageNumber;

            if (x > 0)
            {
                x = game.Messages.rtext[x - 1];
            }

            // !IF >0, LOOK UP IN RTEXT.
            if (x == 0)
            {
                return;
            }

            // !ANYTHING TO DO?
            game.Player.TelFlag = true;

            // !SAID SOMETHING.
            x = ((-x) - 1) * 8;
            game.DataPosition = x + game.Messages.mrloc;

            if (game.DataPosition > game.Data.Length)
            {
                throw new InvalidOperationException($"Error seeking database loc {x}");
            }

            if (newLine)
            {
                game.WriteOutput(Environment.NewLine);
            }

            while (true)
            {
                int i;

                try
                {
                    i = game.Data[game.DataPosition++];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException($"Error reading database loc {x}");
                }

                i ^= (int)(zkey[(int)(x & 0xf)] ^ (x & 0xff));

                x = x + 1;
                if (i == '\0')
                {
                    break;
                }
                else if (i == '\n')
                {
                    game.WriteOutput(Environment.NewLine);
                    if (newLine)
                    {
                        // game.WriteOutput(Environment.NewLine);
                    }
                }
                else if (i == '#' && y != 0)
                {
                    long iloc = game.DataPosition;

                    rspsb2nl_(y, 0, 0, false, game);
                    if (iloc > game.Data.Length)
                    {
                        throw new InvalidOperationException($"Error seeking database loc {iloc}");
                    }

                    game.DataPosition = (int)iloc;

                    y = z;
                    z = 0;
                }
                else
                {
                    game.WriteOutput(((char)i).ToString());
                }
            }

            if (newLine)
            {
                game.WriteOutput(Environment.NewLine);
            }
        }

        /// <summary>
        /// yesno_ - Obtain Yes/No answer.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="questionStringId"></param>
        /// <param name="yesStringId"></param>
        /// <param name="noStringId"></param>
        /// <returns></returns>
        public static bool AskYesNoQuestion(Game game, int questionStringId, int yesStringId, int noStringId)
        {
            // System generated locals
            bool ret_val;

            // Local variables
            string ans = " ";

            L100:
            // !ASK
            MessageHandler.rspeak_(game, questionStringId);

            ans = game.ReadInput();
            MessageHandler.more_input();

            if (string.IsNullOrWhiteSpace(ans))
            {
                ans = " ";
            }

            // !GET ANSWER
            if (ans[0] == 'Y' || ans[0] == 'y')
            {
                goto L200;
            }

            if (ans[0] == 'N' || ans[0] == 'n')
            {
                goto L300;
            }

            MessageHandler.rspeak_(game, 6);
            // !SCOLD.
            goto L100;

            L200:
            // !YES,
            ret_val = true;
            // !OUT WITH IT.
            MessageHandler.rspeak_(game, yesStringId);
            return ret_val;

            L300:
            // !NO,
            ret_val = false;
            // !LIKEWISE.
            MessageHandler.rspeak_(game, noStringId);
            return ret_val;
        }
    }
}
