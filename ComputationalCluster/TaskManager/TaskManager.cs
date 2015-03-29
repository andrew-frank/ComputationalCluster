using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.TaskManager
{
    public class TaskManager
    {
        public void Main()
        {
            startInstance();
        }

        protected void startInstance()
        {
            Console.WriteLine("Task Manager Started");
            waitUntilUserClose();
        }

        private void waitUntilUserClose()
        {
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}
