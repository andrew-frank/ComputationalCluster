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
using ComputationalCluster.Misc;
using ComputationalCluster.Shared.Messages.SolveRequestResponseNamespace;

namespace ComputationalCluster.Nodes
{
    public sealed class Client : Node
    {
        #region Public

        public Client()
        {
            this.CommonInit();
        }

        private void CommonInit()
        {
            this.NodeType = NodeType.Client;
        }


        public void startInstance(UInt16 port, IPAddress server)
        {
            this.Port = port;
            this.IP = server;
            Console.WriteLine("Client Started");

            Console.Write("Debug? [y/n] \n>");
            string debug = Console.ReadLine();

            if (debug == "n") {
                this.Port = 0;
                port = 0;
                this.HostName = "";
            }

            while (port == 0) {
                Console.WriteLine(" Parameters Syntax: [-address [IPv4 address or IPv6 address or host name]] [-port[port number]]");
                Console.Write("> ");

                String parameters;
                parameters = Console.ReadLine();
                parameters = parameters.Replace(" ", string.Empty);
                //Shared.Connection.ConnectionHelpers.CheckInputSyntax(parameters, port, hostName);
            }

            Console.WriteLine("Specify name of the problem file");
            String filename = Console.ReadLine();
            string problem = System.IO.File.ReadAllText(filename);
            byte[] base64Problem = problem.Base64Encode();

            SolveRequest request = new SolveRequest();
            request.Data = base64Problem;
            request.ProblemType = Utilities.ProblemNameForType(ProblemType.DVRP);
            request.SolvingTimeoutSpecified = false;
            request.IdSpecified = false;
        }

        #endregion

        #region Overrides

        //Invalid:
        protected override Status CurrentStatus()
        {
            throw new Exception("Client does not implement CurrentStatus()");
        }

        protected override Register GenerateRegister()
        {
            throw new Exception("Client does not implement GenerateRegister()");
        }

        #endregion

        #region Private

        private void SendSolveRequest(SolveRequest solveRequest)
        {
            string message = solveRequest.SerializeToXML();
            string res = CMSocket.Instance.SendMessage(this.Port, this.HostName, message);
            Object obj = res.DeserializeXML();
            if (!(obj is SolveRequestResponse))
                throw new Exception("Wrong type");

            SolveRequestResponse response = (SolveRequestResponse)obj;
            if (response.IdSpecified) {
                ulong id = response.Id;
            }
        }

        #endregion
    }

}
