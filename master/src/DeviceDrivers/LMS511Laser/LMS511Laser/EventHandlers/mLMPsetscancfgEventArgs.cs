/* This file is part of *LMS511Laser*.
Copyright (C) 2015 Tiszai Istvan

*program name* is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brace.Shared.DeviceDrivers.LMS511Laser.Enums;
using Brace.Shared.DeviceDrivers.LMS511Laser.Commands;

namespace Brace.Shared.DeviceDrivers.LMS511Laser.EventHandlers
{
    public class mLMPsetscancfgEventArgs : EventArgs
    {
        /// <summary>        
        ///  mLMPsetscancfg  : mLMPsetscancfg_R class
        /// </summary>
        /// <remarks>
        ///  Set frequency and angular resolution
        /// </remarks>
        private mLMPsetscancfg_R _setScanCfg;
        public mLMPsetscancfgEventArgs(mLMPsetscancfg_R setScanCfg)
        {
            this._setScanCfg = setScanCfg;
        }

        public mLMPsetscancfg_R SetScanCfg
        {
            get
            {
                return _setScanCfg;
            }
        }
    }
}
