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
    /// Ask timestamp and device status
    /// sRN STlms        
    /// </summary>
    ///  <remarks>
    ///  STlms 
    ///  writing datas storing struct
    ///  </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct STlms
    {
        public byte stx;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] type;
        /// <summary>        
        ///  command    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] cmd;
        /// <summary>        
        ///  end char : 0x03    
        /// </summary>
        public byte etx;
    }

    public struct STlms_R
    {
        /// <summary>        
        ///  statusCode   
        /// </summary>  
        ///  <remarks>
        ///  Status Code                  
        /// 00 00 = undefined
        /// 00 01 = initialization
        /// 00 02 = configuration
        /// 00 03 = lower case
        /// 00 04 = rotating
        /// 00 05 = in preparation
        /// 00 06 = ready
        /// 00 07 = measurement active
        // 00 08 .. 00 11 = reserved
        ///  </remarks>/// 
        public int statusCode;
        /// <summary>        
        ///  OpTemp : Operation Temp.   
        /// </summary> 
        public byte  OpTemp;       
        /// <summary>        
        ///  range : Range.   
        /// </summary>
        public ushort range;
        /// <summary>        
        ///  time : Time: "HH:MM:SS".   
        /// </summary>
        string time;
        /// <summary>        
        ///  date : Date: "DD.MM.YYYY".  
        /// </summary>
        string date;
        /// <summary>        
        ///  led1 : LED1: 00 00 = inactive, 00 01 = active 
        /// </summary>
        uint led1;
        /// <summary>        
        ///  led2 : LED2: 00 00 = inactive, 00 01 = active 
        /// </summary>
        uint led2;
        /// <summary>        
        ///  led3 : LED3: 00 00 = inactive, 00 01 = active 
        /// </summary>
        uint led3;
    }

    public class STlms_class
    {
        private CommandType _type;
        private STlms _data;
        public CommandType Type
        {
            get { return _type; }
        }
        public STlms Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public STlms_class()
        {
            string sTemp;
            ASCIIEncoding encoding = new ASCIIEncoding();
            _type = CommandType.STlms;
            _data = new STlms();
            _data.stx = 0x02;
            sTemp = "sRN ";
            _data.type = encoding.GetBytes(sTemp);
            sTemp = "STlms ";
            _data.cmd = encoding.GetBytes(sTemp);
            _data.etx = 0x03;
        }
    }


}
