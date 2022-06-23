using System;

namespace CsharpSandbox.ObjectPooling
{
    public class ProcessingTask
    {
        public Guid CorrelationId { get; }
        public DateTime CreatedWhen { get; }

        public string Message { get; }

        public ProcessingTask(string message)
        {
            CorrelationId = Guid.NewGuid();
            CreatedWhen = DateTime.Now;
            Message = message;
        }

        public override string ToString()
        {
            return $"CorrelationId={CorrelationId} CreatedWhen={CreatedWhen} Message={Message}";
        }
    }
}
