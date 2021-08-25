using System;

namespace CsharpSandbox.EnumFlags
{
    [Flags]
    enum FlagsEnum
    {
        L0 = 0,
        L1 = 1,
        L2 = 2,
        L4 = 4,
        L8 = 8,
        L16 = 16,
        L32 = 32,
        L64 = 64,
        L128 = 128,
        L256 = 256,
        L512 = 512,
        L1024 = 1024,
        L2048 = 2048,
        L4096 = 4096,
        L8192 = 8192,
        L16384 = 16384,
        L32768 = 32768,
        L65536 = 65536,
        L131072 = 131072
    }
}
