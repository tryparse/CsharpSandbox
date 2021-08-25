using System;

namespace CsharpSandbox.EnumFlags
{
    class EnumFlagsSandbox : ISandboxRunner
    {
        public void Run()
        {
            var flag = FlagsEnum.L0;

            Process(flag);

            foreach (FlagsEnum value in Enum.GetValues(typeof(FlagsEnum)))
            {
                Console.WriteLine($"{value}={flag.HasFlag(value)}");
            }
        }

        static void Process(FlagsEnum flags)
        {
            flags |= FlagsEnum.L1;
        }
    }
}
