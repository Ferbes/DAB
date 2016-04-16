using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net.Security;

namespace Dab_Client
{
    public class DABClient
    {
        public static TcpClient client;
        public static NetworkStream netStream;
        public static SslStream ssl;
        public static BinaryReader br;
        public static BinaryWriter bw;
    }













}
