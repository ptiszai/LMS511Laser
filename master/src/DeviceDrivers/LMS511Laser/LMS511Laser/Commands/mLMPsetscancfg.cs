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
    /// Set frequency and angular resolution         
    /// </summary>
    ///  <remarks>
    ///  mLMPsetscancfg, requested   
    ///  writing datas storing struct to device
    ///  </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct mLMPsetscancfg
    {
        /// <summary>        
        ///  start char : 0x02    
        /// </summary>
        public byte stx;
        /// <summary>        
        ///  type : mLMPsetscancfg    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] type;
        /// <summary>        
        ///  command    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] cmd;
        /// <summary>
        /// Scan Frequency 
        /// [1/100Hz]
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] scan_frequency;
        /// <summary>
        /// space : 0x20         
        /// </summary>
        public byte sp0;
        /// <summary>
        /// not used        
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] value;
        /// <summary>
        /// space : 0x20         
        /// </summary>
        public byte sp1;
        /// <summary>
        /// Angle Resolution
        /// [1/10000°]        
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] angle_resolution;
        /// <summary>
        /// space : 0x20        
        /// </summary>
        public byte sp2;
        /// <summary>
        /// Start Angle
        /// [1/10000°]    
        /// ATTENTION: Scan angle can not be changed here, only in the data output !
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] start_angle;
        /// <summary>
        /// space : 0x20        
        /// </summary>
        public byte sp3;
        /// <summary>
        /// Stop Angle
        /// [1/10000°]    
        /// ATTENTION: Scan angle can not be changed here, only in the data output !
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] stop_angle;
        /// <summary>        
        ///  end char : 0x03    
        /// </summary>                
        public byte etx;
    }

    /// <summary>
    ///  mLMPsetscancfg_R 
    ///  Reading datas storing struct, responded
    /// </summary>
    public struct mLMPsetscancfg_R
    {
        /// <summary>
        /// Status Code
        /// 0 no Error
        /// 1 Frequency Error
        /// 2 Resolution Error
        /// 3 Res. and Scn. Error
        /// 4 Scan area Error
        /// 5 other Errors        
        /// </summary>
        public int statusCode;
        /// <summary>
        /// Scan Frequency 
        /// [1/100Hz]
        /// </summary>
        public uint scan_frequency;
        /// <summary>
        /// reserved
        /// 1
        /// </summary>
        public short value;
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
    ///  mLMPsetscancfg_class class:
    /// </summary>
    public class mLMPsetscancfg_class
    {
        private CommandType _type;
        private mLMPsetscancfg _data;
        /// <summary>
        ///  get Type
        /// </summary>
        public CommandType Type
        {
            get { return _type; }
        }
        /// <summary>
        ///  get, set  mLMPsetscancfg type variable
        /// </summary>
        public mLMPsetscancfg Data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        ///  mLMPsetscancfg_class:
        /// </summary>
        /// <param name="scancfg">select_user_level (ScanCfg type)</param>       
        /// <returns></returns>
        public mLMPsetscancfg_class(ScanCfg scancfg)
        {
            string sTemp;
            ASCIIEncoding encoding = new ASCIIEncoding();
            _type = CommandType.mLMPsetscancfg;
            _data = new mLMPsetscancfg();
            _data.stx = 0x02;
            sTemp = "sMN ";
            _data.type = encoding.GetBytes(sTemp);
            sTemp = "mLMPsetscancfg ";
            _data.cmd = encoding.GetBytes(sTemp);
            _data.scan_frequency = FunctHelper.ConvertUintToHexByteArray(scancfg.scan_frequency);
            _data.sp0 = 0x20;
            _data.value = FunctHelper.ConvertShortToHexByteArray(1);
            _data.sp1 = 0x20;
            _data.angle_resolution = FunctHelper.ConvertUintToHexByteArray(scancfg.angle_resolution);
            _data.sp2 = 0x20;
            _data.start_angle = FunctHelper.ConvertIntToHexByteArray(scancfg.start_angle);
            _data.sp3 = 0x20;
            _data.stop_angle = FunctHelper.ConvertIntToHexByteArray(scancfg.stop_angle);
            _data.etx = 0x03;
        }
    }
}
