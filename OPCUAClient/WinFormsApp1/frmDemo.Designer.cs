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
            groupBox1 = new GroupBox();
            btnDisconnect = new Button();
            btnConnect = new Button();
            txtPort = new TextBox();
            txtServerName = new TextBox();
            label2 = new Label();
            label1 = new Label();
            panel1 = new Panel();
            treeView1 = new TreeView();
            panel2 = new Panel();
            dataGridViewObject = new DataGridView();
            panel3 = new Panel();
            dataGridView1 = new DataGridView();
            panel4 = new Panel();
            dataGridView2 = new DataGridView();
            btnMonitor = new Button();
            groupBox2 = new GroupBox();
            button1 = new Button();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewObject).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnDisconnect);
            groupBox1.Controls.Add(btnConnect);
            groupBox1.Controls.Add(txtPort);
            groupBox1.Controls.Add(txtServerName);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(28, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(890, 92);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Connection";
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new Point(684, 38);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(94, 29);
            btnDisconnect.TabIndex = 5;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(573, 39);
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
            txtPort.Text = "53530";
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
            // panel1
            // 
            panel1.Controls.Add(treeView1);
            panel1.Location = new Point(28, 110);
            panel1.Name = "panel1";
            panel1.Size = new Size(357, 739);
            panel1.TabIndex = 1;
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(357, 739);
            treeView1.TabIndex = 0;
            treeView1.AfterSelect += treeView1_AfterSelect;
            // 
            // panel2
            // 
            panel2.Controls.Add(dataGridViewObject);
            panel2.Location = new Point(391, 110);
            panel2.Name = "panel2";
            panel2.Size = new Size(354, 739);
            panel2.TabIndex = 2;
            // 
            // dataGridViewObject
            // 
            dataGridViewObject.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewObject.Dock = DockStyle.Fill;
            dataGridViewObject.Location = new Point(0, 0);
            dataGridViewObject.Name = "dataGridViewObject";
            dataGridViewObject.RowHeadersWidth = 51;
            dataGridViewObject.Size = new Size(354, 739);
            dataGridViewObject.TabIndex = 0;
            dataGridViewObject.CellEndEdit += dataGridViewObject_CellEndEdit;
            // 
            // panel3
            // 
            panel3.Controls.Add(dataGridView1);
            panel3.Location = new Point(751, 110);
            panel3.Name = "panel3";
            panel3.Size = new Size(354, 739);
            panel3.TabIndex = 3;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(354, 739);
            dataGridView1.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.Controls.Add(dataGridView2);
            panel4.Location = new Point(1111, 110);
            panel4.Name = "panel4";
            panel4.Size = new Size(555, 739);
            panel4.TabIndex = 4;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(0, 0);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(555, 739);
            dataGridView2.TabIndex = 0;
            // 
            // btnMonitor
            // 
            btnMonitor.Location = new Point(72, 37);
            btnMonitor.Name = "btnMonitor";
            btnMonitor.Size = new Size(94, 29);
            btnMonitor.TabIndex = 6;
            btnMonitor.Text = "Monitor";
            btnMonitor.UseVisualStyleBackColor = true;
            btnMonitor.Click += btnMonitor_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button1);
            groupBox2.Controls.Add(btnMonitor);
            groupBox2.Location = new Point(1111, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(346, 92);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "Monitoring";
            // 
            // button1
            // 
            button1.Location = new Point(172, 37);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 7;
            button1.Text = "Delete";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // frmDemo
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1697, 861);
            Controls.Add(groupBox2);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(groupBox1);
            Name = "frmDemo";
            Text = "Demo Client";
            FormClosed += frmDemo_FormClosed;
            Load += frmDemo_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewObject).EndInit();
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label2;
        private Label label1;
        private Button btnDisconnect;
        private Button btnConnect;
        private TextBox txtPort;
        private TextBox txtServerName;
        private Panel panel1;
        private TreeView treeView1;
        private Panel panel2;
        private DataGridView dataGridViewObject;
        private Panel panel3;
        private DataGridView dataGridView1;
        private Panel panel4;
        private DataGridView dataGridView2;
        private Button btnMonitor;
        private GroupBox groupBox2;
        private Button button1;
    }
}