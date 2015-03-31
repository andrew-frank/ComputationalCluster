using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Nodes
{
    public abstract class Node : Object
    {
        public int Timeout { get; set; }
        public int ID { get; set; }
        public System.Net.IPAddress IP {get; set;}
        public string HostName { get; set; }

        public override string ToString()
        {
            return "Node: " + ID + ", IP: " + IP;
        }
    }
}
