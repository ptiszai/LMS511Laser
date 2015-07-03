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
using System.Runtime.InteropServices;
using Brace.Shared.DeviceDrivers.LMS511Laser.Enums;

namespace Brace.Shared.DeviceDrivers.LMS511Laser.Commands
{
    /// <summary>
    /// Device Ident of LM device
    /// DeviceIdent  struct, requested    
    /// </summary>
    /// <remarks>
    ///  sRN DeviceIdent
    ///  reading datas storing struct from the device
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DeviceIdent // sRI 0
    {
        public byte stx;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] type;
        public byte etx;
    }

    public class DeviceIdent_class
    {
        private CommandType _type;
        private DeviceIdent _data;
        public CommandType Type
        {
            get { return _type; }
        }
        public DeviceIdent Data
        {
            get { return _data; }
            set { _data = value; }
        }
        public DeviceIdent_class()
        {
            string sTemp;
            ASCIIEncoding encoding = new ASCIIEncoding();
            _type = CommandType.DeviceIdent;
            _data = new DeviceIdent();
            _data.stx = 0x02;
            sTemp = "sRI0 ";
            _data.type = encoding.GetBytes(sTemp);
            _data.etx = 0x03;
        }
    }
}
