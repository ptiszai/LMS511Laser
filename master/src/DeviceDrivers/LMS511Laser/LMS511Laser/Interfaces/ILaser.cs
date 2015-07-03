/* This file is part of *LMS511Laser*.
Copyright (C) 2015 Tiszai Istvan, tiszaii@hotmail.com

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
using Brace.Shared.DeviceDrivers.LMS511Laser.EventHandlers;

namespace Brace.Shared.DeviceDrivers.LMS511Laser.Interfaces
{
    /// <summary>
    /// Interface for laser devices
    /// </summary>
    public interface ILaser
    {
        /// <summary>
        /// Reboot command for laser devices
        /// </summary>
        void  ReBoot();
        event EventHandler<mSCrebootEventArgs> ReBootEvent;

        /// <summary>
        /// DIO out command for laser devices, ONLY TEST.
        /// </summary>
        void DOOutput();
        void DOSetOutput(int oportNum, int oportStatus);
        event EventHandler<mDOSetOutputEventArgs> DOSetOutputEvent;

        /// <summary>
        /// Setscancfg, Set frequency and angular resolution
        /// </summary>
        void SetSetscancfg(uint scan_frequency, uint angle_resolution, int start_angle, int stop_angle);
        void Setscancfg();       
        event EventHandler<mLMPsetscancfgEventArgs> SetConfigEvent;

        /// <summary>
        /// LMDscandata, Polling one Telegram
        /// </summary>
        void Scandata(); 
        event EventHandler<LMDscandataEventArgs> LMDscandataEvent;

        /// <summary>
        /// Set Date and Time command for laser devices, ONLY TEST.
        /// </summary>
        void Setdatetime();
        void SetSetdatetime(DateTime dt);
        event EventHandler<LSPsetdatetimeEventArgs> LSPsetdatetimeEvent;

        /// <summary>
        /// LMDscandatacfg, Configure the data content for the scan.
        /// </summary>
        void LMDscandatacfg(short outputchannel, int remission, int resolution, int unit, short encoder, short position, short device_name, short comment, short time, short output_rate);
        void Scandatacfg();
        event EventHandler<LMDscandatacfgEventArgs> LMDscandatacfgEvent;

        /// <summary>
        /// DeviceIdent command for laser devices.
        /// </summary>
        void DeviceIdent();
        event EventHandler<DeviceIdentEventArgs> DeviceIdentEvent;

        /// <summary>
        /// Summa error counter for laser devices, saved in PC.
        /// </summary>
        uint getFullErrorCounter();

        /// <summary>
        /// Summa error counter for laser devices, saved in PC.
        /// </summary>
        uint FullSopasErrorCounter();

        /// <summary>
        /// Summa connected counter for laser devices, saved in PC.
        /// </summary>
        uint getFullConnectedCounter();

        /// <summary>
        /// Date Time from laser devices,  saved in PC.
        /// </summary>
        DateTime getLaserInternalDateTime();
    }
}
