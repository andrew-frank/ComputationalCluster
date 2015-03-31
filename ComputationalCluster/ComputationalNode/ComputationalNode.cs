using ComputationalCluster.Shared.Messages.StatusNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.Shared.Utilities;
using ComputationalCluster.Shared.Messages.RegisterNamespace;
using System.Threading;

namespace ComputationalCluster.Nodes
{
    public class ComputationalNode : Node
    {
      
        public void startInstance(Int32 port, String Hostname, Int32 timeout) {
            Timeout = timeout;
            Port = port;
            HostName = Hostname;
            Timeout = timeout;
            
            Console.WriteLine("Computational Node Started");
            String message = "";

            Register register = new Register();
            message = register.SerializeToXML();

            try
            {
                Shared.Connection.ConnectionService.ConnectAndSendMessage(port, HostName, message);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            while (true)
            {
                Thread.Sleep(Timeout);
                Status _status = new Status();
                message = _status.SerializeToXML();

                Shared.Connection.ConnectionService.ConnectAndSendMessage(port, HostName, message);

            } Shared.Utilities.Utilities.waitUntilUserClose();
        }   
    }
}
