using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brace.Shared.Core.Classes;

namespace Brace.Shared.Core.Interfaces
{
    public interface IScanDataReceiver
    {
        event EventHandler<ScanDataEventArgs> ScanDataEvent;
    }
}
