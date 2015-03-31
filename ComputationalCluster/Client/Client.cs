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

namespace ComputationalCluster.Nodes
{
    public sealed class Client : Node
    {
       
        public void startInstance(Int32 _port, String _HostName, Int32 _timeout) {

            Console.WriteLine("Client Started");
            //while (port == 0)
            //{ 
            //Console.WriteLine(" Parameters Syntax: [-address [IPv4 address or IPv6 address or host name]] [-port[port number]]");
            //Console.Write("> ");

           
            //String parameters;
            //parameters = Console.ReadLine();
            //parameters = parameters.Replace(" ", string.Empty);
            //Check(parameters, port, address);
            //}

            Timeout = _timeout;
            Port = _port;
            HostName = _HostName;


            String message = "";
            //for (int i = 0; i < 4; i++) {

            //    SolveRequest solveRequest = new SolveRequest();
            //    message = solveRequest.SerializeToXML();

            //    try {
            //        Shared.Connection.ConnectionService.ConnectAndSendMessage(Port, HostName, message);    
                
            //    } catch(Exception ex) {
            //        Console.WriteLine(ex.ToString());
            //    }
            //}
            Register registerRequest = new Register();
            message = registerRequest.SerializeToXML();
            try {
                Shared.Connection.ConnectionService.ConnectAndSendMessage(Port, HostName, message);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Status statusRequest = new Status();
            message = statusRequest.SerializeToXML();

            while(true)
            {
                Thread.Sleep(Timeout);
                try {
                    Shared.Connection.ConnectionService.ConnectAndSendMessage(Port, HostName, message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }                
            }


            //Shared.Utilities.Utilities.waitUntilUserClose();
        }

    }

}
