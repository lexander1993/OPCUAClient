using LibUA;
using LibUA.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class frmMonitor : Form
    {
        private frmDemo frmParent;

        private Proxy proxy;

        public LibUA.Client Client;

        public frmMonitor(LibUA.Client client)
        {
            proxy = new Proxy();
            Client = client;
            InitializeComponent();
            LoadSelectedObjects();
        }

        private uint _SubsId;

        public frmMonitor()
        {
            InitializeComponent();
        }

        public void LoadSelectedObjects()
        {
            if (dataGridViewMonitor.Columns.Count == 0)
            {
                dataGridViewMonitor.Columns.Add("SubscriptionID", "Subscription ID");
                dataGridViewMonitor.Columns.Add("DisplayName", "Display Name");
                dataGridViewMonitor.Columns.Add("NodeID", "Node ID");
                dataGridViewMonitor.Columns.Add("Value", "Value");
                dataGridViewMonitor.ReadOnly = true;
                dataGridViewMonitor.AllowUserToAddRows = false;
            }

            dataGridViewMonitor.Rows.Clear();

            foreach (var node in Proxy.SelectedMonitoredNodes)
            {
                int rowIndex = dataGridViewMonitor.Rows.Add(node.SubscriptionId, node.TagName, node.NodeId.ToString(), node.Value.ToString());
            }
        }

        private void frmMonitor_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Ensure the index is valid
            if (_SubsId != null)
            {
                // Show a confirmation dialog
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete Subscription ID: {_SubsId} ?",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                // If user confirms, delete the row
                if (result == DialogResult.Yes)
                {
                    uint[] respStatuses = [];
                    proxy.DeleteMonitoredNode(Client, _SubsId);
                    LoadSelectedObjects();

                }

            }
            else
            {
                MessageBox.Show("Please focus on a cell to delete the row.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        public void UpdateDataGrid(uint subscrId, uint handle, string value)
        {
            bool rowUpdated = false;

            foreach (DataGridViewRow row in dataGridViewMonitor.Rows)
            {
                // Check if the row's SubscriptionID column matches the given subscrId
                if (row.Cells[0].Value != null && (uint)row.Cells[0].Value == subscrId)
                {
                    row.Cells[3].Value = value;
                    rowUpdated = true;
                    break;
                }
            }


        }

        private void dataGridViewMonitor_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (_SubsId == null || dataGridViewMonitor.Rows[e.RowIndex].Cells[0] == null)
            {
                MessageBox.Show("Please focus on a cell to delete the row.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = dataGridViewMonitor.Rows[e.RowIndex].Cells[0];
                object cellValue = cell.Value;
                _SubsId = uint.Parse(cell.Value.ToString());

            }
        }
    }
}
