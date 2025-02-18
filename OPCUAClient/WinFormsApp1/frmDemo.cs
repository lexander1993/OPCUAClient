using LibUA.Core;
using LibUA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WinFormsApp1.Proxy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WinFormsApp1
{
    public partial class frmDemo : Form
    {
        private DemoClient client;
        private Proxy proxy;
        private uint _subscriptionId;
        private NodeId _selectedNodeId;
        private string _selectedNodeName;
        private object _selectedValue;

        public frmDemo()
        {
            InitializeComponent();
            proxy = new Proxy();
        }

        private void frmDemo_Load(object sender, EventArgs e)
        {
            proxy = new Proxy();
            InitializeDataGridViewObjects();
            InitializeDataGridViewObjectDetails();
            InitializeDataGridViewHist();
            LoadSelectedObjects();
        }


        public void LoadSelectedObjects()
        {
            dataGridViewObject.Rows.Clear();

            foreach (var node in Proxy.SelectedNodes)
            {
                int rowIndex = dataGridViewObject.Rows.Add(node.TagName, node.NodeId.ToString());
            }
        }


        private void InitializeDataGridViewObjects()
        {
            dataGridViewObject.Invoke((MethodInvoker)(() =>
            {
                dataGridViewObject.Rows.Clear();
                dataGridViewObject.Columns.Clear();

                dataGridViewObject.Columns.Add("TagName", "Object Name");
                dataGridViewObject.Columns.Add("NodeID", "Node ID");

                // Make the "Attribute" column read-only
                dataGridViewObject.ReadOnly = true;

                // Set column width
                dataGridViewObject.Columns["TagName"].Width = 150;
                dataGridViewObject.Columns["NodeID"].Width = 150;
                dataGridViewObject.AllowUserToAddRows = false;
            }));
        }

        private void InitializeDataGridViewObjectDetails()
        {
            dataGridViewObjectDetails.Invoke((MethodInvoker)(() =>
            {
                dataGridViewObjectDetails.Rows.Clear();
                dataGridViewObjectDetails.Columns.Clear();

                dataGridViewObjectDetails.Columns.Add("Attribute", "Attribute");
                dataGridViewObjectDetails.Columns.Add("Value", "Value");

                // Make the "Attribute" column read-only
                dataGridViewObjectDetails.Columns["Attribute"].ReadOnly = true;

                // Set column width
                dataGridViewObjectDetails.Columns["Attribute"].Width = 150;
                dataGridViewObjectDetails.AllowUserToAddRows = false;
            }));
        }

        private void InitializeDataGridViewHist()
        {
            dataGridViewHistory.Invoke((MethodInvoker)(() =>
            {
                // Clear existing data
                dataGridViewHistory.Rows.Clear();
                dataGridViewHistory.Columns.Clear();

                // Add columns
                dataGridViewHistory.Columns.Add("DateTime", "Server Timestamp");
                dataGridViewHistory.Columns.Add("Value", "Value");

                // Adjust column width
                dataGridViewHistory.Columns["DateTime"].Width = 150;
                dataGridViewHistory.Columns["Value"].Width = 150;
                dataGridViewHistory.AllowUserToAddRows = false;

            }));
        }

        private void LoadAttribute(NodeId nodeId)
        {
            InitializeDataGridViewObjectDetails();
            try
            {
                int i = 1;
                for (i = 1; i <= 27; i++)
                {
                    string attrName = ((NodeAttribute)i).ToString();
                    DataValue[] dvs = proxy.ReadObjectByNodeId(client, nodeId, (NodeAttribute)i);

                    if (dvs != null)
                    {
                        foreach (var dv in dvs)
                        {
                            var res = dv?.Value;

                            dataGridViewObjectDetails.Invoke((MethodInvoker)(() =>
                            {
                                int rowIndex = dataGridViewObjectDetails.Rows.Add(attrName, res);

                                // Make all cells read-only except column "Value" when i == 13
                                foreach (DataGridViewCell cell in dataGridViewObjectDetails.Rows[rowIndex].Cells)
                                {
                                    cell.ReadOnly = true;
                                }

                                if (i == 13)
                                {
                                    _selectedValue = res;
                                    dataGridViewObjectDetails.Rows[rowIndex].Cells["Value"].ReadOnly = false;
                                }
                            }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading attributes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private void dataGridViewObject_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Ensure a valid cell is clicked
        //    {
        //        DataGridViewCell cell = dataGridViewObject.Rows[e.RowIndex].Cells[1]; // Get the edited column of row 13
        //        object cellValue = cell.Value; // Extract the cell value

        //        NodeId nodeId = NodeId.TryParse(cellValue.ToString());
        //        _selectedNodeId = nodeId;
        //        _selectedNodeName = dataGridViewObject.Rows[e.RowIndex].Cells[1].Value.ToString();

        //        LoadAttribute(nodeId);
        //        ReadHistoricalData(nodeId);
        //    }
        //}

        private void btnConnect_Click(object sender, EventArgs e)
        {
            proxy.Connect(txtServerName.Text, int.Parse(txtPort.Text));
            client = proxy.client;
            MessageBox.Show($"Connection successful", "Connect", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            client.Dispose();
            client = null;
            InitializeDataGridViewObjects();
            InitializeDataGridViewObjectDetails();
            InitializeDataGridViewHist();
            MessageBox.Show($"Disconnection successful", "Disconnect", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (client == null)
            {
                MessageBox.Show($"Please connect to server", "Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                frmBrowsing browseForm = new frmBrowsing(client);
                browseForm.ShowDialog();
                LoadSelectedObjects();
            }
        }

        private void ReadHistoricalData(NodeId nodeId)
        {
            try
            {
                proxy = new Proxy();
                HistoryReadResult[] histResults = proxy.ReadNodeHistory(client, nodeId);
                InitializeDataGridViewHist();
                try
                {
                    if (histResults != null) // Ensure result and Values are not null
                    {
                        foreach (var result in histResults)
                        {

                            foreach (var val in result.Values)
                            {
                                if (val != null) // Prevent null value entries
                                {

                                    dataGridViewHistory.Rows.Add(val.ServerTimestamp, val.Value);
                                }
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while processing historical data: {ex.Message}");
                }

                dataGridViewHistory.Sort(dataGridViewHistory.Columns["DateTime"], ListSortDirection.Descending);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during read operation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMonitor_Click(object sender, EventArgs e)
        {

            if (_selectedNodeId == null)
            {
                MessageBox.Show($"Please click the cell to monitor", "Monitored Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                _subscriptionId = proxy.CreateSubscription(client, _selectedNodeId);

                proxy.SetPublishingMode(client, _subscriptionId, out uint[] respstatuses);
                proxy.ModifySubscription(client, _subscriptionId, out uint respstatus);
                proxy.CreateMonitoredItems(client, _subscriptionId, _selectedNodeId, out MonitoredItemCreateResult[] monitorCreateResults);

                proxy.AddMonitoredNode(_subscriptionId, _selectedNodeName, _selectedNodeId, _selectedValue);

                MessageBox.Show($"{_selectedNodeName} has been added in Monitored Items", "Monitored Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedNodeId != null)
            {
                proxy.DeleteNode(_selectedNodeId);
                LoadSelectedObjects();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmMonitor monitorForm = new frmMonitor(client);
            monitorForm.ShowDialog();
        }

        private void dataGridViewObject_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) 
            {
                DataGridViewCell cell = dataGridViewObject.Rows[e.RowIndex].Cells[1]; // Get the edited column of row 13
                object cellValue = cell.Value; 

                NodeId nodeId = NodeId.TryParse(cellValue.ToString());
                _selectedNodeId = nodeId;
                _selectedNodeName = dataGridViewObject.Rows[e.RowIndex].Cells[0].Value.ToString();

                LoadAttribute(nodeId);
                ReadHistoricalData(nodeId);
            }
        }
    }
}
