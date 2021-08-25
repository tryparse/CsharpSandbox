using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CsharpSandbox
{
    interface ISandboxRunner
    {
        void Run();
    }

    interface IAsyncSandboxRunner
    {
        Task Run(CancellationToken cancellationToken);
    }
}
