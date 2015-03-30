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


namespace ComputationalCluster.Server
{
    public sealed class Server
    {
   
        public void startInstance(IPAddress localIPAddress)
        {
            Console.WriteLine("Server Started, Specify Parameters");
            Console.WriteLine("Parameters syntax: [-port [port number]] [-backup] [-t [time in seconds]]");
            Console.Write("> ");   
            Int32 port;

            //String parameters;
            //parameters = Console.ReadLine();
            //parameters = parameters.Replace(" ", string.Empty);
            //int t;
            ////wezmiemy tylko tylko cyfre między -port a -t
            //var count = parameters.Count(x => x == '-');
            //if (count == 3) {
            //    string PortS = parameters.Substring(GetNthIndex(parameters, 't', 1) + 1, GetNthIndex(parameters, '-', 2) - GetNthIndex(parameters, 't', 1) - 1);
            //    string backup = parameters.Substring(GetNthIndex(parameters, '-', 2), GetNthIndex(parameters, '-', 3) - GetNthIndex(parameters, '-', 2));
            //    string tS = parameters.Substring(GetNthIndex(parameters, '-', 3) + 2);
            //    Console.WriteLine(PortS);

            //    bool x = Int32.TryParse(tS, out t);
            //    if (x = !true) {
            //        Console.WriteLine(" Wrong timeout");
            //    }
            //    x = Int32.TryParse(PortS, out port);
            //    if (x != true) {
            //        Console.WriteLine("Wrong port number");
            //    }
            //    Listen(port, localIPAddress);
            
            //} else {
            //    Console.WriteLine("Incorrect Syntax");
            //}

              port = 13000;
              Listen(port, localIPAddress);
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

                    }  while (stream.DataAvailable);
                    
                    Console.WriteLine("Received: {0}", response);
                    //parse/map object & react
                    string xml = (string)response;
                    this.ReceivedMessage(xml);

                    //wysylanie powinno byc tu (odpowiedz odbywa sie po odebraniu i przeanalizowaniu wiadomosci), 
                    //a w petli powyzej wywala exception
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes("hej, mam");
                    stream.Write(msg, 0, msg.Length);
                    //Console.WriteLine("Sent: {0}", response);
                   

                    // Shutdown and end connection
                    client.Close();
                }


            }
            catch (SocketException e) {
                Console.WriteLine("Server SocketException: " + e.ToString());
                System.Diagnostics.Debug.WriteLine("SocketException: " + e.ToString());
            }

            catch (Exception e) {
                Console.WriteLine("Server SocketException: " + e.ToString());
                System.Diagnostics.Debug.WriteLine("SocketException: " + e.ToString());
                throw e;
            }            

            finally {
                // Stop listening for new clients.
                TCPServer.Stop();
            }
        }


        private void ReceivedMessage(string xml) 
        {

            Object obj = xml.DeserializeXML();

            //it should be (didn't test the try-catch trick) mapped runtime to a correct object, just check for it's type:
            if (obj is DivideProblem) {

            } else if (obj is NoOperation) {

            } 

            //etc...
        }


        public int GetNthIndex(string s, char t, int n)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

    }
}