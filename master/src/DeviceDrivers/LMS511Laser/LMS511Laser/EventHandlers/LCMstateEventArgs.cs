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

namespace Brace.Shared.DeviceDrivers.LMS511Laser.EventHandlers
{
    public class LCMstateEventArgs : EventArgs
    {
        /// <summary>        
        ///  StatusCode  : int
        /// </summary>
        /// <remarks>
        ///  0 no Error
        ///  1 pollution warning
        ///  2 pollution error
        ///  3 fatal error
        /// </remarks>
        private int _statusCode;

        public LCMstateEventArgs(int statusCode)
        {
            this._statusCode = statusCode;
        }

        public int StatusCode
        {
            get
            {
                return _statusCode;
            }
        }
    }
}
