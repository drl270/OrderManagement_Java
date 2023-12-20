using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quote_Server
{
    public static class FlatFileManager
    {
        public static List<string> Populate_ListNetworkConnectionsPaths()
        {
            List<string> list = new List<string>();

            string path = @"C:\CommonDatabases\NetworkConnections\Paths.txt";

            if (File.Exists(path))
            {
                list.AddRange(File.ReadAllLines(path));
            }

            return list;
        }

        public static ConcurrentDictionary<string, FuturesContract> CreateDictionaryFuturesContracts()
        {
            ConcurrentDictionary<string, FuturesContract>  dict = new ConcurrentDictionary<string, FuturesContract>();

            foreach (string networkPath in SharedLocal.NetworkConnectionsPaths)
            {
                string path = @networkPath + @"\CommonDatabases\Futures_Contract_Info.txt";

                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(path);
                    string line;
                    string[] parser;
                    while (sr.Peek() > -1)
                    {
                        line = sr.ReadLine();
                        parser = line.Split(',');
                        string multiplier = parser[0];
                        string tick_Size = parser[1];
                        string hedge_Units = parser[2];
                        string baseSymbol = parser[3];
                        bool nonStandard = Convert.ToBoolean(parser[4]);
                        dict.TryAdd(baseSymbol, new FuturesContract(multiplier, tick_Size, hedge_Units, baseSymbol, nonStandard));
                        // Shared_Local.Add_List_GeneralInfo("Create_Dictionary_Futures_Contracts  " + baseSymbol);
                    }
                    sr.Close();
                }

                if (dict.Count > 0)
                    break;
            }

            return dict;
        }
    }
}
