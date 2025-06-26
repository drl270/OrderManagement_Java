using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace QuoteServer
{
    public partial class Form1 : Form
    {
        private readonly Parameters _parameters;
        private readonly Server _server;
        private readonly Client _client;
        private readonly string _parametersFilePath;

        public Form1(Server server = null, Client client = null, string parametersFilePath = null)
        {
            InitializeComponent();

            _server = server ?? new Server();
            _client = client ?? new Client();
            _parametersFilePath = parametersFilePath ?? Path.Combine(
                @"\\DESKTOP-9B0FEIE\CommonDatabases",
                Environment.MachineName,
                "QuoteServer_Serialized_Objects",
                "Parameters.bin");

            _parameters = LoadParameters() ?? new Parameters();

            InitializeSharedLocal();
            InitializeUI();
        }

        public event Action TestConnectionStatus;

        private void InitializeSharedLocal()
        {
            try
            {
                SharedLocal.Instance.PopulateNetworkConnectionsPaths();
                SharedLocal.Instance.PopulateFuturesContracts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize shared data: {ex.Message}", "Initialization Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeUI()
        {
            txtIpDataSource.Text = _parameters.IpDataSource;
            txtPortDataSource.Text = _parameters.PortDataSource.ToString();
            txtPortServer.Text = _parameters.ServerPort.ToString();
            txtMaximumNoResponseTimeSeconds.Text = _parameters.MaximumNoResponseTime.ToString();
        }

        private Parameters LoadParameters()
        {
            if (!File.Exists(_parametersFilePath))
            {
                return null;
            }

            try
            {
                using (Stream stream = new FileStream(_parametersFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    IFormatter formatter = new BinaryFormatter();
                    return (Parameters)formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load parameters: {ex.Message}", "Serialization Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void SaveParameters()
        {
            try
            {
                string ipDataSource = txtIpDataSource.Text.Trim();
                if (string.IsNullOrEmpty(ipDataSource))
                {
                    MessageBox.Show("IP Data Source cannot be empty.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtPortDataSource.Text, out int portDataSource) ||
                    !int.TryParse(txtPortServer.Text, out int serverPort) ||
                    !int.TryParse(txtMaximumNoResponseTimeSeconds.Text, out int maxNoResponseTime))
                {
                    MessageBox.Show("Port and Maximum No Response Time must be valid integers.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _parameters.IpDataSource = ipDataSource;
                _parameters.PortDataSource = portDataSource;
                _parameters.ServerPort = serverPort;
                _parameters.MaximumNoResponseTime = maxNoResponseTime;

                Directory.CreateDirectory(Path.GetDirectoryName(_parametersFilePath));
                using (Stream stream = new FileStream(_parametersFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, _parameters);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save parameters: {ex.Message}", "Serialization Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDisplayObjects()
        {
            try
            {
                List<DisplayObject> displayObjects = _server.GetListDisplayObject();
                if (displayObjects != null && displayObjects.Count > 0)
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = displayObjects;
                    dataGridView1.Columns[0].Width = 50;
                    dataGridView1.Columns[1].Width = 60;
                    dataGridView1.Columns[2].Width = 60;
                    dataGridView1.Columns[3].Width = 60;
                    dataGridView1.Columns[4].Width = 135;
                    dataGridView1.Columns[5].Width = 135;
                }

                this.Text = $"Quote Server - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            }
            catch (Exception ex)
            {
                // Log error silently to avoid UI disruption
                // Example: SharedLocal.Instance.AddListGeneralInfo($"Display update error: {ex.Message}");
            }
        }

        private void timerTestConnection_Tick(object sender, EventArgs e)
        {
            if (TestConnectionStatus != null)
            {
                try
                {
                    TestConnectionStatus();
                }
                catch (Exception ex)
                {
                    // Log error silently
                    // Example: SharedLocal.Instance.AddListGeneralInfo($"Connection test error: {ex.Message}");
                }
            }
        }

        private void timerDisplay_Tick(object sender, EventArgs e)
        {
            UpdateDisplayObjects();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPortServer.Text, out int serverPort))
            {
                MessageBox.Show("Server Port must be a valid integer.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (btnServer.BackColor == Color.Salmon)
            {
                btnServer.BackColor = Color.SeaGreen;
                SharedLocal.Instance.PortLocal = serverPort;
                try
                {
                    _server.createTCPServer(this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to start server: {ex.Message}", "Server Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnServer.BackColor = Color.Salmon;
                }
            }
            else
            {
                btnServer.BackColor = Color.Salmon;
                try
                {
                    _server.ShutDown_Server();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to shutdown server: {ex.Message}", "Server Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDataSource_Click(object sender, EventArgs e)
        {
            string ipDataSource = txtIpDataSource.Text.Trim();
            if (string.IsNullOrEmpty(ipDataSource))
            {
                MessageBox.Show("IP Data Source cannot be empty.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtPortDataSource.Text, out int portDataSource) ||
                !int.TryParse(txtMaximumNoResponseTimeSeconds.Text, out int maxNoResponseTime))
            {
                MessageBox.Show("Port Data Source and Maximum No Response Time must be valid integers.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (btnDataSource.BackColor == Color.Salmon)
            {
                btnDataSource.BackColor = Color.SeaGreen;
                SharedLocal.Instance.IpRemote = ipDataSource;
                SharedLocal.Instance.PortRemote = portDataSource;
                SharedLocal.Instance.MaxNoResponseTime = maxNoResponseTime;

                try
                {
                    _client.establishClientConnection();
                    timerTestConnection.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to connect to data source: {ex.Message}", "Client Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnDataSource.BackColor = Color.Salmon;
                    timerTestConnection.Enabled = false;
                }
            }
            else
            {
                timerTestConnection.Enabled = false;
                btnDataSource.BackColor = Color.Salmon;
                try
                {
                    _client.ShutDown_TCPData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to shutdown data source: {ex.Message}", "Client Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveParameters();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
