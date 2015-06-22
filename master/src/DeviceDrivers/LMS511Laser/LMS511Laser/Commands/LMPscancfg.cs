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
    /// Get frequency and angular resolution
    /// LMPscancfg  struct, requested    
    /// </summary>
    /// <remarks>
    ///  sRN LMPscancfg 
    ///  reading datas storing struct from the device
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LMPscancfg
    {
        /// <summary>        
        ///  start char : 0x02    
        /// </summary>
        public byte stx;
        /// <summary>        
        ///  type : LMPscancfg    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] type;
        /// <summary>        
        ///  command    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        public byte[] cmd;
        /// <summary>        
        ///  end char : 0x03    
        /// </summary>
        public byte etx;
    }
    /// <summary>
    /// Get frequency and angular resolution
    /// LMPscancfg_R  struct, responded      
    /// </summary>
    public struct LMPscancfg_R
    {
        /// <summary>
        /// Scan Frequency 
        /// [1/100Hz]
        /// </summary>
        public uint scan_frequency;
        /// <summary>
        /// Angle Resolution
        /// [1/10000°]        
        /// </summary>
        public uint angle_resolution;
        /// <summary>
        /// Start Angle
        /// [1/10000°]           
        /// </summary>
        public int start_angle;
        /// <summary>
        /// Stop Angle
        /// [1/10000°]           
        /// </summary>
        public int stop_angle;
    }

    /// <summary>
    ///  LMPscancfg_class class:
    /// </summary>
    public class LMPscancfg_class
    {
        private CommandType _type;
        private LMPscancfg _data;
        /// <summary>        
        ///  Get type : LMPscancfg    
        /// </summary>
        public CommandType Type
        {
            get { return _type; }
        }
        /// <summary>
        ///  get, set  LMPscancfg type variable
        /// </summary>
        public LMPscancfg Data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        ///  LMPscancfg_class constructor
        /// </summary>      
        public LMPscancfg_class()
        {
            string sTemp;
            ASCIIEncoding encoding = new ASCIIEncoding();
            _type = CommandType.LMPscancfg;
            _data = new LMPscancfg();
            _data.stx = 0x02;
            sTemp = "sRN ";
            _data.type = encoding.GetBytes(sTemp);
            sTemp = "LMPscancfg ";
            _data.cmd = encoding.GetBytes(sTemp);
            _data.etx = 0x03;
        }
    }
}
