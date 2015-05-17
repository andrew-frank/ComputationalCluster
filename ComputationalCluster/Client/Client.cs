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

namespace ComputationalCluster.Nodes
{
    public sealed class Client : Node
    {
        public Client()
        {
            nodeType = NodeType.Client;
        }

        public void startInstance(Int32 port, String hostName, Int32 timeout) 
        {
            //HostName = "P21911";
            //Port = 13000;

            this.Timeout = timeout;
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

            String message = "";
            Register registerRequest = new Register();
            message = registerRequest.SerializeToXML();

            CMSocket.Instance.SendMessage(port, hostName, message);

            Status statusRequest = new Status();
            message = statusRequest.SerializeToXML();

            while(true) {
                Thread.Sleep(timeout);
                try {
                    CMSocket.Instance.SendMessage(port, hostName, message);
                } catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }                
            }
        }

    }

}
