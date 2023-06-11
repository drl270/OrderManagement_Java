using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quote_Server
{
    [Serializable]
    public class QuoteServer_Parameters
    {
        private string _IP_DataSource;
        private int _Port_DataSource;
        private int _Server_Port;
        private int _MaximumNoRepsoneTime;

        private object _IP_DataSource_Object = new object();
        private object _Port_DataSource_Object = new object();
        private object _Server_Port_Object = new object();
        private object _MaximumNoRepsoneTime_Object = new object();

        public string IP_DataSource
        {
            get { lock (_IP_DataSource_Object) { return _IP_DataSource; } }
            set { lock (_IP_DataSource_Object) { _IP_DataSource = value; } }
        }

        public int Port_DataSource
        {
            get { lock (_Port_DataSource_Object) { return _Port_DataSource; } }
            set { lock (_Port_DataSource_Object) { _Port_DataSource = value; } }
        }

        public int Server_Port
        {
            get { lock (_Server_Port_Object) { return _Server_Port; } }
            set { lock (_Server_Port_Object) { _Server_Port = value; } }
        }

        public int MaximumNoRepsoneTime
        {
            get { lock (_MaximumNoRepsoneTime_Object) { return _MaximumNoRepsoneTime; } }
            set { lock (_MaximumNoRepsoneTime_Object) { _MaximumNoRepsoneTime = value; } }
        }
    }
}
