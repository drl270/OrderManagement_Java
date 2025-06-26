using System;
using System.Threading;

namespace QuoteServer
{
    [Serializable]
    public class Quote : EventArgs
    {
        private int _bidSize;
        private int _askSize;
        private int _volume;
        private int _lastQuantity;
        private double _lastPrice;
        private double _close;
        private double _high;
        private double _low;

        public string Symbol { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public double Price => (Bid + Ask) / 2;
        public DateTime Time { get; set; }
        public long Ticks { get; set; }

        public Quote() { }

        public Quote(string symbol, double bid, double ask)
        {
            Symbol = symbol;
            Bid = bid;
            Ask = ask;
            Time = DateTime.Now;
            Ticks = DateTime.Now.Ticks;
        }

        public Quote(string symbol, double bid, double ask, DateTime dateTime, long ticks)
        {
            Symbol = symbol;
            Bid = bid;
            Ask = ask;
            Time = dateTime;
            Ticks = ticks;
        }

        public Quote(string symbol, double bid, double ask, int bidSize, int askSize,
            int volume, int lastQuantity, double lastPrice, double closePrice,
            double high, double low)
        {
            Symbol = symbol;
            Bid = bid;
            Ask = ask;
            Time = DateTime.Now;
            Ticks = DateTime.Now.Ticks;
            _bidSize = bidSize;
            _askSize = askSize;
            _volume = volume;
            _lastQuantity = lastQuantity;
            _lastPrice = lastPrice;
            _close = closePrice;
            _high = high;
            _low = low;
        }

        public Quote(Quote quote)
        {
            Symbol = quote.Symbol;
            Bid = quote.Bid;
            Ask = quote.Ask;
            Time = quote.Time;
            Ticks = quote.Ticks;
            _bidSize = quote.BidSize;
            _askSize = quote.AskSize;
            _volume = quote.Volume;
            _lastQuantity = quote.LastQuantity;
            _lastPrice = quote.LastPrice;
            _close = quote.Close;
            _high = quote.High;
            _low = quote.Low;
        }

        public int BidSize
        {
            get => Interlocked.CompareExchange(ref _bidSize, 0, 0);
            set => Interlocked.Exchange(ref _bidSize, value);
        }

        public int AskSize
        {
            get => Interlocked.CompareExchange(ref _askSize, 0, 0);
            set => Interlocked.Exchange(ref _askSize, value);
        }

        public int Volume
        {
            get => Interlocked.CompareExchange(ref _volume, 0, 0);
            set => Interlocked.Exchange(ref _volume, value);
        }

        public int LastQuantity
        {
            get => Interlocked.CompareExchange(ref _lastQuantity, 0, 0);
            set => Interlocked.Exchange(ref _lastQuantity, value);
        }

        public double LastPrice
        {
            get => Interlocked.CompareExchange(ref _lastPrice, 0, 0);
            set => Interlocked.Exchange(ref _lastPrice, value);
        }

        public double Close
        {
            get => Interlocked.CompareExchange(ref _close, 0, 0);
            set => Interlocked.Exchange(ref _close, value);
        }

        public double High
        {
            get => Interlocked.CompareExchange(ref _high, 0, 0);
            set => Interlocked.Exchange(ref _high, value);
        }

        public double Low
        {
            get => Interlocked.CompareExchange(ref _low, 0, 0);
            set => Interlocked.Exchange(ref _low, value);
        }
    }
}