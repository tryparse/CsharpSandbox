using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace CsharpSandbox.ObjectPooling
{
    public class Connector
    {
        private static int _id;
        private static int _messageId;
        private readonly IRandomDependency _random;
        private readonly ILogger _logger;

        public int ID { get; }

        public Connector(IRandomDependency random, ILogger logger)
        {
            _random = random ?? throw new ArgumentNullException(nameof(random));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            ID = Interlocked.Increment(ref _id);
            _logger.Debug("Connector created. ID={0}", ID);
        }

        public string Read()
        {
            var messageId = Interlocked.Increment(ref _messageId);
            var message = $"MessageId={messageId}";

            _logger.Debug("ConnectorId={0} Message readed: {1}", ID, message);

            return message;
        }

        public void Push(string message)
        {
            _logger.Debug("ConnectorId={0} Message pushed {1}", ID, message);
        }
    }
}
