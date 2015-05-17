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

namespace ComputationalCluster.Nodes
{

    public class RegisteredNodes
    {
        private List<Node> _clients = new List<Node>();
        private List<Node> _taskManagers = new List<Node>();
        private List<Node> _computationalNodes = new List<Node>();
        private List<Node> _backupServers = new List<Node>();

        public List<Node> Clients { get { return _clients; } }
        public List<Node> TaskManagers { get { return _taskManagers; } }
        public List<Node> ComputationalNodes { get { return _computationalNodes; } }
        public List<Node> BackupServers { get { return _backupServers; } }


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
            this.BackupServers.Add(node);
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

        public bool DeregisterBackupServer(Node node)
        {
            int i = 0;
            foreach (Node n in BackupServers) {
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
    }


    public sealed class Server : Node
    {
        private RegisteredNodes _registeredComponents = new RegisteredNodes();
        public RegisteredNodes RegisteredComponents { get { return _registeredComponents; } }

        public string backup;

        public Server()
        {
            this.Init();
        }

        private void Init()
        {
            this.NodeType = NodeType.Server;
            this.ID = Guid.NewGuid();
        }


        public void startInstance(Int32 port, IPAddress localIPAddress, Int32 timeout)
        {
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

            while (Data[0] == null) {
                Console.WriteLine("Parameters syntax: [-port [port number]] [-backup] [-t [time in seconds]]");
                Console.Write("> ");

                String parameters;
                parameters = Console.ReadLine();
                parameters = parameters.Replace(" ", string.Empty);
                Data = this.ParseParameters(parameters);
            }

            this.Port = Int32.Parse(Data[0]);
            this.backup = Data[1];
            this.Timeout = Int32.Parse(Data[2]);

            this.LocalIP = localIPAddress;
            Listen(this.Port, this.LocalIP);
        }


        private void Listen(Int32 port, IPAddress localAddr)
        {
            TcpListener TCPServer = null;
            try {
                // TcpListener server = new TcpListener(port);
                TCPServer = new TcpListener(localAddr, port);

                // Start listening for client requests.
                TCPServer.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;
                String response = null;

                // Enter the listening loop. 
                while (true) {

                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.

                    //TcpClient client = TCPServer.AcceptTcpClient();
                    TcpClient client = TCPServer.AcceptTcpClient();

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    response = "";
                    int temp;

                    // Loop to receive all the data sent by the client.
                    do {
                        temp = stream.Read(bytes, 0, bytes.Length);

                        Console.WriteLine("Rozmiar byte array=" + temp + "\n");

                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, temp);
                        response += data;

                        Console.WriteLine("Received: \n" + data + "\n");

                    } while (stream.DataAvailable);

                    Console.WriteLine("Received: {0}", response);
                    //parse/map object & react
                    string xml = (string)response;
                    string m = this.ReceivedMessage(xml);


                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(m);
                    stream.Write(msg, 0, msg.Length);
                    //Console.WriteLine("Sent: {0}", response);

                    // Shutdown and end connection
                    client.Close();
                }

            } catch (SocketException e) {
                Console.WriteLine("Server SocketException: " + e.ToString());
                System.Diagnostics.Debug.WriteLine("SocketException: " + e.ToString());
            } catch (Exception e) {
                Console.WriteLine("Server SocketException: " + e.ToString());
                System.Diagnostics.Debug.WriteLine("SocketException: " + e.ToString());
                throw e;
            } finally {
                // Stop listening for new clients.
                TCPServer.Stop();
            }
        }


        private string ReceivedMessage(string xml)
        {

            Object obj = xml.DeserializeXML();

            string message = "";

            if (obj is DivideProblem) { //Message to task Manager


            } else if (obj is NoOperation) {//Sent in response to status messge


            } else if (obj is Register) { //Register message is sent by TM, CN and Backup CS to the CS after they are activated.
                RegisterResponse response = new RegisterResponse();
                message = response.SerializeToXML();

            } else if (obj is RegisterResponse) {

            } else if (obj is SolutionRequest) {


            } else if (obj is SolvePartialProblems) {

            } else if (obj is SolveRequest) {

            } else if (obj is Status) {
                NoOperation noOperationResponse = new NoOperation();
                message = noOperationResponse.SerializeToXML();
            }

            return message;

            //etc...
        }



        #region Private

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