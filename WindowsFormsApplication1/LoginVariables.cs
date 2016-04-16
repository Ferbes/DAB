using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dab_Client
{
    class LoginVariables
    {
        /*Dane bieżącej sesji logowania*/
        public static string currentUsername = "";
        public static string currentPassword = "";
        public static int ID=0;
        public static bool connected = false; // czy_polączony
        public static bool logged = false; // czy_zalogowany
        public static bool reg;
        public static string connectionString()
        {
            return "Data Source=Ardu\\Ardubaza;Initial Catalog=MagicTheGathering;Integrated Security=TRUE";
        }
        public static string msg = "";
       
    }
}
