using ComputationalCluster.Shared.Connection;
using ComputationalCluster.Shared.Messages.DivideProblemNamespace;
using ComputationalCluster.Shared.Messages.NoOperationNamespace;
using ComputationalCluster.Shared.Messages.RegisterNamespace;
using ComputationalCluster.Shared.Messages.RegisterResponseNamespace;
using ComputationalCluster.Shared.Messages.SolutionRequestNamespace;
using ComputationalCluster.Shared.Messages.SolutionsNamespace;
using ComputationalCluster.Shared.Messages.SolvePartialProblemsNamespace;
using ComputationalCluster.Shared.Messages.SolveRequestNamespace;
using ComputationalCluster.Shared.Messages.SolveRequestResponseNamespace;
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

        Timer reconnectTimer = new Timer();

        protected void ReconnectTimeoutTimerCallback(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.reconnectTimer != null) {
                this.reconnectTimer.Stop();
                this.reconnectTimer = null;
            }

            if (this.RegisteredComponents.BackupServers.Count <= 0) {
                Console.WriteLine("No more backup servers");
                this.ReconnectMode = false;
                return;
            }

            Node backupServer = this.RegisteredComponents.BackupServers.Dequeue();
            this.RegisteredComponents.BackupServers.Enqueue(backupServer);

            if (backupServer == null) {
                Console.WriteLine("No more backup servers");
                return;
            }

            this.IP = backupServer.IP;
            this.Port = backupServer.Port;
            this.ReconnectMode = false;
            //this.TimeoutTimer.Start();

            //if (message != null)
            //    CMSocket.Instance.SendMessage(this.Port, this.IP, message, this);
        }

        public void FailedToSendToServer(String message)
        {
            if (this.ReconnectMode)
                return;
            this.ReconnectMode = true;

            if (this.NodeType == Nodes.NodeType.Server) {
                Node backupServer = this.RegisteredComponents.BackupServers.Dequeue();
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

            } else {
                if (this.Timeout == 0)
                    this.Timeout = 3;
                this.reconnectTimer = new Timer();
                this.reconnectTimer.Interval = 1000 * this.Timeout * 2;
                this.reconnectTimer.Elapsed += this.TimeoutTimerCallback;
                this.reconnectTimer.Start();
            }
            
        }


        //this method is only called from AsyncClient which sent something to the server
        //and the string xml is the response. 
        //node of type server can execute this method only if it is in BackupMode (so it acts as a normal node -
        //i.e. sends Status etc.)
        public void ReceivedResponse(string xml)
        {
            Console.WriteLine("Node - received response:\n" + xml + "\n");
            Object obj = xml.DeserializeXML();

            if (obj is RegisterResponse) {
                RegisterResponse registerResponse = (RegisterResponse)obj;

                RegisterResponseBackupCommunicationServersBackupCommunicationServer[] backupServers = registerResponse.BackupCommunicationServers;
                foreach (RegisterResponseBackupCommunicationServersBackupCommunicationServer comp in backupServers) {
                    Node backup = new Server();
                    backup.IP = IPAddress.Parse(comp.address);
                    if(backup.IP != null) {
                        backup.Timeout = registerResponse.Timeout;
                        backup.ID = registerResponse.Id;
                        if (comp.portSpecified)
                            backup.Port = comp.port;
                        this.RegisteredComponents.RegisterBackupServer(backup);
                    }
                }

                this.ID = registerResponse.Id;
                this.Timeout = registerResponse.Timeout;
                this.StartTimeoutTimer();

            } else if (obj is DivideProblem) {  //Message to task Manager
                this.ReceivedDivideProblem((DivideProblem)obj);

            } else if (obj is Register) {
                this.ReceivedRegister((Register)obj, null);

            } else if (obj is SolutionRequest) {
                this.ReceivedSolutionRequest((SolutionRequest)obj);

            } else if (obj is SolvePartialProblems) {
                this.ReceivedSolvePartialProblems((SolvePartialProblems)obj);

            } else if (obj is SolveRequest) {
                this.ReceivedSolveRequest((SolveRequest)obj);

            } else if (obj is Status) {
                this.ReceivedStatus((Status)obj);

            } else if (obj is NoOperation) {
                this.ReceivedNoOperation((NoOperation)obj);

            } else if(obj is SolveRequestResponse) {
                this.ReceivedSolveRequestResponse((SolveRequestResponse)obj);

            } else if(obj is Solutions) {
                this.ReceivedSolutions((Solutions)obj);
            }
        }

        #endregion


        #region Overrides/Cluster

        protected abstract string ReceivedSolveRequestResponse(SolveRequestResponse solveRequestResponse);
        protected abstract string ReceivedSolveRequest(SolveRequest solveRequest);
        protected abstract string ReceivedSolvePartialProblems(SolvePartialProblems solvePartialProblems);
        protected abstract string ReceivedSolutions(Solutions solution);
        protected abstract string ReceivedSolutionRequest(SolutionRequest solutionRequest);
        protected abstract string ReceivedDivideProblem(DivideProblem divideProblem);
        protected abstract string ReceivedStatus(Status status);
        protected abstract string ReceivedRegister(Register register, IPAddress senderAddr);

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
                if (backups.BackupCommunicationServer == null)
                    return;
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
