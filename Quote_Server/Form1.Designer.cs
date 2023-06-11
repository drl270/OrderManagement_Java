namespace Quote_Server
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
            this.btn_DataSource = new System.Windows.Forms.Button();
            this.txt_Port_DataSource = new System.Windows.Forms.TextBox();
            this.btn_Server = new System.Windows.Forms.Button();
            this.txt_IP_DataSource = new System.Windows.Forms.TextBox();
            this.txt_Port_Server = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timer_TestConnection = new System.Windows.Forms.Timer(this.components);
            this.timer_Display = new System.Windows.Forms.Timer(this.components);
            this.txt_MaximumNoResponeseTimeSeconds = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_DataSource
            // 
            this.btn_DataSource.BackColor = System.Drawing.Color.Salmon;
            this.btn_DataSource.Location = new System.Drawing.Point(12, 8);
            this.btn_DataSource.Name = "btn_DataSource";
            this.btn_DataSource.Size = new System.Drawing.Size(75, 21);
            this.btn_DataSource.TabIndex = 0;
            this.btn_DataSource.Text = "Data Source";
            this.btn_DataSource.UseVisualStyleBackColor = false;
            this.btn_DataSource.Click += new System.EventHandler(this.btn_DataSource_Click);
            // 
            // txt_Port_DataSource
            // 
            this.txt_Port_DataSource.Location = new System.Drawing.Point(146, 5);
            this.txt_Port_DataSource.Name = "txt_Port_DataSource";
            this.txt_Port_DataSource.Size = new System.Drawing.Size(100, 18);
            this.txt_Port_DataSource.TabIndex = 1;
            this.txt_Port_DataSource.Text = "4001";
            // 
            // btn_Server
            // 
            this.btn_Server.BackColor = System.Drawing.Color.Salmon;
            this.btn_Server.Location = new System.Drawing.Point(12, 35);
            this.btn_Server.Name = "btn_Server";
            this.btn_Server.Size = new System.Drawing.Size(75, 21);
            this.btn_Server.TabIndex = 2;
            this.btn_Server.Text = "Server";
            this.btn_Server.UseVisualStyleBackColor = false;
            this.btn_Server.Click += new System.EventHandler(this.btn_Server_Click);
            // 
            // txt_IP_DataSource
            // 
            this.txt_IP_DataSource.Location = new System.Drawing.Point(146, 31);
            this.txt_IP_DataSource.Name = "txt_IP_DataSource";
            this.txt_IP_DataSource.Size = new System.Drawing.Size(100, 18);
            this.txt_IP_DataSource.TabIndex = 3;
            this.txt_IP_DataSource.Text = "127.0.0.1";
            // 
            // txt_Port_Server
            // 
            this.txt_Port_Server.Location = new System.Drawing.Point(266, 5);
            this.txt_Port_Server.Name = "txt_Port_Server";
            this.txt_Port_Server.Size = new System.Drawing.Size(100, 18);
            this.txt_Port_Server.TabIndex = 4;
            this.txt_Port_Server.Text = "4005";
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
            this.timer_TestConnection.Interval = 1000;
            this.timer_TestConnection.Tick += new System.EventHandler(this.timer_TestConnection_Tick);
            // 
            // timer_Display
            // 
            this.timer_Display.Enabled = true;
            this.timer_Display.Interval = 333;
            this.timer_Display.Tick += new System.EventHandler(this.timer_Display_Tick);
            // 
            // txt_MaximumNoResponeseTimeSeconds
            // 
            this.txt_MaximumNoResponeseTimeSeconds.Location = new System.Drawing.Point(266, 31);
            this.txt_MaximumNoResponeseTimeSeconds.Name = "txt_MaximumNoResponeseTimeSeconds";
            this.txt_MaximumNoResponeseTimeSeconds.Size = new System.Drawing.Size(100, 18);
            this.txt_MaximumNoResponeseTimeSeconds.TabIndex = 8;
            this.txt_MaximumNoResponeseTimeSeconds.Text = "30";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 315);
            this.Controls.Add(this.txt_MaximumNoResponeseTimeSeconds);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_Port_Server);
            this.Controls.Add(this.txt_IP_DataSource);
            this.Controls.Add(this.btn_Server);
            this.Controls.Add(this.txt_Port_DataSource);
            this.Controls.Add(this.btn_DataSource);
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

        private System.Windows.Forms.Button btn_DataSource;
        private System.Windows.Forms.TextBox txt_Port_DataSource;
        private System.Windows.Forms.Button btn_Server;
        private System.Windows.Forms.TextBox txt_IP_DataSource;
        private System.Windows.Forms.TextBox txt_Port_Server;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer_TestConnection;
        private System.Windows.Forms.Timer timer_Display;
        private System.Windows.Forms.TextBox txt_MaximumNoResponeseTimeSeconds;
    }
}

