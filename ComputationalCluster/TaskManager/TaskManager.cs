using ComputationalCluster.Shared.Messages.StatusNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.Shared.Utilities;


namespace ComputationalCluster.TaskManager
{
    public sealed class TaskManager : Node
    {

        public void startInstance(Int32 port, String Hostname, Int32 timeout)
        {
            Timeout = timeout;
            Port = port;
            HostName = Hostname;
            Timeout = timeout;


            Console.WriteLine("Task Manager Started");
            String message = "";
            for (int i = 0; i < 1; i++)
            {
                Status _status = new Status();
                message = _status.SerializeToXML();

                Shared.Connection.ConnectionService.ConnectAndSendMessage(port, HostName, message);
            }

            Shared.Utilities.Utilities.waitUntilUserClose();
        }
    }
}
