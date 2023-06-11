using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quote_Server
{
    [Serializable]
    public class QuoteEventArgs : EventArgs
    {
        private int _BidSize = 0;
        private int _AskSize = 0;
        private int _Volume = 0;
        private int _LastQuantity = 0;
        private double _LAST_PRICE = 0;
        private double _CLOSE = 0;
        private double _HIGH = 0;
        private double _LOW = 0;

        private object _BidSize_Object = new object();
        private object _AskSize_Object = new object();
        private object _Volume_Object = new object();
        private object _LastQuantity_Object = new object();
        private object _LAST_PRICE_Object = new object();
        private object _CLOSE_Object = new object();
        private object _HIGH_Object = new object();
        private object _LOW_Object = new object();

        public string Symbol { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public double Price { get { return (Bid + Ask) / 2; } }
        public DateTime Time { get; set; }
        public long Ticks { get; set; }

        public QuoteEventArgs()
        {

        }

        public QuoteEventArgs(String symbol, Double bid, Double ask)
        {
            this.Symbol = symbol;
            this.Bid = bid;
            this.Ask = ask;
            this.Time = DateTime.Now;
            this.Ticks = DateTime.Now.Ticks;
        }

        public QuoteEventArgs(String symbol, Double bid, Double ask, DateTime p_DateTime, Int64 p_Ticks)
        {
            this.Symbol = symbol;
            this.Bid = bid;
            this.Ask = ask;
            this.Time = p_DateTime;
            this.Ticks = p_Ticks;
        }

        public QuoteEventArgs(String symbol, Double bid, Double ask, int p_BidSize, int p_AskSize, int p_Volume, int p_LastQuantity, double p_LAST_PRICE, double p_CLOSE, double p_HIGH, double p_LOW)
        {
            this.Symbol = symbol;
            this.Bid = bid;
            this.Ask = ask;
            this.Time = DateTime.Now;
            this.Ticks = DateTime.Now.Ticks;
            _BidSize = p_BidSize;
            _AskSize = p_AskSize;
            _Volume = p_Volume;
            _LastQuantity = p_LastQuantity;
            _LAST_PRICE = p_LAST_PRICE;
            _CLOSE = p_CLOSE;
            _HIGH = p_HIGH;
            _LOW = p_LOW;
        }

        public QuoteEventArgs(QuoteEventArgs e)
        {
            this.Symbol = e.Symbol;
            this.Bid = e.Bid;
            this.Ask = e.Ask;
            this.Time = e.Time;
            this.Ticks = e.Ticks;
            _BidSize = e.BidSize;
            _AskSize = e.AskSize;
            _Volume = e.Volume;
        }

        public int BidSize
        {
            get { lock (_BidSize_Object) { return _BidSize; } }
            set { lock (_BidSize_Object) { _BidSize = value; } }
        }

        public int AskSize
        {
            get { lock (_AskSize_Object) { return _AskSize; } }
            set { lock (_AskSize_Object) { _AskSize = value; } }
        }

        public int Volume
        {
            get { lock (_Volume_Object) { return _Volume; } }
            set { lock (_Volume_Object) { _Volume = value; } }
        }

        public int LastQuantity
        {
            get { lock (_LastQuantity_Object) { return _LastQuantity; } }
            set { lock (_LastQuantity_Object) { _LastQuantity = value; } }
        }

        public double LAST_PRICE
        {
            get { lock (_LAST_PRICE_Object) { return _LAST_PRICE; } }
            set { lock (_LAST_PRICE_Object) { _LAST_PRICE = value; } }
        }

        public double CLOSE
        {
            get { lock (_CLOSE_Object) { return _CLOSE; } }
            set { lock (_CLOSE_Object) { _CLOSE = value; } }
        }

        public double HIGH
        {
            get { lock (_HIGH_Object) { return _HIGH; } }
            set { lock (_HIGH_Object) { _HIGH = value; } }
        }

        public double LOW
        {
            get { lock (_LOW_Object) { return _LOW; } }
            set { lock (_LOW_Object) { _LOW = value; } }
        }
    }
}
