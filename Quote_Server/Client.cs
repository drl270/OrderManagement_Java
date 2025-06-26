using System;
using System.Buffers;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace QuoteServer
{
    public class Client : IDisposable
    {
        private const int ReadBufferSize = 1024;
        private readonly byte[] _readBuffer;
        private byte[] _quoteBuffer;
        private int _quoteBufferLength;
        private int _processedOffset;
        private readonly TcpClient _tcpClient;
        private NetworkStream _quoteStream;
        private readonly string _ipAddress;
        private readonly int _port;
        private bool _disposed;

        public event EventHandler<Quote> QuoteUpdate;

        public Client()
        {
            _ipAddress = SharedLocal.Instance.IpRemote;
            _port = SharedLocal.Instance.PortRemote;

            if (string.IsNullOrEmpty(_ipAddress)|| _port == 0)
            {
                throw new ArgumentNullException("ipAddress");
            }

            _readBuffer = new byte[ReadBufferSize];
            _quoteBuffer = ArrayPool<byte>.Shared.Rent(8192);
            _quoteBufferLength = 0;
            _processedOffset = 0;
            _tcpClient = new TcpClient();
            _quoteStream = null; 
        }

        public void EstablishClientConnection()
        {
            if (_tcpClient.Connected)
            {
                return; // Already connected
            }

            try
            {
                _tcpClient.Connect(_ipAddress, _port);
                _quoteStream = _tcpClient.GetStream();
                _quoteStream.BeginRead(_readBuffer, 0, ReadBufferSize, DoRead, null);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to connect to {_ipAddress}:{_port}", ex);
            }
        }

        public bool ShutdownTcpData()
        {
            try
            {
                if (_tcpClient != null && _tcpClient.Connected)
                {
                    _tcpClient.Close();
                }

                if (!_tcpClient.Connected)
                {
                    ArrayPool<byte>.Shared.Return(_quoteBuffer);
                    _quoteBuffer = ArrayPool<byte>.Shared.Rent(8192); // Reset for next connection
                    _quoteBufferLength = 0;
                    _processedOffset = 0;
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to shutdown TCP client", ex);
            }
        }

        private void DoRead(IAsyncResult ar)
        {
            try
            {
                if (_disposed || !_tcpClient.Connected)
                {
                    return;
                }

                int bytesRead = _quoteStream.EndRead(ar);
                if (bytesRead <= 0)
                {
                    return; // Connection closed
                }

                lock (_quoteBuffer)
                {
                    if (_quoteBufferLength + bytesRead > _quoteBuffer.Length)
                    {
                        byte[] newBuffer = ArrayPool<byte>.Shared.Rent(_quoteBuffer.Length * 2);
                        Buffer.BlockCopy(_quoteBuffer, 0, newBuffer, 0, _quoteBufferLength);
                        ArrayPool<byte>.Shared.Return(_quoteBuffer);
                        _quoteBuffer = newBuffer;
                    }

                    Buffer.BlockCopy(_readBuffer, 0, _quoteBuffer, _quoteBufferLength, bytesRead);
                    _quoteBufferLength += bytesRead;
                    ProcessQuote();
                }

                if (_tcpClient.Connected)
                {
                    _quoteStream.BeginRead(_readBuffer, 0, ReadBufferSize, DoRead, null);
                }
            }
            catch (Exception ex)
            {
                if (!_disposed && _tcpClient.Connected)
                {
                    throw new InvalidOperationException("Failed to read from TCP stream", ex);
                }
            }
        }

        private void ProcessQuote()
        {
            try
            {
                lock (_quoteBuffer)
                {
                    Span<byte> buffer = _quoteBuffer.AsSpan(0, _quoteBufferLength);
                    int offset = _processedOffset;

                    while (offset + 2 <= buffer.Length)
                    {
                        ushort symbolLength = (ushort)(buffer[offset] | (buffer[offset + 1] << 8));
                        offset += 2;

                        int fixedFieldSize = 56;
                        if (offset + symbolLength + fixedFieldSize > buffer.Length)
                        {
                            break;
                        }

                        string symbol = Encoding.UTF8.GetString(buffer.Slice(offset, symbolLength).ToArray());
                        offset += symbolLength;

                        double bid = BitConverter.ToDouble(buffer.Slice(offset, 8).ToArray(), 0);
                        offset += 8;
                        double ask = BitConverter.ToDouble(buffer.Slice(offset, 8).ToArray(), 0);
                        offset += 8;

                        int vol1 = (short)(buffer[offset] | (buffer[offset + 1] << 8));
                        offset += 2;
                        int vol2 = (short)(buffer[offset] | (buffer[offset + 1] << 8));
                        offset += 2;
                        int vol3 = (short)(buffer[offset] | (buffer[offset + 1] << 8));
                        offset += 2;
                        int vol4 = (short)(buffer[offset] | (buffer[offset + 1] << 8));
                        offset += 2;

                        double lastPrice = BitConverter.ToDouble(buffer.Slice(offset, 8).ToArray(), 0);
                        offset += 8;
                        double close = BitConverter.ToDouble(buffer.Slice(offset, 8).ToArray(), 0);
                        offset += 8;
                        double high = BitConverter.ToDouble(buffer.Slice(offset, 8).ToArray(), 0);
                        offset += 8;
                        double low = BitConverter.ToDouble(buffer.Slice(offset, 8).ToArray(), 0);
                        offset += 8;

                        CreateQuoteObject(symbol, bid, ask, vol1, vol2, vol3, vol4, lastPrice, close, high, low);
                        _processedOffset = offset;
                    }

                    if (_processedOffset > 0)
                    {
                        int remaining = _quoteBufferLength - _processedOffset;
                        if (remaining > 0)
                        {
                            Buffer.BlockCopy(_quoteBuffer, _processedOffset, _quoteBuffer, 0, remaining);
                        }
                        _quoteBufferLength = remaining;
                        _processedOffset = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to process quote data", ex);
            }
        }

        private void CreateQuoteObject(string symbol, double bid, double ask, int vol1, int vol2, int vol3, int vol4,
            double lastPrice, double close, double high, double low)
        {
            try
            {
                if (string.IsNullOrEmpty(symbol))
                {
                    throw new ArgumentException("Symbol cannot be empty.");
                }

                Quote quote = new Quote(symbol, bid, ask, vol1, vol2, vol3, vol4, lastPrice, close, high, low);
                QuoteUpdate?.Invoke(this, quote);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create quote for symbol {symbol}", ex);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                try
                {
                    if (_tcpClient != null)
                    {
                        if (_tcpClient.Connected)
                        {
                            _tcpClient.Close();
                        }
                        _tcpClient.Dispose();
                    }
                    if (_quoteBuffer != null)
                    {
                        ArrayPool<byte>.Shared.Return(_quoteBuffer);
                        _quoteBuffer = null;
                    }
                }
                catch (Exception ex)
                {
                    // Log error
                }
            }
        }
    }
}
