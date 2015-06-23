using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Brace.Shared.DeviceDrivers.LMS511Laser.Enums;
using Brace.Shared.DeviceDrivers.LMS511Laser.Helpers;

namespace Brace.Shared.DeviceDrivers.LMS511Laser.Commands
{
    /// <summary>
    /// Configure the data content for the scan         
    /// </summary>
    ///  <remarks>
    ///  LMDscandatacfg, requested   
    ///  writing datas storing struct to device
    ///  </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LMDscandatacfg
    {
        /// <summary>        
        ///  start char : 0x02    
        /// </summary>
        public byte stx;
        /// <summary>        
        ///  type : LMDscandatacfg    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] type;
        /// <summary>        
        ///  command    
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] cmd;
        /// <summary>
        /// Data channel         
        /// </summary>  
        /// <remarks>
        /// LMS1xx:
        /// Output channel 1: 01 00
        /// Output channel 2: 02 00
        /// Output channel 1+2: 03
        /// 00
        /// 10 reserved
        /// FF reserved
        /// LMS5xx:
        /// Set via Echo Filter
        /// Set this value to 0
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] data_channel0;
        /// <summary>
        /// space : 0x20         
        /// </summary>
        public byte sp0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] data_channel1;
        /// <summary>
        /// space : 0x20         
        /// </summary>
        public byte sp1;
        /// <summary>
        /// Remission data output        
        /// </summary>   
        /// <remarks>
        /// 0 no, 1 yes
        /// </remarks>      
        public byte remission;
        /// <summary>
        /// space : 0x20         
        /// </summary>
        public byte sp2;
        /// <summary>
        /// Resolution of Remission Data (LMS5xxV1.10 only 8bit)        
        /// </summary>       
        /// <remarks>
        /// 0: 8 Bit, 1: 16 Bit
        /// </remarks>///        
        public byte resolution;
        /// <summary>
        /// space : 0x20        
        /// </summary>
        public byte sp3;
        /// <summary>
        /// Unit of Remission Data       
        /// </summary>
        /// <remarks>
        /// 0 Digits
        /// </remarks>         
        public byte unit;
        /// <summary>
        /// space : 0x20        
        /// </summary>
        public byte sp4;
        /// <summary>
        /// Encoder Data        
        /// </summary>
        /// <remarks>
        /// 00 00 no Encoder
        /// 01 00 Channel 1
        /// 02 00 reserved
        /// FF 00 reserved
        /// </remarks> 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] encoder0;
        /// <summary>
        /// space : 0x20        
        /// </summary>
        public byte sp5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] encoder1;
        /// <summary>
        /// space : 0x20        
        /// </summary>
        public byte sp6;
        /// <summary>
        /// Position Values       
        /// </summary>
        /// <remarks>
        /// 0 no, 1 yes
        /// </remarks>            
        public byte position;
        /// <summary>
        /// space : 0x20        
        /// </summary>
        public byte sp7;
        /// <summary>
        /// Sends the device name      
        /// </summary>
        /// <remarks>
        /// 0 no, 1 yes
        /// </remarks>          
        public byte device_name;
        /// <summary>
        /// space : 0x20        
        /// </summary>
        public byte sp8;
        /// <summary>
        /// Saved comment     
        /// </summary>
        /// <remarks>
        /// 0 no, 1 yes
        /// </remarks>           
        public byte comment;
        /// <summary>
        /// space : 0x20        
        /// </summary>
        public byte sp9;
        /// <summary>
        /// Sends time information     
        /// </summary>
        /// <remarks>
        /// 0 no, 1 yes
        /// </remarks>            
        public byte time;
        /// <summary>
        /// space : 0x20        
        /// </summary>
        public byte sp10;         
        /// <summary>
        /// Sends the output rate.        
        /// </summary>
        /// <remarks>
        /// +1 all Scans
        /// +2 each 2.nd Scan
        /// 50000 each 50000 nd. Scan        
        /// </remarks> 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] output_rate;
        /// <summary>        
        ///  end char : 0x03    
        /// </summary>  
        public byte etx;
    }

    /// <summary>
    ///  LMDscandatacfg_class class:
    /// </summary>
    public class LMDscandatacfg_class
    {
        private CommandType _type;
        private LMDscandatacfg _data;
        /// <summary>
        ///  get Type
        /// </summary>
        public CommandType Type
        {
            get { return _type; }
        }
        /// <summary>
        ///  get, set  LMDscandatacfg type variable
        /// </summary>
        public LMDscandatacfg Data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        ///  mLMPsetscancfg_class:
        /// </summary>
        /// <param name="scancfg">select_user_level (ScanCfg type)</param>       
        /// <returns></returns>
        public LMDscandatacfg_class(short outputchannel, int remission, int resolution, int unit, short encoder, short position, short device_name, short comment, short time, short output_rate)
        {
            string sTemp;
            ASCIIEncoding encoding = new ASCIIEncoding();
            _type = CommandType.LMDscandatacfg;
            _data = new LMDscandatacfg();
            _data.stx = 0x02;
            sTemp = "sWN ";
            _data.type = encoding.GetBytes(sTemp);
            sTemp = "LMDscandatacfg ";
            _data.cmd = encoding.GetBytes(sTemp);
            //_data.data_channel0 = FunctHelper.ConvertShortToHexByteArray(outputchannel);
            _data.data_channel0 = FunctHelper.ByteToHexByteArray((byte)outputchannel);
            _data.sp0 = 0x20;
            _data.data_channel1 = new byte[2];
            _data.data_channel1[0] = (byte)0x30;
            _data.data_channel1[1] = (byte)0x30;
            _data.sp1 = 0x20;
            _data.remission = (byte)(remission + 0x30);            
            _data.sp2 = 0x20;
            _data.resolution = (byte)(resolution + 0x30);
            _data.sp3 = 0x20;
            _data.unit = (byte)(unit + 0x30);
            _data.sp4 = 0x20;
           // _data.encoder0 = FunctHelper.ConvertShortToHexByteArray(encoder);
            _data.encoder0 = FunctHelper.ByteToHexByteArray((byte)encoder);
            _data.sp5 = 0x20;
            _data.encoder1 = new byte[2];
            _data.encoder1[0] = (byte)0x30;
            _data.encoder1[1] = (byte)0x30;
            _data.sp6 = 0x20;
            _data.position = (byte)(position + 0x30);
            _data.sp7 = 0x20;
            _data.device_name = (byte)(device_name + 0x30);
            _data.sp8 = 0x20;
            _data.comment = (byte)(comment + 0x30);
            _data.sp9 = 0x20;
            _data.time = (byte)(time + 0x30);
            _data.sp10 = 0x20;
            //!!!!!!!!!!!!!!!!!!!!
            _data.output_rate = new byte[2];
            _data.output_rate[0] = 0x2B;
            _data.output_rate[1] = 0x31;
            //!!!!!!!!!!!!!!!!!!!!
            _data.etx = 0x03;
        }
    }
}
