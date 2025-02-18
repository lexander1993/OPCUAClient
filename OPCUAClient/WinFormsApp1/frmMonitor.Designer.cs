namespace WinFormsApp1
{
    partial class frmMonitor
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
            dataGridViewMonitor = new DataGridView();
            button1 = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewMonitor).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(dataGridViewMonitor);
            panel1.Location = new Point(12, 63);
            panel1.Name = "panel1";
            panel1.Size = new Size(623, 328);
            panel1.TabIndex = 1;
            // 
            // dataGridViewMonitor
            // 
            dataGridViewMonitor.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewMonitor.Dock = DockStyle.Fill;
            dataGridViewMonitor.Location = new Point(0, 0);
            dataGridViewMonitor.Name = "dataGridViewMonitor";
            dataGridViewMonitor.RowHeadersWidth = 51;
            dataGridViewMonitor.Size = new Size(623, 328);
            dataGridViewMonitor.TabIndex = 1;
         
            dataGridViewMonitor.CellEnter += dataGridViewMonitor_CellEnter;
            // 
            // button1
            // 
            button1.Location = new Point(264, 12);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 7;
            button1.Text = "Delete";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // frmMonitor
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(648, 399);
            Controls.Add(button1);
            Controls.Add(panel1);
            Name = "frmMonitor";
            Text = "Monitor";
            Load += frmMonitor_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewMonitor).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DataGridView dataGridViewMonitor;
        private Button button1;
    }
}