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
        public void startInstance(Int32 port, String HostName) {
            Console.WriteLine("Computational Node Started");

            for (int i = 0; i < 8; i++) {
                Shared.Connection.ConnectionService.ConnectAndSendMessage(port, HostName, "Computational Node [" + i + "] ZAREJESTRUJ");
            }

            Shared.Utilities.Utilities.waitUntilUserClose();
        }   
    }
}
