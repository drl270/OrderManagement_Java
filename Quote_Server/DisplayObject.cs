using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quote_Server
{
    public class DisplayObject
    {
        private string _User = "";
        private string _Symbol = "";
        private string _Bid = "0";
        private string _Ask = "0";
        private string _QuoteTimeStamp = "";
        private string _LastConnectVerifyTime = "";

        private object _User_Obj = new object();
        private object _Symbol_Obj = new object();
        private object _Bid_Obj = new object();
        private object _Ask_Obj = new object();
        private object _LastConnectVerifyTime_Obj = new object();
        private object _QuoteTimeStamp_Obj = new object();

        public DisplayObject( string p_User, string p_Symbol, string p_Bid, string p_Ask, string p_QuoteTimeStamp, string p_LastConnectVerifyTime)
        {
            _Bid = p_Bid;
            _Ask = p_Ask;
            _User = p_User;
            _Symbol = p_Symbol;
            _QuoteTimeStamp = p_QuoteTimeStamp;
            _LastConnectVerifyTime = p_LastConnectVerifyTime;
        }

        public string User
        {
            get { lock (_User_Obj) { return _User; } }
            set { lock (_User_Obj) { _User = value; } }
        }

        public string Symbol
        {
            get { lock (_Symbol_Obj) { return _Symbol; } }
            set { lock (_Symbol_Obj) { _Symbol = value; } }
        }

        public string Bid
        {
            get { lock (_Bid_Obj) { return _Bid; } }
            set { lock (_Bid_Obj) { _Bid = value; } }
        }

        public string Ask
        {
            get { lock (_Ask_Obj) { return _Ask; } }
            set { lock (_Ask_Obj) { _Ask = value; } }
        }

        public string QuoteTimeStamp
        {
            get { lock (_QuoteTimeStamp_Obj) { return _QuoteTimeStamp; } }
            set { lock (_QuoteTimeStamp_Obj) { _QuoteTimeStamp = value; } }
        }

        public string LastConnectVerifyTime
        {
            get { lock (_LastConnectVerifyTime_Obj) { return _LastConnectVerifyTime; } }
            set { lock (_LastConnectVerifyTime_Obj) { _LastConnectVerifyTime = value; } }
        }
    }
}
