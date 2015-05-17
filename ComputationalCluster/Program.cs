using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.Nodes;

namespace ComputationalCluster
{

    public class Program
    {

        static void Main(string[] args) {
            // get config file from runtime args or if none provided, request config via console            
            setConfigFileAtRuntime(args);
        }

        protected static void setConfigFileAtRuntime(string[] args) {

            string componentToStart;

            //if (args.Length == 0) {

            Console.WriteLine("Please specify a component that you want to start:");
            Console.WriteLine("1. Server");
            Console.WriteLine("2. Task manager");
            Console.WriteLine("3. Client");
            Console.WriteLine("4. Computational node");
            Console.Write("> ");

            componentToStart = Console.ReadLine();
            componentToStart = componentToStart.Replace(" ", string.Empty);
            componentToStart = componentToStart.Replace(".", string.Empty);

            Int32 port = 13000;
            string hostName = Dns.GetHostName();
            IPAddress ip = Shared.Connection.ConnectionService.getIPAddressOfTheLocalMachine();

            Int32 timeoutForServer = 10000, timeoutForTaskManager = 7000, timeoutForClient = 5000, timeoutForComputationalNode = 3000;

            int result = 0;
            if (Int32.TryParse(componentToStart, out result)) {
                switch (result) {
                    case 1:
                        componentToStart = "TASKMANAGER";
                        break;
                    case 2:
                        componentToStart = "SERVER";
                        break;
                    case 3:
                        componentToStart = "CLIENT";
                        break;
                    case 4:
                        componentToStart = "COMPUTATIONALNODE";
                        break;
                }
            }

            switch (componentToStart.ToUpper()) {

                case "TASKMANAGER":
                    TaskManager newTaskManagerInstance = new TaskManager();
                    newTaskManagerInstance.startInstance(port, hostName, timeoutForTaskManager);
                    break;

                case "SERVER":
                    Server newServerInstance = new Server();
                    newServerInstance.startInstance(port, ip, timeoutForServer);
                    break;

                case "CLIENT":
                    Client newClientInstance = new Client();
                    newClientInstance.startInstance(port, hostName, timeoutForClient);
                    break;

                case "COMPUTATIONALNODE":
                    ComputationalNode newComputationalNodeInstance = new ComputationalNode();
                    newComputationalNodeInstance.startInstance(port, hostName, timeoutForComputationalNode);
                    break;
            }
        }
    }
}