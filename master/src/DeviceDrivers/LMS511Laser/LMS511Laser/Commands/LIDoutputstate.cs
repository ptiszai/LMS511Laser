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
    /// Ask state of the outputs
    /// LIDoutputstate  struct, requested    
    /// </summary>
    /// <remarks>
    ///  sRN LIDoutputstate
    ///  reading datas storing struct from the device
    /// </remarks>
    public struct LIDoutputstate
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] cmd;
        /// <summary>        
        ///  end char : 0x03    
        /// </summary>
        public byte etx;
    }
    /// <summary>
    ///  LIDoutputstate_R, responded 
    ///  Reading datas storing struct
    /// </summary>
    public struct LIDoutputstate_R
    {
        /// <summary>        
        ///  StatusCode  
        /// </summary>
        /// <remarks>
        /// </remarks>
        public uint statusCode;
        /// <summary>        
        ///  out1State  
        /// </summary>
        /// <remarks>
        /// States:
        /// 00 = low
        /// 01 = High
        /// 02 = Tristate (undefined)
        /// </remarks>
        public byte out1State;
        /// <summary>        
        ///  out1State  
        /// </summary>
        /// <remarks>
        /// Counter:        
        /// </remarks>
        public uint out1Count;
        public byte out2State;
        public uint out2Count;
        public byte out3State;
        public uint out3Count;
        public byte out4State; // only LMS5xx
        public uint out4Count; // only LMS5xx
        public byte out5State; // only LMS5xx
        public uint out5Count; // only LMS5xx
        public byte out6State; // only LMS5xx
        public uint out6Count; // only LMS5xx

        public byte extOut1State;
        public uint extOut1Count;
        public byte extOut2State;
        public uint extOut2Count;
        public byte extOut3State;
        public uint extOut3Count;
        public byte extOut4State;
        public uint extOut4Count;
        public byte extOut5State;
        public uint extOut5Count;
        public byte extOut6State;
        public uint extOut6Count;
        public byte extOut7State;
        public uint extOut7Count;
        public byte extOut8State;
        public uint extOut8Count;   
    }

    /// <summary>
    ///  LIDoutputstate class:
    /// </summary>
    public class LIDoutputstate_class
    {
        private CommandType _type;
        private LIDoutputstate _data;
        /// <summary>        
        ///  Get type : LIDoutputstate    
        /// </summary>
        public CommandType Type
        {
            get { return _type; }
        }
        /// <summary>
        ///  get, set  LIDoutputstate type variable
        /// </summary>
        public LIDoutputstate Data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        ///  LIDoutputstate_class constructor
        /// </summary> 
        public LIDoutputstate_class()
        {
            string sTemp;
            ASCIIEncoding encoding = new ASCIIEncoding();
            _type = CommandType.LIDoutputstate;
            _data = new LIDoutputstate();
            _data.stx = 0x02;
            sTemp = "sRN ";
            _data.type = encoding.GetBytes(sTemp);
            sTemp = "LIDoutputstate ";
            _data.cmd = encoding.GetBytes(sTemp);
            _data.etx = 0x03;
        }
    }
}
