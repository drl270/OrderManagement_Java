using System;
using System.IO;
using Newtonsoft.Json;

namespace QuoteServer
{
    public class ConfigurationManager
    {
        private readonly Configuration _config;

        public ConfigurationManager(string configFilePath = null)
        {
            configFilePath = configFilePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            try
            {
                if (!File.Exists(configFilePath))
                {
                    throw new FileNotFoundException($"Configuration file not found: {configFilePath}");
                }
                string json = File.ReadAllText(configFilePath);
                _config = JsonConvert.DeserializeObject<Configuration>(json) ?? throw new InvalidOperationException("Failed to deserialize configuration.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load configuration from {configFilePath}", ex);
            }
        }

        public string NetworkConnectionsPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _config.Paths.NetworkConnections);
        public string FuturesContractFileName => _config.Paths.FuturesContractFileName;
        public string ParametersFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            string.Format(_config.Paths.ParametersFile, Environment.MachineName));
        public string LogFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _config.Logging.LogFilePath);
        public Configuration.ParametersData Parameters => _config.Parameters;

        public class Configuration
        {
            public PathsData Paths { get; set; }
            public ParametersData Parameters { get; set; }
            public LoggingData Logging { get; set; }

            public class PathsData
            {
                public string NetworkConnections { get; set; }
                public string FuturesContractFileName { get; set; }
                public string ParametersFile { get; set; }
            }

            public class ParametersData
            {
                public string IpDataSource { get; set; }
                public int PortDataSource { get; set; }
                public int ServerPort { get; set; }
                public int MaximumNoResponseTime { get; set; }
            }

            public class LoggingData
            {
                public string LogFilePath { get; set; }
            }
        }
    }
}
