using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Shared.Connection {
    public static class ConnectionService 
    {
        public static void ConnectAndSendMessage(Int32 port , String server, String message)
        {
            try {
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


            } catch (ArgumentNullException e) {
                Console.WriteLine("ArgumentNullException: {0}", e);
                System.Diagnostics.Debug.WriteLine("ArgumentNullException: " + e.ToString());

            } catch (SocketException e) {
                Console.WriteLine("Util SocketException: " + e.ToString());
                System.Diagnostics.Debug.WriteLine("SocketException: " + e.ToString());
            }

        }

        public static IPAddress getIPAddressOfTheLocalMachine() 
        {
            String strHostName = Dns.GetHostName();
            Console.WriteLine("Local Machine's Host Name: " + strHostName);

            IPHostEntry ipEntry; //= Dns.GetHostByName(strHostName);
            ipEntry = Dns.GetHostEntry(strHostName); //Sugeruje użycie GetHostEntry, ale wtedy adres IP jest w innej formie

            IPAddress[] addr = ipEntry.AddressList;
            IPAddress IPv4 = null;
            for (int i = 0; i < addr.Length; i++) {
                if (addr[i].IsIPv6LinkLocal == false && addr[i].AddressFamily == AddressFamily.InterNetwork ) {
                    Console.WriteLine("Correct IP Address {0}: {1} ", i, addr[i].ToString());
                    IPv4 = addr[i];
                } else
                    System.Diagnostics.Debug.WriteLine("Incorrect Address {0}: {1} ", i, addr[i].ToString());
            }

            if (IPv4 == null)
                throw new Exception();
            Console.WriteLine("IP Address: " + IPv4.ToString());

            return addr[0];
        }

        public static void CheckInputSyntax(string parameters, Int32 port, String address)
        {
            var count = parameters.Count(x => x == '-');
            if (count == 2)
            {
                bool x;
                string addressS = parameters.Substring(GetNthIndex(parameters, 's', 2) + 1, GetNthIndex(parameters, '-', 2) - GetNthIndex(parameters, 's', 2) - 1);
                string PortS = parameters.Substring(GetNthIndex(parameters, '-', 2) + 5);
                Console.WriteLine(PortS);
                Console.WriteLine(addressS);

                x = Int32.TryParse(PortS, out port);
                if (x != true)
                {
                    Console.WriteLine("Wrong port number");
                }
                address = addressS;

            }
            else
            {
                Console.WriteLine("Incorrect Syntax");
            }
        }
        
        public static int GetNthIndex(string s, char t, int n)
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
