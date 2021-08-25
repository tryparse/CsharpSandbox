using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CsharpSandbox.Tasks
{
    class ActionTasksSandbox : IAsyncSandboxRunner
    {
        public async Task Run(CancellationToken cancellationToken)
        {
            var random = new Random();

            Func<Guid, Task> process = async id =>
            {
                //await Task.Run(() => { Console.WriteLine($"{id}: start"); });
                await Task.Delay(1000).ConfigureAwait(false);
                await Task.Run(() => { Console.WriteLine($"{id}: end"); });
            };

            ActionBlock<Guid> actionBlockParallel = new ActionBlock<Guid>(
                process, 
                new ExecutionDataflowBlockOptions()
                { 
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    CancellationToken = cancellationToken,
                    BoundedCapacity = 100
                });

            Stopwatch stopwatch = Stopwatch.StartNew();

            while (!cancellationToken.IsCancellationRequested)
            {
                var guid = Guid.NewGuid();

                try
                {
                    if (random.NextDouble() > 0.9)
                    {
                        throw new InvalidOperationException("You doing it wrong");
                    }
                    await actionBlockParallel.SendAsync(guid, cancellationToken);
                }
                catch (TaskCanceledException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }
    }
}
