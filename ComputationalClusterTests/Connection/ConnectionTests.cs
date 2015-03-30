using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Sockets;

namespace ComputationalClusterTests.Connection {
    [TestClass]
    public class ConnectionTests 
    {
        [TestMethod]
        public void TestLocalIpAddress() 
        {
            IPAddress ip = ComputationalCluster.Shared.Connection.ConnectionService.getIPAddressOfTheLocalMachine();
            Assert.IsNotNull(ip);
            //Assert.IsTrue(ip.IsIPv6LinkLocal == false);
            //Assert.IsTrue(ip.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}