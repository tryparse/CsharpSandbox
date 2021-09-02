﻿using System;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary1;
using CsharpSandbox.Tasks;
using CsharpSandbox.TryCatchFinally;

namespace CsharpSandbox
{
    class Program
    {
        private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            Console.WriteLine($"Start: {nameof(Task)}.{nameof(Main)}");

            var runner = new TryFinallySandbox();

            await runner.Run(_cancellationTokenSource.Token);

            Console.WriteLine("done");
            Console.ReadKey(true);
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _cancellationTokenSource.Cancel(false);
        }
    }
}
