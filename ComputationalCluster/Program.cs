using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster {
    public class Program 
    {
        static void Main(string[] args) 
        {
            // get config file from runtime args
            // or if none provided, request config
            // via console
            setConfigFileAtRuntime(args);
        }

        protected static void setConfigFileAtRuntime(string[] args) {

            string componentToStart;

            if (args.Length == 0) {
                Console.WriteLine("Please specify a component that you want to start:");                
                Console.Write("> "); // prompt               
                //runtimeconfigfile = Console.ReadLine();
                componentToStart = Console.ReadLine();
                switch (componentToStart)
                {
                    case "Task Manager":
                        TaskManager.TaskManager.Main();
                        break;
                    case "Server":                                                
                        Server.Server.Main();
                        break;
                    case "Client":
                        Client.Client.Main();
                        break;
                    case "Computational Node":
                        ComputationalNode.ComputationalNode.Main();
                        break;
                }

            } else {
                componentToStart = args[0];
            }
        }
    }
}
