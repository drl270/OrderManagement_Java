namespace QuoteServer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnDataSource = new System.Windows.Forms.Button();
            this.txtPortDataSource = new System.Windows.Forms.TextBox();
            this.btnServer = new System.Windows.Forms.Button();
            this.txtIpDataSource = new System.Windows.Forms.TextBox();
            this.txtPortServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timerTestConnection = new System.Windows.Forms.Timer(this.components);
            this.timer_Display = new System.Windows.Forms.Timer(this.components);
            this.txtMaximumNoResponseTimeSeconds = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_DataSource
            // 
            this.btnDataSource.BackColor = System.Drawing.Color.Salmon;
            this.btnDataSource.Location = new System.Drawing.Point(12, 8);
            this.btnDataSource.Name = "btn_DataSource";
            this.btnDataSource.Size = new System.Drawing.Size(75, 21);
            this.btnDataSource.TabIndex = 0;
            this.btnDataSource.Text = "Data Source";
            this.btnDataSource.UseVisualStyleBackColor = false;
            this.btnDataSource.Click += new System.EventHandler(this.btnDataSource_Click);
            // 
            // txt_Port_DataSource
            // 
            this.txtPortDataSource.Location = new System.Drawing.Point(146, 5);
            this.txtPortDataSource.Name = "txt_Port_DataSource";
            this.txtPortDataSource.Size = new System.Drawing.Size(100, 18);
            this.txtPortDataSource.TabIndex = 1;
            this.txtPortDataSource.Text = "4001";
            // 
            // btn_Server
            // 
            this.btnServer.BackColor = System.Drawing.Color.Salmon;
            this.btnServer.Location = new System.Drawing.Point(12, 35);
            this.btnServer.Name = "btn_Server";
            this.btnServer.Size = new System.Drawing.Size(75, 21);
            this.btnServer.TabIndex = 2;
            this.btnServer.Text = "Server";
            this.btnServer.UseVisualStyleBackColor = false;
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // txt_IP_DataSource
            // 
            this.txtIpDataSource.Location = new System.Drawing.Point(146, 31);
            this.txtIpDataSource.Name = "txt_IP_DataSource";
            this.txtIpDataSource.Size = new System.Drawing.Size(100, 18);
            this.txtIpDataSource.TabIndex = 3;
            this.txtIpDataSource.Text = "127.0.0.1";
            // 
            // txt_Port_Server
            // 
            this.txtPortServer.Location = new System.Drawing.Point(266, 5);
            this.txtPortServer.Name = "txt_Port_Server";
            this.txtPortServer.Size = new System.Drawing.Size(100, 18);
            this.txtPortServer.TabIndex = 4;
            this.txtPortServer.Text = "4005";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(114, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(114, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "IP";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Location = new System.Drawing.Point(8, 62);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(534, 248);
            this.dataGridView1.TabIndex = 7;
            // 
            // timer_TestConnection
            // 
            this.timerTestConnection.Interval = 1000;
            this.timerTestConnection.Tick += new System.EventHandler(this.timerTestConnection_Tick);
            // 
            // timer_Display
            // 
            this.timer_Display.Enabled = true;
            this.timer_Display.Interval = 333;
            this.timer_Display.Tick += new System.EventHandler(this.timerDisplay_Tick);
            // 
            // txt_MaximumNoResponeseTimeSeconds
            // 
            this.txtMaximumNoResponseTimeSeconds.Location = new System.Drawing.Point(266, 31);
            this.txtMaximumNoResponseTimeSeconds.Name = "txt_MaximumNoResponeseTimeSeconds";
            this.txtMaximumNoResponseTimeSeconds.Size = new System.Drawing.Size(100, 18);
            this.txtMaximumNoResponseTimeSeconds.TabIndex = 8;
            this.txtMaximumNoResponseTimeSeconds.Text = "30";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 315);
            this.Controls.Add(this.txtMaximumNoResponseTimeSeconds);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPortServer);
            this.Controls.Add(this.txtIpDataSource);
            this.Controls.Add(this.btnServer);
            this.Controls.Add(this.txtPortDataSource);
            this.Controls.Add(this.btnDataSource);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.Name = "Form1";
            this.Text = "Quote_Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDataSource;
        private System.Windows.Forms.TextBox txtPortDataSource;
        private System.Windows.Forms.Button btnServer;
        private System.Windows.Forms.TextBox txtIpDataSource;
        private System.Windows.Forms.TextBox txtPortServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timerTestConnection;
        private System.Windows.Forms.Timer timer_Display;
        private System.Windows.Forms.TextBox txtMaximumNoResponseTimeSeconds;
    }
}

