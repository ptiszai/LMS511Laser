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
    /// Run struct requested
    /// </summary> 
    /// <remarks>
    ///  Set to run mode.
    ///  Exmaple:
    ///  Parameterize the scan
    ///  1. Log in: sMN SetAccessMode
    ///  2. Set Frequency and Resolution: sMN mLMPsetscancfg
    ///  3. Configure scan data content: sWN LMDscandatacfg
    ///  4. Configure scan data output: sWN LMPoutputRange
    ///  5. Store Parameters: sMN mEEwriteall
    ///  6. Log out: sMN Run
    ///  7. Request Scan: sRN LMDscandata / sEN LMDscandata
    /// </remarks>   
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Run
    {
        /// <summary>        
        ///  start char : 0x02    
        /// </summary>
        public byte stx;
        /// <summary>        
        ///  type:Run     
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] type;
        /// <summary>        
        ///  command    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] cmd;
        /// <summary>        
        ///  end char : 0x03    
        /// </summary>
        public byte etx;
    }

    /*   /// <summary>
       /// Run struct readed
       /// </summary>  
       public struct Run_R
       {
           /// <summary>
           /// runCode: int.        
           /// </summary> 
           /// <remarks>
           ///  0: Success
           ///  1: Error
           /// </remarks> 
           public int runCode;
       }*/

    /// <summary>
    /// Run class
    /// </summary>  
    public class Run_class
    {
        private CommandType _type;
        private Run _data;
        /// <summary>
        ///  get, set  Type
        /// </summary>
        public CommandType Type
        {
            get { return _type; }
        }
        /// <summary>
        ///  get, set  Run type variable
        /// </summary>
        public Run Data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        ///  Construction
        /// </summary>
        public Run_class()
        {
            string sTemp;
            ASCIIEncoding encoding = new ASCIIEncoding();
            _type = CommandType.Run;
            _data = new Run();
            _data.stx = 0x02;
            sTemp = "sMN ";
            _data.type = encoding.GetBytes(sTemp);
            sTemp = "Run";
            _data.cmd = encoding.GetBytes(sTemp);
            _data.etx = 0x03;
        }
    }
}
