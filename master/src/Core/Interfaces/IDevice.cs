/* This file is part of *Core*.
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

using Brace.Shared.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brace.Shared.Diagnostics.Trace;

namespace Brace.Shared.Core.Interfaces
{
    public interface IDevice
    {
        /// <summary>
        /// Initialize the device driver sending the serialized configuration data
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="traceWrapper">Tracer to use for logging.</param>
        void Initialize(string configuration, TraceWrapper traceWrapper);

        /// <summary>
        /// Start the device driver
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the device driver
        /// </summary>
        void Stop();

        /// Re-boot the device driver
        /// </summary>
        void Reboot();

        /// <summary>
        /// Return all monitored attribute value
        /// </summary>
        /// <returns></returns>
        IDeviceStatus GetDeviceStatus();

        /// <summary>
        /// Return monitored attribute value changes
        /// </summary>
        event EventHandler<StatusChangeEventArgs> StatusChange;
        
    }
}
