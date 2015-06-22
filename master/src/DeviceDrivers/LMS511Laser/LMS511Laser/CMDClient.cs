 
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;
using Candid.FalconEye.Shared.Interfaces;
using Candid.FalconEye.Shared.Interfaces.DeviceInterfaces;


namespace Candid.Shared.Drivers.Laser
{
    /// <summary>
    /// CMDClient  class
    /// </summary>
    /// <remarks>    
    /// Commands of laser executed class
    /// </remarks> 
    public class CMDClient : ILaser
    {       
        #region Variable private
        private Socket clientSocket;
        private NetworkStream networkStream;
        private BackgroundWorker bwReceiver;
        private IPEndPoint _serverEP;
        //private string _networkName;
        private Semaphore _semaphor;
       // private Semaphore _semaphor_R;
        private byte[] _rbuffer;
        private byte[][] _rdatas;
        private readonly string[] _errorName = { "No error", "Wrong uselevel, access to method not allowed", "Try to access a variable with an unknown Sopas index",
                                                 "Try to access a variable with an unknown Sopas index", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13","14","15", "16" };
        #endregion

        #region EventHandler part
        /// <summary>
        /// SCdevicestat  event
        /// </summary>
        public event EventHandler<SCdevicestateEventArgs> SCdevicestate_CMD;
        /// <summary>
        /// Sopas_Error  event
        /// </summary>
        public event EventHandler<string> Sopas_Error_CMD;
        /// <summary>
        /// DeviceIdent event
        /// </summary>       
        public event EventHandler<DeviceIdentEventArgs> DeviceIdent_CMD;
        /// <summary>
        /// Run event
        /// </summary> 
        public event EventHandler<RunEventArgs> Run_CMD;
        public event Action<LMDscandata_R> LMDscandata_CMD;
        public event Action<LMDscandata_E_R> LMDscandata_E_CMD;
       
        public event Action<SetAccessMode_R> SetAccessMode_CMD;
        public event Action<mLMPsetscancfg_R> mLMPsetscancfg_CMD;
        public event Action<LMPscancfg_R> LMPscancfg_CMD;
        public event Action<LMPoutputRange_R> LMPoutputRange_CMD;
        public event Action<LMPoutputRange_get_R> LMPoutputRange_get_CMD;             
        public event Action<LCMstate_R> LCMstate_CMD;
        #endregion

        #region Variable property
        public bool Connected
        {
            get
            {
                if ( this.clientSocket != null )
                    return this.clientSocket.Connected;
                else
                    return false;
            }
        }        
        public IPAddress ServerIP
        {
            get
            {
                if ( this.Connected )
                    return _serverEP.Address;
                else
                    return IPAddress.None;
            }
        }
        public int ServerPort
        {
            get
            {
                if ( this.Connected )
                    return _serverEP.Port;
                else
                    return -1;
            }
        }        
        public IPAddress IP
        {
            get
            {
                if ( this.Connected )
                    return ( (IPEndPoint)this.clientSocket.LocalEndPoint ).Address;
                else
                    return IPAddress.None;
            }
        }        
        public int Port
        {
            get
            {
                if ( this.Connected )
                    return ( (IPEndPoint)this.clientSocket.LocalEndPoint ).Port;
                else
                    return -1;
            }
        }       
    /*    public string NetworkName
        {
            get { return _networkName; }
            set { _networkName = value; }
        }*/
        #endregion

        #region Contsructors       
        public CMDClient(IPAddress serverIP , int port/*, string netName*/)
        {              
            _semaphor = new Semaphore(1, 1);         
            _serverEP = new IPEndPoint(serverIP , port);
          //  _networkName = netName;
            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
            _rbuffer = new byte[4096];
          //  _rbuffer = new byte[1570];  
        }
        #endregion

        #region Private Methods

        private void NetworkChange_NetworkAvailabilityChanged(object sender , NetworkAvailabilityEventArgs e)
        {
            if ( !e.IsAvailable )
            {
                OnNetworkDead(new MessageEventArgs("ERROR:Laser network is DEAD!"));
                OnDisconnectedFromServer(new MessageEventArgs("ERROR:Laser disconnected from server!"));
            }
            else
                OnNetworkAlived(new MessageEventArgs("Laser network is ALIVE!"));
        }

       private void StartReceive(object sender , DoWorkEventArgs e)
        {
            byte[] pattern;
           // int readLength = _rbuffer.Length;
            bool bScanStart = false;
           int readBytes;
           int ii = 0;
           byte[] rbuffer;
          
           try
           {         
            rbuffer =  new byte[1];
            while ( this.clientSocket.Connected )
            {
                for (ii = 0; ii < _rbuffer.Length; ii++)
                {
                    readBytes = networkStream.Read(rbuffer, 0, 1);
                    if (readBytes == 0)
                        break;
                    _rbuffer[ii] = rbuffer[0];
                    if (rbuffer[0] == 0x03)
                        break;
                }
                readBytes = ii;
                if (ii != 0)
                    readBytes++;                   
                if ( readBytes == 0 )
                    break;
                DebugClass.DebugMessage("read bytes of length: ", readBytes);            
                String sTempr = BitConverter.ToString(_rbuffer, 0, readBytes).Replace("-", " ");
                DebugClass.DebugMessage("read buffer0: ",FunctHelper.Print_byteArray_Hex(_rbuffer, readBytes));               
                if ((_rbuffer[0] != 0x02) || (_rbuffer[readBytes-1] != 0x03))
                    throw new Exception("ERROR Laser: telegram frame is bad (STX or ETX)!");

                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] truncArray = new byte[readBytes-1];
                Array.Copy(_rbuffer, truncArray, truncArray.Length);

               // String sTemp = BitConverter.ToString(truncArray, 0, truncArray.Length).Replace("-", " ");                
               // Console.WriteLine("read buffer: " + sTemp);
               
                _rdatas = SeparatedToByteArray(truncArray, 0x20);
                ii = 0;              
                foreach (byte[] data in _rdatas)
                {
                    Console.WriteLine("data " + ii.ToString() + ": " + FunctHelper.Print_byteArray_Hex_ASCII(data));
                    ii++;
                }
             /*   if (ii>0)
                {
                    if (_rdatas[ii - 1].Length>0)
                        _rdatas[ii - 1][_rdatas[ii - 1].Length - 1] = 0;
                }*/
                // ERROR : sFA                  
                pattern = encoding.GetBytes("sFA");
                if (BytePatternSearch(_rdatas[0], pattern, 0) >= 0)
                { // sFA
                    if (Sopas_Error_CMD != null)
                    {
                        int iTemp ;
                        if ((_rbuffer[5] > 0x30) && (_rbuffer[5] < 0x39))
                            iTemp = (int)(_rbuffer[5] - 0x30);
                        else
                            iTemp = (int)(_rbuffer[5] - 55);
                        Sopas_Error_CMD(this, _errorName[iTemp]);
                        continue;
                    }
                }

               // SCdevicestate 
                pattern = encoding.GetBytes("SCdevicestate");
                if (BytePatternSearch(_rdatas[1], pattern, 0) >= 0)
                { // SCdevicestate
                   // readLength = _rbuffer.Length;
                    pattern = encoding.GetBytes("sRA");
                    if (BytePatternSearch(_rdatas[0], pattern, 0) >= 0)
                    {
                        // Console.WriteLine("rec:SCdevicestate !");
                        if (SCdevicestate_CMD != null)
                        {
                            SCdevicestateEventArgs s = new SCdevicestateEventArgs((int)_rbuffer[19] - '0');                            
                            SCdevicestate_CMD(this,s);
                            continue;
                        }
                    }
                    else
                        throw new Exception("ERROR Laser: rec:SCdevicestate is bad!");
                }               
                // DeviceIdent          
                pattern = encoding.GetBytes("sRA");
                if (BytePatternSearch(_rdatas[0], pattern, 0) >= 0)
                {
                   // readLength = _rbuffer.Length;
                    if (_rbuffer[5] == 0x30)
                    {
                        // Console.WriteLine("rec:SCdevicestate !");
                        if (DeviceIdent_CMD != null)
                        {
                            _rbuffer[readBytes - 1] = 0x0;
                            DeviceIdentEventArgs s = new DeviceIdentEventArgs(Encoding.UTF8.GetString(_rbuffer, 7, readBytes - 7));                            
                            DeviceIdent_CMD(this, s);
                            continue;
                        }
                    }
                   // else
                   //     throw new Exception("ERROR Laser: rec:DeviceIdent is bad 1!");
                }
             //   else
              //      throw new Exception("ERROR Laser: rec:DeviceIdent is bad 2 !");
               // }
                // LMDscandata  
                pattern = encoding.GetBytes("LMDscandata");
                if (BytePatternSearch(_rdatas[1], pattern, 0) >= 0)
                {
                    pattern = encoding.GetBytes("sSN");
                    if (BytePatternSearch(_rdatas[0], pattern, 0) >= 0)
                    {
                        if (bScanStart)
                        {
                            bScanStart = false;
                         //   readLength = readBytes;
                        }
                        // Console.WriteLine("rec:LMDscandata sSN!");
                        if (LMDscandata_CMD != null)
                        {
                            _rbuffer[readBytes - 1] = 0x0;
                            LMDscandata_R lmdScandata = new LMDscandata_R();
                            parser_rbuffer_to_LMDscandata_R(ref lmdScandata);
                            LMDscandata_CMD(lmdScandata);
                            continue;
                        }
                    }
                    else
                    {
                       // readLength = _rbuffer.Length;
                        pattern = encoding.GetBytes("sRA");
                        if (BytePatternSearch(_rdatas[0], pattern, 0) >= 0)
                        {
                            // Console.WriteLine("rec:LMDscandata sRA!");
                            if (LMDscandata_CMD != null)
                            {
                                _rbuffer[readBytes - 1] = 0x0;
                                LMDscandata_R lmdScandata = new LMDscandata_R();
                                parser_rbuffer_to_LMDscandata_R(ref lmdScandata);
                                LMDscandata_CMD(lmdScandata);
                                continue;
                            }
                        }
                        else
                        {
                          //  readLength = _rbuffer.Length;
                            pattern = encoding.GetBytes("sEA");
                            if (BytePatternSearch(_rdatas[0], pattern, 0) >= 0)
                            {                                
                                // Console.WriteLine("rec:LMDscandata sEA!");
                                if (LMDscandata_CMD != null)
                                {
                                    _rbuffer[readBytes - 1] = 0x0;
                                    LMDscandata_E_R lmdScandata_e = new LMDscandata_E_R();                                    
                                    lmdScandata_e.measurement = (int)_rbuffer[17] - '0';
                                    if (lmdScandata_e.measurement == 1)
                                        bScanStart = true;

                                    LMDscandata_E_CMD(lmdScandata_e);
                                    continue;
                                }
                            }
                            else
                                throw new Exception("ERROR Laser: rec:LMDscandata is bad!");
                        }
                    }
                }

                // Run 
                pattern = encoding.GetBytes("Run");
                if (BytePatternSearch(_rdatas[1], pattern, 0) >= 0)
                { // Run
                    pattern = encoding.GetBytes("sAN");
                    if (BytePatternSearch(_rdatas[0], pattern, 0) >= 0)
                    {
                        // Console.WriteLine("rec:Run !");
                        if (Run_CMD != null)
                        {
                            RunEventArgs s = new RunEventArgs((int)_rbuffer[9] - '0');
                           // s.runCode = (int)_rbuffer[9] - '0';
                            Run_CMD(this,s);
                            continue;
                        }
                    }
                    else
                        throw new Exception("ERROR Laser: rec:Run is bad!");
                }

                // SetAccessMode 
                pattern = encoding.GetBytes("SetAccessMode");
                if (BytePatternSearch(_rdatas[1], pattern, 0) >= 0)
                { // Run
                    pattern = encoding.GetBytes("sAN");
                    if (BytePatternSearch(_rdatas[0], pattern, 0) >= 0)
                    {
                        // Console.WriteLine("rec:Run !");
                        if (SetAccessMode_CMD != null)
                        {
                            SetAccessMode_R s = new SetAccessMode_R();
                            s.change_user_level = (int)_rbuffer[19] - '0';
                            SetAccessMode_CMD(s);
                            continue;
                        }
                    }
                    else
                        throw new Exception("ERROR Laser: rec:SetAccessMode is bad!");
                }

                // mLMPsetscancfg
                pattern = encoding.GetBytes("mLMPsetscancfg");
                if (BytePatternSearch(_rdatas[1], pattern, 0) >= 0)
                { // Run
                    pattern = encoding.GetBytes("sAN");
                    if (BytePatternSearch(_rdatas[0], pattern, 0) >= 0)
                    {
                        if (mLMPsetscancfg_CMD != null)
                        {
                            _rbuffer[readBytes - 1] = 0x0;
                            mLMPsetscancfg_R s = new mLMPsetscancfg_R();
                            parser_rbuffer_to_mLMPsetscancfg_R(ref s);
                            mLMPsetscancfg_CMD(s);
                            continue;
                        }
                    }
                    else
                        throw new Exception("ERROR Laser: rec:mLMPsetscancfg is bad!");
                }

                // LMPscancfg
                pattern = encoding.GetBytes("LMPscancfg");
                if (BytePatternSearch(_rdatas[1], pattern, 0) >= 0)
                { // Run
                    pattern = encoding.GetBytes("sRA");
                    if (BytePatternSearch(_rdatas[0], pattern, 0) >= 0)
                    {
                        if (LMPscancfg_CMD != null)
                        {
                            _rbuffer[readBytes - 1] = 0x0;
                            LMPscancfg_R s = new LMPscancfg_R();
                            parser_rbuffer_to_LMPscancfg_R(ref s);
                            LMPscancfg_CMD(s);
                            continue;
                        }
                    }
                    else
                        throw new Exception("ERROR Laser: rec:LMPscancfg is bad!");
                }

                // LMPoutputRange, set
                pattern = encoding.GetBytes("LMPoutputRange");
                if (BytePatternSearch(_rdatas[1], pattern, 0) >= 0)
                { 
                    pattern = encoding.GetBytes("sWA");
                    if (BytePatternSearch(_rdatas[0], pattern, 0) >= 0)
                    { // LMPoutputRange, set
                        if (LMPoutputRange_CMD != null)
                        {
                            LMPoutputRange_R s = new LMPoutputRange_R();
                            s.statusCode = 1;
                            LMPoutputRange_CMD(s);
                            continue;
                        }
                    }
                    else
                    {
                        pattern = encoding.GetBytes("sRA");
                        if (BytePatternSearch(_rdatas[0], pattern, 0) >= 0)
                        { // LMPoutputRange, get
                            if (LMPoutputRange_get_CMD != null)
                            {
                                LMPoutputRange_get_R s = new LMPoutputRange_get_R();
                                parser_rbuffer_to_LMPoutputRange_R(ref s);
                                LMPoutputRange_get_CMD(s);
                                continue;
                            }
                        }
                        else
                            throw new Exception("ERROR Laser: rec:LMPoutputRange set or get is bad!");
                    }                    
                }
                // LCMstate
                pattern = encoding.GetBytes("LCMstate");
                if (BytePatternSearch(_rdatas[1], pattern, 0) >= 0)
                { // Run
                    pattern = encoding.GetBytes("sRA");
                    if (BytePatternSearch(_rdatas[0], pattern, 0) >= 0)
                    {
                        if (LCMstate_CMD != null)
                        {
                            _rbuffer[readBytes - 1] = 0x0;
                            LCMstate_R s = new LCMstate_R();
                            s.statusCode = (int)FunctHelper.ASCCItoByteOne(_rdatas[2][0]); 
                            LCMstate_CMD(s);
                            continue;
                        }
                    }
                    else
                        throw new Exception("ERROR Laser: rec:LMPscancfg is bad!");
                }
                             
             //   OnCommandReceived(new CommandEventArgs(cmd));

            };
            }
           catch(Exception ex)
           {
               OnCommandReceivingFailed(new MessageEventArgs(ex.Message));
           }
            OnServerDisconnected(new ServerEventArgs(clientSocket));
            Reconnecting();
        }

         private void parser_rbuffer_to_LMDscandata_R(ref LMDscandata_R lmdScandata)
         {
            byte[] bdata;
            int offset = 0;
            bdata = FunctHelper.ASCCItoByte(_rdatas[2]);
            lmdScandata.versionNumber = FunctHelper.ConvertToUShort(bdata);
            bdata = FunctHelper.ASCCItoByte(_rdatas[3]);
            lmdScandata.deviceNumber = FunctHelper.ConvertToUShort(bdata);
            bdata = FunctHelper.ASCCItoByte(_rdatas[4]);
            lmdScandata.serialNumber = FunctHelper.ConvertToUint(bdata);
            lmdScandata.deviceStatus = FunctHelper.ASCCItoByteOne(_rdatas[6][0]);
            bdata = FunctHelper.ASCCItoByte(_rdatas[7]);
            lmdScandata.telegramCounter = FunctHelper.ConvertToUShort(bdata);
            bdata = FunctHelper.ASCCItoByte(_rdatas[8]);
            lmdScandata.scanCounter = FunctHelper.ConvertToUShort(bdata);
            bdata = FunctHelper.ASCCItoByte(_rdatas[9]);
            lmdScandata.timeSinceStartUp = FunctHelper.ConvertToUint(bdata);
            bdata = FunctHelper.ASCCItoByte(_rdatas[10]);
            lmdScandata.timeOfTransmission = FunctHelper.ConvertToUint(bdata);
            lmdScandata.statusOfDigitalInputs = FunctHelper.ASCCItoByteOne(_rdatas[12][0]);
            lmdScandata.statusOfDigitalOutputs = FunctHelper.ASCCItoByteOne(_rdatas[14][0]);
            bdata = FunctHelper.ASCCItoByte(_rdatas[16]);
            lmdScandata.scanFrequency = FunctHelper.ConvertToUint(bdata);
            bdata = FunctHelper.ASCCItoByte(_rdatas[17]);
            lmdScandata.measurementFrequency = FunctHelper.ConvertToUint(bdata);
            bdata = FunctHelper.ASCCItoByte(_rdatas[18]);
            lmdScandata.amountOfEncoder = FunctHelper.ConvertToUShort(bdata);
            if (lmdScandata.amountOfEncoder != 0)
            {
                bdata = FunctHelper.ASCCItoByte(_rdatas[19]);
                lmdScandata.encoderPosition = FunctHelper.ConvertToUShort(bdata);
                bdata = FunctHelper.ASCCItoByte(_rdatas[20]);
                lmdScandata.encoderSpeed = FunctHelper.ConvertToUShort(bdata);
                offset = 2;
            }
            bdata = FunctHelper.ASCCItoByte(_rdatas[19 + offset]);
            lmdScandata.amountOf16BitChannels = FunctHelper.ConvertToUShort(bdata);
            if (lmdScandata.amountOf16BitChannels > 0)
            {
                lmdScandata.content16 = Encoding.UTF8.GetString(_rdatas[20 + offset]);
                bdata = FunctHelper.ASCCItoByte(_rdatas[21 + offset]);
                lmdScandata.scaleFactor16 = FunctHelper.ConvertToFloat(bdata);
                bdata = FunctHelper.ASCCItoByte(_rdatas[22 + offset]);
                lmdScandata.scaleFactorOffset16 = FunctHelper.ConvertToFloat(bdata);
                bdata = FunctHelper.ASCCItoByte(_rdatas[23 + offset]);
                lmdScandata.startAngle16 = FunctHelper.ConvertToUint(bdata);
                bdata = FunctHelper.ASCCItoByte(_rdatas[24 + offset]);
                lmdScandata.steps16 = FunctHelper.ConvertToUShort(bdata);
                bdata = FunctHelper.ASCCItoByte(_rdatas[25 + offset]);
                lmdScandata.amountOfData16 = FunctHelper.ConvertToUShort(bdata);
                if (lmdScandata.amountOfData16 > 0)
                {
                    if (_rdatas.Length < (25 + offset + lmdScandata.amountOfData16))
                        throw new Exception("ERROR Laser: rec:LMDscandata data bad from amountOfData16 !");
                    lmdScandata.data16 = new ushort[lmdScandata.amountOfData16];
                    for (int ii = 0; ii < lmdScandata.amountOfData16; ii++, offset++)
                    {
                        bdata = FunctHelper.ASCCItoByte(_rdatas[26 + offset]);
                        lmdScandata.data16[ii] = FunctHelper.ConvertToUShort(bdata);
                    }
                }
                offset += 6;
            }
            Console.WriteLine("_rdatas index: {0}", 20 + offset);
            bdata = FunctHelper.ASCCItoByte(_rdatas[20 + offset]);
            lmdScandata.amountOf8BitChannels = FunctHelper.ConvertToUShort(bdata);
            if (lmdScandata.amountOf8BitChannels > 0)
            {
                lmdScandata.content8 = Encoding.UTF8.GetString(_rdatas[21 + offset]);
                bdata = FunctHelper.ASCCItoByte(_rdatas[22 + offset]);
                lmdScandata.scaleFactor8 = FunctHelper.ConvertToFloat(bdata);
                bdata = FunctHelper.ASCCItoByte(_rdatas[23 + offset]);
                lmdScandata.scaleFactorOffset8 = FunctHelper.ConvertToFloat(bdata);
                bdata = FunctHelper.ASCCItoByte(_rdatas[24 + offset]);
                lmdScandata.startAngle8 = FunctHelper.ConvertToUint(bdata);
                bdata = FunctHelper.ASCCItoByte(_rdatas[25 + offset]);
                lmdScandata.steps8 = FunctHelper.ConvertToUShort(bdata);
                bdata = FunctHelper.ASCCItoByte(_rdatas[26 + offset]);
                lmdScandata.amountOfData8 = FunctHelper.ConvertToUShort(bdata);
                if (lmdScandata.amountOfData8 > 0)
                {
                    if (_rdatas.Length < (26 + offset + lmdScandata.amountOfData8))
                        throw new Exception("ERROR Laser: rec:LMDscandata data bad from amountOfData8 !");
                    lmdScandata.data16 = new ushort[lmdScandata.amountOfData8];
                    for (int ii = 0; ii < lmdScandata.amountOfData8; ii++, offset++)
                    {
                        bdata = FunctHelper.ASCCItoByte(_rdatas[27 + offset]);
                        lmdScandata.data8[ii] = bdata[0];
                    }
                }
                offset += 6;
            }
            //Console.WriteLine("_rdatas index: {0}", 20 + offset);
            bdata = FunctHelper.ASCCItoByte(_rdatas[21 + offset]);
            lmdScandata.position = FunctHelper.ConvertToUShort(bdata);
            bdata = FunctHelper.ASCCItoByte(_rdatas[22 + offset]);
            lmdScandata.nameMode = FunctHelper.ConvertToUShort(bdata);
            if (lmdScandata.nameMode != 0)
            {
                if (_rdatas.Length < (23 + offset))
                    throw new Exception("ERROR Laser: rec:LMDscandata data bad from nameMode !");
                bdata = FunctHelper.ASCCItoByte(_rdatas[23 + offset]);
                lmdScandata.nameLength = bdata[0];
                lmdScandata.name = Encoding.UTF8.GetString(_rdatas[24 + offset]);
                offset += 2;
            }
            bdata = FunctHelper.ASCCItoByte(_rdatas[23 + offset]);
            lmdScandata.comment = FunctHelper.ConvertToUShort(bdata);
            bdata = FunctHelper.ASCCItoByte(_rdatas[24 + offset]);
            lmdScandata.timeMode = FunctHelper.ConvertToUShort(bdata);
            if (lmdScandata.timeMode != 0)
            {
                if (_rdatas.Length < (24 + offset))
                    throw new Exception("ERROR Laser: rec:LMDscandata data bad from timeMode !");
                bdata = FunctHelper.ASCCItoByte(_rdatas[25 + offset]);
                lmdScandata.timeYear = FunctHelper.ConvertToUShort(bdata);
                bdata = FunctHelper.ASCCItoByte(_rdatas[26 + offset]);
                lmdScandata.timeMonth = bdata[0];
                bdata = FunctHelper.ASCCItoByte(_rdatas[27 + offset]);
                lmdScandata.timeDay = bdata[0];
                bdata = FunctHelper.ASCCItoByte(_rdatas[28 + offset]);
                lmdScandata.timeHour = bdata[0];
                bdata = FunctHelper.ASCCItoByte(_rdatas[29 + offset]);
                lmdScandata.timeMinute = bdata[0];
                bdata = FunctHelper.ASCCItoByte(_rdatas[30 + offset]);
                lmdScandata.timeSecund = bdata[0];
                bdata = FunctHelper.ASCCItoByte(_rdatas[31 + offset]);
                lmdScandata.timeUsecund = FunctHelper.ConvertToUint(bdata);
            }
         }

         private void parser_rbuffer_to_mLMPsetscancfg_R(ref mLMPsetscancfg_R data)
         {
             byte[] bdata;
             data.statusCode = (int)FunctHelper.ASCCItoByteOne(_rdatas[2][0]);
             bdata = FunctHelper.ASCCItoByte(_rdatas[3]);
             data.scan_frequency = FunctHelper.ConvertToUint(bdata);
             bdata = FunctHelper.ASCCItoByte(_rdatas[4]);
             data.value = FunctHelper.ConvertToShort(bdata);
             bdata = FunctHelper.ASCCItoByte(_rdatas[5]);
             data.angle_resolution = FunctHelper.ConvertToUint(bdata);
             bdata = FunctHelper.ASCCItoByte(_rdatas[6]);
             data.start_angle = FunctHelper.ConvertToInt(bdata);
             bdata = FunctHelper.ASCCItoByte(_rdatas[7]);
             data.stop_angle = FunctHelper.ConvertToInt(bdata);
         }

         private void parser_rbuffer_to_LMPoutputRange_R(ref LMPoutputRange_get_R data)
         {
             byte[] bdata;
             data.statusCode = (int)FunctHelper.ASCCItoByteOne(_rdatas[2][0]);             
             bdata = FunctHelper.ASCCItoByte(_rdatas[3]);
             data.angle_resolution = FunctHelper.ConvertToUint(bdata);
             bdata = FunctHelper.ASCCItoByte(_rdatas[4]);
             data.start_angle = FunctHelper.ConvertToInt(bdata);
             bdata = FunctHelper.ASCCItoByte(_rdatas[5]);
             data.stop_angle = FunctHelper.ConvertToInt(bdata);
         }

         private void parser_rbuffer_to_LMPscancfg_R(ref LMPscancfg_R data)
         {
             byte[] bdata;
             bdata = FunctHelper.ASCCItoByte(_rdatas[2]);
             data.scan_frequency = FunctHelper.ConvertToUint(bdata);
             bdata = FunctHelper.ASCCItoByte(_rdatas[3]);
             data.value = FunctHelper.ConvertToShort(bdata);
             bdata = FunctHelper.ASCCItoByte(_rdatas[4]);
             data.angle_resolution = FunctHelper.ConvertToUint(bdata);
             bdata = FunctHelper.ASCCItoByte(_rdatas[5]);
             data.start_angle = FunctHelper.ConvertToInt(bdata);
             bdata = FunctHelper.ASCCItoByte(_rdatas[6]);
             data.stop_angle = FunctHelper.ConvertToInt(bdata);
         }

         byte[][] SeparatedToByteArray(byte[] source, byte separator)
         {
             List<byte[]> Parts = new List<byte[]>();
             int Index = 0;
             byte[] Part;
             for (int i = 0; i < source.Length; ++i)
             {
                 if (source[i] == separator)
                 {
                     Part = new byte[i - Index];
                     Array.Copy(source, Index, Part, 0, Part.Length);
                     Parts.Add(Part);
                     Index = i + 1;
                    // i += separator.Length - 1;
                 }
             }
             Part = new byte[source.Length - Index];
             Array.Copy(source, Index, Part, 0, Part.Length);
             Parts.Add(Part);
             return Parts.ToArray();
         }

       void newConnecting(Object stateInfo)
       {
           ConnectToServer();
       }
       void Reconnecting()
       {
           Disconnect();
           Timer T = new Timer(new TimerCallback(newConnecting), null, 2000, 0);
       }
        private void bwSender_RunWorkerCompleted(object sender , RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null && ((bool)e.Result))
               // OnCommandSent(new MessageEventArgs("\nLaser sended!"));
                OnCommandSent(new MessageEventArgs(""));
            else
            {
                OnCommandSendingFailed(new MessageEventArgs("\nERROR: Laser not sended!"));
                Reconnecting();
            }

            ( (BackgroundWorker)sender ).Dispose();
            GC.Collect();
        }

        private void bwSender_DoWork(object sender , DoWorkEventArgs e)
        {
           // Command<T> cmd = (Command<T>)e.Argument;
            e.Result = SendCommandTo(e.Argument);
        }

        private Byte[] SerializeMessage<T>(T msg) where T : struct
        {
          //  int objsize = Marshal.SizeOf(typeof(T));
            int objsize = Marshal.SizeOf(msg);
            Byte[] ret = new Byte[objsize];
            IntPtr buff = Marshal.AllocHGlobal(objsize);
            Marshal.StructureToPtr(msg, buff, true);
            Marshal.Copy(buff, ret, 0, objsize);
            Marshal.FreeHGlobal(buff);
            return ret;
        }

        private T DeserializeMsg<T>(Byte[] data) where T : struct
        {
            int objsize = Marshal.SizeOf(typeof(T));
            IntPtr buff = Marshal.AllocHGlobal(objsize);
            Marshal.Copy(data, 0, buff, objsize);
            T retStruct = (T)Marshal.PtrToStructure(buff, typeof(T));
            Marshal.FreeHGlobal(buff);
            return retStruct;
        }

        private int BytePatternSearch(byte[] searchIn, byte[] searchBytes, int start = 0)
        {
            int found = -1;
            bool matched = false;
            //only look at this if we have a populated search array and search bytes with a sensible start
            if (searchIn.Length > 0 && searchBytes.Length > 0 && start <= (searchIn.Length - searchBytes.Length) && searchIn.Length >= searchBytes.Length)
            {
                //iterate through the array to be searched
                for (int i = start; i <= searchIn.Length - searchBytes.Length; i++)
                {
                    //if the start bytes match we will start comparing all other bytes
                    if (searchIn[i] == searchBytes[0])
                    {
                        if (searchIn.Length > 1)
                        {
                            //multiple bytes to be searched we have to compare byte by byte
                            matched = true;
                            for (int y = 1; y <= searchBytes.Length - 1; y++)
                            {
                                if (searchIn[i + y] != searchBytes[y])
                                {
                                    matched = false;
                                    break;
                                }
                            }
                            //everything matched up
                            if (matched)
                            {
                                found = i;
                                break;
                            }

                        }
                        else
                        {
                            //search byte is only one bit nothing else to do
                            found = i;
                            break; //stop the loop
                        }

                    }
                }

            }
            return found;
        }
        
       
        private bool SendCommandTo(Object cmd)
        {
            try
            { 
                byte[] sbuffer = null;
                CommandType type = CommandType.None;
                PropertyInfo[] propertyInfos = cmd.GetType().GetProperties();          
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {     
                    if(propertyInfo.PropertyType == typeof(CommandType))
                    {
                        type =  (CommandType)propertyInfo.GetValue(cmd, null);               
                    }
                    if(propertyInfo.PropertyType == typeof(SCdevicestate))
                    {
                     /*   Console.WriteLine("{0} [type = {1}] [value = {2}]",
                        propertyInfo.Name,
                        propertyInfo.PropertyType,
                        propertyInfo.GetValue(cmd, null));*/                    
                        sbuffer = SerializeMessage<SCdevicestate>((SCdevicestate)propertyInfo.GetValue(cmd, null));
                    }
                    else
                    if (propertyInfo.PropertyType == typeof(DeviceIdent))
                    {
                        sbuffer = SerializeMessage<DeviceIdent>((DeviceIdent)propertyInfo.GetValue(cmd, null));
                        break;
                    }
                    else
                    if ((propertyInfo.PropertyType == typeof(LMDscandata)) && (type == CommandType.LMDscandata))
                    {                       
                        sbuffer = SerializeMessage<LMDscandata>((LMDscandata)propertyInfo.GetValue(cmd, null));
                        break;
                    }
                    else
                    if ((propertyInfo.PropertyType == typeof(LMDscandata_E)) && (type == CommandType.LMDscandata_E))
                    {
                        sbuffer = SerializeMessage<LMDscandata_E>((LMDscandata_E)propertyInfo.GetValue(cmd, null));
                        break;
                    }
                    else
                    if ((propertyInfo.PropertyType == typeof(Run)) && (type == CommandType.Run))
                    {
                        sbuffer = SerializeMessage<Run>((Run)propertyInfo.GetValue(cmd, null));
                        break;
                    }
                    else
                    if ((propertyInfo.PropertyType == typeof(SetAccessMode)) && (type == CommandType.SetAccessMode))
                    {
                        sbuffer = SerializeMessage<SetAccessMode>((SetAccessMode)propertyInfo.GetValue(cmd, null));
                        break;
                    }
                    else
                    if ((propertyInfo.PropertyType == typeof(mLMPsetscancfg)) && (type == CommandType.mLMPsetscancfg))
                    {
                        sbuffer = SerializeMessage<mLMPsetscancfg>((mLMPsetscancfg)propertyInfo.GetValue(cmd, null));
                        break;
                    }
                    else
                    if ((propertyInfo.PropertyType == typeof(LMPscancfg)) && (type == CommandType.LMPscancfg))
                    {
                        sbuffer = SerializeMessage<LMPscancfg>((LMPscancfg)propertyInfo.GetValue(cmd, null));
                        break;
                    }
                    else
                    if ((propertyInfo.PropertyType == typeof(LMPoutputRange)) && (type == CommandType.LMPoutputRange))
                    {
                        sbuffer = SerializeMessage<LMPoutputRange>((LMPoutputRange)propertyInfo.GetValue(cmd, null));
                        break;
                    }
                    else
                    if ((propertyInfo.PropertyType == typeof(LMPoutputRange_get)) && (type == CommandType.LMPoutputRange_get))
                    {
                        sbuffer = SerializeMessage<LMPoutputRange_get>((LMPoutputRange_get)propertyInfo.GetValue(cmd, null));
                        break;
                    }
                    else
                    if ((propertyInfo.PropertyType == typeof(LCMstate)) && (type == CommandType.LCMstate))
                    {
                        sbuffer = SerializeMessage<LCMstate>((LCMstate)propertyInfo.GetValue(cmd, null));
                        break;
                    }            
                }
                Console.WriteLine("send buffer: " + FunctHelper.Print_byteArray_Hex_ASCII(sbuffer));               
                _semaphor.WaitOne();
                networkStream.Write(sbuffer, 0, sbuffer.Length);
                networkStream.Flush();
                _semaphor.Release();       
                return true;
            }
            catch (Exception ex)
            { //
                _semaphor.Release();
                OnCommandSendingFailed(new MessageEventArgs("ERROR:Laser sended error: " + ex.Message));
                return false;
            }
        }
        #endregion

        #region Public Methods       
          public void ConnectToServer()
        {
            BackgroundWorker bwConnector = new BackgroundWorker();
            bwConnector.DoWork += new DoWorkEventHandler(bwConnector_DoWork);
            bwConnector.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwConnector_RunWorkerCompleted);
            bwConnector.RunWorkerAsync();
        }

        private void bwConnector_RunWorkerCompleted(object sender , RunWorkerCompletedEventArgs e)
        {
            if (!((bool)e.Result))
            {
                OnConnectingFailed(new MessageEventArgs("ERROR:Laser connected failer!"));
                Reconnecting();
            }
            else
                OnConnectingSuccessed(new MessageEventArgs("Laser connected"));

            ( (BackgroundWorker)sender ).Dispose();
        }

        private void bwConnector_DoWork(object sender , DoWorkEventArgs e)
        {
            try
            {
                this.clientSocket = new Socket(AddressFamily.InterNetwork , SocketType.Stream , ProtocolType.Tcp);
                this.clientSocket.Connect(_serverEP);
                e.Result = true;
                networkStream = new NetworkStream(this.clientSocket);
                this.bwReceiver = new BackgroundWorker();
                this.bwReceiver.WorkerSupportsCancellation = true;
                this.bwReceiver.DoWork += new DoWorkEventHandler(StartReceive);
                this.bwReceiver.RunWorkerAsync();
                //List<string> data = null;           
               // Command status = new Command(CommandType.sRN, "SCdevicestate", null);
               // SendCommandTo(status);
            }
            catch
            {
                e.Result = false;
            }
        }
        
        // Sends a command to the server if the connection is alive.       
        public void SendCommand(Object cmd)
        {
            if ( (clientSocket != null) && clientSocket.Connected )
            {
                BackgroundWorker bwSender = new BackgroundWorker();
                bwSender.DoWork += new DoWorkEventHandler(bwSender_DoWork);
                bwSender.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSender_RunWorkerCompleted);
                bwSender.WorkerSupportsCancellation = true;
                bwSender.RunWorkerAsync(cmd);
            }
            else
                OnCommandSendingFailed(new MessageEventArgs("ERROR:Laser not sended"));
        }
        
        
        // Disconnect the client from the server and returns true if the client had been disconnected from the server.        
        public bool Disconnect()
        {
            if (this.clientSocket != null && this.clientSocket.Connected )
            {
                try
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    bwReceiver.CancelAsync();
                    OnDisconnectedFromServer(new MessageEventArgs("Laser disconnected"));
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
                return true;
        } 
        #endregion

        #region Events        
        // Occurs when a command received from a remote client.        
        public event CommandReceivedEventHandler CommandReceived;
        protected virtual void OnCommandReceived(CommandEventArgs e)
        {
            if ( CommandReceived != null )
            {
                 CommandReceived(this , e);
            }
        }

        // Occurs when a command sending action had been failed.This is because disconnection or sending exception.       
        public event CommandReceivingFailedEventHandler CommandReceivingFailed;
        protected virtual void OnCommandReceivingFailed(MessageEventArgs e)
        {
            if (CommandReceivingFailed != null)
            {
                CommandReceivingFailed(this, e);
            }
        }

        // Occurs when a command had been sent to the the remote server Successfully.        
        public event CommandSentEventHandler CommandSent;
        protected virtual void OnCommandSent(MessageEventArgs e)
        {
            if ( CommandSent != null )
            {
                CommandSent(this , e);
            }
        }
       
        // Occurs when a command sending action had been failed.This is because disconnection or sending exception.       
        public event CommandSendingFailedEventHandler CommandSendingFailed;
        protected virtual void OnCommandSendingFailed(MessageEventArgs e)
        {
            if (CommandSendingFailed != null)
            {
                CommandSendingFailed(this, e);
            }
        }
       
        // Occurs when the client disconnected.  
        public event ServerDisconnectedEventHandler ServerDisconnected;      
        protected virtual void OnServerDisconnected(ServerEventArgs e)
        {
            if ( ServerDisconnected != null )
            {
                ServerDisconnected(this , e);
            }
        }

        // Occurs when this client disconnected from the remote server.       
        public event DisconnectedEventHandler DisconnectedFromServer;
        protected virtual void OnDisconnectedFromServer(MessageEventArgs e)
        {
            if ( DisconnectedFromServer != null )
            {
                DisconnectedFromServer(this , e);
            }
        }
       
        // Occurs when this client connected to the remote server Successfully.        
        public event ConnectingSuccessedEventHandler ConnectingSuccessed;
        protected virtual void OnConnectingSuccessed(MessageEventArgs e)
        {
            if ( ConnectingSuccessed != null )
            {
                ConnectingSuccessed(this , e);
            }
        }
        
        // Occurs when this client failed on connecting to server.        
        public event ConnectingFailedEventHandler ConnectingFailed;
        protected virtual void OnConnectingFailed(MessageEventArgs e)
        {
            if ( ConnectingFailed != null )
            {
                ConnectingFailed(this , e);
            }
        }
       
        // Occurs when the network had been failed.      
        public event NetworkDeadEventHandler NetworkDead;
        protected virtual void OnNetworkDead(MessageEventArgs e)
        {
            if ( NetworkDead != null )
            {          
                NetworkDead(this , e);
            }
        }
        
        // Occurs when the network had been started to work.        
        public event NetworkAlivedEventHandler NetworkAlived;
        protected virtual void OnNetworkAlived(MessageEventArgs e)
        {
            if ( NetworkAlived != null )
            {
                NetworkAlived(this , e);
            }
        }
        #endregion
    }
}
