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

namespace Brace.Shared.DeviceDrivers.LMS511Laser.Counters
{
    public static class ErrorStateCounter
    {
        #region  constans 
        public static  int LIMITREPEATEDNUMBER = 3; 
        #endregion
        #region Remembering setting  properties
        public static uint FullErrorCounter
        {
            get { return LMS511Laser.Properties.Settings.Default.FullErrorCounter; }
            set { LMS511Laser.Properties.Settings.Default.FullErrorCounter = value; LMS511Laser.Properties.Settings.Default.Save(); }
        }
        public static uint FullSopasErrorCounter
        {
            get { return LMS511Laser.Properties.Settings.Default.FullSopasErrorCounter; }
            set { LMS511Laser.Properties.Settings.Default.FullSopasErrorCounter = value; LMS511Laser.Properties.Settings.Default.Save(); }
        }
        public static uint FullConnectedCounter
        {
            get { return LMS511Laser.Properties.Settings.Default.FullConnectedCounter; }
            set { LMS511Laser.Properties.Settings.Default.FullConnectedCounter = value; LMS511Laser.Properties.Settings.Default.Save(); }
        }
              
    /*    public uint FullDisconnectedCounter
        {
            get { return LMS511Laser.Properties.Settings.Default.FullDisconnectedCounter; }
            set { LMS511Laser.Properties.Settings.Default.FullDisconnectedCounter = value; LMS511Laser.Properties.Settings.Default.Save(); }
        }*/
        #endregion
        #region  error flags 
        public static uint SoapErrorCounterFlag { get; set; }
        public static string SoapErrorMsg { get; set; }
        public static uint ReceivedSendedErrorCounterFlag { get; set; }
        public static string ReceivedSendedErrorMsg { get; set; }
        #endregion

        #region public methods
        public static void ClearCounterFlags()
        {
            SoapErrorCounterFlag = 0;
            SoapErrorMsg = "";
            ReceivedSendedErrorCounterFlag = 0;
            ReceivedSendedErrorMsg = "";
        }
        public static void ClearLMS511LaserPropertiesSettings()
        {
            FullErrorCounter = 0;
            FullConnectedCounter = 0;
            FullSopasErrorCounter = 0;
        }

        public static void PrintPropertiesSettings()
        {
            Console.WriteLine("FullErrorCounter: " + FullErrorCounter);
            Console.WriteLine("FullSopasErrorCounter: " + FullSopasErrorCounter);
            Console.WriteLine("FullConnectedCounter: " + FullConnectedCounter);            
        }
        #endregion


    }
}
