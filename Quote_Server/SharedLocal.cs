using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quote_Server
{
    public static class SharedLocal
    {
        static string _IP_Remote;
        static int _Port_Remote;
        static int _Port_Local;
        static int _RemoteUserID = 0;
        static int _MaxNoResponseTime;

        static object _IP_Remote_Obj = new object();
        static object _Port_Remote_Obj = new object();
        static object _Port_Local_Obj = new object();
        static object _MaxNoResponseTime_Obj = new object();

        public static string IP_Remote
        {
            get { lock (_IP_Remote_Obj) return _IP_Remote; }
            set { lock (_IP_Remote_Obj) _IP_Remote = value; }
        }

        public static int Port_Remote
        {
            get { lock (_Port_Remote_Obj) return _Port_Remote; }
            set { lock (_Port_Remote_Obj) _Port_Remote = value; }
        }

        public static int Port_Local
        {
            get { lock (_Port_Local_Obj) return _Port_Local; }
            set { lock (_Port_Local_Obj) _Port_Local = value; }
        }

        public static int MaxNoResponseTime
        {
            get { lock (_MaxNoResponseTime_Obj) return _MaxNoResponseTime; }
            set { lock (_MaxNoResponseTime_Obj) _MaxNoResponseTime = value; }
        }

        public static string Get_RemoteUserID()
        {
            _RemoteUserID++;
            return _RemoteUserID.ToString();
        }

    }
}
