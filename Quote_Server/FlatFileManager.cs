using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace QuoteServer
{
    /// <summary>
    /// Provides utility methods for reading configuration data from flat files.
    /// </summary>
    public static class FlatFileManager
    {
        private const string NetworkConnectionsPath = @"C:\CommonDatabases\NetworkConnections\Paths.txt";
        private const string FuturesContractFileName = @"CommonDatabases\Futures_Contract_Info.txt";

        /// <summary>
        /// Reads network connection paths from a configuration file.
        /// </summary>
        /// <returns>A list of network paths. Returns an empty list if the file is not found or empty.</returns>
        public static IList<string> Populate_ListNetworkConnectionsPaths()
        {
            List<string> list = new List<string>();

            try
            {
                if (File.Exists(NetworkConnectionsPath))
                {
                    string[] lines = File.ReadAllLines(NetworkConnectionsPath);
                    list.AddRange(lines);
                }
            }
            catch (Exception ex)
            {
                // Log error (e.g., via SharedLocal or a logging framework)
                // Example: SharedLocal.Instance.Add_List_GeneralInfo("Error reading network paths: " + ex.Message);
            }

            return list;
        }

        /// <summary>
        /// Creates a dictionary of futures contracts by reading from network path files.
        /// </summary>
        /// <returns>A thread-safe dictionary of futures contracts. Returns an empty dictionary if no valid data is found.</returns>
        public static ConcurrentDictionary<string, FuturesContract> CreateDictionaryFuturesContracts()
        {
            ConcurrentDictionary<string, FuturesContract> dict = new ConcurrentDictionary<string, FuturesContract>();

            IList<string> networkPaths = SharedLocal.Instance.NetworkConnectionsPaths;
            foreach (string networkPath in networkPaths)
            {
                if (string.IsNullOrEmpty(networkPath))
                {
                    continue;
                }

                string path = Path.Combine(networkPath, FuturesContractFileName);
                if (!File.Exists(path))
                {
                    continue;
                }

                try
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(line))
                            {
                                continue;
                            }

                            string[] parser = line.Split(',');
                            if (parser.Length < 5)
                            {
                                // Log invalid line
                                // Example: SharedLocal.Instance.Add_List_GeneralInfo("Invalid futures contract line: " + line);
                                continue;
                            }

                            try
                            {
                                string multiplier = parser[0];
                                string tickSize = parser[1];
                                string hedgeUnits = parser[2];
                                string baseSymbol = parser[3];
                                bool nonStandard = Convert.ToBoolean(parser[4]);

                                dict.TryAdd(baseSymbol, new FuturesContract(multiplier, tickSize, hedgeUnits, baseSymbol, nonStandard));
                            }
                            catch (Exception ex)
                            {
                                // Log parsing error
                                // Example: SharedLocal.Instance.Add_List_GeneralInfo("Error parsing futures contract line: " + line + ", Error: " + ex.Message);
                                continue;
                            }
                        }
                    }

                    if (dict.Count > 0)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    // Log file access error
                    // Example: SharedLocal.Instance.Add_List_GeneralInfo("Error reading futures contract file " + path + ": " + ex.Message);
                    continue;
                }
            }

            return dict;
        }
    }
}