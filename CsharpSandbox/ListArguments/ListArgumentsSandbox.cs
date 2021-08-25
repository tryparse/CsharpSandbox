using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpSandbox.ListArguments
{
    class ListArgumentsSandbox : ISandboxRunner
    {
        public void Run()
        {
            var errors = new List<string>();

            errors.Add("1");

            Test(errors);

            errors.Add("3");

            foreach (var e in errors)
            {
                Console.WriteLine(e);
            }
        }

        public void Test(List<string> errors)
        {
            errors.Add("2");
        }
    }
}
