using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CsharpSandbox.StopwatchSandbox
{
    class StopwatchSandbox : IAsyncSandboxRunner
    {
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                //await Task.WhenAll(
                //    async () => { await Task.Delay(1000); },
                //    async () => { await Task.Delay(1000); }
                //);
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");
            }
        }
    }
}
