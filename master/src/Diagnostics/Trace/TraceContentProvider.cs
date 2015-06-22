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

using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brace.Shared.Diagnostics.Trace
{
    public class TraceContentProvider
    {
        private List<LogEntry> lstFreshTrace;
        private List<LogEntry> lstFreshTraceErrors;

        private int errorsNumber = 5;

        /// <summary>
        /// Gets or sets how many error will be stored.
        /// </summary>
        public int ErrorsNumber
        {
            get { return errorsNumber; }
            set { errorsNumber = value; }
        }

        private int infoNumber = 15;
        /// <summary>
        /// Gets or sets how many trace messaes will be stored.
        /// </summary>
        public int InfoNumber
        {
            get { return infoNumber; }
            set { infoNumber = value; }
        }

        private readonly object lockObject = new object();

        public TraceContentProvider()
        {
            lstFreshTrace = new List<LogEntry>();
            lstFreshTraceErrors = new List<LogEntry>();
        }

        public void AddNewItemToTraceList(TraceEventType eventType, string message)
        {

            LogEntry leNewTraceItem = new LogEntry();
            leNewTraceItem.Message = message;
            leNewTraceItem.Severity = eventType;
            leNewTraceItem.TimeStamp = DateTime.Now;

            lock (lockObject)
            {
                lstFreshTrace.Add(leNewTraceItem);
            
                if ( eventType == TraceEventType.Critical || eventType == TraceEventType.Error || eventType == TraceEventType.Warning )
                {
                    lstFreshTraceErrors.Add(leNewTraceItem);
                }
        
                if ( lstFreshTraceErrors.Count > errorsNumber )
                {
                    lstFreshTraceErrors.RemoveAt(0);
                }
                if (lstFreshTrace.Count > infoNumber)
                {
                    lstFreshTrace.RemoveAt(0);
                }
            }
        }

        //private static bool findErrors(LogEntry le)
        //{
        //    if (le.Severity == TraceEventType.Error)
        //        return true;
        //    return false;
        //}

        public string[] GetLastErrors()
        {
            List<string> lstErrors = new List<string>();

            lock (lockObject)
            {
                for (int i = 0; i < lstFreshTraceErrors.Count; ++i)
                {
                    //if (lstFreshTrace[i].Severity == TraceEventType.Error)
                    //{
                    lstErrors.Add(lstFreshTraceErrors[i].ToString());
                    //}
                }
            }

            return lstErrors.ToArray();  
        }

        public LogEntry[] GetLastErrorsLe()
        {
            return lstFreshTraceErrors.ToArray();
        }

        public string[] GetLastInfos()
        {
            List<string> lstInfos = new List<string>();

            lock (lockObject)
            {
                for (int i = 0; i < lstFreshTrace.Count; ++i)
                {
                    lstInfos.Add(lstFreshTrace[i].ToString());
                }
            }

            return lstInfos.ToArray();
        }

        public LogEntry[] GetLastInfosLe()
        {
            return lstFreshTrace.ToArray();
        }

    }
}
