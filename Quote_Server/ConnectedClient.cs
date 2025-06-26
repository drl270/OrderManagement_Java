using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace QuoteServer
{
    public class ConnectedClient : IDisposable
    {
        private const int BufferSize = 1024;
        private readonly byte[] _readBuffer;
        private readonly NetworkStream _networkStream;
        private readonly TcpClient _tcpClient;
        private readonly Form1 _parentForm;
        private readonly Client _client;
        private readonly Dictionary<string, BidAsk> _bidAskDictionary;
        private readonly List<string> _symbolList;
        private readonly object _messageBufferLock;
        private readonly object _symbolListLock;
        private string _messageBuffer;
        private bool _includeAllData;
        private DateTime _lastConnectionTestTime;
        private DateTime _quoteTimeStamp;
        private string _userId;
        private string _lastSymbol;
        private double _lastBid;
        private double _lastAsk;
        private bool _sendAllSymbols;
        private bool _disposed;

        public event EventHandler<string> ConnectionLost;

        public string UserId
        {
            get => Interlocked.CompareExchange(ref _userId, null, null);
            set
            {
                Interlocked.Exchange(ref _userId, value);
                SendTextDataToClient($"Connected,{value}");
            }
        }

        public ConnectedClient(TcpClient client, Form1 form, Client quoteClient)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (form == null) throw new ArgumentNullException(nameof(form));
            if (quoteClient == null) throw new ArgumentNullException(nameof(quoteClient));

            _tcpClient = client;
            _parentForm = form;
            _client = quoteClient;
            _readBuffer = new byte[BufferSize];
            _bidAskDictionary = new Dictionary<string, BidAsk>();
            _symbolList = new List<string>();
            _messageBuffer = string.Empty;
            _messageBufferLock = new object();
            _symbolListLock = new object();
            _includeAllData = false;
            _sendAllSymbols = false;
            _lastConnectionTestTime = DateTime.Now;
            _quoteTimeStamp = DateTime.Now;
            _lastSymbol = string.Empty;
            _lastBid = 0;
            _lastAsk = 0;

            _networkStream = _tcpClient.GetStream();
            _client.QuoteUpdate += ClientQuoteUpdate;
            _parentForm.TestConnectionStatus += FormTestConnectionStatus;
            BeginRead();
        }

        public DisplayObject GetDisplayObject()
        {
            return new DisplayObject(
                UserId,
                _lastSymbol,
                _lastBid.ToString(),
                _lastAsk.ToString(),
                _quoteTimeStamp.ToString(),
                _lastConnectionTestTime.ToString());
        }

        private void FormTestConnectionStatus()
        {
            TimeSpan elapsed = DateTime.Now - _lastConnectionTestTime;
            if (elapsed.TotalSeconds > SharedLocal.Instance.MaxNoResponseTime)
            {
                HandleConnectionLost();
            }
        }

        private void ClientQuoteUpdate(object sender, Quote quote)
        {
            if (quote == null) return;

            lock (_symbolListLock)
            {
                if (_symbolList.Contains(quote.Symbol) || _sendAllSymbols)
                {
                    if (_sendAllSymbols)
                    {
                        HandleSendAllUpdate(quote);
                    }
                    else
                    {
                        HandleNormalUpdate(quote);
                    }

                    UpdateLastQuoteInfo(quote);
                }
            }
        }

        private void BeginRead()
        {
            try
            {
                if (!_disposed && _tcpClient.Connected)
                {
                    _networkStream.BeginRead(_readBuffer, 0, BufferSize, DoRead, null);
                }
            }
            catch (Exception ex)
            {
                // Log error
                // SharedLocal.Instance.AddListGeneralInfo($"BeginRead error for client {UserId}: {ex.Message}");
                HandleConnectionLost();
            }
        }

        private void DoRead(IAsyncResult ar)
        {
            try
            {
                if (_disposed || !_tcpClient.Connected) return;

                int bytesRead = _networkStream.EndRead(ar);
                if (bytesRead <= 0)
                {
                    HandleConnectionLost();
                    return;
                }

                string message = Encoding.UTF8.GetString(_readBuffer, 0, bytesRead);
                ProcessMessage(message);

                BeginRead();
            }
            catch (Exception ex)
            {
                // Log error
                // SharedLocal.Instance.AddListGeneralInfo($"Read error for client {UserId}: {ex.Message}");
                HandleConnectionLost();
            }
        }

        private void ProcessMessage(string message)
        {
            try
            {
                lock (_messageBufferLock)
                {
                    if (_messageBuffer.Length > 0)
                    {
                        message = _messageBuffer + message;
                    }

                    string tempMessageBuffer = string.Empty;
                    for (int i = 0; i < message.Length; i++)
                    {
                        char currentChar = message[i];
                        if (currentChar != '#')
                        {
                            if (currentChar == '%')
                            {
                                ProcessCommand(tempMessageBuffer);
                                tempMessageBuffer = string.Empty;
                            }
                            else
                            {
                                tempMessageBuffer += currentChar;
                            }
                        }
                    }
                    _messageBuffer = tempMessageBuffer;
                }
            }
            catch (Exception ex)
            {
                // Log error instead of MessageBox
                // SharedLocal.Instance.AddListGeneralInfo($"Message processing error for client {UserId}: {ex.Message}");
                throw new InvalidOperationException($"Failed to process message for client {UserId}", ex);
            }
        }

        private void ProcessCommand(string command)
        {
            string[] parsedCommand = command.Split(',');
            if (parsedCommand.Length == 0) return;

            switch (parsedCommand[0])
            {
                case "List":
                    ProcessListCommand(parsedCommand);
                    break;
                case "ALL":
                    lock (_symbolListLock)
                    {
                        _sendAllSymbols = true;
                    }
                    break;
                case "Tick_Data":
                    ProcessTickDataCommand(parsedCommand);
                    break;
                case "List_ManualTrade":
                    ProcessManualTradeCommand(parsedCommand);
                    break;
                case "TestConnection":
                    ProcessKeepAlive();
                    break;
                default:
                    // Log unknown command
                    // SharedLocal.Instance.AddListGeneralInfo($"Unknown command for client {UserId}: {parsedCommand[0]}");
                    break;
            }
        }

        private void ProcessListCommand(string[] parsedCommand)
        {
            lock (_symbolListLock)
            {
                foreach (string symbol in parsedCommand)
                {
                    if (symbol != "List" && !string.IsNullOrEmpty(symbol) && !_bidAskDictionary.ContainsKey(symbol))
                    {
                        _symbolList.Add(symbol);
                        _bidAskDictionary.Add(symbol, new BidAsk());
                    }
                }
            }
        }

        private void ProcessTickDataCommand(string[] parsedCommand)
        {
            lock (_symbolListLock)
            {
                _includeAllData = true;
                foreach (string symbol in parsedCommand)
                {
                    if (symbol != "Tick_Data" && !string.IsNullOrEmpty(symbol) && !_symbolList.Contains(symbol))
                    {
                        _symbolList.Add(symbol);
                    }
                }
            }
        }

        private void ProcessManualTradeCommand(string[] parsedCommand)
        {
            lock (_symbolListLock)
            {
                _includeAllData = true;
                foreach (string symbol in parsedCommand)
                {
                    if (symbol != "List_ManualTrade" && !string.IsNullOrEmpty(symbol) && !_symbolList.Contains(symbol))
                    {
                        _symbolList.Add(symbol);
                    }
                }
            }
        }

        private void ProcessKeepAlive()
        {
            try
            {
                SendTextDataToClient("ConnectionConfirmed");
                _lastConnectionTestTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                // Log error
                // SharedLocal.Instance.AddListGeneralInfo($"Keep-alive error for client {UserId}: {ex.Message}");
                HandleConnectionLost();
            }
        }

        private void HandleSendAllUpdate(Quote quote)
        {
            lock (_symbolListLock)
            {
                if (_bidAskDictionary.ContainsKey(quote.Symbol))
                {
                    if (HasPriceChanged(quote))
                    {
                        SendTextDataToClient($"{quote.Symbol},{quote.Bid},{quote.Ask}");
                        UpdateBidAsk(quote.Symbol, quote.Bid, quote.Ask);
                    }
                }
                else
                {
                    _bidAskDictionary.Add(quote.Symbol, new BidAsk { Bid = quote.Bid, Ask = quote.Ask });
                    SendTextDataToClient($"{quote.Symbol},{quote.Bid},{quote.Ask}");
                }
            }
        }

        private void HandleNormalUpdate(Quote quote)
        {
            lock (_symbolListLock)
            {
                if (_includeAllData)
                {
                    string fullData = $"{quote.Symbol},{quote.Bid},{quote.Ask},{quote.BidSize},{quote.AskSize}," +
                                      $"{quote.Volume},{quote.LastQuantity},{quote.LastPrice},{quote.Close},{quote.High},{quote.Low}";
                    SendTextDataToClient(fullData);
                }
                else
                {
                    if (_bidAskDictionary.ContainsKey(quote.Symbol))
                    {
                        if (HasPriceChanged(quote))
                        {
                            SendTextDataToClient($"{quote.Symbol},{quote.Bid},{quote.Ask}");
                            UpdateBidAsk(quote.Symbol, quote.Bid, quote.Ask);
                        }
                    }
                    else
                    {
                        _bidAskDictionary.Add(quote.Symbol, new BidAsk { Bid = quote.Bid, Ask = quote.Ask });
                        SendTextDataToClient($"{quote.Symbol},{quote.Bid},{quote.Ask}");
                    }
                }
            }
        }

        private bool HasPriceChanged(Quote quote)
        {
            return _bidAskDictionary[quote.Symbol].Bid != quote.Bid ||
                   _bidAskDictionary[quote.Symbol].Ask != quote.Ask;
        }

        private void UpdateBidAsk(string symbol, double bid, double ask)
        {
            _bidAskDictionary[symbol].Bid = bid;
            _bidAskDictionary[symbol].Ask = ask;
        }

        private void UpdateLastQuoteInfo(Quote quote)
        {
            _lastSymbol = quote.Symbol;
            _lastBid = quote.Bid;
            _lastAsk = quote.Ask;
            _quoteTimeStamp = quote.Time;
        }

        private void SendTextDataToClient(string message)
        {
            try
            {
                if (_disposed || !_tcpClient.Connected) return;

                byte[] data = Encoding.UTF8.GetBytes($"#{message}%");
                _networkStream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                // Log error
                // SharedLocal.Instance.AddListGeneralInfo($"Send text data error for client {UserId}: {ex.Message}");
                HandleConnectionLost();
                throw new InvalidOperationException($"Failed to send text data to client {UserId}", ex);
            }
        }

        private void SendQuoteToClient(string symbol, Quote quote)
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(128);
            try
            {
                Span<byte> span = buffer.AsSpan();
                int offset = 0;

                byte[] symbolBytes = Encoding.UTF8.GetBytes(symbol);
                WriteInt16LittleEndian(span, offset, (short)symbolBytes.Length);
                offset += 2;
                symbolBytes.CopyTo(span.Slice(offset, symbolBytes.Length));
                offset += symbolBytes.Length;

                byte[] bidBytes = BitConverter.GetBytes(quote.Bid);
                bidBytes.CopyTo(span.Slice(offset, 8));
                offset += 8;

                byte[] askBytes = BitConverter.GetBytes(quote.Ask);
                askBytes.CopyTo(span.Slice(offset, 8));
                offset += 8;

                WriteInt32LittleEndian(span, offset, quote.BidSize);
                offset += 4;
                WriteInt32LittleEndian(span, offset, quote.AskSize);
                offset += 4;
                WriteInt32LittleEndian(span, offset, quote.Volume);
                offset += 4;
                WriteInt32LittleEndian(span, offset, quote.LastQuantity);
                offset += 4;

                byte[] lastPriceBytes = BitConverter.GetBytes(quote.LastPrice);
                lastPriceBytes.CopyTo(span.Slice(offset, 8));
                offset += 8;

                byte[] closeBytes = BitConverter.GetBytes(quote.Close);
                closeBytes.CopyTo(span.Slice(offset, 8));
                offset += 8;

                byte[] highBytes = BitConverter.GetBytes(quote.High);
                highBytes.CopyTo(span.Slice(offset, 8));
                offset += 8;

                byte[] lowBytes = BitConverter.GetBytes(quote.Low);
                lowBytes.CopyTo(span.Slice(offset, 8));
                offset += 8;

                _networkStream.Write(buffer, 0, offset);
            }
            catch (Exception ex)
            {
                // Log error
                // SharedLocal.Instance.AddListGeneralInfo($"Send quote error for client {UserId}: {ex.Message}");
                HandleConnectionLost();
                throw new InvalidOperationException($"Failed to send quote to client {UserId}", ex);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        private void WriteInt16LittleEndian(Span<byte> span, int offset, short value)
        {
            span[offset] = (byte)(value & 0xFF);
            span[offset + 1] = (byte)((value >> 8) & 0xFF);
        }

        private void WriteInt32LittleEndian(Span<byte> span, int offset, int value)
        {
            span[offset] = (byte)(value & 0xFF);
            span[offset + 1] = (byte)((value >> 8) & 0xFF);
            span[offset + 2] = (byte)((value >> 16) & 0xFF);
            span[offset + 3] = (byte)((value >> 24) & 0xFF);
        }

        private void HandleConnectionLost()
        {
            if (_disposed) return;

            ConnectionLost?.Invoke(this, UserId);
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            try
            {
                _parentForm.TestConnectionStatus -= FormTestConnectionStatus;
                _client.QuoteUpdate -= ClientQuoteUpdate;
                if (_tcpClient.Connected)
                {
                    _tcpClient.Close();
                }
                _networkStream?.Dispose();
                _tcpClient.Dispose();
            }
            catch (Exception ex)
            {
                // Log error
                // SharedLocal.Instance.AddListGeneralInfo($"Dispose error for client {UserId}: {ex.Message}");
            }
        }
    }

    public class BidAsk
    {
        private double _bid;
        private double _ask;

        public double Bid
        {
            get => Interlocked.CompareExchange(ref _bid, 0, 0);
            set => Interlocked.Exchange(ref _bid, value);
        }

        public double Ask
        {
            get => Interlocked.CompareExchange(ref _ask, 0, 0);
            set => Interlocked.Exchange(ref _ask, value);
        }
    }
}