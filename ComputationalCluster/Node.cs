using ComputationalCluster.Shared.Connection;
using ComputationalCluster.Shared.Messages.NoOperationNamespace;
using ComputationalCluster.Shared.Messages.RegisterNamespace;
using ComputationalCluster.Shared.Messages.RegisterResponseNamespace;
using ComputationalCluster.Shared.Messages.StatusNamespace;
using ComputationalCluster.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Timers;

namespace ComputationalCluster.Nodes
{
    public abstract class Node : Object
    {
        #region Properties


        public bool ReconnectMode = false;
        //public Queue<string> MsgQueues = new Queue<string>();

        protected Timer TimeoutTimer { get; private set; }


        public uint Timeout { get; set; }
        public ulong ID { get; set; }
        public string TypeName {
            get {  return  Utilities.NodeNameForType(NodeType); }
            //set { }
        }
        public NodeType NodeType { get; set; }
        public System.Net.IPAddress LocalIP { get; set; } 
        public System.Net.IPAddress IP {get; set;}
        public string HostName { get; set; }
        public ushort Port { get; set; }


        //the number of threads that could be efficiently run in parallel
        private int _parallelThreads = 1;
        public int ParallelThreads { get { return Convert.ToByte(_parallelThreads); } }


        //server uses all the fields. other components use only backupServers field
        private RegisteredNodes _registeredComponents = new RegisteredNodes();
        public RegisteredNodes RegisteredComponents { get { return _registeredComponents; } }


        #endregion


        #region Public

        public override string ToString()
        {
            string msg;
            if (IP == null)
                msg = TypeName + ": " + ID;
            else
                msg = TypeName + ": " + ID + ", IP: " + IP.ToString();
            return msg;
        }

        public void FailedToSendToServer(String message)
        {
            if (this.ReconnectMode)
                return;
            this.ReconnectMode = true;

            //if(this.TimeoutTimer != null)
            //this.TimeoutTimer.Stop();
            if (this.RegisteredComponents.BackupServers.Count <= 0) {
                Console.WriteLine("No more backup servers");
                this.ReconnectMode = false;
                return;
            }

            Node backupServer = this.RegisteredComponents.BackupServers.Dequeue();

            if (this.NodeType == Nodes.NodeType.Server) {
                if (backupServer.ID == this.ID) {
                    (this as Server).BackupMode = true;
                    this.IP = backupServer.IP;
                    //this.Port = backupServer.Port;
                    if (this.TimeoutTimer != null) {
                        this.TimeoutTimer.Stop();
                        this.TimeoutTimer = null;
                    }
                    this.ReconnectMode = false;
                    Server.MainServer = (Server)this;
                    (this as Server).Listen(this.Port, this.IP);
                }
                return;
            }

            this.RegisteredComponents.BackupServers.Enqueue(backupServer);

            if (backupServer == null) {
                Console.WriteLine("No more backup servers");
                return;
            }

            this.IP = backupServer.IP;
            this.Port = backupServer.Port;
            this.ReconnectMode = false;
            //this.TimeoutTimer.Start();
            if(message != null)
                CMSocket.Instance.SendMessage(this.Port, this.IP, message, this);
        }


        public void ReceivedResponse(string xml)
        {
            Console.WriteLine("Node - received response:\n" + xml + "\n");
            Object obj = xml.DeserializeXML();
            //Debug.Assert(obj is NoOperation, "Wrong server response");
            //this.ReceivedNoOperation((NoOperation)obj);

            if (obj is RegisterResponse) {
                RegisterResponse registerResponse = (RegisterResponse)obj;

                RegisterResponseBackupCommunicationServersBackupCommunicationServer[] backupServers = registerResponse.BackupCommunicationServers;
                foreach (RegisterResponseBackupCommunicationServersBackupCommunicationServer comp in backupServers) {
                    Node backup = new Server();
                    backup.IP = IPAddress.Parse(comp.address);
                    backup.Timeout = registerResponse.Timeout;
                    backup.ID = registerResponse.Id;
                    if (comp.portSpecified)
                        backup.Port = comp.port;
                    this.RegisteredComponents.RegisterBackupServer(backup);
                }

                this.ID = registerResponse.Id;
                this.Timeout = registerResponse.Timeout;
                this.StartTimeoutTimer();

            } else {

            }
        }


        #endregion


        #region Overrides/Cluster

        protected abstract Status CurrentStatus();
        protected abstract Register GenerateRegister();

        #endregion


        #region Common/Cluster

        /// GenerateRegister() needs to be overriden at this point.
        protected void RegisterComponent()
        {
            Register register = this.GenerateRegister();

            String message = register.SerializeToXML();

            Debug.Assert(message != null);
            if (message == null)
                return;

            CMSocket.Instance.SendMessage(this.Port, this.IP, message, this);
            //Object obj = response.DeserializeXML();

            //if (!(obj is RegisterResponse))
            //    throw new Exception("Invalid response");

            //RegisterResponse registerResponse = (RegisterResponse)obj;
            //this.ID = registerResponse.Id;
            //this.Timeout = registerResponse.Timeout;
            //this.StartTimeoutTimer();
        }


        /// CurrentStatus() needs to be overriden at this point.
        protected void TimeoutTimerCallback(object sender, System.Timers.ElapsedEventArgs e)
        {
            String message = "";
            Status status = this.CurrentStatus();
            message = status.SerializeToXML();
            CMSocket.Instance.SendMessage(this.Port, this.IP, message, this);

            //this.ReceivedResponse(response);
        }


        protected void ReceivedNoOperation(NoOperation noOperation)
        {
            List<Node> backupServers = new List<Node>();
            foreach (NoOperationBackupCommunicationServers backups in noOperation.Items) {
                foreach (NoOperationBackupCommunicationServersBackupCommunicationServer backup in backups.BackupCommunicationServer) {
                    Node server = new Server();
                    server.IP = System.Net.IPAddress.Parse(backup.address);
                    server.NodeType = Nodes.NodeType.Server;
                    if(backup.portSpecified)
                        server.Port = backup.port;
                    backupServers.Add(server);
                }
            }

            this.RegisteredComponents.UpdateBackupServers(backupServers);
        }


        /// Starts timer using Timeout value as interval.
        protected void StartTimeoutTimer()
        {
            if (this.Timeout <= 0)
                throw new Exception("Unspecified timeout");

            this.TimeoutTimer = new Timer();
            this.TimeoutTimer.Interval = 1000 * this.Timeout;
            this.TimeoutTimer.Elapsed += this.TimeoutTimerCallback;
            this.TimeoutTimer.Start();
        }

        #endregion

    }

    public enum NodeType
    {
        Server = 0,
        Client,
        ComputationalNode,
        TaskManager,
        Unspecified = -1
    }
}
