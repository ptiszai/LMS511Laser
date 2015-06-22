using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brace.Shared.DeviceDrivers.LMS511Laser
{
    /// <summary>
    ///  ScanCfg class
    /// </summary>
    public class ScanCfg
    {
        /// <summary>
        /// Scan Frequency 
        /// [1/100Hz]
        /// </summary>
        public uint scan_frequency { get; set; }
        /// <summary>
        /// Angle Resolution
        /// [1/10000°]        
        /// </summary>
        public uint angle_resolution { get; set; }
        /// <summary>
        /// Start Angle
        /// [1/10000°]           
        /// </summary>
        public int start_angle { get; set; }
        /// <summary>
        /// Stop Angle
        /// [1/10000°]           
        /// </summary>
        public int stop_angle { get; set; }
    }
}
