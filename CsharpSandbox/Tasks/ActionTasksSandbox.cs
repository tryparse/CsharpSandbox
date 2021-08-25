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

            async Task action(Guid guid)
            {
                await Task.Run(() => { Console.WriteLine($"{guid}: start"); }, cancellationToken);
                await Task.Delay(random.Next(1000, 5000), cancellationToken);
                await Task.Run(() => { Console.WriteLine($"{guid}: done"); }, cancellationToken);
            }

            ActionBlock<Guid> actionBlockParallel = new ActionBlock<Guid>(
                action, 
                new ExecutionDataflowBlockOptions()
                { 
                    MaxDegreeOfParallelism = 1,
                    CancellationToken = cancellationToken,
                    BoundedCapacity = 1
                });

            Stopwatch stopwatch = Stopwatch.StartNew();

            while (!cancellationToken.IsCancellationRequested)
            {
                var guid = Guid.NewGuid();

                Console.WriteLine($"{guid}: added");

                try
                {
                    await actionBlockParallel.SendAsync(guid, cancellationToken);
                }
                catch (AggregateException ex)
                {
                    foreach (var inner in ex.InnerExceptions)
                    {
                        Console.WriteLine(inner.Message);
                    }
                }
                catch (TaskCanceledException ex)
                {
                    actionBlockParallel.Complete();
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
