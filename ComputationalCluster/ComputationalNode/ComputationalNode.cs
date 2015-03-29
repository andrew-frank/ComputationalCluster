using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.ComputationalNode
{
    public class ComputationalNode
    {
        public void Main()
        {
            startInstance();
        }

        protected void startInstance()
        {
            Console.WriteLine("Computational Node Started");
            waitUntilUserClose();
        }

        private void waitUntilUserClose()
        {
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}
