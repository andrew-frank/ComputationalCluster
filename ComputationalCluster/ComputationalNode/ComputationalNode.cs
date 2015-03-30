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
            Shared.Utilities.Utilities.waitUntilUserClose();
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
        }        
    }
}
