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
//mEEwriteall,LMCstartmeas,LMCstopmeas, LMLfpFcn,mSCloadfacdef
//#define SCAN_TEST
namespace Brace.Shared.DeviceDrivers.LMS511Laser
{
using Brace.Shared.Core.Classes;
using Brace.Shared.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Brace.Shared.Diagnostics.Trace;
using Brace.Shared.DeviceDrivers.LMS511Laser.EventHandlers;
using Brace.Shared.DeviceDrivers.LMS511Laser.Commands;
using Brace.Shared.DeviceDrivers.LMS511Laser.Enums;
using Brace.Shared.DeviceDrivers.LMS511Laser.Counters;
using Brace.Shared.DeviceDrivers.LMS511Laser.Helpers;
using Brace.Shared.DeviceDrivers.LMS511Laser.Interfaces;

    internal class Driver : IDevice, ITriggerProvider, ILaser
    {
        #region Variable private
        private TraceWrapper _traceWrapper;      
        private bool _isInitialized;
        private LaserConfig _laserConfig;       
        private bool _started;
        private List<Action> methodeList;
        private int methodeListIndex;
        private int  _yearScan;
        private int  _monthScan;
        private int  _dayScan;
        private int  _hourScan;
        private int  _secundScan;
        private int  _minuteScan;
        private uint  _usecund;
        private ushort _statusOfDigitalOutputs;
        private ushort _maskDigitalOutputs; 
        private int _oportNum;
        private int _oportStatus;
        private ScanCfg _scancfg;
        private DateTime _setdatetime;
        private short _outputchannel;
        private int _remission;
        private int _resolution;
        private int _unit;
        private short _encoder;
        private short _position;
        private short _device_name;
        private short _comment; 
        private short _time;
        private short _output_rate;
        #endregion

        public  TelegramsOperating _telegram;

        #region EventHandler part
        /// <summary>
        /// StatusChange event
        /// </summary>       
        public event EventHandler<StatusChangeEventArgs> StatusChange;
        #endregion

        #region Action part 
        private event Action Run_CMD;
        private event Action SetAccessMode_client_CMD;
        private event Action SetAccessMode_maintenance_CMD;
        private event Action SetAccessMode_service_CMD;
        private event Action LSPsetdatetime_CMD;
        private event Action LMDscandata_E_CMD;
        private event Action mSCreboot_CMD;
        private event Action mDOSetOutput_CMD;
        private event Action mLMPsetscancfg_CMD;
        private event Action LMDscandatacfg_CMD;
        #endregion

        #region Contsructors
        public Driver()
        {
            _isInitialized = false;
            _started = false;
            _statusOfDigitalOutputs = 0;
        }
        #endregion
        #region IDevice implementation
     
        public void Initialize(string configuration, TraceWrapper traceWrapper)
        {
            try 
            {
                _traceWrapper = traceWrapper;
                _traceWrapper.WriteInformation("Laser initialization start.");
                _traceWrapper.WriteVerbose(string.Format("Configuration: {0}", configuration));
                _laserConfig = LaserConfig.LoadFromXml<LaserConfig>(configuration);
                _telegram = new TelegramsOperating(_laserConfig.IPAdrress, 2111, traceWrapper);
                 _telegram.ConnectingSuccessed += new ConnectingSuccessedEventHandler(on_ConnectingSuccessed);
                 _telegram.ConnectingFailed += new ConnectingFailedEventHandler(on_ConnectingFailed);
                 _telegram.CommandReceivingFailed += new CommandReceivingFailedEventHandler(on_CommandReceivingFailed);
                 _telegram.CommandSendingFailed += new CommandSendingFailedEventHandler(on_CommandSendingFailed);
                 _telegram.Sopas_Error_CMD += new EventHandler<SopasErrorEventArgs>(on_SopasError); 
                 _telegram.SetAccessMode_CMD += new EventHandler<SetAccessModeEventArgs>(on_SetAccessMode);
                 _telegram.LSPsetdatetime_CMD += new EventHandler<LSPsetdatetimeEventArgs>(on_LSPsetdatetime);
                 _telegram.Run_CMD += new EventHandler<RunEventArgs>(on_Run);
                 _telegram.LMDscandataE_CMD += new EventHandler<LMDscandataEEventArgs>(on_LMDscandataE);
                 _telegram.LMDscandata_CMD += new EventHandler<LMDscandataEventArgs>(on_LMDscandata);
                 _telegram.mSCreboot_CMD += new EventHandler<mSCrebootEventArgs>(on_mSCreboot);
                 _telegram.mDOSetOutput_CMD += new EventHandler<mDOSetOutputEventArgs>(on_mDOSetOutput);
                 _telegram.mLMPsetscancfg_CMD += new EventHandler<mLMPsetscancfgEventArgs>(on_mLMPsetscancfg);
                 _telegram.LMDscandatacfg_CMD += new EventHandler<LMDscandatacfgEventArgs>(on_LMDscandatacfg);
                 

                 Run_CMD = start_Run;
                 SetAccessMode_client_CMD = start_SetAccessMode_client;
                 SetAccessMode_maintenance_CMD = start_SetAccessMode_maintenance;
                 SetAccessMode_service_CMD = start_SetAccessMode_service;
                 LSPsetdatetime_CMD = start_LSPsetdatetime;
                 LMDscandata_E_CMD = start_LMDscandata_E;
                 mSCreboot_CMD = start_mSCreboot;
                 mDOSetOutput_CMD = start_mDOSetOutput;
                 mLMPsetscancfg_CMD = start_mLMPsetscancfg;
                 LMDscandatacfg_CMD = start_LMDscandatacfg;
                 if ((_laserConfig.TriggerOutputChannelNumber > 0) && (_laserConfig.TriggerOutputChannelNumber < 7))
                     _maskDigitalOutputs = (byte)(1 << (_laserConfig.TriggerOutputChannelNumber-1));
                 else
                     _maskDigitalOutputs = (byte)(1);

                _isInitialized = true;
                ErrorStateCounter.PrintPropertiesSettings();
                _traceWrapper.WriteInformation("Laser initialization end.");
            }
            catch (Exception ex)
            {
                _traceWrapper.WriteError(new ApplicationException("Unhandled exception in LaserDriver.Initialize()",ex));
            }
          //  throw new NotImplementedException();
        }

        public void Start()
        {
            if (_isInitialized)
            {
                if (!_started)
                {
                    methodeList = new List<Action>();
                   // methodeList.Add(SetAccessMode_service_CMD);
                  //  methodeList.Add(LSPsetdatetime_CMD);
                    methodeList.Add(Run_CMD);
                    methodeList.Add(LMDscandata_E_CMD);                   
                    _telegram.ConnectToDevice();                  
                    ErrorStateCounter.ClearCounterFlags();
                    _started = true;
                }
            }
            else
            {
                throw new InvalidOperationException();
            }           
        }

        public void Stop()
        {
            if (!_isInitialized)
            {
                _telegram.Disconnect();
                methodeList = null;
                _started = false;
            }
            else
            {
                throw new InvalidOperationException();
            }         
        }

        public IDeviceStatus GetDeviceStatus()
        {
            if (!_isInitialized)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new InvalidOperationException();
            }          
        }       
        #endregion

        #region ILaser implementation
        public uint getFullErrorCounter()
        {  
            return ErrorStateCounter.FullErrorCounter;
        }
        public uint FullSopasErrorCounter()
        {
            return ErrorStateCounter.FullSopasErrorCounter;
        }
        public uint getFullConnectedCounter()
        {
            return ErrorStateCounter.FullConnectedCounter;
        }
        public DateTime getLaserInternalDateTime()
        {
            return new DateTime(_yearScan, _monthScan, _dayScan, _hourScan, _minuteScan, _secundScan,  (int)(_usecund/1000));
        }
        public void ReBoot()
        {
            methodeList = new List<Action>();
            methodeList.Add(SetAccessMode_service_CMD);
            methodeList.Add(mSCreboot_CMD);
            CallNextMethodeFromList();  
        }
        public void DOSetOutput(int oportNum, int oportStatus)
        {
            _oportNum = oportNum;
            _oportStatus = oportStatus;
        }
        public void DOOutput()
        {
            methodeList = new List<Action>();
            methodeList.Add(SetAccessMode_service_CMD);
            methodeList.Add(mDOSetOutput_CMD);
            methodeList.Add(Run_CMD);
            methodeList.Add(LMDscandata_E_CMD);
            CallNextMethodeFromList();
        }
        public void SetSetscancfg(uint scan_frequency, uint angle_resolution, int start_angle, int stop_angle)
        {
            _scancfg = new ScanCfg();
            _scancfg.scan_frequency = scan_frequency;
            _scancfg.angle_resolution = angle_resolution;
            _scancfg.start_angle = start_angle;
            _scancfg.stop_angle = stop_angle;
        }
        public void Setscancfg()
        {
            methodeList = new List<Action>();
            methodeList.Add(SetAccessMode_service_CMD);
            methodeList.Add(mLMPsetscancfg_CMD);
            methodeList.Add(Run_CMD);
          //  methodeList.Add(LMDscandata_E_CMD);
            CallNextMethodeFromList();
        }

        public void Scandata()
        {
            methodeList = new List<Action>();            
            methodeList.Add(Run_CMD);
            methodeList.Add(LMDscandata_E_CMD);
            CallNextMethodeFromList();
        }
        public void SetSetdatetime(DateTime dt)
        {
            _setdatetime = dt;
        }

        public void Setdatetime()
        {
            methodeList = new List<Action>();
            methodeList.Add(SetAccessMode_service_CMD);
            methodeList.Add(LSPsetdatetime_CMD);
            methodeList.Add(Run_CMD);
           // methodeList.Add(LMDscandata_E_CMD);
            CallNextMethodeFromList();
        }

        public void LMDscandatacfg(short outputchannel, int remission, int resolution, int unit, short encoder, short position, short device_name, short comment, short time, short output_rate)
        {            
            _outputchannel =  outputchannel;
            _remission = remission;
            _resolution = resolution;
            _unit = unit;
            _encoder = encoder;
            _position = position;        
            _device_name = device_name; 
            _comment = comment; 
            _time = time;
            _output_rate = output_rate;
        }

        public void Scandatacfg()
        {
            methodeList = new List<Action>();
            methodeList.Add(SetAccessMode_service_CMD);
            methodeList.Add(LMDscandatacfg_CMD);
         //   methodeList.Add(Run_CMD);
            // methodeList.Add(LMDscandata_E_CMD);
            CallNextMethodeFromList();
        }
        public event EventHandler<mSCrebootEventArgs>    ReBootEvent = null;
        public event EventHandler<mDOSetOutputEventArgs> DOSetOutputEvent = null;
        public event EventHandler<mLMPsetscancfgEventArgs> SetConfigEvent = null;
        public event EventHandler<LMDscandataEventArgs> LMDscandataEvent = null;
        public event EventHandler<LSPsetdatetimeEventArgs> LSPsetdatetimeEvent = null;
        public event EventHandler<LMDscandatacfgEventArgs> LMDscandatacfgEvent = null;
        #endregion

        #region ITriggerProviderImplementation
        public event EventHandler<RaisingEdgeEventArgs> RaisingEdge = null;
        public event EventHandler<FallingEdgeEventArgs> FallingEdge = null;        
        #endregion
    

        #region Event Handlers
        void on_ConnectingSuccessed(object sender, MessageEventArgs e)
        {
         //   setStatusChange(e.Msg, null);
            _traceWrapper.WriteInformation(e.Msg);
            // SetAccessMode      
            // LSPsetdatetime
            // Run
            // LMDscandata_E
            ErrorStateCounter.FullConnectedCounter++;
            methodeListIndex = 0;                 
            CallNextMethodeFromList();        
        }

        void on_ConnectingFailed(object sender, MessageEventArgs e)
        {
             errorControl(true, e.Msg);         
            _traceWrapper.WriteError(e.Msg);
        }

        void on_CommandSendingFailed(object sender, MessageEventArgs e)
        {
            errorControl(true, e.Msg);
            _traceWrapper.WriteError(e.Msg);
        }
        void on_CommandReceivingFailed(object sender, MessageEventArgs e)
        {
            errorControl(true, e.Msg);
            _traceWrapper.WriteError(e.Msg);       
        }

        void on_SopasError(object sender, SopasErrorEventArgs e)
        { // Laser internal error: 1-26, Telegram.pdf: 75.-76. page
             errorControl(false, e.Error);
            _traceWrapper.WriteError(e.Error);
        }

        void on_SetAccessMode(object sender, SetAccessModeEventArgs e)
        {       
            if (e.StatusCode == StatusEnum.SUCCESS)
            {
                _traceWrapper.WriteInformation("SetAccessMode : SUCCESS");
                CallNextMethodeFromList();               
            }
            else
            {
                _traceWrapper.WriteError("SetAccessMode receive failed");
            }           
        }

        void on_LSPsetdatetime(object sender, LSPsetdatetimeEventArgs e)
        {           
            if (e.StatusCode == StatusEnum.SUCCESS)
            {
             //  _traceWrapper.WriteInformation("LSPsetdatetime : SUCCESS");               
                CallNextMethodeFromList();
            }
          /*  else
            {
                _traceWrapper.WriteError("LSPsetdatetime receive failed");
            }*/
            LSPsetdatetimeEvent(this, e);            
        }
        void on_Run(object sender, RunEventArgs e)
        {            
            if (e.RunCode == StatusEnum.SUCCESS)
            {
                _traceWrapper.WriteInformation("Run : SUCCESS");
                CallNextMethodeFromList();
            }
            else
            {
                _traceWrapper.WriteError("Run receive failed");
            }           
        }

        void on_LMDscandataE(object sender, LMDscandataEEventArgs e)
        {           
            if (e.Measurement == StartStopEnum.START)
            {
               _traceWrapper.WriteInformation("LMDscandataE : STARTED");
                CallNextMethodeFromList();
            }
            else
            {
                _traceWrapper.WriteError("LMDscandata receive STOPED or failed");
            }           
        }

        void on_LMDscandata(object sender, LMDscandataEventArgs e)
        {
            if (e.ScanData.timeMode != 0)
            {
                _yearScan = e.ScanData.timeYear;
                _monthScan = e.ScanData.timeMonth;
                _dayScan = e.ScanData.timeDay;
                _hourScan = e.ScanData.timeHour;
                _secundScan = e.ScanData.timeSecund;
                _minuteScan = e.ScanData.timeMinute;
                _usecund = e.ScanData.timeUsecund;
            }
          //  Console.WriteLine("Dig. Output: " + String.Format("{0:X}", e.ScanData.statusOfDigitalOutputs));
            if ((_statusOfDigitalOutputs & _maskDigitalOutputs) != (e.ScanData.statusOfDigitalOutputs & _maskDigitalOutputs))
            {
                _statusOfDigitalOutputs = e.ScanData.statusOfDigitalOutputs;
                if ((_statusOfDigitalOutputs & _maskDigitalOutputs) != 0)
                {
                    if (RaisingEdge != null)
                    {
                        RaisingEdgeEventArgs re =  new RaisingEdgeEventArgs(true, getLaserInternalDateTime());
                        RaisingEdge(this, re);
                    }
                }
                else
                {
                    if (FallingEdge != null)
                    {
                        FallingEdgeEventArgs re = new FallingEdgeEventArgs(true, getLaserInternalDateTime());
                        FallingEdge(this, re);
                    }
                }
            }
            if (LMDscandataEvent != null)
            {
                LMDscandataEvent(this, e);
            }
#if SCAN_TEST
            //_traceWrapper.WriteInformation("LMDscandata : receive");
            string sTemp = "";
            string msg = "\nScandata -----------------------------------------------";
            msg += "\nVersion Number: " +  e.ScanData.versionNumber.ToString();
            msg += "\nDevice Number: " +  e.ScanData.deviceNumber.ToString();
            msg += "\nSerial Number: " +  e.ScanData.serialNumber.ToString();
            msg += "\nDevice Status: " +  e.ScanData.deviceStatus.ToString();
            msg += "\nTelegram Counter: " +  e.ScanData.telegramCounter.ToString();
            msg += "\nScan Counter: " +  e.ScanData.scanCounter.ToString();
            msg += "\nTime Since Start Up: " + e.ScanData.timeSinceStartUp.ToString();
            msg += "\nTime Of Transmission: " + e.ScanData.timeOfTransmission.ToString();
            msg += "\nDig. Input : " + String.Format("{0:X}", e.ScanData.statusOfDigitalInputs);
            msg += "\nDig. Output: " + String.Format("{0:X}", e.ScanData.statusOfDigitalOutputs);
            msg += "\nScan frequency: " + ((e.ScanData.scanFrequency) / 100).ToString();
            msg += "\nMeasurement frequency: " + e.ScanData.measurementFrequency.ToString();
            msg += "\nAmount of Encoder: " + e.ScanData.amountOfEncoder.ToString();
            if (e.ScanData.amountOfEncoder != 0)
            {
                msg += "\nEncoderPosition: " + e.ScanData.encoderPosition.ToString();
                msg += "\nEncoder Speed: " + e.ScanData.encoderSpeed.ToString();
            }
            msg += "\nAmount of 16 Bit Channels: " + e.ScanData.amountOf16BitChannels.ToString();
            if (e.ScanData.amountOf16BitChannels > 0)
            {
                msg += "\nContent: " + e.ScanData.content16;
                msg += "\nScale factor: " + e.ScanData.scaleFactor16.ToString();
                msg += "\nScale factor offset: " + e.ScanData.scaleFactorOffset16.ToString();
                msg += "\nStart angle: " + (e.ScanData.startAngle16 / 10000).ToString();
                msg += "\nSteps: " + (e.ScanData.steps16 / 10000).ToString();
                msg += "\nAmount of Data: " + e.ScanData.amountOfData16.ToString();
                msg += "\nData0 .. DataN:";
                for (int ii = 0; ii < e.ScanData.amountOfData16; ii++)
                {
                    if (((ii % 8) != 0) || (ii == 0))
                    {
                        sTemp += e.ScanData.data16[ii].ToString("D5") + ", ";
                    }
                    else
                    {
                        if (ii == 8)
                            msg += "\n";
                        else
                            msg += "\n" + sTemp + e.ScanData.data16[ii].ToString("D5");
                        sTemp = "";
                    }
                }
            }
            msg += "\nAmount of 8 Bit Channels: " + e.ScanData.amountOf8BitChannels.ToString();
            if (e.ScanData.amountOf8BitChannels > 0)
            {
                msg += "\nContent: " + e.ScanData.content8;
                msg += "\nScale factor: " + e.ScanData.scaleFactor8.ToString();
                msg += "\nScale factor offset: " + e.ScanData.scaleFactorOffset8.ToString();
                msg += "\nStart angle: " + (e.ScanData.startAngle8 / 10000).ToString();
                msg += "\nSteps: " + (e.ScanData.steps8 / 10000).ToString();
                msg += "\nAmount of Data: " + e.ScanData.amountOfData8.ToString();
                msg += "\nData0 .. DataN:";
                for (int ii = 0; ii < e.ScanData.amountOfData8; ii++)
                {
                    if (((ii % 8) != 0) || (ii == 0))
                    {
                        sTemp += e.ScanData.data16[ii].ToString("D5") + ", ";
                    }
                    else
                    {
                        if (ii == 8)
                            msg += "\n";
                        else
                            msg += "\n" + sTemp + e.ScanData.data8[ii].ToString("D5");
                        sTemp = "";
                    }
                }
            }
            msg += "\nPosition: " + e.ScanData.position.ToString();
            msg += "\nDevice Name enabled: " + e.ScanData.nameMode.ToString();
            if (e.ScanData.nameMode != 0)
            {
                msg += "\nLength of Name: " + e.ScanData.nameLength.ToString();
                msg += "\nDevice Name: " + e.ScanData.name;
            }
            msg += "\nComment: " + e.ScanData.comment.ToString();
            msg += "\nTime enabled: " + e.ScanData.timeMode.ToString();
            if (e.ScanData.timeMode != 0)
            {
                msg += "\nTime: " + e.ScanData.timeYear.ToString("D4") + ":" + e.ScanData.timeMonth.ToString("D2") + ":" + e.ScanData.timeDay.ToString("D2") + " " + e.ScanData.timeHour.ToString("D2") + ":" + e.ScanData.timeMinute.ToString("D2") + ":" + e.ScanData.timeSecund.ToString("D2") + ":" + e.ScanData.timeUsecund.ToString("D4");
            }
            _traceWrapper.WriteInformation("Laser:LMDscandata :\n" + msg);
#endif
        }
        void on_mSCreboot(object sender, mSCrebootEventArgs e)
        {
           if (ReBootEvent != null)
           {           
               ReBootEvent(this, e);
           }
     //     setStatusChange("Reboot : SUCCESS", null);
        }

        void on_mDOSetOutput(object sender, mDOSetOutputEventArgs e)
        {
            if (e.StatusCode == StatusEnum.SUCCESS)
                _traceWrapper.WriteInformation("mDOSetOutput : SUCCESS");
            else
                _traceWrapper.WriteInformation("mDOSetOutput : ERROR");
            if (DOSetOutputEvent != null)
            {
                DOSetOutputEvent(this, e);
            }
            CallNextMethodeFromList();
            //     setStatusChange("Reboot : SUCCESS", null);
        }

        public void on_mLMPsetscancfg(object sender, mLMPsetscancfgEventArgs e)
        {
            if (e.SetScanCfg.statusCode == SetscancfgEnum.SUCCESS)
            {
                if (SetConfigEvent != null)
                {
                    SetConfigEvent(this, e);
                }
            }
            else
                _traceWrapper.WriteInformation("Laser:LMPsetscancfg : ERROR: " + e.SetScanCfg.statusCode.ToString());           
            CallNextMethodeFromList();
        }
        public void on_LMDscandatacfg(object sender, LMDscandatacfgEventArgs e)
        {
            if (e.StatusCode == StatusEnum.SUCCESS)
            {
                if (LMDscandatacfgEvent != null)
                {
                    LMDscandatacfgEvent(this, e);
                }
            }
            else
                _traceWrapper.WriteInformation("Laser:LMDscandatacfg : ERROR: " + e.StatusCode.ToString());           
            CallNextMethodeFromList();
        }
        
        #endregion

        #region Commands sender members

        void start_SetAccessMode_client()
        {// Authorized client : 0xF4724744            
            SetAccessMode_class sdata = new SetAccessMode_class(3, _laserConfig.PWD_authorized_client);
            _telegram.SendCommand(sdata);
        }
        void start_SetAccessMode_service()
        {   // Service : 0x81BE23AA      
            SetAccessMode_class sdata = new SetAccessMode_class(4, _laserConfig.PWD_service);
            _telegram.SendCommand(sdata);
        }
        void start_SetAccessMode_maintenance()
        {  // Maintenance :  0xB21ACE26
            SetAccessMode_class sdata = new SetAccessMode_class(2, _laserConfig.PWD_maintenance);
            _telegram.SendCommand(sdata);
        }
        void start_LSPsetdatetime()
        {
           // DateTime now = DateTime.Now;           
            LSPsetdatetime_class sdata = new LSPsetdatetime_class(_setdatetime.Year, _setdatetime.Month, _setdatetime.Day, _setdatetime.Hour, _setdatetime.Minute, _setdatetime.Second, _setdatetime.Millisecond);
            _telegram.SendCommand(sdata);
        }
        void start_Run()
        {
            Run_class sdata = new Run_class();
            _telegram.SendCommand(sdata);
        }

        void start_LMDscandata_E()
        {
            LMDscandata_class sdata = new LMDscandata_class(CommandType.LMDscandata_E, true);
            _telegram.SendCommand(sdata);
        }
        void start_mSCreboot()
        {
            mSCreboot_class sdata = new mSCreboot_class();
            _telegram.SendCommand(sdata);
        }
        void start_mDOSetOutput()
        {
            mDOSetOutput_class sdata = new mDOSetOutput_class(_oportNum, _oportStatus);
            _telegram.SendCommand(sdata);
        }
        void start_mLMPsetscancfg()
        {
            mLMPsetscancfg_class sdata = new mLMPsetscancfg_class(_scancfg);
            _telegram.SendCommand(sdata);
        }

        void start_LMDscandatacfg()
        {
            LMDscandatacfg_class sdata = new LMDscandatacfg_class(_outputchannel, _remission, _resolution, _unit, _encoder, _position, _device_name, _comment, _time, _output_rate);
            _telegram.SendCommand(sdata);
        }
        #endregion 
        #region Private members
        void CallNextMethodeFromList()
        {            
            if (methodeList.Count > 0)
            {               
                if (methodeList.Count > methodeListIndex)
                {
                    methodeList[methodeListIndex]();
                    methodeListIndex++;
                }
                else
                {
                    methodeListIndex = 0;
                    methodeList.Clear();
                }              
            }
        }

        void setStatusChange(string newValue, string oldValue)
        {
            if (StatusChange != null)
            { 
                StatusChangeEventArgs sdata = new StatusChangeEventArgs(new MonitorAttribute(newValue), new MonitorAttribute(oldValue), DateTime.Now);
                StatusChange(this, sdata);
            }
        }

        public void errorControl(bool receive, string errorMsg)
        {
            if (receive)
            { // receive error and timeout error
                
                if (errorMsg != null)
                    ErrorStateCounter.ReceivedSendedErrorMsg = errorMsg;
                setStatusChange("RECV. ERROR :" + ErrorStateCounter.ReceivedSendedErrorMsg, null); // !!!!!!!!!!!!!!!!!!!!!
                if (ErrorStateCounter.ReceivedSendedErrorCounterFlag > ErrorStateCounter.LIMITREPEATEDNUMBER)
                {
                    ErrorStateCounter.ReceivedSendedErrorCounterFlag = 0;
                    ErrorStateCounter.FullErrorCounter++;
                    setStatusChange( ErrorStateCounter.ReceivedSendedErrorMsg, null);
                    ErrorStateCounter.ReceivedSendedErrorMsg = "";
                }
                else
                    ErrorStateCounter.ReceivedSendedErrorCounterFlag++;
               
            }
            else
            { // Soap error
                
                if (errorMsg != null)
                    ErrorStateCounter.SoapErrorMsg = errorMsg;
                setStatusChange("SOPAS ERROR :" + ErrorStateCounter.SoapErrorMsg, null); // !!!!!!!!!!!!!!!!!!!!!
                if (ErrorStateCounter.SoapErrorCounterFlag > ErrorStateCounter.LIMITREPEATEDNUMBER)
                {
                    ErrorStateCounter.SoapErrorCounterFlag = 0;
                    ErrorStateCounter.FullSopasErrorCounter++;
                    setStatusChange(ErrorStateCounter.SoapErrorMsg, null);
                    ErrorStateCounter.SoapErrorMsg = "";
                }
                else
                    ErrorStateCounter.FullSopasErrorCounter++;
               
            }
            _telegram.Reconnecting();            
        }
    	#endregion Private members
    }
}