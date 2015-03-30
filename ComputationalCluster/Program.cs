using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster {

    public class Program
    {

        static void Main(string[] args) 
        {
            // get config file from runtime args or if none provided, request config via console            
            setConfigFileAtRuntime(args);
        }

        protected static void setConfigFileAtRuntime(string[] args) {

            string componentToStart;

            if (args.Length == 0) {

                Console.WriteLine("Please specify a component that you want to start:");                
                Console.Write("> ");       
              
                componentToStart = Console.ReadLine();
                componentToStart = componentToStart.Replace(" ", string.Empty);

                Int32 port = 13000;
                string hostName = Dns.GetHostName();
                IPAddress ip = Shared.Connection.ConnectionService.getIPAddressOfTheLocalMachine();

                switch (componentToStart.ToUpper()) {

                    case "TASKMANAGER":
                        TaskManager.TaskManager newTaskManagerInstance = new TaskManager.TaskManager();
                        newTaskManagerInstance.startInstance(port, hostName);
                        break;

                    case "SERVER":
                        Server.Server newServerInstance = new Server.Server();
                        newServerInstance.startInstance(ip);
                        break;    
      
                    case "CLIENT":
                        Client.Client newClientInstance = new Client.Client();
                        newClientInstance.startInstance(hostName);
                        break;

                    case "COMPUTATIONALNODE":
                        ComputationalNode.ComputationalNode newComputationalNodeInstance = new ComputationalNode.ComputationalNode();
                        newComputationalNodeInstance.startInstance(port, hostName);
                        break;
                }

            } else {
                componentToStart = args[0];
            }


        }
    }
}