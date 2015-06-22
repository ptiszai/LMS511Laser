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
   // class LSPsetdatetime
   /// <summary>
    /// Set timestamp
    /// LSPsetdatetime  struct, requested    
    /// </summary>
    /// <remarks>
    ///  sMN LSPsetdatetime
    ///  writting datas storing struct from the device
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LSPsetdatetime
    {
        /// <summary>        
        ///  start char : 0x02    
        /// </summary>
        public byte stx;
        /// <summary>        
        ///  type : LSPsetdatetime    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] type;
        /// <summary>        
        ///  command    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] cmd;
        /// <summary>        
        ///  Year    
        /// </summary> 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] year;
        /// <summary>
        /// space : 0x20         
        /// </summary>
        public byte sp0;
        /// <summary>        
        ///  Month   
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] month;
        /// <summary>
        /// space : 0x20         
        /// </summary>
        public byte sp1;
        /// <summary>        
        ///  Day   
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] day;
        /// <summary>
        /// space : 0x20         
        /// </summary>
        public byte sp2;
        /// <summary>        
        ///  Hour   
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] hour;
        /// <summary>
        /// space : 0x20         
        /// </summary>
        public byte sp3;
        /// <summary>        
        ///  Minute   
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] minute;
        /// <summary>
        /// space : 0x20         
        /// </summary>
        public byte sp4;
        /// <summary>        
        ///  Second   
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] second;
        /// <summary>
        /// space : 0x20         
        /// </summary>
        public byte sp5;
        /// <summary>        
        ///  Microsecond  
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] 
        public byte[] microsecond;
        /// <summary>        
        ///  end char : 0x03    
        /// </summary>
        public byte etx;
    }
    /// <summary>   
    /// LSPsetdatetime_R  struct, responded      
    /// </summary>
    public struct LSPsetdatetime_R
    {
        /// <summary>        
        ///  StatusCode  
        /// </summary>
        /// <remarks>
        ///  01 = Success
        ///  00 = Error
        /// </remarks>
        public int statusCode;
    }

    /// <summary>
    ///  LSPsetdatetime_class class:
    /// </summary>
    public class LSPsetdatetime_class
    {
        private CommandType _type;
        private LSPsetdatetime _data;
        /// <summary>        
        ///  Get type : LSPsetdatetime   
        /// </summary>
        public CommandType Type
        {
            get { return _type; }
        }
        /// <summary>
        ///  get, set  LMPscancfg type variable
        /// </summary>
        public LSPsetdatetime Data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        ///  LSPsetdatetime_class constructor
        /// </summary>      
        public LSPsetdatetime_class(int year, int month, int day, int hour, int min, int sec, int microsecond)
        {
            string sTemp;
            ASCIIEncoding encoding = new ASCIIEncoding();
            _type = CommandType.LSPsetdatetime;
            _data = new LSPsetdatetime();
            _data.stx = 0x02;
            sTemp = "sMN ";
            _data.type = encoding.GetBytes(sTemp);
            sTemp = "LSPsetdatetime ";
            _data.cmd = encoding.GetBytes(sTemp);
            _data.year = FunctHelper.ConvertShortToHexByteArray((short)year);
            _data.sp0  = 0x20;
            _data.month = FunctHelper.ByteToHexByteArray((byte)month);
            _data.sp1  = 0x20;
            _data.day = FunctHelper.ByteToHexByteArray((byte)day);
            _data.sp2  = 0x20;
            _data.hour = FunctHelper.ByteToHexByteArray((byte)hour);
            _data.sp3  = 0x20;
            _data.minute = FunctHelper.ByteToHexByteArray((byte)min);
            _data.sp4  = 0x20;
            _data.second = FunctHelper.ByteToHexByteArray((byte)sec);
            _data.sp5  = 0x20;
            _data.microsecond = FunctHelper.ConvertIntToHexByteArray(microsecond);
            _data.etx = 0x03;
        }
    }
}
