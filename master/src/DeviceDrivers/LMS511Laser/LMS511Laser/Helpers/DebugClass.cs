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

#define DEBUG_CONSOLE
#undef DEBUG_LFILE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Brace.Shared.DeviceDrivers.LMS511Laser.Helpers
{
    /// <summary>
    /// DebugClass  class
    /// </summary>
    /// <remarks>    
    /// Debug messages wrote to the console or file.
    /// #define DEBUG_CONSOLE : to console
    /// #define LFILE  : to log file
    /// </remarks>   
    internal class DebugClass
    {
        /// <summary>
        /// Log file name string
        /// </summary>
        static internal string log_file_name { get; set; }

        static private string strFullpath = "@C:\\Users\\Public\\FalconEye";
        /// <summary>
        /// DebugMessage methode.
        /// </summary
        /// <param name="message">The first message string.</param>
        /// <param name="otherValues">More other message list string</param>
        /// <returns></returns>     
        static internal void DebugMessage(string message, params object[] otherValues)
        {
            string result =  String.Format(message, new[] { message }.Concat(otherValues).ToArray<object>());
            ToConsole(result);
            ToFile(result);
        }
        /// <summary>
        /// To File methode, if it is DEBUG_LFILE define. Wrote to file.
        /// </summary
        /// <param name="result">The message string.</param>      
        /// <returns></returns>
        /// 
        [Conditional("DEBUG_LFILE")]
        static private void ToFile(string result)
        {          
            if (!Directory.Exists(strFullpath))
                Directory.CreateDirectory(strFullpath);
            using (StreamWriter writer = new StreamWriter("c:\\Users\\Public\\FalconEye\\" + log_file_name))
            {               
                writer.WriteLine(DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss : ") + result);             
            }
        }
        /// <summary>
        /// DebugMessage methode, if it is DEBUG_CONSOLE define. Wrote to console.
        /// </summary
        /// <param name="message">The message string.</param>        
        /// <returns></returns>
        [Conditional("DEBUG_CONSOLE")]
        static private void ToConsole(string result)
        {
            Console.WriteLine(result); 
        }
    }
}
