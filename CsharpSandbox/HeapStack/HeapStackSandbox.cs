using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpSandbox.HeapStack
{
    class HeapStackSandbox : ISandboxRunner
    {
        int b = 1;

        public void Run()
        {
            int a = 0;

            Console.WriteLine(a);
            Console.WriteLine(b);
        }
    }
}
