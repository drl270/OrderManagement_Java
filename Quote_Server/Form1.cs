using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Quote_Server
{
    public partial class Form1 : Form
    {

        private QuoteServer_Parameters _QuoteServer_Parameters;

        public Form1()
        {
            InitializeComponent();
            StartUp_Serialization();
        }

        public event Action TestConnectionStatus;   
        public void Update_DisplayObjects()
        {
            List<DisplayObject> List_DisplayObjects = Server.Get_ListDisplayObject();

            if (List_DisplayObjects.Count > 0)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = List_DisplayObjects;
                dataGridView1.Columns[0].Width = 50;
                dataGridView1.Columns[1].Width = 60;
                dataGridView1.Columns[2].Width = 60;
                dataGridView1.Columns[3].Width = 60;
                dataGridView1.Columns[4].Width = 135;
                dataGridView1.Columns[5].Width = 135;
            }

            this.Text = "Quote Server                  " +  DateTime.Now.ToString();         
        }

        private void StartUp_Serialization()
        {
            string Path_Parameters = @"\\DESKTOP-9B0FEIE\CommonDatabases\" + Environment.MachineName + @"\QuoteServer_Serialized_Objects\Parameters.bin";

            if (File.Exists(Path_Parameters))
            {
                using (Stream stream = new FileStream(Path_Parameters, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    IFormatter formatter = new BinaryFormatter();
                    _QuoteServer_Parameters = (QuoteServer_Parameters)formatter.Deserialize(stream);
                }

                if (_QuoteServer_Parameters != null)
                    QuoteServer_Parameters_StartUp();
            }

            if (_QuoteServer_Parameters == null)
                _QuoteServer_Parameters = new QuoteServer_Parameters();
        }

        private void ShutDown_Serialization()
        {
            string Path_Parameters = @"\\DESKTOP-9B0FEIE\CommonDatabases\" + Environment.MachineName + @"\QuoteServer_Serialized_Objects\Parameters.bin";

            if (txt_IP_DataSource.Text != "" && txt_Port_DataSource.Text != "")
            {
                _QuoteServer_Parameters.IP_DataSource = txt_IP_DataSource.Text;
                _QuoteServer_Parameters.Port_DataSource = Convert.ToInt32(txt_Port_DataSource.Text);
                _QuoteServer_Parameters.Server_Port = Convert.ToInt32(txt_Port_Server.Text);
                _QuoteServer_Parameters.MaximumNoRepsoneTime = Convert.ToInt32(txt_MaximumNoResponeseTimeSeconds.Text); 

                using (Stream stream = new FileStream(Path_Parameters, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    try
                    {
                        IFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, _QuoteServer_Parameters);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.ToString());
                    }
                }
            }
        }

        private void QuoteServer_Parameters_StartUp()
        {
            txt_IP_DataSource.Text = _QuoteServer_Parameters.IP_DataSource;
            txt_Port_DataSource.Text = _QuoteServer_Parameters.Port_DataSource.ToString();
            txt_Port_Server.Text = _QuoteServer_Parameters.Server_Port.ToString();
            txt_MaximumNoResponeseTimeSeconds.Text = _QuoteServer_Parameters.MaximumNoRepsoneTime.ToString();
        }

        private void timer_TestConnection_Tick(object sender, EventArgs e)
        {
            Func<int> del = delegate ()
            {
                if (TestConnectionStatus != null)
                    TestConnectionStatus();
                return 0;
            };
            Invoke(del);
        }

        private void timer_Display_Tick(object sender, EventArgs e)
        {
            Func<int> del = delegate ()
            {
                Update_DisplayObjects();
                return 0;
            };
            Invoke(del);
        }

        private void btn_Server_Click(object sender, EventArgs e)
        {
            if (btn_Server.BackColor == Color.Salmon)
            {
                btn_Server.BackColor = Color.SeaGreen;
                SharedLocal.Port_Local = Convert.ToInt32(txt_Port_Server.Text);
                Server.createTCPServer(this);
            }
            else
            {
                btn_Server.BackColor = Color.Salmon;
                Server.ShutDown_Server();
            }           
        }

        private void btn_DataSource_Click(object sender, EventArgs e)
        {
            if (btn_DataSource.BackColor == Color.Salmon)
            {
                btn_DataSource.BackColor = Color.SeaGreen;
                SharedLocal.Port_Remote = Convert.ToInt32(txt_Port_DataSource.Text);
                SharedLocal.IP_Remote = txt_IP_DataSource.Text;
                SharedLocal.MaxNoResponseTime = Convert.ToInt32(txt_MaximumNoResponeseTimeSeconds.Text);
                Client.establishClientConnection();
                timer_TestConnection.Enabled = true;
            }
            else
            {
                timer_TestConnection.Enabled = false;
                btn_DataSource.BackColor = Color.Salmon;
                Client.ShutDown_TCPData();
            }         
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShutDown_Serialization();
        }
    }
}
