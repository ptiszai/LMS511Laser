/* This file is part of *DeviceDriverFactory*.
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

namespace Brace.Shared.DeviceDrivers.DeviceDriverFactory.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class InvalidDeviceTypeException : Exception
    {
        #region Constructor Logics

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDeviceTypeException"/> class.
        /// </summary>
        public InvalidDeviceTypeException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDeviceTypeException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidDeviceTypeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDeviceTypeException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public InvalidDeviceTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDeviceTypeException"/> class.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        /// <param name="context">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        public InvalidDeviceTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion Constructor Logics
    }
}


