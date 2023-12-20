using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quote_Server
{
    public class FuturesContract
    {
        private int _Multiplier;
        private decimal _Tick_Size = 0;
        private int _Hedge_Units = 0;
        private string _BaseSymbol;
        private bool _NonStandard = false;

        private object _Multiplier_Obj = new object();
        private object _Tick_Size_Obj = new object();
        private object _Hedge_Units_Obj = new object();
        private object _BaseSymbol_Object = new object();
        private object _NonStandard_Object = new object();

        public FuturesContract(string multiplier, string tick_Size, string hedge_Units, string baseSymbol, bool nonStandard)
        {
            _Multiplier = Convert.ToInt32(multiplier);
            _Tick_Size = Convert.ToDecimal(tick_Size);
            _Hedge_Units = Convert.ToInt32(hedge_Units);
            _BaseSymbol = baseSymbol;
            _NonStandard = nonStandard;
        }


        public FuturesContract(FuturesContract obj)
        {
            _Multiplier = obj.Multiplier;
            _Tick_Size = obj.Tick_Size;
            _Hedge_Units = obj._Hedge_Units;
            _BaseSymbol = obj.BaseSymbol;
            _NonStandard = obj.NonStandard;
        }


        public int Multiplier
        {
            get { lock (_Multiplier_Obj) { return _Multiplier; } }
            set { lock (_Multiplier_Obj) { _Multiplier = value; } }
        }

        public decimal Tick_Size
        {
            get { lock (_Tick_Size_Obj) { return _Tick_Size; } }
            set { lock (_Tick_Size_Obj) { _Tick_Size = value; } }
        }

        public int Hedge_Units
        {
            get { lock (_Hedge_Units_Obj) { return _Hedge_Units; } }
            set { lock (_Hedge_Units_Obj) { _Hedge_Units = value; } }
        }

        public string BaseSymbol
        {
            get { lock (_BaseSymbol_Object) { return _BaseSymbol; } }
            set { lock (_BaseSymbol_Object) { _BaseSymbol = value; } }
        }

        public bool NonStandard
        {
            get { lock (_NonStandard_Object) { return _NonStandard; } }
            set { lock (_NonStandard_Object) { _NonStandard = value; } }
        }
    }
}
