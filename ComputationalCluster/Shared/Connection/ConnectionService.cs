using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Shared.Connection {
    public static class ConnectionService 
    {
        public static void SendXMLToAddress(this string xml, IPAddress address) 
        {

        }

        public static void ConnectAndSendMessage(String server, String message)
        {
            try
            {

                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing. 
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response. 

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            //Console.WriteLine("\n Press Enter to continue...");
            //Console.Read();
        }
        
        public static IPAddress getIPAddressOfTheLocalMachine()
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
    }
}
