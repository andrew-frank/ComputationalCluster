using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.BackupServer
{
    public class BackupServer
    {
        public static void Main()
        {
            startInstance();
        }

        static protected void startInstance()
        {
            Console.WriteLine("Backup Server Started");
            waitUntilUserClose();
        }

        private static void waitUntilUserClose()
        {
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}
