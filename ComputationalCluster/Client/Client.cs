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
namespace ComputationalCluster.Client
{
    public class Client
    {
        Int32 _port=0;
        String address;        

        public void startInstance(Int32 port, String HostName, Int32 timeout) {

            //Console.WriteLine("Client Started");
            //while (port == 0)
            //{ 
            //Console.WriteLine(" Parameters Syntax: [-address [IPv4 address or IPv6 address or host name]] [-port[port number]]");
            //Console.Write("> ");

           
            //String parameters;
            //parameters = Console.ReadLine();
            //parameters = parameters.Replace(" ", string.Empty);
            //Check(parameters, port, address);
            //}

           _port = port;

            String message = "";
            for (int i = 0; i < 4; i++) {

                SolveRequest solveRequest = new SolveRequest();
                message = solveRequest.SerializeToXML();

                try { 
                    Shared.Connection.ConnectionService.ConnectAndSendMessage(_port, HostName, message);    
                
                } catch(Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }

            Shared.Utilities.Utilities.waitUntilUserClose();
        }

    }

}
