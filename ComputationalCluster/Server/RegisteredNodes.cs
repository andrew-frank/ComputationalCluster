using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Nodes
{
    public sealed class RegisteredNodes
    {
        #region Properties/ivars

        private static ulong _nodeIDCounter = 0;

        public static ulong NextNodeID {
            get { _nodeIDCounter++; return _nodeIDCounter; } 
        }

        private List<Node> _clients = new List<Node>();
        private List<Node> _taskManagers = new List<Node>();
        private List<Node> _computationalNodes = new List<Node>();
        private Queue<Node> _backupServers = new Queue<Node>();

        public List<Node> Clients { get { return _clients; } }
        public List<Node> TaskManagers { get { return _taskManagers; } }
        public List<Node> ComputationalNodes { get { return _computationalNodes; } }
        public Queue<Node> BackupServers { get { return _backupServers; } }

        #endregion

        public void RegisterClient(Node node)
        {
            this.Clients.Add(node);
        }

        public void RegisterTaskManager(Node node)
        {
            this.TaskManagers.Add(node);
        }

        public void RegisterComputationalNode(Node node)
        {
            this.ComputationalNodes.Add(node);
        }

        public void RegisterBackupServer(Node node)
        {
            foreach (Node n in this.BackupServers) { //do not duplicate same backup
                if (n.IP.Equals(node.IP) && n.Port == node.Port)
                    return;
            }

            this.BackupServers.Enqueue(node);
        }

        public void UpdateBackupServers(List<Node> backupservers)
        {
            return;
            _backupServers.Clear();
            foreach (Node n in backupservers)
                _backupServers.Enqueue(n);
            //_backupServers = backupservers;
        }

        public bool DeregisterClient(Node node)
        {
            int i = 0;
            foreach (Node n in Clients) {
                if (n.ID == node.ID) {
                    Clients.RemoveAt(i);
                    return true;
                }

                i++;
            }
            return false;
        }


        public bool DeregisterComputationalNode(Node node)
        {
            int i = 0;
            foreach (Node n in ComputationalNodes) {
                if (n.ID == node.ID) {
                    Clients.RemoveAt(i);
                    return true;
                }

                i++;
            }
            return false;
        }

        public bool DeregisterTaskManagers(Node node)
        {
            int i = 0;
            foreach (Node n in TaskManagers) {
                if (n.ID == node.ID) {
                    Clients.RemoveAt(i);
                    return true;
                }

                i++;
            }
            return false;
        }



        public Node NodeWithID(ulong ID)
        {
            foreach (Node node in TaskManagers)
                if (node.ID == ID)
                    return node;
            foreach (Node node in ComputationalNodes)
                if (node.ID == ID)
                    return node;
            foreach (Node node in BackupServers)
                if (node.ID == ID)
                    return node;
            return null;
        }
    }


}
