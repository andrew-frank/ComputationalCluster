using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.Shared.Utilities;
using ComputationalCluster.Shared.Messages.SolveRequestNamespace;
using ComputationalCluster.Shared.Messages.DivideProblemNamespace;

namespace ComputationalCluster.Client
{
    public class Client
    {
        public void startInstance(Int32 port, String HostName) {
            Console.WriteLine("Client Started");
            String message = "";
            for (int i = 0; i < 4; i++) {

                //SolveRequest _solveRequest = new SolveRequest();
                DivideProblem devide = new DivideProblem();
                message = devide.SerializeToXML();

                Shared.Connection.ConnectionService.ConnectAndSendMessage(port, HostName, message);                
            }
            Shared.Utilities.Utilities.waitUntilUserClose();
        }
    }
}
