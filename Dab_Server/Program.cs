using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dab_Server
{
    public class Program
    {
        public IPAddress ip = IPAddress.Parse("127.0.0.1");
        public int port = 2000;
        public bool running = true;
        public TcpListener server;
        public X509Certificate2 cert = new X509Certificate2("C:\\Users\\sklep\\Documents\\Visual Studio 2015\\Projects\\Dab_serv\\Dab_serv\\server.pfx", "instant");
        static void Main(string[] args)
        {
            Program p = new Program();

        }
        public Program()
        {
            Console.WriteLine("---------------------------------- DAB Server ----------------------------------");
            Console.Title = "Dab Server";
            //Register("Janina", "Nowak");
            //LoggIn("Janusz", "Nowak");
            server = new TcpListener(ip, port);
            server.Start();
             
            Listen();
        
            { Console.WriteLine("Serwer zakończył oczekiwanie na połączenie"); }
            
    }
        public static string connectionString()            // polaczenie z baza
        {
            return "Data Source=Ardu\\Ardubaza;Initial Catalog=Dab_DB;Integrated Security=TRUE";
        }

        public static int LoggIn(string login, string password) {
            SqlConnection sqlConn = new SqlConnection(connectionString());
            int ID = 0;
            //File.WriteAllText(@"c:\a\b\debug.txt", LoginVariables.connectionString());
            try
            {
                sqlConn.Open();
                SqlCommand command = new SqlCommand("SELECT ID_User FROM Users WHERE Login_user=@Name_profile AND Password_user=@Password_profile", sqlConn);
                command.Parameters.AddWithValue("@Name_profile", login);
                command.Parameters.AddWithValue("@Password_profile", password);
                //File.WriteAllText(@"c:\a\b\error.txt", command.CommandText);
                
                ID =  (Int32)command.ExecuteScalar(); // zwroc ID uzytkownika
                Console.WriteLine("Znaleziono uzytkownika: " + login);
                Console.WriteLine("Jego ID to: " + ID);
            }           
           // catch (System.Data.SqlClient.SqlException se)
           catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ID;
        }
        public static int Register(string login, string password)
        {
            SqlConnection sqlConn = new SqlConnection(connectionString());
            //File.WriteAllText(@"c:\a\b\debug.txt", LoginVariables.connectionString());
            int ID = 0;
            try
            {
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand("SELECT ID_User FROM Users WHERE Login_user=@Name_profile", sqlConn);
                cmd.Parameters.AddWithValue("@Name_profile", login);
                //File.WriteAllText(@"c:\a\b\error.txt", command.CommandText);
                ID = (Int32)cmd.ExecuteScalar();
                Console.WriteLine("Podany uzytkownik istnieje już w bazie danych podaj inne hasło.");

            }
            catch (System.NullReferenceException se)
            {
                SqlCommand command = new SqlCommand("INSERT INTO Users (Login_user, Password_user, Name) VALUES (@Name_profile, @Password_profile, @Name_profile)", sqlConn);
                command.Parameters.AddWithValue("@Name_profile", login);
                command.Parameters.AddWithValue("@Password_profile", password);
                command.ExecuteNonQuery();
                Console.WriteLine("Account has been created. " + Environment.NewLine + "Login : " + login + Environment.NewLine + "Password : " + password + Environment.NewLine );
                SqlCommand cmd = new SqlCommand("SELECT ID_User FROM Users WHERE Login_user=@Name_profile", sqlConn);
                cmd.Parameters.AddWithValue("@Name_profile", login);
                //File.WriteAllText(@"c:\a\b\error.txt", command.CommandText);
                ID = (Int32)cmd.ExecuteScalar();
                Console.WriteLine("ID: " + ID);
            }
            catch (System.Data.SqlClient.SqlException se)
            {
            }
            return ID;
        }
        void Listen()
        {
            
            
            while (running)
            {
                Console.WriteLine("Serwer rozpoczyna nasłuchiwanie");
                TcpClient tcpClient = server.AcceptTcpClient();
                Client client = new Client(this, tcpClient);
                
           }
          
        }
    
    }
}
