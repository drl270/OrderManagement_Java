using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace QuoteServer
{
    public class Server : IDisposable
    {
        private readonly Dictionary<string, ConnectedClient> _connectedClients;
        private readonly Form1 _form;
        private readonly TcpListener _tcpListener;
        private bool _disposed;

        public Server(Form1 form, int port)
        {
            if (form == null)
            {
                throw new ArgumentNullException("form");
            }

            _form = form;
            _connectedClients = new Dictionary<string, ConnectedClient>();
            _tcpListener = new TcpListener(IPAddress.Any, port);
        }

        public void CreateTcpServer()
        {
            try
            {
                _tcpListener.Start();
                Thread listenerThread = new Thread(BeginListen)
                {
                    IsBackground = true
                };
                listenerThread.Start();
            }
            catch (Exception ex)
            {
                // Log error via SharedLocal or other mechanism
                // SharedLocal.Instance.AddListGeneralInfo($"Failed to start TCP server: {ex.Message}");
                throw new InvalidOperationException("Failed to start TCP server", ex);
            }
        }

        private void BeginListen()
        {
            try
            {
                while (!_disposed)
                {
                    TcpClient tcpClient = _tcpListener.AcceptTcpClient();
                    ConnectedClient connectedClient = new ConnectedClient(tcpClient, _form);
                    connectedClient.ConnectedClientConnectionLost += ConnectedClientConnectionLost;
                    connectedClient.UserID = SharedLocal.Instance.GetNextRemoteUserId();

                    lock (_connectedClients)
                    {
                        _connectedClients.Add(connectedClient.UserID, connectedClient);
                    }
                }
            }
            catch (SocketException) when (_disposed)
            {
                // Expected when server is shutting down
            }
            catch (Exception ex)
            {
                // Log error
                // SharedLocal.Instance.AddListGeneralInfo($"TCP server listen error: {ex.Message}");
            }
        }

        public List<DisplayObject> GetDisplayObjects()
        {
            List<DisplayObject> displayObjects = new List<DisplayObject>();

            lock (_connectedClients)
            {
                foreach (KeyValuePair<string, ConnectedClient> pair in _connectedClients)
                {
                    DisplayObject displayObject = pair.Value.GetDisplayObject();
                    if (displayObject != null)
                    {
                        displayObjects.Add(displayObject);
                    }
                }
            }

            return displayObjects;
        }

        public void ShutdownServer()
        {
            try
            {
                _disposed = true;
                _tcpListener.Stop();

                lock (_connectedClients)
                {
                    foreach (ConnectedClient client in _connectedClients.Values)
                    {
                        client.Dispose();
                    }
                    _connectedClients.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to shutdown server", ex);
            }
        }

        private void ConnectedClientConnectionLost(object sender, string userId)
        {
            lock (_connectedClients)
            {
                if (_connectedClients.ContainsKey(userId))
                {
                    _connectedClients.Remove(userId);
                }
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                ShutdownServer();
            }
        }
    }
}
