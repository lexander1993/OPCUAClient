

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using LibUA;
using LibUA.Core;
using static LibUA.Core.SLChannel;



namespace WinFormsApp1
{
    internal class Program
    {
           private static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new frmDemo());


        }
    }

    
}
