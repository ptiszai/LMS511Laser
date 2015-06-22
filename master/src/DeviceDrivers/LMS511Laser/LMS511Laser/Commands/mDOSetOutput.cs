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
   // class mDOSetOutput
   /// <summary>
    /// Set output state
    /// mDOSetOutput  struct, requested    
    /// </summary>
    /// <remarks>
    ///  sMN mDOSetOutput 
    ///  reading datas storing struct from the device
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct mDOSetOutput
    {
        /// <summary>        
        ///  start char : 0x02    
        /// </summary>
        public byte stx;
        /// <summary>        
        ///  type : mDOSetOutput    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] type;
        /// <summary>        
        ///  command    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        public byte[] cmd;
        /// <summary>
        /// space : 0x20         
        /// </summary>
        public byte sp0;
        /// <summary>        
        ///  Output Number    
        /// </summary>
        /// <remarks> 
        /// 1 - 3
        /// </remarks> 
        public byte outputNumber;
        /// <summary>
        /// space : 0x20         
        /// </summary>
        public byte sp1;
        /// <summary>        
        ///  Output State   
        /// </summary>
        /// <remarks> 
        /// 00 = inactive, 01 = active
        /// </remarks> 
        public byte outputState;
        /// <summary>        
        ///  end char : 0x03    
        /// </summary>
        public byte etx;
    }
    /// <summary>   
    /// mDOSetOutput_R  struct, responded      
    /// </summary>
    public struct mDOSetOutput_R
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
    ///  mDOSetOutput_class class:
    /// </summary>
    public class mDOSetOutput_class
    {
        private CommandType _type;
        private mDOSetOutput _data;
        /// <summary>        
        ///  Get type : mDOSetOutput   
        /// </summary>
        public CommandType Type
        {
            get { return _type; }
        }
        /// <summary>
        ///  get, set  LMPscancfg type variable
        /// </summary>
        public mDOSetOutput Data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        ///  mDOSetOutput_class constructor
        /// </summary>      
        public mDOSetOutput_class(int oportNum, int oportStatus)
        {
            string sTemp;
            ASCIIEncoding encoding = new ASCIIEncoding();
            _type = CommandType.mDOSetOutput;
            _data = new mDOSetOutput();
            _data.stx = 0x02;
            sTemp = "sMN ";
            _data.type = encoding.GetBytes(sTemp);
            sTemp = "mDOSetOutput ";          
            _data.cmd = encoding.GetBytes(sTemp);
            _data.sp0 = 0x20;
            _data.outputNumber = (byte)(oportNum + 0x30);
            _data.sp1 = 0x20;
            _data.outputState = (byte)(oportStatus + 0x30);
            _data.etx = 0x03;
        }
    }
}
