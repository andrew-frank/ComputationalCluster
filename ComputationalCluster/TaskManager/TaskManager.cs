using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            String HostName = "";
            HostName = Dns.GetHostName();
            for (int i = 0; i < 1; i++)
            {
                Shared.Connection.ConnectionService.ConnectAndSendMessage(HostName, "TaskManager [" + i + "] ZAREJESTRUJ");
            }
            waitUntilUserClose();
        }

        private void waitUntilUserClose()
        {
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}
