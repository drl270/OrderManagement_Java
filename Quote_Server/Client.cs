using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Quote_Server
{
    public static class Client
    {
        const int bytes_to_read = 255;
        static byte[] readBuffer = new byte[bytes_to_read];
        static string quoteBuffer = "";
        static TcpClient tcpClient;
        static NetworkStream quoteStream;

        public static event EventHandler<QuoteEventArgs> QuoteUpdate;

        public static void establishClientConnection()
        {
            try
            {
                tcpClient = new TcpClient(SharedLocal.IP_Remote, SharedLocal.Port_Remote);
                quoteStream = tcpClient.GetStream();
                quoteStream.BeginRead(readBuffer, 0, bytes_to_read, doRead, tcpClient);
            }
            catch (Exception exception) 
            {
                MessageBox.Show(Convert.ToString(exception));
            }
        }

        public static bool ShutDown_TCPData()
        {
            if (tcpClient != null)
            {
                if (tcpClient.Connected)
                    tcpClient.Close();

                if (tcpClient.Connected == false)
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        static void doRead(IAsyncResult ar)
        {
            TcpClient tcpClient = (TcpClient)ar.AsyncState;
            try
            {
                int bytesRead = tcpClient.GetStream().EndRead(ar);
                if (bytesRead > 0)
                {
                    string quote = System.Text.Encoding.UTF8.GetString(readBuffer, 0, bytesRead);
                    proccessQuote(quote);
                }
            }
            catch (Exception exception) { MessageBox.Show(Convert.ToString(exception)); }
            try
            {
                NetworkStream quoteStream = tcpClient.GetStream();
                quoteStream.BeginRead(readBuffer, 0, bytes_to_read, doRead, tcpClient);
            }
            catch (Exception e) { MessageBox.Show(Convert.ToString(e)); }
        }

        static void proccessQuote(string quote)
        {
            try
            {
                if (quoteBuffer.Length > 0)
                {
                    quote = quoteBuffer + quote;
                }
                string tempQuote = "";
                for (int i = 0; i < quote.Length; i++)
                {
                    if (quote.Substring(i, 1) != "#")
                    {
                        if (quote.Substring(i, 1) == "%")
                        {
                            createQuoteObject(tempQuote);
                            tempQuote = "";
                        }
                        else { tempQuote = tempQuote + quote.Substring(i, 1); }
                    }
                }
                quoteBuffer = tempQuote;
            }
            catch (Exception exception) { MessageBox.Show(Convert.ToString(exception)); }
        }

        static void createQuoteObject(string quote)
        {
            try
            {
                string[] parsedQuote = quote.Split(',');
                QuoteEventArgs obj = null;
                string symbol = parsedQuote[0];
                string bid = parsedQuote[1];
                string ask = parsedQuote[2];
                string lastPrice = parsedQuote[7];
                string close = parsedQuote[8];
                string high = parsedQuote[9];
                string low = parsedQuote[10];             

                if (parsedQuote.Length == 11)
                {
                    obj = new QuoteEventArgs(parsedQuote[0], Convert.ToDouble(bid), Convert.ToDouble(ask), Convert.ToInt32(parsedQuote[3]), Convert.ToInt32(parsedQuote[4]), Convert.ToInt32(parsedQuote[5]), Convert.ToInt32(parsedQuote[6]), Convert.ToDouble(lastPrice), Convert.ToDouble(close), Convert.ToDouble(high), Convert.ToDouble(low));
                }
                else if (parsedQuote.Length == 3)
                {
                    obj = new QuoteEventArgs(parsedQuote[0], Convert.ToDouble(bid), Convert.ToDouble(ask));
                }
                else if (parsedQuote.Length == 5)
                {
                    obj = new QuoteEventArgs(parsedQuote[0], Convert.ToDouble(bid), Convert.ToDouble(ask), Convert.ToDateTime(parsedQuote[3]), Convert.ToInt64(parsedQuote[4]));
                }


                if (QuoteUpdate != null && obj != null)
                {
                    EventHandler<QuoteEventArgs> UpdateHandler = QuoteUpdate;
                    UpdateHandler(null, obj);
                }
            }
            catch (Exception ex) { }           
        }
    }
}
