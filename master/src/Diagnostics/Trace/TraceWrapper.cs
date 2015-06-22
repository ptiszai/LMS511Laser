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
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    public class TraceWrapper
    {
        private LogWriter logWriter;

        public event EventHandler WriteTraceEventHandler;
     
        protected virtual void OnWriteTrace(EventArgs e)
        {
            //Raise the Tick event (see below for an explanation of this)
            var writeTrace = WriteTraceEventHandler;
            if (writeTrace != null)
                writeTrace(this, e);
        }

        public TraceWrapper()
        {
            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            string logWriterMessage = logWriter == null ? "No logwriter configured. Log to console." : "";
            WriteInformation(string.Format("Trace engine (version {0}) initialized. {1}", myFileVersionInfo.FileVersion, logWriterMessage));
        }
		

        public TraceWrapper(LogWriter logWriter) : base()
        {
            this.logWriter = logWriter;
        }

        public void WriteInformation(string message)
        {
            WriteTrace(TraceEventType.Information, message);
        }

        public void WriteInformation(string message, ICollection<string> categories)
        {
            WriteTrace(TraceEventType.Information, message, categories);
        }


        public void WriteVerbose(string message)
        {
            WriteTrace(TraceEventType.Verbose, message);
        }

        public void WriteVerbose(string message, ICollection<string> categories)
        {
            WriteTrace(TraceEventType.Verbose, message, categories);
        }


        public void WriteWarning(string message)
        {
            WriteTrace(TraceEventType.Warning, message);
        }

        public void WriteWarning(string message, ICollection<string> categories)
        {
            WriteTrace(TraceEventType.Warning, message, categories);
        }

        public void WriteError(string message)
        {
            WriteTrace(TraceEventType.Error, message);
        }

        public void WriteError(Exception ex)
        {
            WriteTrace(TraceEventType.Error, ex.ToString());
        }

        public void WriteError(Exception ex, ICollection<string> categories)
        {
            WriteTrace(TraceEventType.Error, ex.ToString(), categories);
        }

        public void WriteTrace(TraceEventType eventType, string message)
        {
            WriteTrace(eventType, message, new List<string>() { "General" });
        }

        public void WriteTrace(TraceEventType eventType, string message, ICollection<string> categories)
        {
            if (logWriter != null)
            {
                LogEntry logEntry = new LogEntry()
                {
                    Message = message,
                    Severity = eventType,
                    Categories = categories
                };
                logWriter.Write(logEntry);
            }

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + message);

            #if TRACE
                Trace.WriteLine("<TRACE> "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + message);
            #endif
			
			OnWriteTrace(new TraceEventArgs() { Message = message, EventType = eventType });
        }

        public StackTracer GetStackTracer(TraceEventType eventType, string message)
        {
            return new StackTracer(eventType, WriteTrace, message);
        }
   
    }
}
