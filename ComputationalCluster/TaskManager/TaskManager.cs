using ComputationalCluster.Shared.Messages.StatusNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.Shared.Utilities;
using System.Threading;
using ComputationalCluster.Shared.Messages.SolveRequestNamespace;
using ComputationalCluster.Shared.Messages.RegisterNamespace;
using ComputationalCluster.Shared.Connection;



namespace ComputationalCluster.Nodes
{
    public sealed class TaskManager : Node
    {
        public TaskManager()
        {
            nodeType = NodeType.TaskManager;
        }


        public void startInstance(Int32 _port, String _HostName, Int32 _timeout)
        {
            Timeout = _timeout;
            Port = _port;
            HostName = _HostName;

            Console.WriteLine("Task Manager Started");
            String message = "";

            Register registerRequest = new Register();
            message = registerRequest.SerializeToXML();

            CMSocket.Instance.SendMessage(Port, HostName, message);

            Status statusRequest = new Status();
            message = statusRequest.SerializeToXML();

            while (true) {
                Thread.Sleep(Timeout);
                try {
                    CMSocket.Instance.SendMessage(Port, HostName, message);
                } catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
