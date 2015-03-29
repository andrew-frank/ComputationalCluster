using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.ComputationalNode
{
    public class ComputationalNode
    {
        public void Main()
        {
            startInstance();
        }

        protected void startInstance()
        {
            Console.WriteLine("Computational Node Started");
            String HostName = "";
            HostName = Dns.GetHostName();
            for (int i = 0; i < 8; i++)
            {
                Shared.Connection.ConnectionService.ConnectAndSendMessage(HostName, "Computational Node [" + i + "] ZAREJESTRUJ");
            }
            waitUntilUserClose();
        }

        private void waitUntilUserClose()
        {
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}
