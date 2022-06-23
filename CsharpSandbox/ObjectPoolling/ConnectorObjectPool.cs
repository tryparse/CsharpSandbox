using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace CsharpSandbox.ObjectPooling
{
    public class ConnectorObjectPool : IObjectPoolAsync<Connector>
    {
        public int PoolSize { get; }

        private readonly IRandomDependency _random;
        private readonly ConcurrentQueue<Connector> _connectors;
        private readonly ILogger _logger;

        private readonly SemaphoreSlim _semaphoreSlim;

        public ConnectorObjectPool(int poolSize, IRandomDependency random, ILogger logger)
        {
            PoolSize = poolSize;
            _semaphoreSlim = new SemaphoreSlim(poolSize);
            _connectors = new ConcurrentQueue<Connector>();
            _random = random ?? throw new ArgumentNullException(nameof(random));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger.Debug("Init pool with {0} objects", poolSize);

            Parallel.For(0, poolSize, (i) =>
            {
                var connector = new Connector(_random, _logger);
                _connectors.Enqueue(connector);
            });
        }

        public async Task<Connector> GetAsync()
        {
            var stopwatch = Stopwatch.StartNew();

            await _semaphoreSlim.WaitAsync();

            stopwatch.Stop();

            _logger.Debug("Waiting for Connector for {0}", stopwatch.ElapsedMilliseconds);
            
            if (_connectors.TryDequeue(out var connector))
            {
                _logger.Debug("ConnectorObjectPool.GetAsync {0}", connector.ID);
                return connector;
            }

            _logger.Error("ConnectorObjectPool.GetAsync {0} Creating new Connector", connector.ID);
            connector = new Connector(_random, _logger);

            return connector;
        }

        public void Release(Connector obj)
        {
            _connectors.Enqueue(obj);
            _semaphoreSlim.Release();

            _logger.Debug("ConnectorObjectPool.Release Connector {0} to the pool", obj.ID);
        }
    }
}
