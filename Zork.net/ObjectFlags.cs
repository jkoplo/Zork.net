﻿using System;

namespace Zork.Core
{
    [Flags]
    public enum ObjectFlags : int
    {
        IsVisible = 32768,
        READBT = 16384,
        IsTakeable = 8192,
        DOORBT = 4096,
        IsTransparent = 2048,
        FOODBT = 1024,
        NDSCBT = 512,
        DRNKBT = 256,
        CONTBT = 128,
        LITEBT = 64,
        VICTBT = 32,
        BURNBT = 16,
        FLAMBT = 8,
        IsTool = 4,
        TURNBT = 2,
        ONBT = 1,
    }

    [Flags]
    public enum ObjectFlags2
    {
        FINDBT = 32768,
        IsSleeping = 16384,
        SCRDBT = 8192,
        TIEBT = 4096,
        IsClimbable = 2048,
        ACTRBT = 1024,
        WEAPBT = 512,
        FITEBT = 256,
        VILLBT = 128,
        STAGBT = 64,
        TRYBT = 32,
        NOCHBT = 16,
        IsOpen = 8,
        TCHBT = 4,
        VEHBT = 2,
        // Was SCHBT - I believe that means 'searchable'.
        IsSearchable = 1,
    }

    public enum SyntaxObjectFlags
    {
        VABIT = 16384,
        VRBIT = 8192,
        VTBIT = 4096,
        VCBIT = 2048,
        VEBIT = 1024,
        VFBIT = 512,
        VPMASK = 511,
    }

    public enum SyntaxFlags
    {
        SDIR = 16384,
        SIND = 8192,
        SSTD = 4096,
        SFLIP = 2048,
        SDRIV = 1024,
        SVMASK = 511,
    }
}