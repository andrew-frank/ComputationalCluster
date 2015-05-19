using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Shared.Connection
{

    public class CMSocket
    {
        private static CMSocket instance;
        private TcpClient client;

        private CMSocket() { } //hidden initializer - use Instance

        public static CMSocket Instance
        {
            get
            {
                if (instance == null) {
                    instance = new CMSocket();
                }
                return instance;
            }
        }

        public string SendMessage(Int32 port, IPAddress server, String message)
        {

            return AsynchronousClient.StartClient(port, server, message);
            
            
            
        //    // String to store the response ASCII representation.
        //    String responseData = String.Empty;

        //    try {
        //        this.client = new TcpClient(server, port);

        //        // Translate the passed message into ASCII and store it as a Byte array.
        //        Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

        //        // Get a client stream for reading and writing. 
        //        //  Stream stream = client.GetStream();
        //        NetworkStream stream = client.GetStream();

        //        // Send the message to the connected TcpServer. 
        //        stream.Write(data, 0, data.Length);

        //        Console.WriteLine("Sent: {0}", message);

        //        // Receive the TcpServer.response. 

        //        // Buffer to store the response bytes.
        //        data = new Byte[256];
        //        int temp;
        //        String response = "";
        //        String tempData = "";
        //        // Read the first batch of the TcpServer response bytes.
        //        // Loop to receive all the data sent by the client.
        //        do
        //        {
        //            temp = stream.Read(data, 0, data.Length);

        //            Console.WriteLine("Rozmiar byte array=" + temp + "\n");

        //            // Translate data bytes to a ASCII string.
        //            tempData = System.Text.Encoding.ASCII.GetString(data, 0, temp);
        //            response += tempData;

        //            Console.WriteLine("Received: \n" + tempData + "\n");

        //        } while (stream.DataAvailable);

        //        responseData = response;
        //        Console.WriteLine("Received: {0}", responseData);
        //        //Int32 bytes = stream.Read(data, 0, data.Length);
        //        //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

        //        // Close everything.
        //        stream.Close();
        //        client.Close();

        //    } catch (ArgumentNullException e) {
        //        Console.WriteLine("ArgumentNullException: {0}", e);
        //        System.Diagnostics.Debug.WriteLine("ArgumentNullException: " + e.ToString());

        //    } catch (SocketException e) {
        //        Console.WriteLine("Util SocketException: " + e.ToString());
        //        System.Diagnostics.Debug.WriteLine("SocketException: " + e.ToString());
        //    }

        //    return responseData;
        }

    }


    public static class ConnectionHelpers
    {
        public static IPAddress getIPAddressOfTheLocalMachine()
        {
            String strHostName = Dns.GetHostName();
            Console.WriteLine("Local Machine's Host Name: " + strHostName);

            IPHostEntry ipEntry;
            ipEntry = Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;
            IPAddress IPv4 = null;
            for (int i = 0; i < addr.Length; i++) {
                if (addr[i].IsIPv6LinkLocal == false && addr[i].AddressFamily == AddressFamily.InterNetwork) {
                    Console.WriteLine("Correct IP Address {0}: {1} ", i, addr[i].ToString());
                    IPv4 = addr[i];
                } else
                    System.Diagnostics.Debug.WriteLine("Incorrect Address {0}: {1} ", i, addr[i].ToString());
            }

            Debug.Assert(IPv4 != null, "No ip4 addresses found");
            Console.WriteLine("IP Address: " + IPv4.ToString());

            if (IPv4 != null)
                return IPv4;
            return addr[addr.GetLength(0) - 1];
        }

        public static void CheckInputSyntax(string parameters, Int32 port, String address)
        {
            var count = parameters.Count(x => x == '-');
            if (count == 2) {
                bool x;
                string addressS = parameters.Substring(GetNthIndex(parameters, 's', 2) + 1, GetNthIndex(parameters, '-', 2) - GetNthIndex(parameters, 's', 2) - 1);
                string PortS = parameters.Substring(GetNthIndex(parameters, '-', 2) + 5);
                Console.WriteLine(PortS);
                Console.WriteLine(addressS);

                x = Int32.TryParse(PortS, out port);
                if (x != true) {
                    Console.WriteLine("Wrong port number");
                }
                address = addressS;

            } else {
                Console.WriteLine("Incorrect Syntax");
            }
        }

        public static int GetNthIndex(string s, char t, int n)
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
    }
}