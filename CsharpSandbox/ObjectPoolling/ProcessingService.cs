using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace CsharpSandbox.ObjectPooling
{
    public class ProcessingService
    {
        private readonly IRandomDependency _random;
        private readonly IObjectPoolAsync<Connector> _pool;
        private readonly ILogger _logger;

        public ProcessingService(IRandomDependency random, IObjectPoolAsync<Connector> pool, ILogger logger)
        {
            _random = random ?? throw new ArgumentNullException(nameof(random));
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Process(ProcessingTask processingTask, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            _logger.Verbose("ProcessingTask=[{0}] started", processingTask.CorrelationId);

            await Task.Delay(1000, cancellationToken);

            _logger.Debug("Processed message CorrelationId=[{0}] Message=[{1}]", processingTask.CorrelationId, processingTask.Message);

            var connector = await _pool.GetAsync();

            try
            {
                connector.Push(processingTask.Message);
            }
            finally
            {
                _pool.Release(connector);
            }

            _logger.Verbose("ProcessingTask=[{0}] finished", processingTask.CorrelationId);
        }
    }
}
