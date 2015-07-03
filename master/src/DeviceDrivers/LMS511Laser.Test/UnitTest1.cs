/* This file is part of *UnitTest*.
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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using Brace.Shared.Diagnostics.Trace;
using Brace.Shared.DeviceDrivers.LMS511Laser.Interfaces;
using Brace.Shared.DeviceDrivers.LMS511Laser.EventHandlers;
using Brace.Shared.Core.Interfaces;
using Brace.Shared.Core.Classes;

namespace Brace.Shared.DeviceDrivers.LMS511Laser.Test
{
    [TestClass]
    public class TestBase
    {
        static StringBuilder s_log;
        public IList<object> _devices;
        public TraceWrapper _traceWrapper;
        public LaserConfig _laserCondig;

        public TestBase()
        {
            s_log = new StringBuilder();
            _traceWrapper = new TraceWrapper();
            _laserCondig = new LaserConfig();
            Log.AppendLine("TestBase.TestBase()");
        }

        [TestInitialize]
        public void BaseTestInit()
        {
            _laserCondig.SetDefaults();
            _devices = Brace.Shared.DeviceDrivers.LMS511Laser.DriverFactory.CreateDriver(_laserCondig.SaveToXml(), _traceWrapper);
            Log.AppendLine("TestBase.BaseTestInit()");
        }

        [TestCleanup]
        public void BaseTestCleanup()
        {
           // Console.WriteLine(Log.ToString());
        }

        public static StringBuilder Log
        {
            get { return s_log; }
        }
    }

    [TestClass]
    public class LaserTest : TestBase
    {
        public LaserTest()
        {
            Log.AppendLine("LaserTest.LaserTest()");
        }

        [TestInitialize]
        public void TestInit()
        {
            Log.AppendLine("LaserTest.TestInit()");
        }

        [TestMethod()]
        public void start()
        {
            if ((_devices != null) && (_devices.Count > 0))
            {
                EventHandler<StatusChangeEventArgs> shandler = (sender, e) =>
                {
                    if (e.OldValue != null)
                        _traceWrapper.WriteInformation("new Msg: " + e.NewValue.DisplayValue + " ; old Msg: " + e.OldValue.DisplayValue + " ; date: " + e.TimeReached.ToUniversalTime());
                    else
                        _traceWrapper.WriteInformation("Msg: " + e.NewValue.DisplayValue + " ; date: " + e.TimeReached.ToUniversalTime());                    
                };
                EventHandler<RaisingEdgeEventArgs> rhandler = (sender, e) =>
                {
                    _traceWrapper.WriteInformation("Raising: " + e.Raising.ToString() + " ; date: " + e.TimeReached.ToUniversalTime());                   
                };
                EventHandler<FallingEdgeEventArgs> fhandler = (sender, e) =>
                {
                    _traceWrapper.WriteInformation("FallingEdge: " + e.Falling.ToString() + " ; date: " + e.TimeReached.ToUniversalTime());                  
                };
                ITriggerProvider tr = (ITriggerProvider)(((IDevice)_devices[0]));
                ((IDevice)_devices[0]).StatusChange += shandler;               
                tr.RaisingEdge += rhandler;                
                tr.FallingEdge += fhandler;
                _traceWrapper.WriteInformation("start()");
                ((IDevice)_devices[0]).Start();
                Thread.Sleep(3000);              // 3 sec
                ((IDevice)_devices[0]).StatusChange -= shandler;
                tr.RaisingEdge -= rhandler;
                tr.FallingEdge -= fhandler;             
            }
        }

       [TestMethod()]
        public void StatusChange()
        {          
            using (var resetEvent = new ManualResetEventSlim(false))
            {                
                EventHandler<StatusChangeEventArgs> handler = (sender, e) =>              
                {
                    if (e.OldValue != null)
                        _traceWrapper.WriteInformation("new Msg: " + e.NewValue.DisplayValue + " ; old Msg: " + e.OldValue.DisplayValue + " ; date: " + e.TimeReached.ToUniversalTime());
                    else
                        _traceWrapper.WriteInformation("Msg: " + e.NewValue.DisplayValue + " ; date: " + e.TimeReached.ToUniversalTime());
                    resetEvent.Set();
                };
                ((IDevice)_devices[0]).StatusChange += handler;
                ((IDevice)_devices[0]).Start();            
                Assert.IsTrue(resetEvent.Wait(TimeSpan.FromMilliseconds(30000)));                           
                ((IDevice)_devices[0]).StatusChange -= handler;
            }
        }

        [TestMethod()]
        public void RaisingEdge()
        {
            using (var resetEvent = new ManualResetEventSlim(false))
            {
                EventHandler<RaisingEdgeEventArgs> handler = (sender, e) =>
                {                  
                   _traceWrapper.WriteInformation("Raising: " + e.Raising.ToString() + " ; date: " + e.TimeReached.ToUniversalTime());
                   resetEvent.Set();
                };
                ITriggerProvider tr = (ITriggerProvider)(((IDevice)_devices[0]));
                tr.RaisingEdge += handler;
                ((IDevice)_devices[0]).Start();
                Assert.IsTrue(resetEvent.Wait(TimeSpan.FromMilliseconds(30000)));
                tr.RaisingEdge -= handler;
            }
        }

        [TestMethod()]
        public void FallingEdge()
        {
            using (var resetEvent = new ManualResetEventSlim(false))
            {
                EventHandler<FallingEdgeEventArgs> handler = (sender, e) =>
                {
                    _traceWrapper.WriteInformation("FallingEdge: " + e.Falling.ToString() + " ; date: " + e.TimeReached.ToUniversalTime());
                    resetEvent.Set();
                };
                ITriggerProvider tr = (ITriggerProvider)(((IDevice)_devices[0]));
                tr.FallingEdge += handler;
                ((IDevice)_devices[0]).Start();
                Assert.IsTrue(resetEvent.Wait(TimeSpan.FromMilliseconds(30000)));
                tr.FallingEdge -= handler;
            }
        }
        
        [TestMethod()]
        public void ReBoot()
        {
            using (var resetEvent = new ManualResetEventSlim(false))
            {
                EventHandler<mSCrebootEventArgs> handler = (sender, e) =>
                {
                    _traceWrapper.WriteInformation("Reboot SUCCESS: " + " ; date: " + e.TimeReached.ToUniversalTime());
                    resetEvent.Set();
                };               
                ((ILaser)_devices[0]).ReBootEvent += handler;
                ((IDevice)_devices[0]).Start();
                Thread.Sleep(5000);
                ((ILaser)_devices[0]).ReBoot();
                Assert.IsTrue(resetEvent.Wait(TimeSpan.FromMilliseconds(50000)));             
                ((ILaser)_devices[0]).ReBootEvent -= handler;
            }
        }

        [TestMethod()]
        public void SoftDOTrigger()
        {
            using (var resetEvent = new ManualResetEventSlim(false))
            {
                EventHandler<RaisingEdgeEventArgs> rhandler = (sender, e) =>
                {
                    _traceWrapper.WriteInformation("Reboot SUCCESS: " + " ; date: " + e.TimeReached.ToUniversalTime());
                    resetEvent.Set();
                };
                EventHandler<FallingEdgeEventArgs> fhandler = (sender, e) =>
                {
                    _traceWrapper.WriteInformation("Reboot SUCCESS: " + " ; date: " + e.TimeReached.ToUniversalTime());
                    resetEvent.Set();
                };
                ITriggerProvider tr = (ITriggerProvider)(((IDevice)_devices[0]));
                tr.RaisingEdge += rhandler;
                tr.FallingEdge += fhandler;
                ((IDevice)_devices[0]).Start();
                Thread.Sleep(3000);
                ((ILaser)_devices[0]).DOSetOutput(1, 1);              
                ((ILaser)_devices[0]).DOOutput();
                Assert.IsTrue(resetEvent.Wait(TimeSpan.FromMilliseconds(3000)));
                tr.RaisingEdge -= rhandler;
                tr.FallingEdge -= fhandler;
            }
        }

        [TestMethod()]
        public void DeviceIdent()
        {
            using (var resetEvent = new ManualResetEventSlim(false))
            {
                EventHandler<DeviceIdentEventArgs> handler = (sender, e) =>
                {
                    _traceWrapper.WriteInformation("DeviceIdent: " + e.IdentInformation);
                    resetEvent.Set();
                };
                ((ILaser)_devices[0]).DeviceIdentEvent += handler;
                ((IDevice)_devices[0]).Start();               
                Assert.IsTrue(resetEvent.Wait(TimeSpan.FromMilliseconds(5000)));
                ((ILaser)_devices[0]).DeviceIdentEvent -= handler;
            }
        }
    }
}

