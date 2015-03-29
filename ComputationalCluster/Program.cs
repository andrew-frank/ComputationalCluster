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
            // Test2 comment from Abramus
            // Test3 from milkduds
            setConfigFileAtRuntime(args);
        }
        //here goes a comment to see if I COMMIT 
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
                        TaskManager.TaskManager.Main();
                        break;
                    case "server":                                                
                        Server.Server.Main();
                        //Environment.Exit(0);
                        break;          
                    case "client":
                        Client.Client.Main();
                        break;
                    case "computationalnode":
                        ComputationalNode.ComputationalNode.Main();
                        break;
                }

            } else {
                componentToStart = args[0];
            }
        
        }


    }



}
