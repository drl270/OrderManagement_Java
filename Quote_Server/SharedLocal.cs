using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace QuoteServer
{
    public sealed class SharedLocal
    {
        private static readonly Lazy<SharedLocal> _instance = new Lazy<SharedLocal>(() => new SharedLocal());
        private string _ipRemote = string.Empty;
        private int _portRemote = 0;
        private int _portLocal;
        private int _maxNoResponseTime;
        private int _remoteUserId;
        private readonly ConcurrentDictionary<string, FuturesContract> _futuresContracts;
        private readonly List<string> _networkConnectionsPaths;

        private SharedLocal()
        {
            _futuresContracts = new ConcurrentDictionary<string, FuturesContract>();
            _networkConnectionsPaths = new List<string>();
        }

        public static SharedLocal Instance
        {
            get { return _instance.Value; }
        }

        public string IpRemote
        {
            get { return Interlocked.CompareExchange(ref _ipRemote, string.Empty, string.Empty); }
            set
            {
                if (value == null) throw new ArgumentNullException("IpRemote");
                Interlocked.Exchange(ref _ipRemote, value);
            }
        }

        public int PortRemote
        {
            get { return Interlocked.CompareExchange(ref _portRemote, 0, 0); }
            set { Interlocked.Exchange(ref _portRemote, value); }
        }

        public int PortLocal
        {
            get { return Interlocked.CompareExchange(ref _portLocal, 0, 0); }
            set { Interlocked.Exchange(ref _portLocal, value); }
        }

        public int MaxNoResponseTime
        {
            get { return Interlocked.CompareExchange(ref _maxNoResponseTime, 0, 0); }
            set { Interlocked.Exchange(ref _maxNoResponseTime, value); }
        }

        // Returns a read-only view of the network connections paths to prevent external modification
        public IList<string> NetworkConnectionsPaths
        {
            get
            {
                lock (_networkConnectionsPaths)
                {
                    return _networkConnectionsPaths.AsReadOnly();
                }
            }
        }

        public void PopulateNetworkConnectionsPaths()
        {
            try
            {
                IList<string> paths = FileManager.Populate_ListNetworkConnectionsPaths();
                if (paths == null)
                {
                    paths = new List<string>();
                }
                lock (_networkConnectionsPaths)
                {
                    _networkConnectionsPaths.Clear();
                    _networkConnectionsPaths.AddRange(paths);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to populate network connections paths", ex);
            }
        }

        public void PopulateFuturesContracts()
        {
            try
            {
                IDictionary<string, FuturesContract> contracts = FileManager.CreateDictionaryFuturesContracts();
                if (contracts == null)
                {
                    contracts = new Dictionary<string, FuturesContract>();
                }
                _futuresContracts.Clear();
                foreach (var kvp in contracts)
                {
                    _futuresContracts.TryAdd(kvp.Key, kvp.Value);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to populate futures contracts", ex);
            }
        }

        // Returns null if no non-standard futures contract is found for the symbol
        public FuturesContract CheckNonStandardFuturesQuote(string symbol)
        {
            if (symbol == null) throw new ArgumentNullException("symbol");
            FuturesContract contract;
            if (_futuresContracts.TryGetValue(symbol, out contract) && contract.NonStandard)
            {
                return contract;
            }
            return null;
        }

        public string GetNextRemoteUserId()
        {
            int newId = Interlocked.Increment(ref _remoteUserId);
            return newId.ToString();
        }
    }
}