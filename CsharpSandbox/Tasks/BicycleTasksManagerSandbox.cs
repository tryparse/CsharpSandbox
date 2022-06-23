using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CsharpSandbox.Tasks
{

    class BicycleTasksManagerSandbox : IAsyncSandboxRunner
    {
        private volatile int _taskCount;
        private const int TASK_COUNT_LIMIT = 3;

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var random = new Random();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (_taskCount < TASK_COUNT_LIMIT)
                {
                    await Task.Factory.StartNew(async () =>
                    {
                        if (_taskCount < TASK_COUNT_LIMIT)
                        {
                            Interlocked.Increment(ref _taskCount);

                            var guid = Guid.NewGuid();

                            Console.WriteLine($"{guid}: Start");

                            var iterations = 1;
                            var tasks = new List<Task>(iterations);

                            for (var i = 0; i < iterations; i++)
                            {
                                var delayedTask = new DelayedTask
                                {
                                    ID = i,
                                    Delay = random.Next(500, 1500)
                                };

                                tasks.Add(delayedTask.Run(cancellationToken));
                            }

                            await Task.WhenAll(tasks);

                            Console.WriteLine($"{guid}: End");

                            Interlocked.Decrement(ref _taskCount);

                            return;
                        }
                    }, TaskCreationOptions.PreferFairness);
                }
                else
                {
                    Console.WriteLine("All tasks are busy");
                    await Task.Delay(1000);
                }
            }
        }
    }
}
