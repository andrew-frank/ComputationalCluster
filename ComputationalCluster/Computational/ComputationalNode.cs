using ComputationalCluster.Shared.Messages.StatusNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.Shared.Utilities;
using ComputationalCluster.Shared.Messages.RegisterNamespace;
using System.Threading;
using ComputationalCluster.Shared.Connection;
using System.Diagnostics;
using ComputationalCluster.Misc;
using ComputationalCluster.Shared.Messages.RegisterResponseNamespace;

namespace ComputationalCluster.Nodes
{
    public class ComputationalNode : Node
    {
        private ProblemType[] _solvableProblems;
        public ProblemType[] SolvableProblems { get { return _solvableProblems; } }


        public ComputationalNode()
        {
            this.CommonInit();
        }

        private void CommonInit()
        {
            this.NodeType = NodeType.ComputationalNode;
            _solvableProblems = new ProblemType[1];
            _solvableProblems[0] = ProblemType.DVRP;
        }


        public void startInstance(Int32 port, String hostName)
        {
            this.Port = port;
            this.HostName = hostName;
            Console.WriteLine("Computational Node Started");

            this.RegisterComponent();
            this.StartTimeoutTimer();
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

            List<RegisterSolvableProblemsProblemName> solvableProblems = new List<RegisterSolvableProblemsProblemName>();
            foreach (ProblemType type in this.SolvableProblems) {
                RegisterSolvableProblemsProblemName solvProb = new RegisterSolvableProblemsProblemName();
                solvProb.Value = Utilities.ProblemNameForType(type);
                solvableProblems.Add(solvProb);
            }

            register.SolvableProblems = solvableProblems.ToArray();

            return register;
        }
    }
}
