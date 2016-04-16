using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dab_Client;
using System.Threading;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


namespace Dab_Client
{
    public partial class FormSignUp : Form
    {
        


        Thread tcpThread; // watek połączeniowy
        public String Server { get { return "localhost"; } }      // IP serwera
        public int Port { get { return 2000; } }     // TCP port serwera

        public void SetupConnection()    // cale info odnosnie ustawiania polaczanie, narazie do testow w takiej formie
        {
           // try {
                DABClient.client = new TcpClient(Server, Port);

                DABClient.netStream = DABClient.client.GetStream();
                DABClient.ssl = new SslStream(DABClient.netStream, false, new RemoteCertificateValidationCallback(ValidateCert));
                DABClient.ssl.AuthenticateAsClient("Dab");
                DABClient.br = new BinaryReader(DABClient.ssl, Encoding.UTF8);
                DABClient.bw = new BinaryWriter(DABClient.ssl, Encoding.UTF8);
            



            int hello = DABClient.br.ReadInt32();
            if (hello == Komunikaty.DAB_HELLO)
            {
                MessageBox.Show("Hello dziala");
            }

            DABClient.bw.Write(Komunikaty.DAB_HELLO);
           // DABClient.bw.Flush();

            DABClient.bw.Write(LoginVariables.reg ? Komunikaty.DAB_REG : Komunikaty.DAB_LOG);
          //  DABClient.bw.Flush();
            hello = 0;
            hello = DABClient.br.ReadInt32();
            string user = "Test";
            string pass = LoginVariables.currentPassword;
            MessageBox.Show("Wysyłam dane: " + LoginVariables.currentUsername + "  " + LoginVariables.currentPassword);
            
            DABClient.bw.Write(user);
            DABClient.bw.Flush();
           // DABClient.bw.Write(pass);
           // DABClient.bw.Flush();

            int answer = DABClient.br.ReadByte();
            if (answer == Komunikaty.DAB_OK)
            {
                MessageBox.Show("Operacja powiodła się.");

            }

            /*     if (DABClient.netStream.CanWrite)
                      {


                         Byte[] sendLogin = Encoding.UTF8.GetBytes(LoginVariables.currentUsername); //bufor na login
                         Byte[] sendLoginSize = Encoding.UTF8.GetBytes(sendLogin.Length.ToString()); //ile zajmie login
                         DABClient.netStream.Write(sendLoginSize, 0, sendLoginSize.Length); // wyslij rozmiar
                         DABClient.netStream.Write(sendLogin, 0, sendLogin.Length);   //wyslij login

                         Byte[] sendPass = Encoding.UTF8.GetBytes(LoginVariables.currentPassword); // bufor na haslo
                         Byte[] sendPassSize = Encoding.UTF8.GetBytes(sendPass.Length.ToString()); // ile zajmie haslo
                         DABClient.netStream.Write(sendPassSize, 0, sendPassSize.Length); // wyslij rozmiar
                         DABClient.netStream.Write(sendPass, 0, sendPass.Length);      // wyslij haslo

                     }
                     else
                    {
                         MessageBox.Show("Nie dziala");
                     }
     */
            CloseConnection();
     /*   }catch (Exception e)
            {
                MessageBox.Show("Oops wystąpił błąd.");
            }

    */

        }
        




        void CloseConnection() // zakonczenie polaczenia
        {
            DABClient.netStream.Close();
           
            DABClient.client.Close();
            
            LoginVariables.connected = false;
            DABClient.br.Close();
            DABClient.bw.Close();
            DABClient.ssl.Close();
        }
        public static bool ValidateCert(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        public void connect(string username, string password)
        {
            if (!LoginVariables.connected)
            {
                LoginVariables.connected = true;
                tcpThread = new Thread(new ThreadStart(SetupConnection));
                tcpThread.Start();
                
            }
            else
            { MessageBox.Show("Dane konto jest już zalogowane."); }
        }
        public void Disconnect()
        {
            if (LoginVariables.connected)
            {
                CloseConnection();
                
            }
        }
        public FormSignUp()
        {
            InitializeComponent();
        }

        private void textBoxUsername_TextChanged(object sender, EventArgs e)
        {
            LoginVariables.currentUsername = textBoxUsername.Text;
        }

        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            LoginVariables.reg = false;
            connect(LoginVariables.currentUsername, LoginVariables.currentPassword);
            
            
        }
        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            LoginVariables.currentPassword = textBoxPassword.Text;
        }

        private void buttonReg_Click(object sender, EventArgs e)
        {
            LoginVariables.reg = true;
            connect(LoginVariables.currentUsername, LoginVariables.currentPassword);
        }
    }
}

