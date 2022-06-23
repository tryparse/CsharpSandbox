using System.Threading.Tasks;

namespace CsharpSandbox.ObjectPooling
{
    public class ReadingService
    {
        private readonly ConnectorObjectPool _pool;

        public ReadingService(ConnectorObjectPool pool)
        {
            _pool = pool;
        }

        public async Task<ProcessingTask> Read()
        {
            var connector = await _pool.GetAsync();
            var message = connector.Read();
            _pool.Release(connector);

            return new ProcessingTask(message);
        }
    }
}
