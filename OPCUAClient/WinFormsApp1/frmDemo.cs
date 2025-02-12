using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using LibUA;
using LibUA.Core;

using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static WinFormsApp1.OPC;


namespace WinFormsApp1
{
    public partial class frmDemo : Form
    {
        private static DemoClient client;
        private static readonly object _lock = new object();

        private NodeId _selectedNodeId;
        private string _selectedNodeName;
        private uint _subscriptionId;

        //public static DemoClient GetClient()
        //{
        //    if (client == null)
        //    {
        //        lock (_lock) // Ensure thread safety
        //        {
        //            if (client == null)
        //            {
        //                try
        //                {
        //                    client = new DemoClient("127.0.0.1", 53530, 1000);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine("Failed to create client: " + ex.Message);
        //                    client = null; // Prevent a half-initialized instance
        //                }
        //            }
        //        }
        //    }
        //    return client;
        //}


        public frmDemo()
        {
            InitializeComponent();

        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            OPC opc = new OPC();
            DemoClient client = opc.EnsureConnected(txtServerName.Text, int.Parse(txtPort.Text)); // Ensure client is connected before performing any action

            try
            {
                // Clear the tree before populating
                treeView1.BeginUpdate();
                treeView1.Nodes.Clear();

                Queue<(NodeId, TreeNode)> nodeQueue = new Queue<(NodeId, TreeNode)>();
                nodeQueue.Enqueue((new NodeId(0, (uint)UAConst.ObjectsFolder), null)); // Start browsing from ObjectsFolder

                while (nodeQueue.TryDequeue(out var current))
                {
                    NodeId currentNode = current.Item1;
                    TreeNode parentTreeNode = current.Item2;

                    client.Browse(new BrowseDescription[]
                    {
                new BrowseDescription(
                    currentNode,
                    BrowseDirection.Forward,
                    NodeId.Zero,
                    true, 0xFFFFFFFFu, BrowseResultMask.All)
                    }, 10000, out BrowseResult[] childrenBrowseResults);

                    if (childrenBrowseResults.Length > 0 && childrenBrowseResults[0].Refs != null)
                    {
                        var references = childrenBrowseResults[0].Refs
                            .Where(refType => refType.ReferenceTypeId.EqualsNumeric(0, (uint)RefType.Organizes))
                            .OrderBy(refType => refType.DisplayName.Text) // Sort alphabetically
                            .ToList();

                        foreach (var reference in references)
                        {
                            TreeNode childNode = new TreeNode(reference.DisplayName.Text)
                            {
                                Tag = reference.TargetId
                            };

                            if (parentTreeNode == null)
                            {
                                treeView1.Nodes.Add(childNode); // Directly add to TreeView root
                            }
                            else
                            {
                                parentTreeNode.Nodes.Add(childNode);
                            }

                            nodeQueue.Enqueue((reference.TargetId, childNode));
                        }
                    }
                }

                treeView1.EndUpdate(); // Refresh UI in one go for better performance

                MessageBox.Show("Connection to server succesful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during browsing: {ex.Message}");
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            OPC opc = new OPC();
            DemoClient client = opc.EnsureConnected(txtServerName.Text, int.Parse(txtPort.Text));

            if (e.Node?.Tag is NodeId nodeId)
            {
                LoadAttribute(client, nodeId);

                ReadHistoricalData(client, nodeId);
                _selectedNodeId = nodeId;
                _selectedNodeName = e.Node.Text;
            }
        }

        private void InitializeDataGridViewAttr()
        {
            dataGridViewObject.Invoke((MethodInvoker)(() =>
            {
                dataGridViewObject.Rows.Clear();
                dataGridViewObject.Columns.Clear();

                dataGridViewObject.Columns.Add("Attribute", "Attribute");
                dataGridViewObject.Columns.Add("Value", "Value");

                // Make the "Attribute" column read-only
                dataGridViewObject.Columns["Attribute"].ReadOnly = true;

                // Set column width
                dataGridViewObject.Columns["Attribute"].Width = 150;
            }));
        }
        private void LoadAttribute(Client client, NodeId nodeId)
        {
            try
            {

                InitializeDataGridViewAttr();

                for (int i = 1; i <= 27; i++)
                {
                    string attrName = ((NodeAttribute)i).ToString();
                    var readRes = client.Read(new ReadValueId[]
                    {
                new ReadValueId(nodeId, ((NodeAttribute)i), null, new QualifiedName(0, null)),
                    }, out DataValue[] dvs);

                    if (dvs != null)
                    {
                        foreach (var dv in dvs)
                        {
                            var res = dv?.Value;
                            Console.WriteLine($"Attribute: {attrName}, Value: {res}");

                            dataGridViewObject.Invoke((MethodInvoker)(() =>
                            {
                                int rowIndex = dataGridViewObject.Rows.Add(attrName, res);

                                // Make all cells read-only except column "Value" when i == 13
                                foreach (DataGridViewCell cell in dataGridViewObject.Rows[rowIndex].Cells)
                                {
                                    cell.ReadOnly = true;
                                }

                                if (i == 13)
                                {
                                    dataGridViewObject.Rows[rowIndex].Cells["Value"].ReadOnly = false;
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



        private void InitializeDataGridViewHist()
        {
            dataGridView1.Invoke((MethodInvoker)(() =>
            {
                // Clear existing data
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                // Add columns
                dataGridView1.Columns.Add("DateTime", "Server Timestamp");
                dataGridView1.Columns.Add("Value", "Value");

                // Adjust column width
                dataGridView1.Columns["DateTime"].Width = 150;
                dataGridView1.Columns["Value"].Width = 150;

            }));
        }
        private void ReadHistoricalData(Client client, NodeId nodeId)
        {
            try
            {
                client.HistoryRead(new ReadRawModifiedDetails(
                    false,
                    new DateTime(2025, 01, 01), // Start time
                    new DateTime(2025, 03, 01), // End time
                    100,
                    true),
                    TimestampsToReturn.Both,
                    false,
                    new HistoryReadValueId[]
                    {
                new HistoryReadValueId(nodeId, null, new QualifiedName(), null),
                    },
                    out HistoryReadResult[] histResults);

                InitializeDataGridViewHist();

                
                try {
                    if (histResults != null) // Ensure result and Values are not null
                    {
                        foreach (var result in histResults)
                        {

                            foreach (var val in result.Values)
                            {
                                if (val != null) // Prevent null value entries
                                {

                                    dataGridView1.Rows.Add(val.ServerTimestamp, val.Value);
                                }
                            }
                        } 

                }
            }
                
            catch (Exception ex)
            {
                    Console.WriteLine($"An error occurred while processing historical data: {ex.Message}");
                
            }

            dataGridView1.Sort(dataGridView1.Columns["DateTime"], ListSortDirection.Descending);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during read operation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateMonitoring(Client client, NodeId nodeId)
        {
            uint[] respstatuses = [];
            client.CreateSubscription(0, 1000, true, 0, out uint subscrId);

            _subscriptionId = subscrId;

            // Second will have response BadSubscriptionIdInvalid
            client.SetPublishingMode(true, new[] { subscrId, 10u }, out respstatuses);

            client.ModifySubscription(subscrId, 0, 100, true, 0, out uint respStatus);

            uint clientHandleEventMonitor = 0;
            var tagsMonitorId = new uint[3];
            for (int i = 0; i < 3; i++) { tagsMonitorId[i] = (uint)(1 + i); }

            client.CreateMonitoredItems(subscrId, TimestampsToReturn.Both,
                new MonitoredItemCreateRequest[]
                {

                    new MonitoredItemCreateRequest(
                        new ReadValueId(nodeId, NodeAttribute.Value, null, new QualifiedName()),
                        MonitoringMode.Reporting,
                        new MonitoringParameters(tagsMonitorId[0], 0, null, 100, false)),


                }, out MonitoredItemCreateResult[] monitorCreateResults);

            dataGridView2.Rows.Add(subscrId, _selectedNodeId, _selectedNodeName, 0);
        }

        public void UpdateDataGrid(uint subscrId, uint handle, string value)
        {
            bool rowUpdated = false;

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                // Check if the row's SubscriptionID column matches the given subscrId
                if (row.Cells[0].Value != null && (uint)row.Cells[0].Value == subscrId)
                {
                    // Update the handle and value columns

                    row.Cells[3].Value = value;
                    rowUpdated = true;
                    break; // Exit loop since we found and updated the row
                }
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OPC opc = new OPC();
            DemoClient client = opc.EnsureConnected(txtServerName.Text, int.Parse(txtPort.Text));

            uint[] respstatuses = [];
            client.CreateSubscription(0, 1000, true, 0, out uint subscrId);

            // Second will have response BadSubscriptionIdInvalid
            client.SetPublishingMode(true, new[] { subscrId, 10u }, out respstatuses);

            client.ModifySubscription(subscrId, 0, 100, true, 0, out uint respStatus);

            uint clientHandleEventMonitor = 0;
            var tagsMonitorId = new uint[3];
            for (int i = 0; i < 3; i++) { tagsMonitorId[i] = (uint)(1 + i); }

            client.CreateMonitoredItems(subscrId, TimestampsToReturn.Both,
                new MonitoredItemCreateRequest[]
                {
                    new MonitoredItemCreateRequest(
                        new ReadValueId(new NodeId(2, 1), NodeAttribute.EventNotifier, null, new QualifiedName()),
                        MonitoringMode.Reporting,
                        new MonitoringParameters(clientHandleEventMonitor, 0, null, 100, true)),

                    new MonitoredItemCreateRequest(
                        new ReadValueId(new NodeId(2, 2), NodeAttribute.Value, null, new QualifiedName()),
                        MonitoringMode.Reporting,
                        new MonitoringParameters(tagsMonitorId[0], 0, null, 100, false)),


                }, out MonitoredItemCreateResult[] monitorCreateResults);


        }




        private void frmDemo_Load(object sender, EventArgs e)
        {
            InitializeDataGridViewAttr();
            InitializeDataGridViewHist();


            if (dataGridView2.Columns.Count == 0)
            {
                dataGridView2.Columns.Add("SubscriptionID", "Subscription ID");
                dataGridView2.Columns.Add("NodeID", "Node ID");
                dataGridView2.Columns.Add("DisplayName", "Display Name");
                dataGridView2.Columns.Add("Value", "Value");
                dataGridView2.ReadOnly = true;
            }
        }

        private void dataGridViewObject_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                OPC opc = new OPC();
                DemoClient client = opc.EnsureConnected(txtServerName.Text, int.Parse(txtPort.Text));

                if (treeView1.SelectedNode?.Tag is NodeId nodeId)
                {


                    DataGridViewCell cell = dataGridViewObject.Rows[12].Cells[e.ColumnIndex]; // Get the edited column of row 13
                    object cellValue = cell.Value; // Extract the cell value

                    if (cellValue != null)
                    {

                        // Attempt to write value
                        client.Write(new WriteValue[]
                        {
                        new WriteValue(
                            nodeId, NodeAttribute.Value, null, new DataValue(cellValue, StatusCode.Good, DateTime.Now))
                        }, out uint[] responseStatuses);

                        

                        MessageBox.Show("Write operation successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Cell value is null. Please enter a valid value.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    ReadHistoricalData(client, nodeId);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during write operation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            OPC opc = new OPC();
            DemoClient client = opc.EnsureConnected(txtServerName.Text, int.Parse(txtPort.Text)); // Ensure client is connected before performing any action

            
            //client.Dispose();
            treeView1.Nodes.Clear();
            dataGridViewObject.Rows.Clear();
            

            // Loop through all rows and delete subscriptions
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["SubscriptionID"].Value != null)
                {
                 
                    uint.TryParse(row.Cells["SubscriptionID"].Value.ToString(), out uint subscriptionId);
                    uint[] respStatuses = [];
                    client.DeleteMonitoredItems(subscriptionId, new uint[] { 0, 1, 2, 3, 4, 5 }, out respStatuses);
                    client.DeleteSubscription(new[] { subscriptionId }, out respStatuses);
                  
                }
            }

            dataGridView1.Rows.Clear();


            dataGridView2.Rows.Clear();
            client.Disconnect();
            MessageBox.Show($"Disconnection successful", "Disconnect", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnMonitor_Click(object sender, EventArgs e)
        {
            if (_selectedNodeId == null)
            {
                MessageBox.Show("Please select a node in the TreeView before monitoring.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OPC opc = new OPC();
            DemoClient client = opc.EnsureConnected(txtServerName.Text, int.Parse(txtPort.Text));

            // Use the stored NodeId
            CreateMonitoring(client, _selectedNodeId);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OPC opc = new OPC();
            DemoClient client = opc.EnsureConnected(txtServerName.Text, int.Parse(txtPort.Text)); // Ensure client is connected before performing any action


            // Ensure there's an active cell selected
            if (dataGridView2.CurrentCell != null)
            {
                // Get the row index of the focused cell
                int rowIndex = dataGridView2.CurrentCell.RowIndex;

                // Ensure the index is valid
                if (rowIndex >= 0)
                {
                    // Get the values from the selected row
                    string subscriptionID = dataGridView2.Rows[rowIndex].Cells["SubscriptionID"].Value?.ToString();
                    string displayName = dataGridView2.Rows[rowIndex].Cells["DisplayName"].Value?.ToString();

                    // Show a confirmation dialog
                    DialogResult result = MessageBox.Show(
                        $"Are you sure you want to delete Subscription ID: {subscriptionID} \nDisplay Name: {displayName}?",
                        "Confirm Deletion",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    // If user confirms, delete the row
                    if (result == DialogResult.Yes)
                    {
                        uint[] respStatuses = [];
                        client.DeleteMonitoredItems(uint.Parse(subscriptionID), new uint[] { 0, 1, 2, 3, 4, 5 }, out respStatuses);
                        client.DeleteSubscription(new[] { uint.Parse(subscriptionID) }, out respStatuses);
                        dataGridView2.Rows.RemoveAt(rowIndex);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please focus on a cell to delete the row.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            
        }

        private void frmDemo_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                OPC opc = new OPC();
                DemoClient client = opc.EnsureConnected(txtServerName.Text, int.Parse(txtPort.Text)); // Ensure client is connected before performing any action

                client.Disconnect();
          
                // Perform cleanup tasks here (if needed)
                MessageBox.Show("The application has been closed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while closing the form:\n" + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

    }
}
