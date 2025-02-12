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
    internal class OPC
    {

        public static DemoClient client;
        public bool isConnected = false;


        public DemoClient EnsureConnected(string strServer, int intPort)
        {
            if (!isConnected || client == null || !client.IsConnected) 
            {
               
                try
                {
                    var appDesc = new ApplicationDescription(
                        "urn:DemoApplication", "uri:DemoApplication", new LocalizedText("UA SDK client"),
                        ApplicationType.Client, null, null, null);

                    var client = new DemoClient(strServer, intPort, 1000);
                    var messageSecurityMode = MessageSecurityMode.SignAndEncrypt;
                    var securityPolicy = SecurityPolicy.Basic256Sha256;
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
                catch (Exception ex)
                {
                    MessageBox.Show($"Error connecting: {ex.Message}");
                    return null;
                }


            }
            return client;
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
                
                if (Application.OpenForms["frmDemo"] is frmDemo demoForm)
                {
                    demoForm.Invoke(new Action(() =>
                    {
                        for (int i = 0; i < clientHandles.Length; i++)
                        {
                            demoForm.UpdateDataGrid(subscrId, clientHandles[i], notifications[i].Value.ToString());
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
