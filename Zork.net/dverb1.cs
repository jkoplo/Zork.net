﻿
namespace Zork.Core
{
    public static class dverb1
    {
        /// <summary>
        /// take_ - Take Sequence.
        /// Triggered for verbs: take, put, drop, read, etc.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="tellUser">Tells user that something was taken.</param>
        /// <returns>true is taken, false otherwise</returns>
        public static bool TakeParsedObject(Game game, bool tellUser)
        {
            // System generated locals
            int i__1;
            bool ret_val;

            // Local variables
            int oa;
            int x;

            // !ASSUME LOSES.
            ret_val = false;

            // !GET OBJECT ACTION.
            oa = game.Objects.oactio[game.ParserVectors.prso - 1];

            // !STAR?
            if (game.ParserVectors.prso <= game.Star.strbit)
            {
                goto L100;
            }

            // !YES, LET IT HANDLE.
            ret_val = ObjectHandler.objact_(game);
            return ret_val;

            L100:
            // !INSIDE?
            x = game.Objects.ocan[game.ParserVectors.prso - 1];

            // !HIS VEHICLE?
            if (game.ParserVectors.prso != game.Adventurers.Vehicles[(int)game.Player.Winner - 1])
            {
                goto L400;
            }

            // !DUMMY.
            MessageHandler.rspeak_(game, 672);
            return ret_val;

            L400:
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.IsTakeable) != 0)
            {
                goto L500;
            }

            if (!ObjectHandler.oappli_(oa, 0, game))
            {
                i__1 = game.rnd_(5) + 552;
                MessageHandler.rspeak_(game, i__1);
            }

            return ret_val;

            // OBJECT IS TAKEABLE AND IN POSITION TO BE TAKEN.

            L500:
            if (x != 0 || ObjectHandler.IsObjectInRoom((ObjectIds)game.ParserVectors.prso, game.Player.Here, game))
            {
                goto L600;
            }

            // !ALREADY GOT IT?
            if (game.Objects.oadv[game.ParserVectors.prso - 1] == (int)game.Player.Winner)
            {
                MessageHandler.rspeak_(game, 557);
            }

            return ret_val;

            L600:
            // !TOO MUCH WEIGHT.
            if (x != 0 && game.Objects.oadv[x - 1] == (int)game.Player.Winner || ObjectHandler.GetWeight(0, game.ParserVectors.prso, game.Player.Winner, game) + game.Objects.Sizes[game.ParserVectors.prso - 1] <= game.State.MaxLoad)
            {
                goto L700;
            }

            MessageHandler.rspeak_(game, 558);
            return ret_val;

            L700:
            // !AT LAST.
            ret_val = true;
            // !DID IT HANDLE?
            if (ObjectHandler.oappli_(oa, 0, game))
            {
                return ret_val;
            }

            // !TAKE OBJECT FOR WINNER.
            ObjectHandler.SetNewObjectStatus((ObjectIds)game.ParserVectors.prso, 0, 0, 0, game.Player.Winner, game);
            game.Objects.oflag2[game.ParserVectors.prso - 1] |= ObjectFlags2.TCHBT;

            // !UPDATE SCORE.
            AdventurerHandler.ScoreUpdate(game, game.Objects.ofval[game.ParserVectors.prso - 1]);

            // !CANT BE SCORED AGAIN.
            game.Objects.ofval[game.ParserVectors.prso - 1] = 0;

            // !TELL TAKEN.
            if (tellUser)
            {
                MessageHandler.rspeak_(game, 559);
            }

            return ret_val;
        }

        // DROP- DROP VERB PROCESSOR
        public static bool drop_(Game game, bool z)
        {
            // System generated locals
            bool ret_val;

            // Local variables
            bool f;
            int i, x;

            ret_val = true;
            // !ASSUME WINS.
            x = game.Objects.ocan[game.ParserVectors.prso - 1];
            // !GET CONTAINER.
            if (x == 0)
            {
                goto L200;
            }
            // !IS IT INSIDE?
            if (game.Objects.oadv[x - 1] != (int)game.Player.Winner)
            {
                goto L1000;
            }
            // !IS HE CARRYING CON?
            if ((game.Objects.oflag2[x - 1] & ObjectFlags2.IsOpen) != 0)
            {
                goto L300;
            }
            MessageHandler.rspsub_(game, 525, game.Objects.odesc2[x - 1]);
            // !CANT REACH.
            return ret_val;

            L200:
            if (game.Objects.oadv[game.ParserVectors.prso - 1] != (int)game.Player.Winner)
            {
                goto L1000;
            }
            // !IS HE CARRYING OBJ?
            L300:
            if (game.Adventurers.Vehicles[(int)game.Player.Winner - 1] == 0)
            {
                goto L400;
            }
            // !IS HE IN VEHICLE?
            game.ParserVectors.prsi = game.Adventurers.Vehicles[(int)game.Player.Winner - 1];
            // !YES,
            f = put_(game, true);
            // !DROP INTO VEHICLE.
            game.ParserVectors.prsi = 0;
            // !DISARM PARSER.
            return ret_val;
            // !DONE.

            L400:
            ObjectHandler.SetNewObjectStatus((ObjectIds)game.ParserVectors.prso, 0, (RoomIds)game.Player.Here, 0, 0, game);
            // !DROP INTO ROOM.

            if (game.Player.Here == RoomIds.mtree)
            {
                ObjectHandler.SetNewObjectStatus((ObjectIds)game.ParserVectors.prso, 0, RoomIds.Forest3, 0, 0, game);
            }

            AdventurerHandler.ScoreUpdate(game, game.Objects.ofval[game.ParserVectors.prso - 1]);
            // !SCORE OBJECT.
            game.Objects.ofval[game.ParserVectors.prso - 1] = 0;
            // !CANT BE SCORED AGAIN.
            game.Objects.oflag2[game.ParserVectors.prso - 1] |= ObjectFlags2.TCHBT;

            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }
            // !DID IT HANDLE?
            i = 0;
            // !ASSUME NOTHING TO SAY.
            if (game.ParserVectors.prsa == (int)VerbIds.Drop)
            {
                i = 528;
            }

            if (game.ParserVectors.prsa == (int)VerbIds.Throw)
            {
                i = 529;
            }
            if (i != 0 && game.Player.Here == RoomIds.mtree)
            {
                i = 659;
            }
            MessageHandler.rspsub_(game, i, game.Objects.odesc2[game.ParserVectors.prso - 1]);
            return ret_val;

            L1000:
            MessageHandler.rspeak_(game, 527);
            // !DONT HAVE IT.
            return ret_val;

        } // drop_

        // PUT- PUT VERB PROCESSOR
        public static bool put_(Game game, bool flg)
        {
            // System generated locals
            bool ret_val;

            // Local variables
            int j;
            int svi, svo;

            ret_val = false;
            if (game.ParserVectors.prso <= game.Star.strbit && game.ParserVectors.prsi <= game.Star.strbit)
            {
                goto L200;
            }
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.rspeak_(game, 560);
            }
            // !STAR
            ret_val = true;
            return ret_val;

            L200:
            if ((game.Objects.oflag2[game.ParserVectors.prsi - 1] & ObjectFlags2.IsOpen) != 0
                || (game.Objects.oflag1[game.ParserVectors.prsi - 1] & (int)ObjectFlags.DOORBT + ObjectFlags.CONTBT) != 0 || (game.Objects.oflag2[game.ParserVectors.prsi - 1] & ObjectFlags2.VEHBT) != 0)
            {
                goto L300;
            }

            MessageHandler.rspeak_(game, 561);
            // !CANT PUT IN THAT.
            return ret_val;

            L300:
            if ((game.Objects.oflag2[game.ParserVectors.prsi - 1] & ObjectFlags2.IsOpen) != 0)
            {
                goto L400;
            }
            // !IS IT OPEN?
            MessageHandler.rspeak_(game, 562);
            // !NO, JOKE
            return ret_val;

            L400:
            if (game.ParserVectors.prso != game.ParserVectors.prsi)
            {
                goto L500;
            }
            // !INTO ITSELF?
            MessageHandler.rspeak_(game, 563);
            // !YES, JOKE.
            return ret_val;

            L500:
            if (game.Objects.ocan[game.ParserVectors.prso - 1] != game.ParserVectors.prsi)
            {
                goto L600;
            }
            // !ALREADY INSIDE.
            MessageHandler.rspsb2_(game, 564, game.Objects.odesc2[game.ParserVectors.prso - 1], game.Objects.odesc2[game.ParserVectors.prsi - 1]);
            ret_val = true;
            return ret_val;

            L600:
            if (ObjectHandler.GetWeight(0, game.ParserVectors.prso, 0, game) + ObjectHandler.GetWeight(0, game.ParserVectors.prsi, 0, game) + game.Objects.Sizes[game.ParserVectors.prso - 1] <= game.Objects.ocapac[game.ParserVectors.prsi - 1])
            {
                goto L700;
            }

            MessageHandler.rspeak_(game, 565);
            // !THEN CANT DO IT.
            return ret_val;

            // NOW SEE IF OBJECT (OR ITS CONTAINER) IS IN ROOM

            L700:
            j = game.ParserVectors.prso;
            // !START SEARCH.
            L725:
            if (ObjectHandler.IsObjectInRoom((ObjectIds)j, game.Player.Here, game))
            {
                goto L750;
            }
            // !IS IT HERE?
            j = game.Objects.ocan[j - 1];
            if (j != 0)
            {
                goto L725;
            }
            // !MORE TO DO?
            goto L800;
            // !NO, SCH FAILS.

            L750:
            svo = game.ParserVectors.prso;
            // !SAVE PARSER.
            svi = game.ParserVectors.prsi;
            game.ParserVectors.prsa = (int)VerbIds.takew;
            game.ParserVectors.prsi = 0;
            if (!TakeParsedObject(game, false))
            {
                return ret_val;
            }
            // !TAKE OBJECT.
            game.ParserVectors.prsa = (int)VerbIds.Put;
            game.ParserVectors.prso = svo;
            game.ParserVectors.prsi = svi;
            goto L1000;

            // NOW SEE IF OBJECT IS ON PERSON.

            L800:
            if (game.Objects.ocan[game.ParserVectors.prso - 1] == 0)
            {
                goto L1000;
            }
            // !INSIDE?
            if ((game.Objects.oflag2[game.Objects.ocan[game.ParserVectors.prso - 1] - 1] & ObjectFlags2.IsOpen) != 0)
            {
                goto L900;
            }
            // !OPEN?
            MessageHandler.rspsub_(game, 566, game.Objects.odesc2[game.ParserVectors.prso - 1]);
            // !LOSE.
            return ret_val;

            L900:
            AdventurerHandler.ScoreUpdate(game, game.Objects.ofval[game.ParserVectors.prso - 1]);
            // !SCORE OBJECT.
            game.Objects.ofval[game.ParserVectors.prso - 1] = 0;
            game.Objects.oflag2[game.ParserVectors.prso - 1] |= ObjectFlags2.TCHBT;
            ObjectHandler.SetNewObjectStatus((ObjectIds)game.ParserVectors.prso, 0, 0, 0, game.Player.Winner, game);
            // !TEMPORARILY ON WINNER.

            L1000:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }
            // !NO, GIVE OBJECT A SHOT.
            ObjectHandler.SetNewObjectStatus((ObjectIds)game.ParserVectors.prso, 2, 0, (ObjectIds)game.ParserVectors.prsi, 0, game);
            // !CONTAINED INSIDE.
            ret_val = true;
            return ret_val;

        } // put_

        // VALUAC- HANDLES VALUABLES/EVERYTHING
        public static void valuac_(Game game, int v)
        {
            // System generated locals
            int i__1;

            // Local variables
            bool f;
            int i;
            bool f1;
            int savep, saveh;

            f = true;
            // !ASSUME NO ACTIONS.
            i = 579;
            // !ASSUME NOT LIT.
            if (!RoomHandler.IsRoomLit(game.Player.Here, game))
            {
                goto L4000;
            }
            // !IF NOT LIT, PUNT.
            i = 677;
            // !ASSUME WRONG VERB.
            savep = game.ParserVectors.prso;
            // !SAVE PRSO.
            saveh = (int)game.Player.Here;
            // !SAVE HERE.

            // L100:
            if (game.ParserVectors.prsa != (int)VerbIds.takew)
            {
                goto L1000;
            }
            // !TAKE EVERY/VALUA?
            i__1 = game.Objects.Count;
            for (game.ParserVectors.prso = 1; game.ParserVectors.prso <= i__1; ++game.ParserVectors.prso)
            {
                // !LOOP THRU OBJECTS.
                if (!ObjectHandler.IsObjectInRoom((ObjectIds)game.ParserVectors.prso, game.Player.Here, game)
                    || (game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.IsVisible) == 0
                    || (game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.ACTRBT) != 0
                    || savep == v && game.Objects.otval[game.ParserVectors.prso - 1] <= 0)
                {
                    goto L500;
                }

                if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.IsTakeable) == 0 && (
                    game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.TRYBT) == 0)
                {
                    goto L500;
                }

                f = false;
                MessageHandler.rspsub_(game, 580, game.Objects.odesc2[game.ParserVectors.prso - 1]);
                f1 = TakeParsedObject(game, true);
                if (saveh != (int)game.Player.Here)
                {
                    return;
                }
                L500:
                ;
            }
            goto L3000;

            L1000:
            if (game.ParserVectors.prsa != (int)VerbIds.Drop)
            {
                goto L2000;
            }
            // !DROP EVERY/VALUA?
            i__1 = game.Objects.Count;
            for (game.ParserVectors.prso = 1; game.ParserVectors.prso <= i__1; ++game.ParserVectors.prso)
            {
                if (game.Objects.oadv[game.ParserVectors.prso - 1] != (int)game.Player.Winner || savep == v
                    && game.Objects.otval[game.ParserVectors.prso - 1] <= 0)
                {
                    goto L1500;
                }
                f = false;
                MessageHandler.rspsub_(game, 580, game.Objects.odesc2[game.ParserVectors.prso - 1]);
                f1 = drop_(game, true);
                if (saveh != (int)game.Player.Here)
                {
                    return;
                }
                L1500:
                ;
            }
            goto L3000;

            L2000:
            if (game.ParserVectors.prsa != (int)VerbIds.Put)
            {
                goto L3000;
            }
            // !PUT EVERY/VALUA?
            i__1 = game.Objects.Count;
            for (game.ParserVectors.prso = 1; game.ParserVectors.prso <= i__1; ++game.ParserVectors.prso)
            {
                // !LOOP THRU OBJECTS.
                if (game.Objects.oadv[game.ParserVectors.prso - 1] != (int)game.Player.Winner
                    || game.ParserVectors.prso == game.ParserVectors.prsi
                    || savep == v
                    && game.Objects.otval[game.ParserVectors.prso - 1] <= 0
                    || (game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.IsVisible) == 0)
                {
                    goto L2500;
                }

                f = false;
                MessageHandler.rspsub_(game, 580, game.Objects.odesc2[game.ParserVectors.prso - 1]);
                f1 = put_(game, true);
                if (saveh != (int)game.Player.Here)
                {
                    return;
                }
                L2500:
                ;
            }

            L3000:
            i = 581;
            if (savep == v)
            {
                i = 582;
            }
            // !CHOOSE MESSAGE.
            L4000:
            if (f)
            {
                MessageHandler.rspeak_(game, i);
            }
            // !IF NOTHING, REPORT.
        }
    }
}