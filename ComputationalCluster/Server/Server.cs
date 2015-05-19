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

namespace ComputationalCluster.Nodes
{
    public sealed class Server : Node
    {
        #region Properties/ivars

        

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
        }


        public void startInstance(UInt16 port, IPAddress localIPAddress)
        {
            this.ID = 0;
            Console.WriteLine("Server Started, Specify Parameters");
            String[] Data = new String[3];

            Console.Write("Debug? [y/n] \n>");
            string debug = Console.ReadLine();
            if (debug == "y") {
                Console.WriteLine("");
                this.Port = port;
                this.Listen(this.Port, localIPAddress);
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
            this.Listen(this.Port, this.LocalIP);

            //TODO: backup server code. These below are unreachable, beacuse of blocking Listen() method
            if (this.BackupMode) {
                this.RegisterComponent();
                this.StartTimeoutTimer();
            }
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

        private string ReceivedRegister(Register register)
        {
            string type = register.Type;

            //Register message is sent by TM, CN and Backup CS to the CS after they are activated.
            RegisterResponse response = new RegisterResponse();
            response.Id = RegisteredNodes.NextNodeID;
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
            NoOperation noOperationResponse = new NoOperation();
            return noOperationResponse.SerializeToXML();
        }

        private string ReceivedDivideProblem(DivideProblem divideProblem)
        {
            return "";
        }

        private string ReceivedSolutionRequest(SolutionRequest solutionRequest)
        {
            return "";
        }

        private string ReceivedSolvePartialProblems(SolvePartialProblems solvePartialProblems)
        {
            return "";
        }

        private string ReceivedSolveRequest(SolveRequest solveRequest)
        {
            return "";
        }

        #endregion


        #region Private

        /// <summary>
        /// Deserializes message, generates appriopriate action and returns message to response.
        /// </summary>
        /// <param name="xml">Message received from a node</param>
        /// <returns>Serialized response to a node</returns>
        private string ReceivedMessage(string xml)
        {
            Object obj = xml.DeserializeXML();

            if (obj is DivideProblem) {  //Message to task Manager
                return this.ReceivedDivideProblem((DivideProblem)obj);

            } else if (obj is Register) {
                return this.ReceivedRegister((Register)obj);

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


        #region Connection/Private


        private void Listen(Int32 port, IPAddress localAddr)
        {


            AsynchronousSocketListener.StartListening(port, localAddr);
            
            //TcpListener TCPServer = null;
            //try {
            //    // TcpListener server = new TcpListener(port);
            //    TCPServer = new TcpListener(localAddr, port);

            //    // Start listening for client requests.
            //    TCPServer.Start();

            //    // Buffer for reading data
            //    Byte[] bytes = new Byte[256];
            //    String data = null;
            //    String response = null;

            //    // Enter the listening loop. 
            //    while (true) {

            //        Console.Write("Waiting for a connection... ");

            //        // Perform a blocking call to accept requests. 
            //        // You could also user server.AcceptSocket() here.

            //        //TcpClient client = TCPServer.AcceptTcpClient();
            //        TcpClient client = TCPServer.AcceptTcpClient();

            //        data = null;

            //        // Get a stream object for reading and writing
            //        NetworkStream stream = client.GetStream();

            //        response = "";
            //        int temp;

            //        // Loop to receive all the data sent by the client.
            //        do {
            //            temp = stream.Read(bytes, 0, bytes.Length);

            //            Console.WriteLine("Rozmiar byte array=" + temp + "\n");

            //            // Translate data bytes to a ASCII string.
            //            data = System.Text.Encoding.ASCII.GetString(bytes, 0, temp);
            //            response += data;

            //            Console.WriteLine("Received: \n" + data + "\n");

            //        } while (stream.DataAvailable);

            //        Console.WriteLine("Received: {0}", response);
            //        //parse/map object & react
            //        string xml = (string)response;
            //        string m = this.ReceivedMessage(xml);


            //        byte[] msg = System.Text.Encoding.ASCII.GetBytes(m);
            //        //stream.Write(msg, 0, msg.Length);

            //        stream.WriteAsync(msg, 0, msg.Length);
            //        //Console.WriteLine("Sent: {0}", response);

            //        // Shutdown and end connection
            //        client.Close();
            //    }

            //} catch (SocketException e) {
            //    Console.WriteLine("Server SocketException: " + e.ToString());
            //    System.Diagnostics.Debug.WriteLine("SocketException: " + e.ToString());
            //} catch (Exception e) {
            //    Console.WriteLine("Server SocketException: " + e.ToString());
            //    System.Diagnostics.Debug.WriteLine("SocketException: " + e.ToString());
            //    throw e;
            //} finally {
            //    // Stop listening for new clients.
            //    TCPServer.Stop();
            //}
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

        #endregion

    }
}