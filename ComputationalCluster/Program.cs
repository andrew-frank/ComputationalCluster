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
                Console.Write("> ");                     
                componentToStart = Console.ReadLine();
                componentToStart = componentToStart.ToLower();
                componentToStart = componentToStart.Replace(" ", string.Empty);
                switch (componentToStart)
                {
                    case "taskmanager":
                        TaskManager.TaskManager newTaskManagerInstance = new TaskManager.TaskManager();
                        newTaskManagerInstance.Main();
                        break;
                    case "server":
                        Server.Server newServerInstance = new Server.Server();
                        newServerInstance.Main();
                        //Environment.Exit(0);
                        break;          
                    case "client":
                        Client.Client newClientInstance = new Client.Client();
                        newClientInstance.Main();
                        break;
                    case "computationalnode":
                        ComputationalNode.ComputationalNode newComputationalNodeInstance = new ComputationalNode.ComputationalNode();
                        newComputationalNodeInstance.Main();
                        break;
                }

            } else {
                componentToStart = args[0];
            }
        
        }


    }



}
