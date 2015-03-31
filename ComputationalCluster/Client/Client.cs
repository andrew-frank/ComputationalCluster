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
        Int32 port=0;
        String address;
        

        public void Check(string parameters) {
            var count = parameters.Count(x => x == '-');
            if (count == 2) {
                bool x;
                string addressS = parameters.Substring(GetNthIndex(parameters, 's', 2) + 1, GetNthIndex(parameters, '-', 2) - GetNthIndex(parameters, 's', 2) - 1);
                string PortS = parameters.Substring(GetNthIndex(parameters, '-', 2) + 5);
                Console.WriteLine(PortS);
                Console.WriteLine(addressS);

                x = Int32.TryParse(PortS, out port);
                if (x != true) {
                    Console.WriteLine("Wrong port number");
                }
                address = addressS;

            } else {
                Console.WriteLine("Incorrect Syntax");
            }
        }

        public void startInstance( String HostName) {
          //  public void StarInstance(){




            //Console.WriteLine("Client Started");
            //while (port == 0)
            //{ 
            //Console.WriteLine(" Parameters Syntax: [-address [IPv4 address or IPv6 address or host name]] [-port[port number]]");
            //Console.Write("> ");

           
            //String parameters;
            //parameters = Console.ReadLine();
            //parameters = parameters.Replace(" ", string.Empty);
            //Check(parameters);
            //}
           port = 13000;

            String message = "";
            for (int i = 0; i < 4; i++) {

                SolveRequest solveRequest = new SolveRequest();
                message = solveRequest.SerializeToXML();

                //DivideProblem devide = new DivideProblem();
                //message = devide.SerializeToXML();

                try { 
                    Shared.Connection.ConnectionService.ConnectAndSendMessage(port, HostName, message);    
                
                } catch(Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }

            Shared.Utilities.Utilities.waitUntilUserClose();
        }


        public int GetNthIndex(string s, char t, int n)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }

}
