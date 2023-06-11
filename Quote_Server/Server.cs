using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Xml.Serialization;

namespace Quote_Server
{
    public static class Server
    {
        static TcpListener tcpListener;
        static Form1 _Form1;

        static Dictionary<string, ConnectedClient> Dictionary_ConnectedClients = new Dictionary<string, ConnectedClient>();
        static Dictionary<string, DisplayObject> Dictionary_DisplayObjects = new Dictionary<string, DisplayObject>();

        public static void createTCPServer(Form1 p_Form1)
        {
            _Form1 = p_Form1;
            tcpListener = new TcpListener(IPAddress.Any, SharedLocal.Port_Local);
            tcpListener.Start();
            Thread ListenerThread = new Thread(() => BeginListen());
            ListenerThread.IsBackground = true;
            ListenerThread.Start();
        }

        private static void BeginListen()
        {
            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                ConnectedClient connectedClient = new ConnectedClient(tcpClient, _Form1);
                connectedClient.ConnectedClientConnectionLost += ConnectedClient_ConnectedClientConnectionLost;
                connectedClient.UserID = SharedLocal.Get_RemoteUserID();
                lock (Dictionary_ConnectedClients)
                {
                    Dictionary_ConnectedClients.Add(connectedClient.UserID, connectedClient);
                }
            }
        }

        public static List<DisplayObject> Get_ListDisplayObject()
        {
            List<DisplayObject> List_DisplayObjects = new List<DisplayObject>();

            lock (Dictionary_ConnectedClients)
            {
                foreach (KeyValuePair<string, ConnectedClient> pair in Dictionary_ConnectedClients)
                    List_DisplayObjects.Add(pair.Value.Get_displayObject());
            }

            return List_DisplayObjects;
        }

        public static void ShutDown_Server()
        {
           
        }

        private static void ConnectedClient_ConnectedClientConnectionLost(object sender, string e)
        {
            lock (Dictionary_ConnectedClients)
            {
                if (Dictionary_ConnectedClients.ContainsKey(e))
                    Dictionary_ConnectedClients.Remove(e);
            }
        }
    }
}
