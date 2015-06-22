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
using Brace.Shared.DeviceDrivers.LMS511Laser.Helpers;

namespace Brace.Shared.DeviceDrivers.LMS511Laser.Commands
{
    /// <summary>
    /// SetAccessMode struct, requested
    /// Log in to device command
    /// </summary> 
    /// <remarks>    
    ///  writing datas storing struct
    ///  sMN SetAccessMode    
    /// </remarks> 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SetAccessMode
    {
        /// <summary>        
        ///  start char : 0x02    
        /// </summary>
        public byte stx;
        /// <summary>        
        ///  type : SetAccessMode    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] type;
        /// <summary>        
        ///  command    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
        public byte[] cmd;
        /// <summary>        
        ///  User level   
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] user_level;
        /// <summary>        
        ///  Password   
        /// </summary>        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] password;
        /// <summary>        
        ///  end char : 0x03    
        /// </summary>
        public byte etx;
    }
    /// <summary>
    ///  SetAccessMode_R, responded 
    ///  Reading datas storing struct
    /// </summary>
    public struct SetAccessMode_R
    {
        /// <summary>        
        ///  User level  
        ///  0 error
        ///  1 success
        /// </summary>
        public int change_user_level;
    }
    /// <summary>
    ///  SetAccessMode class
    /// </summary>
    public class SetAccessMode_class
    {
        private CommandType _type;
        private SetAccessMode _data;
        /// <summary>        
        ///  Get type : SetAccessMode    
        /// </summary>
        public CommandType Type
        {
            get { return _type; }
        }
        /// <summary>        
        ///  Get data : SetAccessMode writing store struct    
        /// </summary>
        public SetAccessMode Data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        ///  SetAccessMode constructor
        /// </summary>
        /// <param name="select_user_level">select_user_level:
        ///  02 maintenance
        ///  03 authorized client
        ///  04 Service         
        /// </param>
        /// <param name="password"></param>
        /// <returns></returns>
        public SetAccessMode_class(int select_user_level, uint password)
        {
            string sTemp;
            ASCIIEncoding encoding = new ASCIIEncoding();
            _type = CommandType.SetAccessMode;
            _data = new SetAccessMode();
            _data.stx = 0x02;
            sTemp = "sMN ";
            _data.type = encoding.GetBytes(sTemp);
            sTemp = "SetAccessMode ";
            _data.cmd = encoding.GetBytes(sTemp);
            _data.user_level = new byte[3];
            _data.user_level[0] = 0x30;
            _data.user_level[1] = (byte)(select_user_level + 0x30);
            _data.user_level[2] = 0x20;
            _data.password = FunctHelper.ConvertUintToHexByteArray(password);
            _data.etx = 0x03;
        }
    }

}
