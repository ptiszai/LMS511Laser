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
using System.Text;
using System.Net;
using System.Net.Sockets;
using Brace.Shared.DeviceDrivers.LMS511Laser.Enums;

namespace Brace.Shared.DeviceDrivers.LMS511Laser.EventHandlers
{   
    // Occurs when a command received from the server.    
    public delegate void CommandReceivedEventHandler(object sender , CommandEventArgs e);

     // Occurs when a command receiving action had been failed.   
    public delegate void CommandReceivingFailedEventHandler(object sender, MessageEventArgs e);
    
    // Occurs when a command had been sent to the the remote server Successfully.   
    public delegate void CommandSentEventHandler(object sender, MessageEventArgs e);
   
    // Occurs when a command sending action had been failed.This is because disconnection or sending exception.   
    public delegate void CommandSendingFailedEventHandler(object sender, MessageEventArgs e);
    
    // The class that contains information about a command.    
    public class CommandEventArgs : EventArgs
    {
        private Object _data;
        private CommandType _type;
        public Object Data
        {
            get { return _data; }
        }
        public CommandType Type
        {
            get { return _type; }
        }
        public CommandEventArgs(Object data, CommandType ctype)
        {
            _data = data;
            _type = CommandType.None;
        }
    }
     
    // Occurs when the server had been disconnected from this client.    
    public delegate void ServerDisconnectedEventHandler(object sender , ServerEventArgs e);       
    public class ServerEventArgs : EventArgs
    {
        private Socket _socket;       
        public IPAddress IP
        {
            get { return ( (IPEndPoint)_socket.RemoteEndPoint ).Address; }
        }     
        public int Port
        {
            get { return ( (IPEndPoint)_socket.RemoteEndPoint ).Port; }
        }
        public ServerEventArgs(Socket clientSocket)
        {
            _socket = clientSocket;
        }
    }

    // General message sender EventArg.
    public class MessageEventArgs : EventArgs
    {
        private string _msg;
        public string Msg  
        {
            get { return _msg; }            
        }
        public MessageEventArgs(String msg)
        {
            _msg = msg;
        }
    }
   
    // Occurs when this client disconnected from the server.  
    public delegate void DisconnectedEventHandler(object sender, MessageEventArgs e);
    
    // Occurs when this client connected to the remote server Successfully.    
    public delegate void ConnectingSuccessedEventHandler(object sender, MessageEventArgs e);
   
    // Occurs when this client failed on connecting to server.   
    public delegate void ConnectingFailedEventHandler(object sender, MessageEventArgs e);
  
    // Occurs when the network had been failed.   
    public delegate void NetworkDeadEventHandler(object sender, MessageEventArgs e);
   
    // Occurs when the network had been started to work.  
    public delegate void NetworkAlivedEventHandler(object sender, MessageEventArgs e);
}
