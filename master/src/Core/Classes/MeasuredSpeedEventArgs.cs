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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brace.Shared.Core.Classes
{
    public class MeasuredSpeedEventArgs : EventArgs
    {
        private DateTime measurementDate;
        private float speed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="measurementDate">Date of measurement</param>
        /// <param name="speed">Measured speed in km/h</param>
        public MeasuredSpeedEventArgs(DateTime measurementDate, float speed)
        {
            this.measurementDate = measurementDate;
            this.speed = speed;
        }

        /// <summary>
        /// Date of measurement
        /// </summary>
        public DateTime MeasurementDate 
        {
            get
            {
                return this.measurementDate;
            }
        }

        /// <summary>
        /// Measured speed in km/h
        /// </summary>
        public float Speed 
        {
            get
            {
                return this.speed;
            }
        }
    }
}
