using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Globalization;
using Brace.Shared.Core.Interfaces;
using Brace.Shared.DeviceDrivers.LMS511Laser;
using Brace.Shared.Core.Classes;
using Brace.Shared.Diagnostics.Trace;

namespace LMS511LaserApplication
{
    public class Laser
    {
        public event Action<string> StatusComponentEvent;
        public event Action<string> ScanResultComponentEvent;

        private IDevice laserDevice;
        private bool laserDeviceStarted;
        public Laser()
        {
            laserDeviceStarted = false;
        }
        public void Initialize()
        {           
            TraceWrapper _traceWrapper  = new TraceWrapper();
            string _laserConfig = getLaserConfig();
            IList<object> _laserDevices = Brace.Shared.DeviceDrivers.LMS511Laser.DriverFactory.CreateDriver(_laserConfig, _traceWrapper);
            laserDevice = ((IDevice)_laserDevices[0]);
            EventHandler<StatusChangeEventArgs> _statusHandler = (sender, e) =>
            {
                if (StatusComponentEvent != null)
                {
                    if ((e.OldValue != null) && (e.OldValue.DisplayValue != null))
                        StatusComponentEvent(e.NewValue.DisplayValue + " ; old Msg: " + e.OldValue.DisplayValue + " ; date: " + e.TimeReached.ToUniversalTime());
                    else
                        StatusComponentEvent(e.NewValue.DisplayValue + " ; date: " + e.TimeReached.ToUniversalTime());
                }
            };
            EventHandler<RaisingEdgeEventArgs> _raisingedgeHandler = (sender, e) =>
            {
                StatusComponentEvent(e.Raising.ToString() + " ; date: " + e.TimeReached.ToUniversalTime());
               // _traceWrapper.WriteInformation("Raising: " + e.Raising.ToString() + " ; date: " + e.TimeReached.ToUniversalTime());
            };
            EventHandler<FallingEdgeEventArgs> _fallingedgeHandler = (sender, e) =>
            {
                StatusComponentEvent(e.Falling.ToString() + " ; date: " + e.TimeReached.ToUniversalTime());
                //_traceWrapper.WriteInformation("FallingEdge: " + e.Falling.ToString() + " ; date: " + e.TimeReached.ToUniversalTime());
            };
            EventHandler<ScanDataEventArgs> _scandataHandler = (sender, e) =>
            {
              //  string sTemp = "";
                //string msg = "\nScandata -----------------------------------------------";
                string  msg = "\nVersion Number: " + e.ScanData.versionNumber.ToString();
                msg += "\nDevice Number: " + e.ScanData.deviceNumber.ToString();
                msg += "\nSerial Number: " + e.ScanData.serialNumber.ToString();
                msg += "\nDevice Status: " + e.ScanData.deviceStatus.ToString();
                msg += "\nTelegram Counter: " + e.ScanData.telegramCounter.ToString();
                msg += "\nScan Counter: " + e.ScanData.scanCounter.ToString();
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
                    msg += "\nData0 .. DataN: NOW do not show!";
                  /*  for (int ii = 0; ii < e.ScanData.amountOfData16; ii++)
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
                    }*/
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
                    msg += "\nData0 .. DataN: NOW do not show!";
                 /*   for (int ii = 0; ii < e.ScanData.amountOfData8; ii++)
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
                    }*/
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
                ScanResultComponentEvent(msg + "\nDate time: " + e.TimeReached.ToUniversalTime());                
            };

            laserDevice.StatusChange += _statusHandler;
            ((ITriggerProvider)laserDevice).RaisingEdge += _raisingedgeHandler;
            ((ITriggerProvider)laserDevice).FallingEdge += _fallingedgeHandler;
            ((IScanDataReceiver)laserDevice).ScanDataEvent   += _scandataHandler;
        }
        public bool Start()
        {
            if (!laserDeviceStarted)
            {
                laserDevice.Start();
                laserDeviceStarted = true;
            }
            return laserDeviceStarted;
        }
        public bool Stop()
        {
            if (laserDeviceStarted)
            {
                laserDevice.Stop();
                laserDeviceStarted = false;
            }
            return !laserDeviceStarted;
        }
        public void ReBoot()
        {
            if (laserDeviceStarted)
            {
                laserDevice.Reboot();
            }
        }

        private string getLaserConfig()
        {
            try
            {
                using (FromIniOfFile laserConfigFromIniFile = new FromIniOfFile())
                {
                    uint pwd;
                    laserConfigFromIniFile.ReadData();
                    LaserConfig laserConfig = new LaserConfig();
                    if (laserConfigFromIniFile.FactoryDefault)
                    {
                        laserConfig.SetDefaults();
                    }
                    else
                    {                       
                        laserConfig.IPAddress = laserConfigFromIniFile.IpAddress;                                         
                        if ((laserConfigFromIniFile.TriggerOutputChannelNumber > 0) && (laserConfigFromIniFile.TriggerOutputChannelNumber < 7))
                        {
                            laserConfig.TriggerOutputChannelNumber = laserConfigFromIniFile.TriggerOutputChannelNumber;
                        }
                        else
                        {
                            throw new Exception("Bad Trigger Output Channel Number of laser from 'lms511.ini' file, from 1 to 6!");
                        }
                        try
                        {
                            pwd = UInt32.Parse(laserConfigFromIniFile.Password_maintenance, NumberStyles.HexNumber);                           
                        }
                        catch (Exception)
                        {
                            throw new Exception("Bad Password_maintenance of laser from 'lms511.ini' file, not hexa format");
                        }
                        laserConfig.PWD_maintenance = pwd;
                        try
                        {
                            pwd = UInt32.Parse(laserConfigFromIniFile.Password_authorized_client, NumberStyles.HexNumber);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Bad Password_authorized_client of laser from 'lms511.ini' file, not hexa format");
                        }
                        laserConfig.PWD_authorized_client = pwd;
                        try
                        {
                            pwd = UInt32.Parse(laserConfigFromIniFile.Password_service, NumberStyles.HexNumber);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Bad Password_service  of laser from 'lms511.ini' file, not hexa format");
                        }
                        laserConfig.PWD_service = pwd;                                                  
                    }
                    return laserConfig.SaveToXml();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
