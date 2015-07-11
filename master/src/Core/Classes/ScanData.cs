using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brace.Shared.Core.Classes
{
    public class ScanData
    {
        /// <summary>        
        ///  versionNumber : For detecting format changes by the version. Version is always 1 up to now.    
        /// </summary>
        public ushort versionNumber;
        /// <summary>        
        ///  deviceNumber :defined with Sopas    
        /// </summary>
        public ushort deviceNumber;
        /// <summary>        
        ///  serialNumber :defined in Factory  
        /// </summary>
        public uint serialNumber;
        /// <summary>        
        ///  deviceStatus : 00 00 OK;00 01 Error;00 02 Pollution Warning;00 04 Pollution Error
        /// </summary>      
        public byte deviceStatus;
        /// <summary>        
        ///  telegramCounter : Counter starting with first measured value after reaching the highest number  
        /// </summary>
        public ushort telegramCounter;
        /// <summary>        
        ///  scanCounter : Counter starting with first measured value after reaching the highest number 
        /// </summary>
        public ushort scanCounter;
        /// <summary>        
        ///  timeSinceStartUp : Counting the time since power up the device; starting with 0. In the output telegram this is the time at the zero index (-14°) before the measurement itself starts.
        /// </summary>
        public uint timeSinceStartUp;
        /// <summary>        
        ///   timeOfTransmission : /Time in μs when the complete scan is transmitted to the buffer for data output; starting with 0 at scanner boot up.
        /// </summary>
        public uint timeOfTransmission;
        /// <summary>        
        /// statusOfDigitalInputs : Low byte represents Input 1; 00 00 all Inputs low 00 03 all input high    
        /// </summary>       
        public ushort statusOfDigitalInputs;
        /// <summary>        
        /// statusOfDigitalOutputs : Low byte represents Output 1; 00 00 all Outputs low 00 07 all Output high  
        /// </summary>       
        public ushort statusOfDigitalOutputs;
        /// <summary>        
        /// scanFrequency :  Output in 1/100Hz
        /// </summary>
        /// <remarks>    
        /// LMS1xx:
        ///  25Hz: 9C4h (2500d)
        ///  50Hz: 1388h (5000d)
        /// LMS5xx: 
        ///  25Hz: 9C4h (2500d)
        ///  35Hz: DACh (3500d)
        ///  50Hz: 1388h (5000d)
        ///  75Hz: 1A0Bh (7500d)
        ///  100Hz: 2710h (10000d)
        /// </remarks> 
        public uint scanFrequency;
        /// <summary>        
        /// measurementFrequency : Inverse of the time between two   measurement shots (in 100Hz) example: 50Hz, 0,5° Resolution  720 shots/20ms  36 kHz
        /// </summary>
        public uint measurementFrequency;
        /// <summary>        
        /// amountOfEncoder : 0..3 if 0, than next two values are missing
        /// </summary>
        public ushort amountOfEncoder;      // 0..3 if 0, than next two values are missing
        /// <summary>        
        /// encoderPosition : Info in Ticks; LMS1xx: 0000h - 3FFFh;  LMS5xx: 0000h - FFFFh
        /// </summary>
        public ushort encoderPosition;
        /// <summary>        
        /// encoderSpeed : Ticks/mm
        /// </summary>
        public ushort encoderSpeed;
        /// <summary>        
        /// amountOf16BitChannels : Amount of 16 Bit channels, giving out the Measured Data; LMS1xx: 1..2 Output channels; LMS5xx: 0 or 5 Output channels
        /// </summary>
        public ushort amountOf16BitChannels;
        /// <summary>        
        /// content16 : Defines the Content of the Output channel
        /// </summary>
        /// <remarks> 
        ///  LMS1xx:
        ///    DIST1: radial Values of first pulse in mm
        ///    RSSI1:Energy Values of first pulse
        ///    DIST2: radial Values of 2nd pulse in mm
        ///    RSSI2:Energy Values of 2nd pulse
        ///  LMS5xx:
        ///    DIST1
        ///    DIST2
        ///    DIST3
        ///    DIST4
        ///    DIST5
        ///    No RSSI Values
        /// </remarks>         
        public string content16;
        /// <summary>        
        /// scaleFactor16 : scale factor or of measurement value 
        /// </summary>                  
        public float scaleFactor16;
        /// <summary>        
        /// scaleFactorOffset16 : LMS = 0 
        /// </summary> 
        public float scaleFactorOffset16;
        /// <summary>        
        /// startAngle16 : Output format : 1/10.000° 
        /// </summary>
        /// <remarks>
        /// LMS1xx:
        ///    -450.000 +2250.000
        /// LMS5xx:
        ///    -50.000 +1850.000  
        /// </remarks>
        public uint startAngle16;
        /// <summary>        
        /// steps16 : Output format : 1/10.000° 
        /// </summary> 
        /// <remarks>
        /// LMS1xx: 1000 10.000
        /// LMS5xx: 1667..10.000
        /// </remarks>
        public ushort steps16;
        /// <summary>        
        /// amountOfData16 : Defines the number of items on measured output 
        /// </summary>        
        public ushort amountOfData16;
        /// <summary>        
        /// data16 :  Data stream starting Data_1 to Data_n, defines number of items on measured output.
        /// </summary> 
        /// <remarks>
        ///  0000h - 4E20 (LMS100)
        ///  C350 (LMS150)
        ///  FDE8 (LMS 1xx without limit)
        /// </remarks>      
        public ushort[] data16;
        /// <summary>        
        /// amountOf8BitChannels : Amount of 8 Bit channels, 1...4O Output channels
        /// </summary>        
        public ushort amountOf8BitChannels;
        /// <summary>        
        /// content8 : Defines the Content of the Output channel   
        /// </summary>
        /// <remarks>
        /// LMS1xx:
        ///  DIST1: 
        ///  RSSI1:
        ///  DIST2: 
        ///  RSSI2:
        /// LMS5xx:
        ///  DIST1
        ///  DIST2
        ///  DIST3
        ///  DIST4
        ///  DIST5
        ///  No RSSI Values
        /// </remarks>
        public string content8;
        /// <summary>        
        /// scaleFactor8 :  scale factor or of measurement value 
        /// </summary>             
        public float scaleFactor8;
        /// <summary>        
        /// scaleFactorOffset8 : LMS = 0 
        /// </summary>  
        public float scaleFactorOffset8;
        /// <summary>        
        /// startAngle8 : Output format : 1/10.000°
        /// </summary> 
        /// <remarks>
        /// LMS1xx:
        ///    -450.000 +2250.000
        /// LMS5xx:
        ///    -50.000 +1850.000
        /// </remarks>
        public uint startAngle8;
        /// <summary>        
        /// startAngle8 : Output format : 1/10.000°
        /// </summary> 
        /// <remarks>
        /// LMS1xx: 1000 10.000
        /// LMS5xx: 1667..10.000
        /// </remarks>
        public ushort steps8;
        /// <summary>        
        /// amountOfData8 : Defines the number of items on measured output
        /// </summary>       
        public ushort amountOfData8;
        /// <summary>        
        /// data8 : Data stream starting Data_1 to Data_n, defines number of items on measured output.
        /// </summary>     
        public byte[] data8;
        /// <summary>        
        /// position :Output of Position data; 0 no position Data
        /// </summary> 
        public ushort position;
        /// <summary>        
        /// nameMode : Device Name mode; 00 00 no name, 00 01 name
        /// </summary>
        public ushort nameMode;
        /// <summary>        
        /// nameLength : Length of Name
        /// </summary>
        public byte nameLength;
        /// <summary>        
        /// name : Device Name
        /// </summary>
        public string name;
        /// <summary>        
        /// comment : 0 no Comment
        /// </summary>
        public ushort comment;
        /// <summary>        
        /// timeMode : transmits a time stamp; 00 00 no time, 00 01 time
        /// </summary>
        public ushort timeMode;
        /// <summary>        
        /// timeYear
        /// </summary> 
        public ushort timeYear;
        /// <summary>        
        /// timeMonth
        /// </summary> 
        public byte timeMonth;
        /// <summary>        
        /// timeDay
        /// </summary> 
        public byte timeDay;
        /// <summary>        
        /// timeHour
        /// </summary> 
        public byte timeHour;
        /// <summary>        
        /// timeMinute
        /// </summary> 
        public byte timeMinute;
        /// <summary>        
        /// timeSecund
        /// </summary> 
        public byte timeSecund;
        /// <summary>        
        /// timeUsecund : micro secundum
        /// </summary> 
        public uint timeUsecund;       
    }
}
