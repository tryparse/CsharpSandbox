namespace CsharpSandbox.Tasks
{
    public class PerformanceRecord
    {
        public string Operation { get; }
        public long ElapsedMilliseconds { get; }

        public PerformanceRecord(string operation, long elapsedMilliseconds)
        {
            Operation = operation;
            ElapsedMilliseconds = elapsedMilliseconds;
        }
    }
}
