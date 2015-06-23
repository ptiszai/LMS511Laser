using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brace.Shared.DeviceDrivers.LMS511Laser.Enums;

namespace Brace.Shared.DeviceDrivers.LMS511Laser.EventHandlers
{
    /// <summary>
    /// LMDscandatacfg EventArgs class
    /// </summary> 
    public class LMDscandatacfgEventArgs : EventArgs
    {
        /// <summary>
        /// statusCode: int.        
        /// </summary> 
        /// <remarks>
        /// 0 no Error
        /// 1 Success             
        /// </remarks>         
        private StatusEnum _statusCode;

        public LMDscandatacfgEventArgs(int statusCode)
        {
            this._statusCode = (StatusEnum)statusCode;
        }

        public StatusEnum StatusCode
        {
            get
            {
                return _statusCode;
            }
        }
    }
}
