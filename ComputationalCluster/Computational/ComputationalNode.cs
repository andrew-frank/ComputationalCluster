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
using ComputationalCluster.Shared.Connection;
using System.Diagnostics;

namespace ComputationalCluster.Nodes
{
    public class ComputationalNode : Node
    {
        public ComputationalNode()
        {
            this.CommonInit();
        }

        private void CommonInit()
        {
            this.NodeType = NodeType.ComputationalNode;
        }


        public void startInstance(Int32 port, String hostName, Int32 timeout, ulong id)
        {
            this.ID = id;
            this.Timeout = timeout;
            this.Port = port;
            this.HostName = hostName;
            Console.WriteLine("Computational Node Started");

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
