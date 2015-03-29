using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Client
{
    public class Client : IDisposable
    {
        public void Main()
        {
            startInstance();
        }

        protected void startInstance()
        {
            Console.WriteLine("Client Started");
            String HostName = "";            
            HostName = Dns.GetHostName();
            for (int i = 0; i < 4; i++)
            {
                Shared.Connection.ConnectionService.ConnectAndSendMessage(HostName, "Client [" + i + "] ZAREJESTRUJ");
            }
                //Shared.Connection.ConnectionService.ConnectAndSendMessage(HostName, "Siema server, zarejestrujesz mnie?");
            waitUntilUserClose();
        }
        

        private void waitUntilUserClose()
        {
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
