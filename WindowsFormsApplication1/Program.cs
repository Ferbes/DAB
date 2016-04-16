using Dab_Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
         public X509Certificate2 cert = new X509Certificate2("server.pfx", "instant");

        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormSignUp());

        }
    }
}
