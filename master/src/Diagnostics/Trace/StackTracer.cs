/* This file is part of *Trace*.
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
namespace Brace.Shared.Diagnostics.Trace
{
    using System;
    using System.Diagnostics;

    public class StackTracer : IDisposable
    {
        private TraceEventType eventType;
        private DateTime startTime;
        private string message;
        private Action<TraceEventType, string> logWriter;

        public StackTracer(TraceEventType eventType, Action<TraceEventType, string> logWriter, string message)
        {
            this.eventType = eventType;
            this.message = message;
            this.logWriter = logWriter;
            startTime = DateTime.Now;
            logWriter(eventType, message+" START");
        }

        public void Dispose()
        {
            logWriter(eventType, message + " FINISHED "+(DateTime.Now-startTime).TotalMilliseconds.ToString());
        }
    }
}
