using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Client
{
    public class Client
    {
        public static void Main()
        {
            startInstance();
        }

        static protected void startInstance()
        {
            Console.WriteLine("Client Started");
            waitUntilUserClose();
        }

        private static void waitUntilUserClose()
        {
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}
