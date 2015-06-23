using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brace.Shared.DeviceDrivers.LMS511Laser.Enums
{
    public enum SetscancfgEnum
    {
        SUCCESS = 0,
        FrequencyError = 1,   
        ResolutionError = 2,
        Res_and_Scn_Error = 3,
        ScanAreaError = 4,
        OtherErrors = 5
    }
}