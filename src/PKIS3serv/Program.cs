using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Programming_of_corporate_industrial_systems_Practice_2_server
{
    internal class Program
    {
        private const int ServerPort = 5000;

        public static async Task Main(string[] args)
        {
            var server = new FileServer(ServerPort);
            await server.StartAsync();
        }
    }
}
