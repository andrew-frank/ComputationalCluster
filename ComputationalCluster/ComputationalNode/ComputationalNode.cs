using ComputationalCluster.Shared.Messages.StatusNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.Shared.Utilities;

namespace ComputationalCluster.ComputationalNode
{
    public class ComputationalNode
    {
        public void startInstance(Int32 port, String HostName) {
            Console.WriteLine("Computational Node Started");
            String message = "";
            for (int i = 0; i < 8; i++)
            {
                Status _status = new Status();
                message = _status.SerializeToXML();

                Shared.Connection.ConnectionService.ConnectAndSendMessage(port, HostName, message);
            }

            Shared.Utilities.Utilities.waitUntilUserClose();
        }   
    }
}
