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
            this.Init();
        }

        private void Init()
        {
            this.NodeType = NodeType.TaskManager;
            this.ID = Guid.NewGuid();
        }



        public void startInstance(Int32 port, String hostName, Int32 timeout)
        {
            this.Timeout = timeout;
            this.Port = port;
            this.HostName = hostName;

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
