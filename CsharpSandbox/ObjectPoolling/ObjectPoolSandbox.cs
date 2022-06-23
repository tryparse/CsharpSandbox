using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Serilog;

namespace CsharpSandbox.ObjectPooling
{
    public class ObjectPoolSandbox : IAsyncSandboxRunner
    {
        private readonly IObjectPoolAsync<Connector> _pool;
        private readonly IRandomDependency _random = new RandomGenerator();
        private readonly ProcessingService _service;
        private readonly ILogger _logger;
        private ActionBlock<ProcessingTask> _processingBlock;

        public ObjectPoolSandbox(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _pool = new ConnectorObjectPool(25, _random, _logger);
            _service = new ProcessingService(_random, _pool, _logger);
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _processingBlock = GetProcessingBlock(cancellationToken, 100);

            for (var i = 0; i < 100; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await Process(cancellationToken);
            }

            _processingBlock.Complete();
            await _processingBlock.Completion;
        }

        private async Task Process(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            var connector = await _pool.GetAsync();

            string message;
            ProcessingTask task;

            try
            {
                message = connector.Read();
                task = new ProcessingTask(message);
            }
            finally
            {
                _pool.Release(connector);
            }

            var stopwatch = Stopwatch.StartNew();

            await _processingBlock.SendAsync(task, cancellationToken);


            stopwatch.Stop();

            _logger.Debug("CorrelationId={0} Waiting for processing for {1} ms", task.CorrelationId, stopwatch.ElapsedMilliseconds);
            _logger.Debug("CorrelationId={0} InputCount={1}", task.CorrelationId, _processingBlock.InputCount);
        }

        private ActionBlock<ProcessingTask> GetProcessingBlock(CancellationToken cancellationToken, int maxDegreeOfParallelism = 1)
        {
            ExecutionDataflowBlockOptions options = new ExecutionDataflowBlockOptions
            {
                BoundedCapacity = maxDegreeOfParallelism,
                EnsureOrdered = false,
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
                MaxMessagesPerTask = DataflowBlockOptions.Unbounded,
                CancellationToken = cancellationToken,
            };

            return new ActionBlock<ProcessingTask>((message) => _service.Process(message, cancellationToken), options);
        }
    }
}
