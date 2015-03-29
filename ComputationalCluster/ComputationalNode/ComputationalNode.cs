using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.ComputationalNode
{
    public class ComputationalNode
    {
        public static void Main()
        {
            startInstance();
        }

        static protected void startInstance()
        {
            Console.WriteLine("Computational Node Started");
            waitUntilUserClose();
        }

        private static void waitUntilUserClose()
        {
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}
