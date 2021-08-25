using System.Threading;
using System.Threading.Tasks;

namespace CsharpSandbox.Tasks
{
    class DelayedTask
    {
        public int ID { get; set; }

        public int Delay { get; set; }

        public async Task Run(CancellationToken cancellationToken)
        {
            //Console.WriteLine($"Task {ID} started");
            await Task.Delay(Delay, cancellationToken);
            //Console.WriteLine($"Task {ID} completed (delay={Delay})");
        }
    }
}
