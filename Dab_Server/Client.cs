using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dab_Server
{
    public class Client
    {
        TcpClient client;
        Program prog;
        NetworkStream netStream;
        SslStream ssl;
        BinaryReader br;
        BinaryWriter bw;
        

        public Client(Program p, TcpClient c)
        {
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine("Serwer otrzymał nowe połączenie");
            prog = p;
            client = c;
            Console.WriteLine("Uruchamiam wątek klienta");
            
            (new Thread(new ThreadStart(SetupConnection))).Start();

            

        }



        public void SetupConnection()
        {


         //   while (client.Connected)
           // {
                netStream = client.GetStream();
                ssl = new SslStream(netStream, false);
                ssl.AuthenticateAsServer(prog.cert, false, SslProtocols.Tls, true);
                br = new BinaryReader(ssl, Encoding.UTF8);
                bw = new BinaryWriter(ssl, Encoding.UTF8);

                bw.Write(Komunikaty.DAB_HELLO);
                bw.Flush();

                int hello = br.ReadInt32();
                if (hello == Komunikaty.DAB_HELLO)
                {
                    Console.WriteLine("Hello dziala");
                
                }
                byte logMode = br.ReadByte();
                bw.Write(Komunikaty.DAB_HELLO);
            bw.Flush();

            string name = br.ReadString();
            Console.WriteLine(name);

            bw.Write(Komunikaty.DAB_OK);
            bw.Flush();
               
                
               // Console.WriteLine(logMode + " " + name + " " + password);
                /*if (logMode == Komunikaty.DAB_REG)
                {
                  //  Program.Register(name, password);
                    bw.Write(Komunikaty.DAB_HELLO);
                bw.Flush();

                }
                if (logMode == Komunikaty.DAB_LOG)
                {
                   // Program.LoggIn(name, password);
                    bw.Write(Komunikaty.DAB_HELLO);
                    bw.Flush();
                }
*/
                /*     if (netStream.CanRead)
                         {


                             Byte[] size_ = new byte[1];  //bufor na rozmiar
                             netStream.Read(size_, 0, 1);  // odczytaj rozmiar
                             string sizeA = (Encoding.UTF8.GetString(size_)); // zamien na stringa
                             int size_login = Int32.Parse(sizeA);            // parsnij na inta

                             Byte[] bytesLogin = new byte[size_login];   // bufor na login
                             //if(bytes[0]!= 0)
                             netStream.Read(bytesLogin, 0, size_login);  // czytaj co sle

                             string returnlogin = Encoding.UTF8.GetString(bytesLogin);  // co odebral na stringa
                             Console.WriteLine("Klient przesyla login: " + returnlogin);    // zwroc co odebral 

                             netStream.Read(size_, 0, 1);  // odczyt rozmiaru
                             sizeA = (Encoding.UTF8.GetString(size_));   // zamien na stringa
                             int size_Pass = Int32.Parse(sizeA);         // parsnij inta
                             Byte[] bytesPass = new byte[size_Pass];     // bufor haslo
                             //    if (bytesPass[0] != 0)
                             netStream.Read(bytesPass, 0, size_Pass);   // czytaj haslo

                             string returnPass = Encoding.UTF8.GetString(bytesPass); // co odebral na stringa 
                             Console.WriteLine("Klient przesyla haslo: " + returnPass);      //zwroc co odebrales

                             Program.LoggIn(returnlogin, returnPass);   // logowanie


                         }
                         else
                         {
                             Console.WriteLine("Wystąpił nieokreślony błąd");

                         }
                         */
            
            Console.WriteLine("Zamykam polaczenie");
            
           CloseConnection();
                
          //  }
        }

        void CloseConnection() // zakonczenie polaczenia
        {


            netStream.Close();
            client.Close();
            br.Close();
            bw.Close();
            ssl.Close();
            Console.WriteLine("Zamknięto polączenie");
            Console.WriteLine();
            
            

        }
    }
}
