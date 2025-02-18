namespace WinFormsApp1
{
    partial class frmDemo
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
            panel1 = new Panel();
            dataGridViewObject = new DataGridView();
            panel2 = new Panel();
            dataGridViewObjectDetails = new DataGridView();
            panel3 = new Panel();
            dataGridViewHistory = new DataGridView();
            groupBox1 = new GroupBox();
            btnDisconnect = new Button();
            btnConnect = new Button();
            txtPort = new TextBox();
            txtServerName = new TextBox();
            label2 = new Label();
            label1 = new Label();
            btnBrowse = new Button();
            btnMonitor = new Button();
            btnDelete = new Button();
            button2 = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewObject).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewObjectDetails).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewHistory).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(dataGridViewObject);
            panel1.Location = new Point(12, 162);
            panel1.Name = "panel1";
            panel1.Size = new Size(375, 623);
            panel1.TabIndex = 0;
            // 
            // dataGridViewObject
            // 
            dataGridViewObject.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewObject.Dock = DockStyle.Fill;
            dataGridViewObject.Location = new Point(0, 0);
            dataGridViewObject.Name = "dataGridViewObject";
            dataGridViewObject.RowHeadersWidth = 51;
            dataGridViewObject.Size = new Size(375, 623);
            dataGridViewObject.TabIndex = 1;
            dataGridViewObject.CellEnter += dataGridViewObject_CellEnter;
            // 
            // panel2
            // 
            panel2.Controls.Add(dataGridViewObjectDetails);
            panel2.Location = new Point(508, 162);
            panel2.Name = "panel2";
            panel2.Size = new Size(375, 623);
            panel2.TabIndex = 1;
            // 
            // dataGridViewObjectDetails
            // 
            dataGridViewObjectDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewObjectDetails.Dock = DockStyle.Fill;
            dataGridViewObjectDetails.Location = new Point(0, 0);
            dataGridViewObjectDetails.Name = "dataGridViewObjectDetails";
            dataGridViewObjectDetails.RowHeadersWidth = 51;
            dataGridViewObjectDetails.Size = new Size(375, 623);
            dataGridViewObjectDetails.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Controls.Add(dataGridViewHistory);
            panel3.Location = new Point(917, 162);
            panel3.Name = "panel3";
            panel3.Size = new Size(375, 623);
            panel3.TabIndex = 2;
            // 
            // dataGridViewHistory
            // 
            dataGridViewHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewHistory.Dock = DockStyle.Fill;
            dataGridViewHistory.Location = new Point(0, 0);
            dataGridViewHistory.Name = "dataGridViewHistory";
            dataGridViewHistory.RowHeadersWidth = 51;
            dataGridViewHistory.Size = new Size(375, 623);
            dataGridViewHistory.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnDisconnect);
            groupBox1.Controls.Add(btnConnect);
            groupBox1.Controls.Add(txtPort);
            groupBox1.Controls.Add(txtServerName);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(598, 122);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Connection";
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new Point(464, 72);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(94, 29);
            btnDisconnect.TabIndex = 5;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(364, 72);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(94, 29);
            btnConnect.TabIndex = 4;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(433, 39);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(125, 27);
            txtPort.TabIndex = 3;
            txtPort.Text = "4840";
            // 
            // txtServerName
            // 
            txtServerName.Location = new Point(100, 40);
            txtServerName.Name = "txtServerName";
            txtServerName.Size = new Size(226, 27);
            txtServerName.TabIndex = 2;
            txtServerName.Text = "127.0.0.1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(368, 39);
            label2.Name = "label2";
            label2.Size = new Size(35, 20);
            label2.TabIndex = 1;
            label2.Text = "Port";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 42);
            label1.Name = "label1";
            label1.Size = new Size(50, 20);
            label1.TabIndex = 0;
            label1.Text = "Server";
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(393, 162);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(94, 29);
            btnBrowse.TabIndex = 6;
            btnBrowse.Text = "Browse";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // btnMonitor
            // 
            btnMonitor.Location = new Point(393, 235);
            btnMonitor.Name = "btnMonitor";
            btnMonitor.Size = new Size(94, 29);
            btnMonitor.TabIndex = 7;
            btnMonitor.Text = "Monitor";
            btnMonitor.UseVisualStyleBackColor = true;
            btnMonitor.Click += btnMonitor_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(393, 197);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(94, 29);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // button2
            // 
            button2.Location = new Point(393, 270);
            button2.Name = "button2";
            button2.Size = new Size(94, 71);
            button2.TabIndex = 9;
            button2.Text = "View Monitored Item";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // frmDemo
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1304, 797);
            Controls.Add(button2);
            Controls.Add(btnDelete);
            Controls.Add(btnMonitor);
            Controls.Add(btnBrowse);
            Controls.Add(groupBox1);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "frmDemo";
            Text = "Demo";
            Load += frmDemo_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewObject).EndInit();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewObjectDetails).EndInit();
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewHistory).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DataGridView dataGridViewObject;
        private Panel panel2;
        private DataGridView dataGridViewObjectDetails;
        private Panel panel3;
        private DataGridView dataGridViewHistory;
        private GroupBox groupBox1;
        private Button btnDisconnect;
        private Button btnConnect;
        private TextBox txtPort;
        private TextBox txtServerName;
        private Label label2;
        private Label label1;
        private Button btnBrowse;
        private Button btnMonitor;
        private Button btnDelete;
        private Button button2;
    }
}