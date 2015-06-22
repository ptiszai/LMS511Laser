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
using System.Runtime.InteropServices;
using Brace.Shared.DeviceDrivers.LMS511Laser.Enums;

namespace Brace.Shared.DeviceDrivers.LMS511Laser.Commands
{
    /// <summary>
    /// Reboot Device
    /// mSCreboot 
    /// </summary>
    /// <remarks>
    ///  sMN mSCreboot   
    /// </remarks>
    public struct mSCreboot
    {
        /// <summary>        
        ///  start char : 0x02    
        /// </summary>
        public byte stx;
        /// <summary>        
        ///  type   
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] type;
        /// <summary>        
        ///  command    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] cmd;
        /// <summary>        
        ///  end char : 0x03    
        /// </summary>
        public byte etx;
    }
    /// <summary>
    ///  mSCreboot class:
    /// </summary>
    public class mSCreboot_class
    {
        private CommandType _type;
        private mSCreboot _data;
        /// <summary>        
        ///  Get type : mSCreboot   
        /// </summary>
        public CommandType Type
        {
            get { return _type; }
        }
        /// <summary>
        ///  get, set  mSCreboot type variable
        /// </summary>
        public mSCreboot Data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        ///  LmSCreboot_class constructor
        /// </summary> 
        public mSCreboot_class()
        {
            string sTemp;
            ASCIIEncoding encoding = new ASCIIEncoding();
            _type = CommandType.mSCreboot;
            _data = new mSCreboot();
            _data.stx = 0x02;
            sTemp = "sMN ";
            _data.type = encoding.GetBytes(sTemp);
            sTemp = "mSCreboot ";
            _data.cmd = encoding.GetBytes(sTemp);
            _data.etx = 0x03;
        }
    }

}
