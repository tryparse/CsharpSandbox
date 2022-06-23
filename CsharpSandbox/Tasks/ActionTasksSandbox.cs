using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CsharpSandbox.Tasks
{
    public static class MetricMonitoring
    {
        public static Meter AppMeter = new Meter("Program.Metrics");
    }

    class ActionTasksSandbox : IAsyncSandboxRunner
    {
        private readonly Counter<int> _readCounter;

        public ActionTasksSandbox()
        {
             _readCounter = MetricMonitoring.AppMeter.CreateCounter<int>($"{GetType().Name}.{nameof(ReadAsync)}");
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            TransformBlock<int, ProcessingTask> initialBlock = new TransformBlock<int, ProcessingTask>(
                async (id) => await ReadAsync(id, cancellationToken),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 4,
                    EnsureOrdered = false,
                    MaxMessagesPerTask = DataflowBlockOptions.Unbounded,
                    BoundedCapacity = 4
                });

            ActionBlock<ProcessingTask> actionBlock = new ActionBlock<ProcessingTask>(
                async (id) => await ProcessAsync(id, cancellationToken),
                new ExecutionDataflowBlockOptions()
                {
                    MaxDegreeOfParallelism = 2,
                    EnsureOrdered = false,
                    MaxMessagesPerTask = DataflowBlockOptions.Unbounded,
                    BoundedCapacity = 2
                });

            initialBlock.LinkTo(
                actionBlock,
                x => null != x);

            initialBlock.LinkTo(DataflowBlock.NullTarget<ProcessingTask>());

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                for (int i = 1; i <= 1000; i++)
                {
                    try
                    {
                        var task = await initialBlock.SendAsync(i, cancellationToken);
                        Console.WriteLine($"{i} {task}");
                    }
                    catch (AggregateException ex)
                    {
                        actionBlock.Complete();

                        foreach (var inner in ex.InnerExceptions)
                        {
                            Console.WriteLine(inner.Message);
                        }
                    }
                    catch (TaskCanceledException ex)
                    {
                        actionBlock.Complete();
                        Console.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        actionBlock.Complete();
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            finally
            {
                stopwatch.Stop();

                //Console.WriteLine($"{count} tasks processed in {stopwatch.ElapsedMilliseconds} milliseconds");
            }
        }

        private async Task<ProcessingTask> ReadAsync(int id, CancellationToken cancellationToken)
        {
            _readCounter.Add(1);

            cancellationToken.ThrowIfCancellationRequested();

            var stopwatch = Stopwatch.StartNew();

            try
            {

                Console.WriteLine($"{nameof(ReadAsync)} {id}");

                await Task.Delay(1000);

                //if (id == 1)
                //{
                //    Console.WriteLine($"{nameof(ReadAsync)} {id} return null");
                //    return null;
                //}
                //else
                //{
                return new ProcessingTask(id);
                //}
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private async Task ProcessAsync(ProcessingTask task, CancellationToken cancellationToken)
        {
            if (task == null)
            {
                return;
            }

            cancellationToken.ThrowIfCancellationRequested();

            Console.WriteLine($"{nameof(ProcessAsync)} {task.ID}");

            await Task.Delay(2000);
        }
    }

    internal class ProcessingTask
    {
        public int ID { get; }

        public ProcessingTask(int id)
        {
            ID = id;
        }
    }
}
