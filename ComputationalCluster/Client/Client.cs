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
using ComputationalCluster.Shared.Messages.RegisterNamespace;
using System.Threading;
using ComputationalCluster.Shared.Messages.StatusNamespace;
using ComputationalCluster.Shared.Connection;
using System.Diagnostics;

namespace ComputationalCluster.Nodes
{
    public sealed class Client : Node
    {
        public Client()
        {
            this.CommonInit();
        }

        private void CommonInit()
        {
            this.NodeType = NodeType.Client;
        }


        public void startInstance(Int32 port, String hostName)
        {
            this.Port = port;
            this.HostName = hostName;

            Console.WriteLine("Client Started");
            while (port == 0)
            {
                Console.WriteLine(" Parameters Syntax: [-address [IPv4 address or IPv6 address or host name]] [-port[port number]]");
                Console.Write("> ");

                String parameters;
                parameters = Console.ReadLine();
                parameters = parameters.Replace(" ", string.Empty);
                Shared.Connection.ConnectionHelpers.CheckInputSyntax(parameters, port, hostName);
            }
        }

        //Invalid:
        protected override Status CurrentStatus()
        {
            throw new Exception("Client does not implement CurrentStatus()");
        }

        protected override Register GenerateRegister()
        {
            throw new Exception("Client does not implement GenerateRegister()");
        }

    }

}
