using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net;

namespace Candid.Shared.Drivers.Laser
{
    public class LMS5xx : CMDClient
    {
        #region  Variable public
        public event Action<string> DeviceMessage;
        public event Action<string> DeviceError;
        
      //  public event Action<int> DeviceConnection;
        public string ipAddress;
        public int ipPort;
        #endregion
        #region  Variable private
   

        #endregion    

        #region Constructor
        public LMS5xx(IPAddress serverIP, int port)
            : base(serverIP, port)
        {
          
        }
        #endregion
        #region Prublic Methods
        public void Connect()
        {                      
            CommandSent += new CommandSentEventHandler(client_CommandSent);
            CommandSendingFailed += new CommandSendingFailedEventHandler(client_CommandSendingFailed);
            CommandReceivingFailed += new CommandReceivingFailedEventHandler(client_CommandReceivingFailed);
            ServerDisconnected += new ServerDisconnectedEventHandler(client_ServerDisconnected);
            DisconnectedFromServer += new DisconnectedEventHandler(client_DisconnectedFromServer);
            ConnectingSuccessed += new ConnectingSuccessedEventHandler(client_ConnectingSuccessed);
            ConnectingFailed += new ConnectingFailedEventHandler(client_ConnectingFailed);
            NetworkDead += new NetworkDeadEventHandler(client_NetworkDead);
            NetworkAlived += new NetworkAlivedEventHandler(client_NetworkAlived);
            ConnectToServer();
        }

        public void Disconnect()
        {
            base.Disconnect();
        }

        public void sendCmd(Object cmd) 
        {
            SendCommand(cmd);
        }
        #endregion
        #region Events
      /*  void client_CommandReceived(object sender, CommandEventArgs e)
        {
            PrintMsg(e.Command.Cmd);
        }*/
        void client_CommandSent(object sender, MessageEventArgs e)
        {
            PrintMsg(e.Msg);
        }
        void client_CommandSendingFailed(object sender, MessageEventArgs e)
        {
            PrintMsg(e.Msg);
        }

        void client_CommandReceivingFailed(object sender, MessageEventArgs e)
        {
            PrintMsg(e.Msg);
        }

        void client_ServerDisconnected(object sender, ServerEventArgs e)
        {
            PrintMsg(e.ToString());
        }

        void client_DisconnectedFromServer(object sender, MessageEventArgs e)
        {
            PrintMsg(e.Msg);
        }
        void client_ConnectingSuccessed(object sender, MessageEventArgs e)
        {
            PrintMsg(e.Msg);
        }
        void client_ConnectingFailed(object sender, MessageEventArgs e)
        {
            PrintMsg(e.Msg);
        }
        void client_NetworkDead(object sender, MessageEventArgs e)
        {
            PrintMsg(e.Msg);
        }
        void client_NetworkAlived(object sender, MessageEventArgs e)
        {
            PrintMsg(e.Msg);
        }
        public void PrintMsg(string msg)
        {
            if (DeviceMessage != null)
            {
                DeviceMessage(msg);
            }
            Console.WriteLine(msg);
        }

        public void ErrorMsg(string msg)
        {
            if (DeviceError != null)
            {
                DeviceError(msg);
            }
            Console.WriteLine(msg);
        }
        #endregion
    }
}
