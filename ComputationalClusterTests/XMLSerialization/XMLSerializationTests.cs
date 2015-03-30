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
        public void SerializeDivideProblem() {
            DivideProblem d = new DivideProblem();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
            DeserializeDivideProblem(xml);
        }

        [TestMethod]
        public void SerializeNoOperation() {
            NoOperation d = new NoOperation();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
            DeserializeNoOperation(xml);
        }

        [TestMethod]
        public void SerializeRegister() {
            Register d = new Register();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
            DeserializeRegister(xml);
        }

        [TestMethod]
        public void SerializeRegisterResponse() {
            RegisterResponse d = new RegisterResponse();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
            DeserializeRegisterResponse(xml);
        }

        [TestMethod]
        public void SerializeSolutionRequest() {
            SolutionRequest d = new SolutionRequest();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
            DeserializeSolutionRequest(xml);
        }

        [TestMethod]
        public void SerializeSolutions() {
            Solutions d = new Solutions();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
            DeserializeSolutions(xml);
        }

        [TestMethod]
        public void SerializeSolvePartialProblems() {
            SolvePartialProblems d = new SolvePartialProblems();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
            DeserializeSolvePartialProblems(xml);
        }

        [TestMethod]
        public void SerializeSolveRequest() {
            SolveRequest d = new SolveRequest();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
            DeserializeSolveRequest(xml);
        }

        [TestMethod]
        public void SerializeSolveRequestResponse() {
            SolveRequestResponse d = new SolveRequestResponse();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
            DeserializeSolveRequestResponse(xml);
        }

        [TestMethod]
        public void SerializeStatus() {
            Status d = new Status();
            string xml = d.SerializeToXML();
            Assert.IsNotNull(xml);
            DeserializeStatus(xml);
        }


        [TestMethod]
        public void DeserializeDivideProblem(string xml) {
            DivideProblem d = new DivideProblem();
            d = (DivideProblem)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeNoOperation(string xml) {
            NoOperation d = new NoOperation();
            d = (NoOperation)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeRegister(string xml) {
            Register d = new Register();
            d = (Register)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeRegisterResponse(string xml) {
            RegisterResponse d = new RegisterResponse();
            d = (RegisterResponse)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeSolutionRequest(string xml) {
            SolutionRequest d = new SolutionRequest();
            d = (SolutionRequest)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeSolutions(string xml) {
            Solutions d = new Solutions();
            d = (Solutions)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeSolvePartialProblems(string xml) {
            SolvePartialProblems d = new SolvePartialProblems();
            d = (SolvePartialProblems)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeSolveRequest(string xml) {
            SolveRequest d = new SolveRequest();
            d = (SolveRequest)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeSolveRequestResponse(string xml) {
            SolveRequestResponse d = new SolveRequestResponse();
            d = (SolveRequestResponse)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }

        [TestMethod]
        public void DeserializeStatus(string xml) {
            Status d = new Status();
            d = (Status)xml.DeserializeXML();
            Assert.IsNotNull(d);
        }
    }
}
