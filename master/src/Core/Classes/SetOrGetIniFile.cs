/* This file is part of *LMS511Laser*.
Copyright (C) 2015 Tiszai Istvan, tiszaii@hotmail.com

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
using System.Runtime.InteropServices;

static public class SetOrGetIniFile
{
    [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW",
        SetLastError = true,
        CharSet = CharSet.Unicode, ExactSpelling = true,
        CallingConvention = CallingConvention.StdCall)]
    private static extern int GetPrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpDefault,
        string lpReturnString,
        int nSize,
        string lpFilename);

    [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW",
        SetLastError = true,
        CharSet = CharSet.Unicode, ExactSpelling = true,
        CallingConvention = CallingConvention.StdCall)]
    private static extern int WritePrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpString,
        string lpFilename);

    public static void SetIniFileString(string iniFile, string category, string key, string value)
    {
        WritePrivateProfileString(category, key, value, iniFile);
    }

    /// <summary>
    /// Gets the content.
    /// </summary>
    /// <param name="iniFile">The ini file.</param>
    /// <param name="category">The category.</param>
    /// <param name="key">The key.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static string GetIniFileString(string iniFile, string category, string key, string defaultValue)
    {
        string returnString = new string(' ', 1024);
        GetPrivateProfileString(category, key, defaultValue, returnString, 1024, iniFile);
        return returnString.Split('\0')[0];
    }

    /// <summary>
    /// Gets the keys.
    /// </summary>
    /// <param name="iniFile">The ini file.</param>
    /// <param name="category">The category.</param>
    public static List<string> GetKeys(string iniFile, string category)
    {
        string returnString = new string(' ', 32768);
        GetPrivateProfileString(category, null, null, returnString, 32768, iniFile);
        List<string> result = new List<string>(returnString.Split('\0'));
        result.RemoveRange(result.Count - 2, 2);
        return result;
    }

    /// <summary>
    /// Gets the categories.
    /// </summary>
    /// <param name="iniFile">The ini file.</param>
    /// <returns></returns>
    public static List<string> GetCategories(string iniFile)
    {
        string returnString = new string(' ', 65536);
        GetPrivateProfileString(null, null, null, returnString, 65536, iniFile);
        List<string> result = new List<string>(returnString.Split('\0'));
        result.RemoveRange(result.Count - 2, 2);
        return result;
    }
}
