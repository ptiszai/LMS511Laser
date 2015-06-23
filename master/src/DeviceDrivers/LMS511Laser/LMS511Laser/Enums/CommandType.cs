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
using System.Text;

namespace Brace.Shared.DeviceDrivers.LMS511Laser.Enums
{
    /// <summary>
    /// Enum for setting the laser commands
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// LMDscandata asked after sended
        /// </summary>
        LMDscandata,
        /// <summary>
        /// LMDscandata continued
        /// </summary>
        LMDscandata_E,
        /// <summary>
        /// SCdevicestate
        /// </summary>
        SCdevicestate,
        /// <summary>
        /// DeviceIdent
        /// </summary>
        DeviceIdent,
        /// <summary>
        /// Run
        /// </summary>
        Run,
        /// <summary>
        /// SetAccessMode
        /// </summary>
        SetAccessMode,
        /// <summary>
        /// LMPscancfg
        /// </summary>
        LMPscancfg,
        /// <summary>
        /// mLMPsetscancfg
        /// </summary>
        mLMPsetscancfg,
        /// <summary>
        /// LMPoutputRange
        /// </summary>
        LMPoutputRange,
        /// <summary>
        /// LMPoutputRange_get
        /// </summary>
        LMPoutputRange_get,
        /// <summary>
        /// LCMstate
        /// </summary>
        LCMstate,
        /// <summary>
        /// mDOSetOutput
        /// </summary>
        mDOSetOutput,
        /// <summary>
        /// STlms
        /// </summary>
        STlms,
        /// <summary>
        /// LSPsetdatetime
        /// </summary>
        LSPsetdatetime,
        /// <summary>
        /// LIDoutputstate
        /// </summary>
        LIDoutputstate,
        /// <summary>
        /// LIDrstoutpcnt
        /// </summary>
        LIDrstoutpcnt,
        /// <summary>
        /// mSCreboot
        /// </summary>
        mSCreboot,
        /// <summary>
        /// LMDscandatacfg
        /// </summary>
        LMDscandatacfg
    }
}
