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

namespace Quote_Server
{
    public class ConnectedClient
    {
        private const Int32 bytes_to_read = 1024;
        private byte[] readBuffer = new byte[bytes_to_read];
        NetworkStream _NetworkStream;
        TcpClient _TcpClient;
        private string MessageBuffer = "";
        private bool _IncludeAllData = false;

        private DateTime _LastConnectionTestTime;
        private DateTime _QuoteTimeStamp;
        private string _UserID;
        private string _LastSymbol = "";
        private double _LastBid = 0;
        private double _LastAsk = 0;
        private Form1 _Form1;

        private object _UserID_Obj = new object();

        private Dictionary<string, Bid_Ask> Dictionary_BidAskObjects = new Dictionary<string, Bid_Ask>();

        public event EventHandler<string> ConnectedClientConnectionLost;

        private List<string> List_Symbols = new List<string>();
        private bool SendAll = false;

        public ConnectedClient(TcpClient tcpClient, Form1 p_Form1)
        {
            _Form1 = p_Form1;
            Client.QuoteUpdate += Client_QuoteUpdate;
            p_Form1.TestConnectionStatus += P_Form1_TestConnectionStatus; 
            _TcpClient = tcpClient;
            _NetworkStream = _TcpClient.GetStream();
            _NetworkStream.BeginRead(readBuffer, 0, bytes_to_read, DoRead, _TcpClient);
            _LastConnectionTestTime = DateTime.Now;
        }

        private void P_Form1_TestConnectionStatus()
        {
            TimeSpan ts = DateTime.Now - _LastConnectionTestTime;
            if (ts.Seconds > SharedLocal.MaxNoResponseTime)
            {
                _Form1.TestConnectionStatus -= P_Form1_TestConnectionStatus;
                Client.QuoteUpdate -= Client_QuoteUpdate;
                Connectionlost();
            }
        }

        private void Client_QuoteUpdate(object sender, QuoteEventArgs e)
        {
            if (List_Symbols.Contains(e.Symbol) || SendAll)
            {
               if (SendAll)
                {
                    if (Dictionary_BidAskObjects.ContainsKey(e.Symbol))
                    {
                        if (Dictionary_BidAskObjects[e.Symbol].Bid != e.Bid || Dictionary_BidAskObjects[e.Symbol].Ask != e.Ask)
                        {
                            SendDataToClient(e.Symbol + "," + e.Bid.ToString() + "," + e.Ask.ToString());
                            Dictionary_BidAskObjects[e.Symbol].Bid = e.Bid;
                            Dictionary_BidAskObjects[e.Symbol].Ask = e.Ask;
                        }
                    }
                    else
                    {
                        Dictionary_BidAskObjects.Add(e.Symbol, new Bid_Ask());
                        Dictionary_BidAskObjects[e.Symbol].Bid = e.Bid;
                        Dictionary_BidAskObjects[e.Symbol].Ask = e.Ask;
                        SendDataToClient(e.Symbol + "," + e.Bid.ToString() + "," + e.Ask.ToString());                      
                    }
                }                
               else
                {
                    if (_IncludeAllData)
                    {
                        SendDataToClient(e.Symbol + "," + e.Bid.ToString() + "," + e.Ask.ToString() + "," + e.BidSize.ToString() + "," + e.AskSize.ToString() + "," + e.Volume.ToString() + "," + e.LastQuantity.ToString() + "," + e.LAST_PRICE.ToString() + "," + e.CLOSE.ToString() + "," + e.HIGH.ToString() + "," + e.LOW.ToString());
                    }
                    else
                    {
                        if (Dictionary_BidAskObjects[e.Symbol].Bid != e.Bid || Dictionary_BidAskObjects[e.Symbol].Ask != e.Ask)
                        {
                            SendDataToClient(e.Symbol + "," + e.Bid.ToString() + "," + e.Ask.ToString());
                            Dictionary_BidAskObjects[e.Symbol].Bid = e.Bid;
                            Dictionary_BidAskObjects[e.Symbol].Ask = e.Ask;
                        }
                    }
                }
               
                _LastSymbol = e.Symbol;
                _LastBid = e.Bid;
                _LastAsk = e.Ask;
                _QuoteTimeStamp = e.Time;
            }
        }

        public DisplayObject Get_displayObject()
        {
            return new DisplayObject(_UserID, _LastSymbol, _LastBid.ToString(), _LastAsk.ToString(), _QuoteTimeStamp.ToString(), _LastConnectionTestTime.ToString());
        }

        public string UserID
        {
            get { lock (_UserID_Obj) { return _UserID; } }
            set
            {
                lock (_UserID_Obj)
                {
                    _UserID = value;
                    SendDataToClient("Connected" + "," + _UserID);
                }
            }
        }

        private void DoRead(IAsyncResult ar)
        {
            TcpClient tcpClient = (TcpClient)ar.AsyncState;
            try
            {
                int bytesRead = tcpClient.GetStream().EndRead(ar);
                if (bytesRead > 0)
                {
                    string Message = System.Text.Encoding.UTF8.GetString(readBuffer, 0, bytesRead);
                    ProcessMessage(Message);
                }
            }
            catch (Exception ex) {  }
            try
            {
                NetworkStream quoteStream = tcpClient.GetStream();
                quoteStream.BeginRead(readBuffer, 0, bytes_to_read, DoRead, tcpClient);
            }
            catch (Exception ex) { }
        }

        private void ProcessMessage(string message)
        {
            try
            {
                if (MessageBuffer.Length > 0)
                    message = MessageBuffer + message;

                string TempMessageBuffer = "";

                for (int i = 0; i < message.Length; i++)
                {
                    if (message.Substring(i, 1) != "#")
                    {
                        if (message.Substring(i, 1) == "%")
                        {
                            string[] ParsedString = TempMessageBuffer.Split(',');
                            if (ParsedString[0] == "List")
                                ProcessList(TempMessageBuffer);
                            else if (ParsedString[0] == "ALL")
                                SendAll = true;
                            else if (ParsedString[0] == "Tick_Data")
                            {
                                _IncludeAllData = true;
                            }                               
                            else if (ParsedString[0] == "List_ManualTrade")
                                Process_List_ManualTrade(TempMessageBuffer);
                            else if (ParsedString[0] == "TestConnection")
                                Process_TestConnectionAlive();
                                                
                            TempMessageBuffer = "";
                        }
                        else { TempMessageBuffer = TempMessageBuffer + message.Substring(i, 1); }
                    }
                }
                MessageBuffer = TempMessageBuffer;
            }
            catch (Exception exception) { MessageBox.Show(Convert.ToString(exception)); }           
        }


        private void ProcessList(string message)
        {
            string[] ParsedString = message.Split(',');
            foreach (string Symbol in ParsedString)
            {
                if (Symbol != "List")
                {
                    List_Symbols.Add(Symbol);
                    Dictionary_BidAskObjects.Add(Symbol, new Bid_Ask());
                }
            }
        }

        private void Process_List_ManualTrade(string message)
        {
            string[] ParsedString = message.Split(',');
            _IncludeAllData = true;
            foreach (string Symbol in ParsedString)
            {
                if (Symbol != "List_ManualTrade")
                    List_Symbols.Add(Symbol);
            }
        }

        private void Process_TestConnectionAlive()
        {
            try
            {
                SendDataToClient("ConnectionConfirmed");
                _LastConnectionTestTime = DateTime.Now;
            }
            catch (Exception ex)
            {

            }
           
        }

        private void SendDataToClient(string p_message)
        {
            byte[] message = Encoding.UTF8.GetBytes("#" + p_message + "%");
            _NetworkStream.Write(message, 0, message.Length);
        }

        private void Connectionlost()
        {
            EventHandler<string> handler = ConnectedClientConnectionLost;
            handler(this, _UserID);
        }
    }

    public class Bid_Ask
    {
        private double _Bid = 0;
        private double _Ask = 0;

        private object _Bid_Object = new object();
        private object _Ask_Object = new object();

        public double Bid
        {
            get { lock (_Bid_Object) { return _Bid; } }
            set {  lock (_Bid_Object) { _Bid = value; } }
        }

        public double Ask
        {
            get {  lock (_Ask_Object) { return _Ask; } }
            set {  lock (_Ask_Object) { _Ask = value; } }
        }
    }
}
