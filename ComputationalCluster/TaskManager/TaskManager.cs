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
        public void startInstance(Int32 port, String HostName)
        {
            Console.WriteLine("Task Manager Started");

            for (int i = 0; i < 1; i++) {
                Shared.Connection.ConnectionService.ConnectAndSendMessage(port, HostName, "TaskManager [" + i + "] ZAREJESTRUJ");
            }   

            Shared.Utilities.Utilities.waitUntilUserClose();
        }
    }
}
