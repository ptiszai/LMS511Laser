using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brace.Shared.Core.Classes
{
    public class ScanDataEventArgs
    {
        public ScanData ScanData { get; internal set; }
        public DateTime TimeReached { get; internal set; }

        public ScanDataEventArgs(ScanData scanData, DateTime timeReached)
        {
            this.ScanData = scanData;
            this.TimeReached = timeReached;
        }
    }
}
