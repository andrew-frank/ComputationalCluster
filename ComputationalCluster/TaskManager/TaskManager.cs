using ComputationalCluster.Shared.Messages.StatusNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ComputationalCluster.Shared.Utilities;
using ComputationalCluster.Shared.Messages.SolveRequestNamespace;
using ComputationalCluster.Shared.Messages.RegisterNamespace;
using ComputationalCluster.Shared.Connection;
using System.Diagnostics;



namespace ComputationalCluster.Nodes
{
    public sealed class TaskManager : Node
    {
        public TaskManager()
        {
            this.CommonInit();
        }

        private void CommonInit()
        {
            this.NodeType = NodeType.TaskManager;
        }


        public void startInstance(Int32 port, String hostName, Int32 timeout, ulong id)
        {
            this.Timeout = timeout;
            this.Port = port;
            this.HostName = hostName;
            Console.WriteLine("Task Manager Started");

            this.RegisterComponent();
            this.StartTimeoutTimer();
        }


        protected override Status CurrentStatus()
        {
            Status status = new Status();
            return status;
        }

        protected override void RegisterComponent()
        {
            Register register = new Register();
            String message = register.SerializeToXML();

            Debug.Assert(message != null);
            if (message == null)
                return;

            CMSocket.Instance.SendMessage(this.Port, this.HostName, message);
        }
    }
}
