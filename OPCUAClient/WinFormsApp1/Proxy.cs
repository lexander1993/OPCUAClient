using LibUA.Core;
using LibUA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    internal class Proxy
    {

        public DemoClient client;
        public DemoClient Client
        {
            get { return client; }
            set { client = value; }
        }

        public bool isConnected = false;

        public static List<(string TagName, NodeId NodeId)> SelectedNodes { get; set; } = new List<(string, NodeId)>();
        public static List<(uint SubscriptionId, string TagName, NodeId NodeId, object Value)> SelectedMonitoredNodes { get; set; }
            = new List<(uint, string, NodeId, object)>();


        public void AddMonitoredNode(uint subscriptionId, string tagName, NodeId nodeId, object value = null)
        {
            if (!SelectedMonitoredNodes.Exists(x => x.NodeId == nodeId))
            {
                SelectedMonitoredNodes.Add((subscriptionId, tagName, nodeId, value));
            }
        }
        public void AddNode(string tagName, NodeId nodeId)
        {
            if (!SelectedNodes.Exists(x => x.NodeId == nodeId))
            {
                SelectedNodes.Add((tagName, nodeId));
            }
        }

        public void DeleteNode(NodeId nodeId)
        {
            SelectedNodes.RemoveAll(node => node.NodeId.Equals(nodeId));
        }
        public void DeleteMonitoredNode(LibUA.Client client,uint subscriptionId)
        {
            SelectedMonitoredNodes.RemoveAll(x => x.SubscriptionId == subscriptionId);
            
            client.DeleteMonitoredItems(subscriptionId, new uint[] { 0}, out uint[] respStatuses);
            client.DeleteSubscription(new[] {subscriptionId}, out respStatuses);

        }

        public DemoClient Connect(string strServer, int intPort)
        {
            if (client == null)
            {
                    var appDesc = new ApplicationDescription(
                        "urn:DemoApplication", "uri:DemoApplication", new LocalizedText("UA SDK client"),
                        ApplicationType.Client, null, null, null);

                    client = new DemoClient(strServer, intPort, 1000);
                    //var messageSecurityMode = MessageSecurityMode.SignAndEncrypt;
                    //var securityPolicy = SecurityPolicy.Basic256Sha256;
                    var messageSecurityMode = MessageSecurityMode.None;
                    var securityPolicy = SecurityPolicy.None;
                    bool useAnonymousUser = true;

                    client.Connect();
                    client.OpenSecureChannel(MessageSecurityMode.None, SecurityPolicy.None, null);
                    client.FindServers(out ApplicationDescription[] appDescs, new[] { "en" });
                    client.GetEndpoints(out EndpointDescription[] endpointDescs, new[] { "en" });
                    client.Disconnect();

                    // Will fail if no matching message security mode and security policy is found
                    var endpointDesc = endpointDescs.First(e =>
                        e.SecurityMode == messageSecurityMode &&
                        e.SecurityPolicyUri == Types.SLSecurityPolicyUris[(int)securityPolicy]);
                    byte[] serverCert = endpointDesc.ServerCertificate;

                    var connectRes = client.Connect();
                    var openRes = client.OpenSecureChannel(messageSecurityMode, securityPolicy, serverCert);
                    var createRes = client.CreateSession(appDesc, "urn:DemoApplication", 120);

                    StatusCode activateRes;
                    if (useAnonymousUser)
                    {
                        // Will fail if this endpoint does not allow Anonymous user tokens
                        string policyId = endpointDesc.UserIdentityTokens.First(e => e.TokenType == UserTokenType.Anonymous).PolicyId;
                        activateRes = client.ActivateSession(new UserIdentityAnonymousToken(policyId), new[] { "en" });
                    }
                    else
                    {
                        // Will fail if this endpoint does not allow UserName user tokens
                        string policyId = endpointDesc.UserIdentityTokens.First(e => e.TokenType == UserTokenType.UserName).PolicyId;
                        activateRes = client.ActivateSession(
                           new UserIdentityUsernameToken(policyId, "plc-user",
                               (new UTF8Encoding()).GetBytes("123"), Types.SignatureAlgorithmRsaOaep),
                           new[] { "en" });
                    }
                    return client;
            }
            return client;
        }

        public List<TreeNode> BrowseAvailableNodes(LibUA.Client client)
        {
            List<TreeNode> rootNodes = new List<TreeNode>();
            Queue<(NodeId, TreeNode)> nodeQueue = new Queue<(NodeId, TreeNode)>();
            nodeQueue.Enqueue((new NodeId(0, (uint)UAConst.ObjectsFolder), null)); 

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
                        .Where(refType => refType.ReferenceTypeId.EqualsNumeric(0, (uint)RefType.Organizes) ||
                                          refType.ReferenceTypeId.EqualsNumeric(0, (uint)RefType.HasComponent) ||
                                          refType.ReferenceTypeId.EqualsNumeric(0, (uint)RefType.HasProperty))
                        .OrderBy(refType => refType.DisplayName.Text) // Sort alphabetically
                        .ToList();

                    foreach (var reference in references)
                    {
                        TreeNode childNode = new TreeNode($"{reference.DisplayName.Text}") 
                        {
                            Tag = reference.TargetId
                        };

                        if (parentTreeNode == null)
                        {
                            rootNodes.Add(childNode); // Add to root list
                        }
                        else
                        {
                            parentTreeNode.Nodes.Add(childNode);
                        }

                        
                        if (reference.NodeClass == NodeClass.Object || reference.NodeClass == NodeClass.Method)
                        {
                            nodeQueue.Enqueue((reference.TargetId, childNode));
                        }
                    }
                }
            }

            return rootNodes;
        }


        //public List<TreeNode> BrowseAvailableNodes(LibUA.Client client)
        //{
        //    List<TreeNode> rootNodes = new List<TreeNode>();
        //    Queue<(NodeId, TreeNode)> nodeQueue = new Queue<(NodeId, TreeNode)>();
        //    nodeQueue.Enqueue((new NodeId(0, (uint)UAConst.ObjectsFolder), null)); // Start browsing from ObjectsFolder

        //    while (nodeQueue.TryDequeue(out var current))
        //    {
        //        NodeId currentNode = current.Item1;
        //        TreeNode parentTreeNode = current.Item2;

        //        client.Browse(new BrowseDescription[]
        //        {
        //    new BrowseDescription(
        //        currentNode,
        //        BrowseDirection.Forward,
        //        NodeId.Zero,
        //        true, 0xFFFFFFFFu, BrowseResultMask.All)
        //        }, 10000, out BrowseResult[] childrenBrowseResults);

        //        if (childrenBrowseResults.Length > 0 && childrenBrowseResults[0].Refs != null)
        //        {
        //            var references = childrenBrowseResults[0].Refs
        //                .Where(refType => refType.ReferenceTypeId.EqualsNumeric(0, (uint)RefType.Organizes))
        //                .OrderBy(refType => refType.DisplayName.Text) // Sort alphabetically
        //                .ToList();

        //            foreach (var reference in references)
        //            {
        //                TreeNode childNode = new TreeNode(reference.DisplayName.Text)
        //                {
        //                    Tag = reference.TargetId
        //                };

        //                if (parentTreeNode == null)
        //                {
        //                    rootNodes.Add(childNode); // Add to root list
        //                }
        //                else
        //                {
        //                    parentTreeNode.Nodes.Add(childNode);
        //                }

        //                nodeQueue.Enqueue((reference.TargetId, childNode));
        //            }
        //        }
        //    }

        //    return rootNodes;
        //}

        public DataValue[] ReadObjectByNodeId(DemoClient client, NodeId nodeId, NodeAttribute attributeId)
        {
            
            var readRes = client.Read(new ReadValueId[]
            {
            new ReadValueId(nodeId, attributeId, null, new QualifiedName(0, null)),
            }, out DataValue[] dvs);
            
            return dvs;
        }

        public HistoryReadResult[] ReadNodeHistory(DemoClient client, NodeId nodeId)
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
            return histResults;
        }

        public uint CreateSubscription(DemoClient client, NodeId nodeId)
        {
            uint subscrId;
            client.CreateSubscription(0, 1000, true, 0, out subscrId);
            return subscrId;
        }

        public StatusCode SetPublishingMode(DemoClient client, uint subscrId, out uint[] results)
        {
            StatusCode status = client.SetPublishingMode(true, new[] { subscrId, 10u }, out uint[] respstatuses);

            results = respstatuses; 

            return status;
        }

        public StatusCode ModifySubscription(DemoClient client, uint subscrId, out uint results)
        {
            StatusCode status = client.ModifySubscription(subscrId, 0, 100, true, 0, out uint respStatuses); 

            results = respStatuses;

            return status;
        }

        public StatusCode CreateMonitoredItems(DemoClient client, uint subscrId, NodeId nodeId, out MonitoredItemCreateResult[] results)
        {
            var tagsMonitorId = new uint[3];

            StatusCode status = client.CreateMonitoredItems(subscrId, TimestampsToReturn.Both,
                new MonitoredItemCreateRequest[]
                {

                    new MonitoredItemCreateRequest(
                        new ReadValueId(nodeId, NodeAttribute.Value, null, new QualifiedName()),
                        MonitoringMode.Reporting,
                        new MonitoringParameters(tagsMonitorId[0], 0, null, 100, false)),


                }, out MonitoredItemCreateResult[] monitorCreateResults);

            results = monitorCreateResults;

            return status;
        }

        public class DemoClient : Client
        {


            private X509Certificate2 appCertificate = null;
            private RSA cryptPrivateKey = null;

            public override X509Certificate2 ApplicationCertificate
            {
                get { return appCertificate; }
            }

            public override RSA ApplicationPrivateKey
            {
                get { return cryptPrivateKey; }
            }

            public void LoadCertificateAndPrivateKey()
            {
                try
                {
                    // Try to load existing (public key) and associated private key
                    appCertificate = new X509Certificate2("ClientCert.der");
                    cryptPrivateKey = RSA.Create();
                    cryptPrivateKey.KeySize = 2048;

                    var rsaPrivParams = UASecurity.ImportRSAPrivateKey(File.ReadAllText("ClientKey.pem"));
                    cryptPrivateKey.ImportParameters(rsaPrivParams);
                }
                catch
                {
                    // Make a new certificate (public key) and associated private key
                    var dn = new X500DistinguishedName("CN=Client certificate;OU=Demo organization",
                        X500DistinguishedNameFlags.UseSemicolons);
                    SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
                    sanBuilder.AddUri(new Uri("urn:DemoApplication"));

                    using (RSA rsa = RSA.Create(4096))
                    {
                        var request = new CertificateRequest(dn, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                        request.CertificateExtensions.Add(sanBuilder.Build());
                        request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
                        request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));

                        request.CertificateExtensions.Add(new X509KeyUsageExtension(
                            X509KeyUsageFlags.DigitalSignature |
                            X509KeyUsageFlags.NonRepudiation |
                            X509KeyUsageFlags.DataEncipherment |
                            X509KeyUsageFlags.KeyEncipherment, false));

                        request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection {
                            new Oid("1.3.6.1.5.5.7.3.8"),
                            new Oid("1.3.6.1.5.5.7.3.1"),
                            new Oid("1.3.6.1.5.5.7.3.2"),
                            new Oid("1.3.6.1.5.5.7.3.3"),
                            new Oid("1.3.6.1.5.5.7.3.4"),
                            new Oid("1.3.6.1.5.5.7.3.8"),
                            new Oid("1.3.6.1.5.5.7.3.9"),
                        }, true));

                        var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
                            new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));

                        appCertificate = new X509Certificate2(certificate.Export(X509ContentType.Pfx, ""),
                            "", X509KeyStorageFlags.DefaultKeySet);

                        var certPrivateParams = rsa.ExportParameters(true);
                        File.WriteAllText("ClientCert.der", UASecurity.ExportPEM(appCertificate));
                        File.WriteAllText("ClientKey.pem", UASecurity.ExportRSAPrivateKey(certPrivateParams));

                        cryptPrivateKey = RSA.Create();
                        cryptPrivateKey.KeySize = 2048;
                        cryptPrivateKey.ImportParameters(certPrivateParams);
                    }
                }
            }

            public DemoClient(string Target, int Port, int Timeout)
                : base(Target, Port, Timeout)
            {
                LoadCertificateAndPrivateKey();
            }


            public override void NotifyDataChangeNotifications(uint subscrId, uint[] clientHandles, DataValue[] notifications)
            {

                if (Application.OpenForms["frmMonitor"] is frmMonitor monitorForm)
                {
                    monitorForm.Invoke(new Action(() =>
                    {
                        for (int i = 0; i < clientHandles.Length; i++)
                        {
                            monitorForm.UpdateDataGrid(subscrId, clientHandles[i], notifications[i].Value.ToString());
                        }
                    }));
                }
            }

            public override void NotifyEventNotifications(uint subscrId, uint[] clientHandles, object[][] notifications)
            {
                for (int i = 0; i < clientHandles.Length; i++)
                {

                    Console.WriteLine("subscrId {0} handle {1}: {2}", subscrId, clientHandles[i], string.Join(",", notifications[i]));
                }
            }

        }



    }
}
