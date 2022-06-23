using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CsharpSandbox.TryCatchFinally
{
    class TryFinallySandbox : IAsyncSandboxRunner
    {
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await TestReturn();
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task<int> TestReturn()
        {
            var stopWatch = Stopwatch.StartNew();

            try
            {
                await Task.Delay(1000);

                //await Task.Run(() =>
                //{
                //    throw new InvalidOperationException("Exception");
                //});

                return 100;
            }
            finally
            {
                stopWatch.Stop();

                Console.WriteLine($"ElapsedMilliseconds={stopWatch.ElapsedMilliseconds}");
            }
        }
    }
}
