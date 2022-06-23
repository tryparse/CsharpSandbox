using System.Collections.Generic;
using System.Diagnostics;

namespace CsharpSandbox.Tasks
{
    public class PerformanceMonitor : IPerformanceMonitor
    {
        private readonly Queue<PerformanceRecord> _records;
        private readonly Stopwatch _stopwatch;

        public PerformanceMonitor()
        {
            _records = new Queue<PerformanceRecord>(20);
            _stopwatch = new Stopwatch();
        }

        public void AddRecord(string operation)
        {
            _records.Enqueue(new PerformanceRecord(operation, _stopwatch.ElapsedMilliseconds));
        }

        public void Start()
        {
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }
    }
}
