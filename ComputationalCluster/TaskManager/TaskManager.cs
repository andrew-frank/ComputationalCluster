using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.TaskManager
{
    public class TaskManager
    {
        public static void Main()
        {
            startInstance();
        }

        static protected void startInstance()
        {
            Console.WriteLine("Task Manager Started");
            waitUntilUserClose();
        }

        private static void waitUntilUserClose()
        {
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}
