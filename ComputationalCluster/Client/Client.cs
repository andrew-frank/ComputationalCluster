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
        public void startInstance(Int32 port, String HostName) {
          //  public void StarInstance(){

            Console.WriteLine("Client Started");
            Console.WriteLine(" Parameters Syntax: [-address [IPv4 address or IPv6 address or host name]] [-port[port number]]");
            Console.Write("> ");

            String parameters;
            parameters = Console.ReadLine();
            parameters = parameters.Replace(" ", string.Empty);
            int address;
            var count = parameters.Count(x => x == '-');
            if (count == 2)
            {
                string addressS = parameters.Substring(GetNthIndex(parameters, 's', 2) + 1, GetNthIndex(parameters, '-', 2) - GetNthIndex(parameters, 's', 2) - 1);
                string PortS = parameters.Substring(GetNthIndex(parameters, '-', 2) + 5);
                Console.WriteLine(PortS);
                Console.WriteLine(addressS);
                bool x = Int32.TryParse(addressS, out address);
             
                x = Int32.TryParse(PortS, out port);
                if (x != true)
                {
                    Console.WriteLine("Wrong port number");
                }
    
            }
            else
                Console.WriteLine("Incorrect Syntax");


            String message = "";
            for (int i = 0; i < 4; i++) {

                //SolveRequest _solveRequest = new SolveRequest();
                DivideProblem devide = new DivideProblem();
                message = devide.SerializeToXML();
                try { 
                Shared.Connection.ConnectionService.ConnectAndSendMessage(port, HostName, message);    
                    }
                catch(Exception ex)
                {
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
