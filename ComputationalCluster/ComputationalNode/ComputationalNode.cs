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
      
        public void startInstance(Int32 _port, String _HostName, Int32 _timeout) {
            Timeout = _timeout;
            Port = _port;
            HostName = _HostName;
            
            
            Console.WriteLine("Computational Node Started");
            String message = "";

            Register register = new Register();
            message = register.SerializeToXML();

            try
            {
                Shared.Connection.ConnectionService.ConnectAndSendMessage(_port, HostName, message);

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

                Shared.Connection.ConnectionService.ConnectAndSendMessage(_port, HostName, message);

            } Shared.Utilities.Utilities.waitUntilUserClose();
        }   
    }
}
