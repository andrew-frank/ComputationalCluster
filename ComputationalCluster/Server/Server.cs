using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Server
{
    public class Server
    {
        IPAddress localIPAddress;
        
        public void Main()
        {
            localIPAddress = getIPAddressOfTheLocalMachine();            
            startInstance(localIPAddress);
            waitUntilUserClose();
        }       

        protected void startInstance(IPAddress localIPAddress)
        {
            Console.WriteLine("Server Started");
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = localIPAddress;

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;
                String response = null;
                // Enter the listening loop. 
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client. 
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        //data = data.ToUpper();

                        //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        //// Send back a response.
                        //stream.Write(msg, 0, msg.Length);
                        //Console.WriteLine("Sent: {0}", data);
                        response = data.Replace("ZAREJESTRUJ", "DONE!");
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", response);

                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        }

        protected IPAddress getIPAddressOfTheLocalMachine()
        {
            String strHostName = "";       
            // Getting Ip address of local machine...
            // First get the host name of local machine.
            strHostName = Dns.GetHostName();            
            Console.WriteLine("Local Machine's Host Name: " + strHostName);
            IPHostEntry ipEntry = Dns.GetHostByName(strHostName); //Sugeruje użycie GetHostEntry, ale wtedy adres IP jest w innej formie
            IPAddress[] addr = ipEntry.AddressList;
            //for (int i = 0; i < addr.Length; i++)
            //{
            //    Console.WriteLine("IP Address {0}: {1} ", i, addr[i].ToString());
            //}
            Console.WriteLine("IP Address {0}: {1} ", 0, addr[0].ToString());
            return addr[0];           
            
        }
        
        private void waitUntilUserClose()
        {
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}