using ComputationalCluster.Nodes;
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

        public string SendMessage(Int32 port, IPAddress server, String message, Node node)
        {
            return AsynchronousClient.StartClient(port, server, message, node);
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