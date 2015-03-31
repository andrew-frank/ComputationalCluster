using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using ComputationalCluster.Shared.Utilities;

namespace ComputationalClusterTests.XMLSerialization {
    [TestClass]
    public class XMLSerializationTests {
        [TestMethod]
        public void TestMethod1() {}


        [TestMethod]
        public void DeserializeDivideProblem()
        {
            DivideProblem d = new DivideProblem();
            string xml = d.SerializeToXML();
            d = (DivideProblem)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeNoOperation()
        {
            NoOperation d = new NoOperation();
            string xml = d.SerializeToXML();
            d = (NoOperation)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeRegister()
        {
            Register d = new Register();
            string xml = d.SerializeToXML();
            d = (Register)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeRegisterResponse()
        {
            RegisterResponse d = new RegisterResponse();
            string xml = d.SerializeToXML();
            d = (RegisterResponse)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeSolutionRequest()
        {
            SolutionRequest d = new SolutionRequest();
            string xml = d.SerializeToXML();
            d = (SolutionRequest)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeSolutions()
        {
            Solutions d = new Solutions();
            string xml = d.SerializeToXML();
            d = (Solutions)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeSolvePartialProblems()
        {
            SolvePartialProblems d = new SolvePartialProblems();
            string xml = d.SerializeToXML();
            d = (SolvePartialProblems)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeSolveRequest()
        {
            SolveRequest d = new SolveRequest();
            string xml = d.SerializeToXML();
            d = (SolveRequest)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeSolveRequestResponse()
        {
            SolveRequestResponse d = new SolveRequestResponse();
            string xml = d.SerializeToXML();
            d = (SolveRequestResponse)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeStatus()
        {
            Status d = new Status();
            string xml = d.SerializeToXML();
            d = (Status)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }



        [TestMethod]
        public void SerializeDivideProblem() {
            DivideProblem d = new DivideProblem();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);            
        }

        [TestMethod]
        public void SerializeNoOperation() {
            NoOperation d = new NoOperation();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
            
        }

        [TestMethod]
        public void SerializeRegister() {
            Register d = new Register();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
            
        }

        [TestMethod]
        public void SerializeRegisterResponse() {
            RegisterResponse d = new RegisterResponse();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
            
        }

        [TestMethod]
        public void SerializeSolutionRequest() {
            SolutionRequest d = new SolutionRequest();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
        }

        [TestMethod]
        public void SerializeSolutions() {
            Solutions d = new Solutions();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
        }

        [TestMethod]
        public void SerializeSolvePartialProblems() {
            SolvePartialProblems d = new SolvePartialProblems();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
        }

        [TestMethod]
        public void SerializeSolveRequest() {
            SolveRequest d = new SolveRequest();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
        }

        [TestMethod]
        public void SerializeSolveRequestResponse() {
            SolveRequestResponse d = new SolveRequestResponse();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
        }

        [TestMethod]
        public void SerializeStatus() {
            Status d = new Status();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
        }


        
    }
}
