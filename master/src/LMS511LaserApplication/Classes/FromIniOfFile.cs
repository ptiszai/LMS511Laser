using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Brace.Shared.Core.Classes;

//-----------------------------------
namespace LMS511LaserApplication
{
    class FromIniOfFile : IDisposable
    {
        #region Constans
        public const string rootDir = "c:\\users\\public\\LMS511\\";
        public const string iniFile = "lms511.ini";
        #endregion

        #region Property variables  
        public bool FactoryDefault;
        public string IpAddress { get; internal set; }
        public int TriggerOutputChannelNumber { get; internal set; }       
        public string Password_maintenance { get; internal set; }
        public string Password_authorized_client { get; internal set; }
        public string Password_service { get; internal set; }
        #endregion

        #region private variables
        private bool disposed = false;
        #endregion

        #region Read datas from INI file
        public bool ReadData()
        {
            string _value;
            int _itemp;
            try
            {
                if (!File.Exists(rootDir + iniFile))
                    throw new Exception(rootDir + iniFile + " not founded!");
                _value = SetOrGetIniFile.GetIniFileString(rootDir + iniFile, "General", "factory", "0");
                _itemp = Convert.ToInt32(_value);
                FactoryDefault = (_itemp > 0) ? true : false;
                if (FactoryDefault)
                    return true;

                _value = SetOrGetIniFile.GetIniFileString(rootDir + iniFile, "General", "ip_address", "127.0.0.1");
                if (_value == "")
                {
                    throw new Exception(rootDir + iniFile + " file error: " + " Ip address is empty!");
                }
                if (IPAddress.Parse(_value) != null)
                    IpAddress = _value;    
            
                _value = SetOrGetIniFile.GetIniFileString(rootDir + iniFile, "General", "trigger_output_channel_number", "1");
                if (_value == "")
                {
                    throw new Exception(rootDir + iniFile + " file error: " + " trigger_output_channel_number is empty!");
                }
                TriggerOutputChannelNumber = Convert.ToInt32(_value);               

                _value = SetOrGetIniFile.GetIniFileString(rootDir + iniFile, "password", "maintenance", "");
                if (_value == "")
                {
                    throw new Exception(rootDir + iniFile + " file error: " + " password.maintenance is empty!");
                }
                Password_maintenance = _value;

                _value = SetOrGetIniFile.GetIniFileString(rootDir + iniFile, "password", "authorized_client", "");
                if (_value == "")
                {
                    throw new Exception(rootDir + iniFile + " file error: " + " password.authorized_client is empty!");
                }
                Password_authorized_client = _value;

                _value = SetOrGetIniFile.GetIniFileString(rootDir + iniFile, "password", "service", "");
                if (_value == "")
                {
                    throw new Exception(rootDir + iniFile + " file error: " + " password.service is empty!");
                }
                Password_service = _value;
            }
            catch (Exception)
            { 
                throw;                    
            }           
            return true;
        }
        #endregion

        #region Write datas from INI file
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                   
                }
            }

            disposed = true;
        }
        #endregion
    }
}
