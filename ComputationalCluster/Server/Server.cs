using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.Shared.Utilities;
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
using ComputationalCluster.Nodes;
using System.Diagnostics;
using ComputationalCluster.Shared.Connection;
using ComputationalCluster.Misc;

namespace ComputationalCluster.Nodes
{
    public sealed class Server : Node
    {
        #region Properties/ivars

        public static Server MainServer { get; private set; }

        static ulong TaskIDCounter = 0;

        private ServerQueues serverQueues { get; set; }

        [Obsolete] //po co nam to tu? patrz 'Registered Nodes'
        public string backupAddr;

        public bool BackupMode { get; private set; }

        #endregion

        #region Public

        public Server()
        {
            this.CommonInit();
        }

        private void CommonInit()
        {
            this.NodeType = NodeType.Server;
            this.serverQueues = new ServerQueues();
            this.BackupMode = false;

            ///
            if (this.BackupMode == false)
                Server.MainServer = this;
        }


        public void startInstance(UInt16 port, IPAddress localIPAddress)
        {
            this.ID = 0;
            Console.WriteLine("Server Started, Specify Parameters");
            String[] Data = new String[3];

            Console.Write("Debug? [y/n] \n>");
            string debug = "y";// Console.ReadLine();
            if (debug == "y") {
                Console.WriteLine("Backup? [y/n] \n>");
                string backup = "n"; // Console.ReadLine();
                this.Port = port;
                this.LocalIP = localIPAddress;
                this.IP = localIPAddress;
                this.Timeout = 3;
                if (backup == "y") {
                    this.BackupMode = true;
                    this.RegisterComponent();
                    this.StartTimeoutTimer();
                    while (true) Console.ReadLine();
                } else {
                    this.BackupMode = false;
                    this.Listen(this.Port, localIPAddress);
                }
                return;
            }

            while (Data == null || Data[0] == null || Data[1] == null ||  Data[2] == null ) {
                Console.WriteLine("Parameters syntax: [-port [port number]] [-backup] [-t [time in seconds]]");
                Console.Write("> ");

                String parameters;
                parameters = Console.ReadLine();
                parameters = parameters.Replace(" ", string.Empty);
                Data = this.ParseParameters(parameters);
            }

            this.Port = UInt16.Parse(Data[0]);
            this.backupAddr = Data[1];
            this.Timeout = UInt32.Parse(Data[2]);
            this.LocalIP = localIPAddress;

            if (this.BackupMode) {
                this.RegisterComponent();
                this.StartTimeoutTimer();
                return;
            }

            this.Listen(this.Port, this.LocalIP);
        }

        #endregion

        #region Overrides

        protected override Status CurrentStatus()
        {
            Status status = new Status();
            return status;
        }

        protected override Register GenerateRegister()
        {
            Register register = new Register();
            register.Type = this.TypeName;
            return register;
        }

        #endregion


        #region Private/Communication handling


        private string ReceivedRegister(Register register, IPAddress senderAddr)
        {
            string type = register.Type;
            Node newNode;
            ulong ID = RegisteredNodes.NextNodeID;
            switch (Utilities.NodeTypeForName(type)) {
                case NodeType.TaskManager:
                    newNode = new TaskManager();
                    newNode.ID = ID;
                    newNode.IP = senderAddr;
                    this.RegisteredComponents.RegisterTaskManager(newNode);
                    break;
                case NodeType.ComputationalNode:
                    newNode = new ComputationalNode();
                    newNode.ID = ID;
                    newNode.IP = senderAddr;
                    this.RegisteredComponents.RegisterComputationalNode(newNode);
                    break;
                case NodeType.Server:
                    newNode = new Server();
                    newNode.ID = ID;
                    newNode.IP = senderAddr;
                    this.RegisteredComponents.RegisterBackupServer(newNode);
                    break;
                case NodeType.Client: //not needed!
                    newNode = new ComputationalNode();
                    newNode.ID = ID;
                    newNode.IP = senderAddr;
                    this.RegisteredComponents.RegisterClient(newNode);
                    break;
                default:
                    break;
            }

            //Register message is sent by TM, CN and Backup CS to the CS after they are activated.
            RegisterResponse response = new RegisterResponse();
            response.Id = ID;
            response.Timeout = this.Timeout;
            List<RegisterResponseBackupCommunicationServersBackupCommunicationServer> backupServers = new List<RegisterResponseBackupCommunicationServersBackupCommunicationServer>();
            foreach (Node comp in this.RegisteredComponents.BackupServers) {
                RegisterResponseBackupCommunicationServersBackupCommunicationServer backup = new RegisterResponseBackupCommunicationServersBackupCommunicationServer();
                backup.address = comp.IP.ToString();
                if (comp.Port > 0) {
                    backup.port = comp.Port;
                    backup.portSpecified = true;
                }
            }

            response.BackupCommunicationServers = backupServers.ToArray();
            return response.SerializeToXML();
        }


        private string ReceivedStatus(Status status)
        {
            Debug.Assert(this.serverQueues != null, "null server queue");
            Node node = this.RegisteredComponents.NodeWithID(status.Id);
            switch (node.NodeType) {
                case NodeType.TaskManager:
                    if (this.serverQueues.SolveRequests.Count > 0) {
                        SolveRequest solveRequest = this.serverQueues.SolveRequests.Dequeue();
                        return solveRequest.SerializeToXML();
                    }
                    if (this.serverQueues.PartialSolutions.Count > 0) {
                        Solutions solutions = this.serverQueues.PartialSolutions.Dequeue();
                        return solutions.SerializeToXML();
                    }
                    break;
                case NodeType.ComputationalNode: //TODO: check!!
                    if (this.serverQueues.ProblemsToSolve.Count > 0) {
                        PartialProblem partialProblems = this.serverQueues.ProblemsToSolve.Dequeue();
                        return partialProblems.SerializeToXML();
                    }
                    break;
                default:
                    break;
            }

            Debug.Assert(node != null, "Received unregistered node status");
            if (node == null) {
                Console.WriteLine("Received unregistered node status");
                return (new NoOperation()).SerializeToXML();
            }

            NoOperation noOperationResponse = new NoOperation();
            return noOperationResponse.SerializeToXML();
        }

        private string ReceivedDivideProblem(DivideProblem divideProblem)
        {
            /* Divide Problem is sent to TM to start the action of dividing the problem instance to smaller tasks. 
             * TM is provided with information about the computational power of the cluster in terms of total number 
             * of available threads. The same message is used to relay information for synchronizing info with Backup CS.
             */
            Debug.Assert(this.BackupMode == true, "ReceivedDivideProblem received to primary Server");

            NoOperation noOperationResponse = new NoOperation();
            return noOperationResponse.SerializeToXML();
        }

        private string ReceivedSolutionRequest(SolutionRequest solutionRequest)
        {
            /* Solution Request message is sent from the CC in order to check whether the cluster has successfully 
             * computed the solution. It allows CC to be shut down and disconnected from server during computations.
             */
            Solutions solution = new Solutions();
            List<Solutions> solutions = this.serverQueues.FinalSolutions.ToList();
            foreach (Solutions s in solutions) {
                if (s.Id == solutionRequest.Id) {
                    solution = s;
                    break;
                }
            }

            return solution.SerializeToXML();
        }

        private string ReceivedSolvePartialProblems(SolvePartialProblems solvePartialProblems)
        {
            /* Partial problems message is sent by the TM after dividing the problem into smaller partial problems. 
             * The data in it consists of two parts – common for all partial problems and specific for the given task. 
             * The same Partial Problems schema is used for the messages sent to be computed by the CN and to relay 
             * information for synchronizing info with Backup CS.
             */

            NoOperationBackupCommunicationServers servers = new NoOperationBackupCommunicationServers();
            List<NoOperationBackupCommunicationServersBackupCommunicationServer> backups = new List<NoOperationBackupCommunicationServersBackupCommunicationServer>();
            foreach (Node comp in this.RegisteredComponents.BackupServers) {
                NoOperationBackupCommunicationServersBackupCommunicationServer backup = new NoOperationBackupCommunicationServersBackupCommunicationServer();
                backup.address = comp.IP.ToString();
                if (comp.Port > 0) {
                    backup.port = comp.Port;
                    backup.portSpecified = true;
                }
                backups.Add(backup);
            }
            servers.BackupCommunicationServer = backups.ToArray();
            return (new NoOperation()).SerializeToXML();
        }

        private string ReceivedSolveRequest(SolveRequest solveRequest)
        {
            this.serverQueues.SolveRequests.Enqueue(solveRequest);

            SolveRequestResponse response = new SolveRequestResponse();
            response.Id = TaskIDCounter++;
            response.IdSpecified = true;

            return response.SerializeToXML();
        }

        /// <summary>
        /// Deserializes message, generates appriopriate action and returns message to response.
        /// </summary>
        /// <param name="xml">Message received from a node</param>
        /// <returns>Serialized response to a node</returns>
        public string ReceivedMessage(string xml, IPAddress senderAddr)
        {
            Object obj = xml.DeserializeXML();

            if (obj is DivideProblem) {  //Message to task Manager
                return this.ReceivedDivideProblem((DivideProblem)obj);

            } else if (obj is Register) {
                return this.ReceivedRegister((Register)obj, senderAddr);

            }else if (obj is SolutionRequest) {
                return this.ReceivedSolutionRequest((SolutionRequest)obj);

            } else if (obj is SolvePartialProblems) {
                return this.ReceivedSolvePartialProblems((SolvePartialProblems)obj);

            } else if (obj is SolveRequest) {
                return this.ReceivedSolveRequest((SolveRequest)obj);

            } else if (obj is Status) {
                return this.ReceivedStatus((Status)obj);

            } else if (obj is RegisterResponse) {
                Debug.Assert(false, "RegisterResponse received to primary Server");

            } else if (obj is NoOperation) {  //Sent in response to status messge
                Debug.Assert(false, "NoOperation received to primary Server");
            }

            Debug.Assert(false, "Unrecognized request");
            return "Error";
        }


        #endregion


        
        

        #region Connection/Private


        private void Listen(Int32 port, IPAddress localAddr)
        {
            AsynchronousSocketListener.StartListening(port, localAddr);         
        }
        
        #endregion


        #region Misc/Private

        private String[] ParseParameters(string parameters)
        {
            String[] Data = new String[3];
            int t;
            int port;
            //wezmiemy tylko tylko cyfre między -port a -t
            var count = parameters.Count(x => x == '-');
            if (count == 3) {
                string PortS = parameters.Substring(GetNthIndex(parameters, 't', 1) + 1, GetNthIndex(parameters, '-', 2) - GetNthIndex(parameters, 't', 1) - 1);
                string backup = parameters.Substring(GetNthIndex(parameters, '-', 2), GetNthIndex(parameters, '-', 3) - GetNthIndex(parameters, '-', 2));
                string tS = parameters.Substring(GetNthIndex(parameters, '-', 3) + 2);
                Console.WriteLine(PortS);

                bool x = Int32.TryParse(tS, out t);
                if (x = !true) {
                    Console.WriteLine("Wrong timeout");
                    return null;
                }

                x = Int32.TryParse(PortS, out port);
                if (x != true) {
                    Console.WriteLine("Wrong _port number");
                    return null;
                }

                Data[0] = PortS;
                Data[1] = backup;
                Data[2] = tS;

            } else {
                Console.WriteLine("Incorrect Syntax");
                return null;
            }

            return Data;
        }


        private int GetNthIndex(string s, char t, int n)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++) {
                if (s[i] == t) {
                    count++;
                    if (count == n) {
                        return i;
                    }
                }
            }
            return -1;
        }

        #endregion


    }
}