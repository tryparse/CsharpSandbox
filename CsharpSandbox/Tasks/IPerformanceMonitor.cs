namespace CsharpSandbox.Tasks
{
    public interface IPerformanceMonitor
    {
        void AddRecord(string operation);
        void Start();
        void Stop();
    }
}
