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

namespace Brace.Shared.DeviceDrivers.LMS511Laser.Helpers
{
    /// <summary>
    /// FunctHelper, static helper class
    /// </summary>   
    public static class FunctHelper
    {
        /// <summary>
        /// Print_byteArray_Hex_ASCII methode: Byte array converted to hex ASCII string.
        /// </summary> 
        /// <param name="data">binaries data array (byte array type) </param>        
        /// <returns>Hex ASCII string</returns>
        public static string Print_byteArray_Hex_ASCII(byte[] data)
        {
            string sTemp0 = BitConverter.ToString(data, 0, data.Length).Replace("-", " ");
            string sTemp1 = " ; ";
            foreach (byte d in data)
            {
                sTemp1 += ((char)d + ",");
            }
            return sTemp0 + sTemp1;           
        }
        /// <summary>
        /// Print_byteArray_Hex methode: Byte array converted to hex ASCII string width length.
        /// </summary> 
        /// <param name="data">binaries data array (byte array type) </param>  
        /// <param name="len">length of binaries data array (int type) </param>
        /// <returns>Hex ASCII string</returns>
        public static string Print_byteArray_Hex(byte[] data, int len)
        {
           String sTempr = BitConverter.ToString(data, 0, len).Replace("-", " ");
           return sTempr;
        }
        /// <summary>
        /// ConvertShortToHexByteArray: Short value converted to hex byte array.
        /// </summary> 
        /// <param name="value">short value (short type) </param>          
        /// <returns>Hex byte array</returns>
        public static byte[] ConvertShortToHexByteArray(short value)
        {
            byte[] shortBytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(shortBytes);
            return ByteArrayToHexByteArray(shortBytes);
        }
        /// <summary>
        /// ConvertIntToHexByteArray: Integer value converted to hex byte array.
        /// </summary> 
        /// <param name="value">integer value (int type) </param>          
        /// <returns>Hex byte array</returns>
        public static byte[] ConvertIntToHexByteArray(int value)
        {
            return  ConvertUintToHexByteArray((uint)value);
        }
        /// <summary>
        /// ConvertUintToHexByteArray: unsigner integer value converted to hex byte array.
        /// </summary> 
        /// <param name="value">unsigner integer value (unsigner int type) </param>          
        /// <returns>Hex byte array</returns>
        public static byte[] ConvertUintToHexByteArray(uint value)
        {
            byte[] uintBytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(uintBytes);
            return ByteArrayToHexByteArray(uintBytes);
        }
        /// <summary>
        ///  ByteArrayToHexByteArray:  Binaries byte array converted to hex byte array.
        /// </summary> 
        /// <param name="value">binaries data array (byte array type) </param>          
        /// <returns>Hex byte array</returns>
        public static byte[] ByteArrayToHexByteArray(byte[] value)
        {
            string hex = BitConverter.ToString(value);
            hex = hex.Replace("-", "");
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetBytes(hex);           
        }
        /// <summary>
        ///  ByteArrayToHexString:  Binaries byte converted to hex byte array.
        /// </summary> 
        /// <param name="value">binaries data (byte array type) </param>          
        /// <returns>Hex byte array</returns>
        public static byte[] ByteToHexByteArray(byte value)
        {           
            return Encoding.ASCII.GetBytes(value.ToString("X2"));
        }

        public static byte[] ToBcd(short value)
        {           
            byte[] ret = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                ret[i] = (byte)(value % 10);
                value /= 10;
                ret[i] |= (byte)((value % 10) << 4);
                value /= 10;
            }
            return ret;
        }
       
        /// <summary>
        ///  ByteArrayToHexString:  Binary byte value converted to hex ASCII byte.
        /// </summary> 
        /// <param name="value">binaries data array (byte array type) </param>          
        /// <returns>Hex byte</returns>
        public static byte ASCIItoByteOne(byte value)
        {
            byte bret = 0;
            switch ((char)value)
            {
                case '0':
                    bret = 0;
                    break;
                case '1':
                    bret = 1;
                    break;
                case '2':
                    bret = 2;
                    break;
                case '3':
                    bret = 3;
                    break;
                case '4':
                    bret = 4;
                    break;
                case '5':
                    bret = 5;
                    break;
                case '6':
                    bret = 6;
                    break;
                case '7':
                    bret = 7;
                    break;
                case '8':
                    bret = 8;
                    break;
                case '9':
                    bret = 9;
                    break;
                case 'a':
                case 'A':
                    bret = 10;
                    break;
                case 'b':
                case 'B':
                    bret = 11;
                    break;
                case 'c':
                case 'C':
                    bret = 12;
                    break;
                case 'd':
                case 'D':
                    bret = 13;
                    break;
                case 'e':
                case 'E':
                    bret = 14;
                    break;
                case 'f':
                case 'F':
                    bret = 15;
                    break;
                default:
                    throw new FormatException("Unrecognized hex char " + value);

            }
            return bret;
        }
        /// <summary>
        ///  ASCIItoByte:  ASCII byte array value converted to hex  byte array.
        /// </summary> 
        /// <param name="value">ASCII data array (byte array type) </param>          
        /// <returns>Hex byte array</returns>
        public static byte[] ASCIItoByte(byte[] value)
        {
            byte[] bdatas = null;
            byte[] btmparray = null;
            bool bOdd = false;
            int len, ee;
            byte btempH, btempL;
            if ((value != null) && (value.Length > 0))
            {
                if ((value.Length == 1))
                {
                    bdatas = new byte[1];
                    bdatas[0] = ASCIItoByteOne(value[0]);
                    return bdatas;
                }
                len = value.Length;
                if ((value.Length % 2) != 0)
                { //odd ?                    
                    len++;
                    bOdd = true;
                }
                btmparray = new byte[len];
                if (bOdd)
                { //odd ? 
                    Array.Copy(value, 0, btmparray, 1, len - 1);
                    btmparray[0] = 0x30;
                }
                else
                {
                    Array.Copy(value, btmparray, len);
                }
                bdatas = new byte[len / 2];
                ee = 0;
                btempH = 0; btempL = 0;
                for (int ii = 0; ii < len; ii++)
                {
                    if ((ii % 2) != 0)
                    { //odd ?  
                        bOdd = false;
                    }
                    else
                    {
                        bOdd = true;
                    }
                    switch ((char)btmparray[ii])
                    {
                        case '0':
                            if (bOdd)
                                btempH = 0;
                            else
                                btempL = 0;
                            break;
                        case '1':
                            if (bOdd)
                                btempH = 0x10;
                            else
                                btempL = 1;
                            break;
                        case '2':
                            if (bOdd)
                                btempH = 0x20;
                            else
                                btempL = 2;
                            break;
                        case '3':
                            if (bOdd)
                                btempH = 0x30;
                            else
                                btempL = 3;
                            break;
                        case '4':
                            if (bOdd)
                                btempH = 0x40;
                            else
                                btempL = 4;
                            break;
                        case '5':
                            if (bOdd)
                                btempH = 0x50;
                            else
                                btempL = 5;
                            break;
                        case '6':
                            if (bOdd)
                                btempH = 0x60;
                            else
                                btempL = 6;
                            break;
                        case '7':
                            if (bOdd)
                                btempH = 0x70;
                            else
                                btempL = 7;
                            break;
                        case '8':
                            if (bOdd)
                                btempH = 0x80;
                            else
                                btempL = 8;
                            break;
                        case '9':
                            if (bOdd)
                                btempH = 0x90;
                            else
                                btempL = 9;
                            break;
                        case 'a':
                        case 'A':
                            if (bOdd)
                                btempH = 0xa0;
                            else
                                btempL = 10;
                            break;
                        case 'b':
                        case 'B':
                            if (bOdd)
                                btempH = 0xb0;
                            else
                                btempL = 11;
                            break;
                        case 'c':
                        case 'C':
                            if (bOdd)
                                btempH = 0xc0;
                            else
                                btempL = 12;
                            break;
                        case 'd':
                        case 'D':
                            if (bOdd)
                                btempH = 0xd0;
                            else
                                btempL = 13;
                            break;
                        case 'e':
                        case 'E':
                            if (bOdd)
                                btempH = 0xe0;
                            else
                                btempL = 14;
                            break;
                        case 'f':
                        case 'F':
                            if (bOdd)
                                btempH = 0xf0;
                            else
                                btempL = 15;
                            break;
                        default:
                            throw new FormatException("Unrecognized hex char " + btmparray[ii]);                          
                    }
                    if (!bOdd)
                    {
                        bdatas[ee] = (byte)(btempH | btempL);
                        ee++;
                    }
                }
            }
            return bdatas;
        }
        /// <summary>
        /// ConvertToUint: Unsigned integer value converted to hex byte array.
        /// </summary> 
        /// <param name="value">biníries data array </param>          
        /// <returns>Unsigbned int</returns>
        public static uint ConvertToUint(byte[] bdata)
        {
            if (bdata.Length == 1)
            {
                return (uint)bdata[0];
            }
            byte[] bdatas;
            if ((bdata.Length != 4))
            {
                bdatas = new byte[4];
                Array.Copy(bdata, 0, bdatas, 4 - bdata.Length, bdata.Length);
            }
            else
                bdatas = bdata;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bdatas); //need the bytes in the reverse order           
            return BitConverter.ToUInt32(bdatas, 0);
        }
        /// <summary>
        /// ConvertToInt: hex byte array converted to Integer value converted.
        /// </summary> 
        /// <param name="bdata">hex byte array </param>          
        /// <returns>integer</returns>
        public static int ConvertToInt(byte[] bdata)
        {
            if (bdata.Length == 1)
            {
                return (int)bdata[0];
            }
            byte[] bdatas;
            if ((bdata.Length != 4))
            {
                bdatas = new byte[4];
                Array.Copy(bdata, 0, bdatas, 4 - bdata.Length, bdata.Length);
            }
            else
                bdatas = bdata;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bdatas); //need the bytes in the reverse order           
            return BitConverter.ToInt32(bdatas, 0);
        }

        public static ushort ConvertToUShort(byte[] bdata)
        {
            if (bdata.Length == 1)
            {
                return (ushort)bdata[0];
            }
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bdata); //need the bytes in the reverse order           
            return BitConverter.ToUInt16(bdata, 0);
        }

        public static short ConvertToShort(byte[] bdata)
        {
            if (bdata.Length == 1)
            {
                return (short)bdata[0];
            }
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bdata); //need the bytes in the reverse order           
            return BitConverter.ToInt16(bdata, 0);
        }

        public static float ConvertToFloat(byte[] bdata)
        {
            if (bdata.Length == 1)
            {
                return (float)bdata[0];
            }
            byte[] bdatas;
            if ((bdata.Length != 4))
            {
                bdatas = new byte[4];
                Array.Copy(bdata, 0, bdatas, 4 - bdata.Length, bdata.Length);
            }
            else
                bdatas = bdata;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bdatas); //need the bytes in the reverse order           
            return (float)BitConverter.ToSingle(bdatas, 0);
        }
    }
}
