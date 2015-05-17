using ComputationalCluster.Shared.Messages.RegisterNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Nodes
{
    public abstract class Node : Object
    {
        public Int32 Timeout { get; set; }
        public ulong ID { get; set; }
        public string TypeName {
            get {  return NameForType(NodeType); }
            //set { }
        }
        public NodeType NodeType { get; set; }
        public System.Net.IPAddress LocalIP { get; set; } //?
        public System.Net.IPAddress IP {get; set;}
        public string HostName { get; set; }
        public int Port { get; set; }

        public override string ToString() {
            string msg;
            if (IP == null)
                msg = TypeName + ": " + ID;
            else
                msg = TypeName + ": " + ID + ", IP: " + IP.ToString();
            return msg;
        }


        public Register GenerateRegisterMessage()
        {
            Register register = new Register();
            register.Id = this.ID;

            return register;
        }


        protected Timer TimeoutTimer;

        #region util
        public string NameForType(NodeType t)
        {
            switch (t) {
                case NodeType.Server: return "CommunicationServer";
                case NodeType.Client: return "Client";
                case NodeType.ComputationalNode: return "ComputationalNode";
                case NodeType.TaskManager: return "TaskManager";
                default: return null;
            }
        }
        #endregion
    }

    public enum NodeType
    {
        Server = 0,
        Client,
        ComputationalNode,
        TaskManager
    }
}
