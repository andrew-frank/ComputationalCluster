using ComputationalCluster.Shared.Messages.RegisterNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Nodes
{
    public abstract class Node : Object
    {
        public Int32 Timeout { get; set; }
        public int ID { get; set; }
        public string TypeName {
            get {  return NameForType(nodeType); }
            //set { }
        }
        public NodeType nodeType { get; set; }
        public System.Net.IPAddress IP {get; set;}
        public string HostName { get; set; }
        public int Port { get; set; }


        public override string ToString() {
            string msg;
            if (IP == null)
                msg = TypeName + ": " + ID;
            else
                msg = TypeName + ": " + ID + ", IP: " + IP;
            return msg;
        }


        //util
        public string NameForType(NodeType t) {
            switch (t) {
                case NodeType.Server: return "Server";
                case NodeType.Client: return "Client";
                case NodeType.ComputationalNode: return "ComputationalNode";
                case NodeType.TaskManager: return "TaskManager";
                default: return null;
            }
        }

        public Register GenerateRegisterMessage()
        {
            Register register = new Register();

            return register;
        }
    }

    public enum NodeType
    {
        Server = 0,
        Client,
        ComputationalNode,
        TaskManager
    }
}
