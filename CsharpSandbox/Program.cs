using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CsharpSandbox.Median;
using CsharpSandbox.ObjectPooling;
using CsharpSandbox.Tasks;
using Serilog;

namespace CsharpSandbox
{
    static class Program
    {
        private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        delegate int Transformer(int x);

        static async Task Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            using var _logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.File(path: "logs/log.txt", rollingInterval: RollingInterval.Day, flushToDiskInterval: TimeSpan.FromMilliseconds(500))
                .CreateLogger();

            //Console.WriteLine("press any key to start");
            //Console.ReadKey(true);

            //var sandbox = new ActionTasksSandbox();

            //await sandbox.RunAsync(_cancellationTokenSource.Token);
            var t = new TestClass();
            Transformer transformer = t.Square;
            Console.WriteLine(transformer(2));

            _logger.Verbose("press any key to close");
            Console.ReadKey(true);
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (e.SpecialKey == ConsoleSpecialKey.ControlC)
            {
                _cancellationTokenSource.Cancel();
                e.Cancel = true;
            }
        }
    }

    public class TestClass
    {
        public int Square(int x)
        {
            return x * x;
        }
    }
}
