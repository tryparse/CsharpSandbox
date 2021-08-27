using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpSandbox.TryCatchFinally
{
    class TryCatchFinallySandbox : ISandboxRunner
    {
        public void Run()
        {
            try
            {
                Console.WriteLine("try");
                throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                Console.WriteLine("catch");
                throw new InvalidOperationException();
            }
            finally
            {
                Console.WriteLine("finally");
            }
        }
    }
}
