using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.Shared.Messages.NDivideProblem;
using ComputationalCluster.Shared.Utilities;

namespace ComputationalCluster.Client
{
    public class Client
    {
        public void StartInstance()
        {
            Console.WriteLine("Client Started");


            DivideProblem divideProblem = new DivideProblem();
            string xml = divideProblem.SerializeToXML();
            Console.WriteLine(xml);

            waitUntilUserClose();
        }

        private void waitUntilUserClose()
        {
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}
