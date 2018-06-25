﻿namespace Zork.Core
{
    public class ParserVectors
    {
        public int prsa { get; set; }
        public ObjectIds prsi { get; set; }
        public ObjectIds prso { get; set; }
        public bool prswon { get; set; }
        public int prscon { get; set; }
    }

    public class ParserVector
    {
        public int act { get; set; }
        public int o1 { get; set; }
        public int o2 { get; set; }
        public int p1 { get; set; }
        public int p2 { get; set; }
    }
}
