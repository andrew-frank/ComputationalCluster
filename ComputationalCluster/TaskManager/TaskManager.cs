using ComputationalCluster.Shared.Messages.StatusNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ComputationalCluster.Shared.Utilities;
using ComputationalCluster.Shared.Messages.SolveRequestNamespace;
using ComputationalCluster.Shared.Messages.RegisterNamespace;
using ComputationalCluster.Shared.Connection;
using System.Diagnostics;



namespace ComputationalCluster.Nodes
{
    public sealed class TaskManager : Node
    {
        public TaskManager()
        {
            this.CommonInit();
        }

        private void CommonInit()
        {
            this.NodeType = NodeType.TaskManager;
        }


        public void startInstance(UInt16 port, IPAddress server)
        {
            this.Port = port;
            //this.HostName = hostName;
            this.IP = server;
            Console.WriteLine("Task Manager Started");

            this.RegisterComponent();
            this.StartTimeoutTimer();

            while (true) Console.ReadLine();
        }


        protected override Status CurrentStatus()
        {
            Status status = new Status();
            return status;
        }

        protected override Register GenerateRegister()
        {
            Register register = new Register();
            register.Type = this.TypeName;

            //List<RegisterSolvableProblemsProblemName> solvableProblems = new List<RegisterSolvableProblemsProblemName>();
            //foreach (ProblemType type in this.SolvableProblems) {
            //    RegisterSolvableProblemsProblemName solvProb = new RegisterSolvableProblemsProblemName();
            //    solvProb.Value = Utilities.ProblemNameForType(type);
            //    solvableProblems.Add(solvProb);
            //}
            //register.SolvableProblems = solvableProblems.ToArray();

            return register;
        }
    }
}
