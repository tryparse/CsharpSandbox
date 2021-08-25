using System;

namespace CsharpSandbox.Regex
{
    class RegexSandbox : ISandboxRunner
    {
        private const string cSTR_TAG_UDFNAME_42_PATTERN = "^[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]$";

        public void Run()
        {
            Console.WriteLine(System.Text.RegularExpressions.Regex.IsMatch("01234567890123456789", cSTR_TAG_UDFNAME_42_PATTERN));
            Console.WriteLine(System.Text.RegularExpressions.Regex.IsMatch("01234567890123456789A", cSTR_TAG_UDFNAME_42_PATTERN));
        }
    }
}
