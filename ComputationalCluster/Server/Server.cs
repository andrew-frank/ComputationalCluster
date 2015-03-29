using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Server
{
    
    public class Server
    {       
        public static void Main()
        {
            startInstance();
        }

        static protected void startInstance()
        {
            Console.WriteLine("Server Started");
            waitUntilUserClose(); 
        }
        
        private static void waitUntilUserClose()
        {
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }


    }
    
}
