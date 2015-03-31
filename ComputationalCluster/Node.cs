using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster
{
    public abstract class Node
    {
        public int Timeout { get; set; }
        public int ID { get; set; }
        public System.Net.IPAddress IP {get; set;}
        public string HostName { get; set; }
        public int Port { get; set; }
    }
}
