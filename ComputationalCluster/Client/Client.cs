using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.Shared.Messages.DivideProblemNamespace;
using ComputationalCluster.Shared.Utilities;

namespace ComputationalCluster.Client
{
    public class Client
    {
        public void startInstance(Int32 port, String HostName) {
            Console.WriteLine("Client Started");
            String message = "";

            for (int i = 0; i < 4; i++) {

                DivideProblem divide = new DivideProblem();
                message = divide.SerializeToXML();

                Shared.Connection.ConnectionService.ConnectAndSendMessage(port, HostName, message);
            }

            Shared.Utilities.Utilities.waitUntilUserClose();
        }
    }
}
