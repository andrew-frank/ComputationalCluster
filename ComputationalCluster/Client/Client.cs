using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Client
{
    public class Client
    {
        public void Main()
        {
            startInstance();
            Shared.Utilities.Utilities.waitUntilUserClose();
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
        }        
    }
}
