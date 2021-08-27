using System;

namespace ClassLibrary1
{
    public class Example : IExample
    {
        public void PublicRun()
        {
            throw new NotImplementedException();
        }

        void IExample.InternalRun()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class BaseClass
    {
        public abstract void Run();
    }
}
