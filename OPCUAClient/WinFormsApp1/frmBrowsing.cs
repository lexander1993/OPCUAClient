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

namespace WinFormsApp1
{

    public partial class frmBrowsing : Form
    {
        private frmDemo frmParent;

        private Proxy proxy;

        public LibUA.Client Client { get; set; }

        public frmBrowsing(LibUA.Client client)
        {

            Client = client;
            InitializeComponent();
            LoadTreeView();
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {

       
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            Client.Dispose();
            Client = null;
            MessageBox.Show($"Disconnection successful", "Disconnect", MessageBoxButtons.OK, MessageBoxIcon.Information); 
        }

        private void LoadTreeView()
        {
            try
            {
                Proxy proxy = new Proxy();
                treeView1.BeginUpdate();
                treeView1.Nodes.Clear();
                
                List<TreeNode> nodes = proxy.BrowseAvailableNodes(Client);
                treeView1.Nodes.AddRange(nodes.ToArray());

                treeView1.EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tree view: {ex.Message}");
            }
        }



        private void treeView1_NodeMouseDoubleClick_1(object sender, TreeNodeMouseClickEventArgs e)
        {
            Proxy proxy = new Proxy();
            if (e.Node != null && e.Node.Tag is NodeId nodeId)
            {
                string tagName = e.Node.Text;
                proxy.AddNode(tagName, nodeId);
                MessageBox.Show($"{tagName} has been selected");
            }
          
        }

        private void frmBrowsing_Load(object sender, EventArgs e)
        {

        }
    }
}
