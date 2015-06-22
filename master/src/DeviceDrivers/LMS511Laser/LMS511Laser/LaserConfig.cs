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
using Brace.Shared.Core.Configuration;


namespace Brace.Shared.DeviceDrivers.LMS511Laser
{
    public class LaserConfig : BaseDeviceConfiguration
    {
        /// <summary>
        /// LaserConfig  class
        /// </summary>
        /// <remarks>    
        /// SICK laser driver configuration class
        /// </remarks>          
            #region  public property
            /// <summary>
            /// IP address string
            /// </summary>
            public string IPAdrress { get; set; }
            /// <summary>
            /// Port number integer
            /// </summary>
    //        public int port { get; set; }
            /// <summary>
            /// Maintenance password
            /// </summary>
            ///  <remarks> 
            ///  4 hexabyte, example:B21ACE26
            ///  </remarks>  
            public uint PWD_maintenance { get; set; }
            /// <summary>
            /// Authorized client password
            /// </summary>
            ///  <remarks> 
            ///  4 hexabyte, example:F4724744
            ///  </remarks>  
            public uint PWD_authorized_client { get; set; }
            /// <summary>
            /// Service password
            /// </summary>
            ///  <remarks> 
            ///  4 hexabyte, example:81BE23AA
            ///  </remarks>  
            public uint PWD_service { get; set; }
            /// <summary>
            /// Triggered output number
            /// </summary>
            ///  <remarks> 
            ///  numver is may be 1 - 6.
            ///  </remarks>  
            public int TriggerOutputChannelNumber { get; set; } 
            #endregion

            #region  public methodes
            /// <summary>
            /// Saves config datas to XML file
            /// </summary>
            public string SaveToXml()
            {
                return base.SaveToXml<LaserConfig>();
            }
            /// <summary>
            /// Sets the properties to their default values.        
            /// </summary>
            /// <remarks>    
            /// PWD_maintenance: "main"
            /// PWD_authorized_client : "client"
            /// PWD_service: "Service"
            /// </remarks>   
            public void SetDefaults()
            {
                IPAdrress = "192.168.0.66";
             //   port = 2111; // or 2112
                PWD_maintenance = 0xB21ACE26;
                PWD_authorized_client = 0xF4724744;
                PWD_service = 0x81BE23AA;
                TriggerOutputChannelNumber = 1; // 1. outport
            }
            #endregion
        }    
}
