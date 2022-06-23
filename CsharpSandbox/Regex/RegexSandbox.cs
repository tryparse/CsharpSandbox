using System;
using System.Text.RegularExpressions;

namespace CsharpSandbox.RegexSandbox
{
    public class RegexSandbox : ISandboxRunner
    {
        private const string cSTR_TAG_UDFNAME_42_PATTERN = "^[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]$";

        public void Run()
        {
            Console.WriteLine(ParseCodePurpose(string.Empty, string.Empty));// "ABCD/20:2/ZXCVBNM123"));
        }

        private static string ParseCodePurpose(string udfName40, string paymentDetails)
        {
            if (!string.IsNullOrWhiteSpace(udfName40))
            {
                return udfName40;
            }

            var match = Regex.Match(paymentDetails, "/20:[0-9]/");

            if (!match.Success)
            {
                return default;
            }

            return paymentDetails.Substring(match.Index + 4, 1);
        }
    }
}
