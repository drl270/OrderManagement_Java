using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteServer
{
    [Serializable]
    public class Parameters
    {
        private string _IpDataSource;
        private int _PortDataSource;
        private int _ServerPort;
        private int _MaximumNoResponseTime;

        private object _IP_DataSource_Object = new object();
        private object _Port_DataSource_Object = new object();
        private object _Server_Port_Object = new object();
        private object _MaximumNoRepsoneTime_Object = new object();

        public string IpDataSource
        {
            get { lock (_IP_DataSource_Object) { return _IpDataSource; } }
            set { lock (_IP_DataSource_Object) { _IpDataSource = value; } }
        }

        public int PortDataSource
        {
            get { lock (_Port_DataSource_Object) { return _PortDataSource; } }
            set { lock (_Port_DataSource_Object) { _PortDataSource = value; } }
        }

        public int ServerPort
        {
            get { lock (_Server_Port_Object) { return _ServerPort; } }
            set { lock (_Server_Port_Object) { _ServerPort = value; } }
        }

        public int MaximumNoResponseTime
        {
            get { lock (_MaximumNoRepsoneTime_Object) { return _MaximumNoResponseTime; } }
            set { lock (_MaximumNoRepsoneTime_Object) { _MaximumNoResponseTime = value; } }
        }
    }
}
