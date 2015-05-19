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
using ComputationalCluster.Computational;

namespace ComputationalCluster.Nodes
{
    public class ComputationalNode : Node
    {

        #region Properties/ivars

        private ProblemType[] _solvableProblems;
        public ProblemType[] SolvableProblems { get { return _solvableProblems; } }


        private List<NodeWorker> _workers = new List<NodeWorker>();
        public List<NodeWorker> Workers {get {return _workers; } }

        #endregion

        #region Public

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


        public void startInstance(UInt16 port, IPAddress server)
        {
            this.Port = port;
            this.IP = server;
            Console.WriteLine("Computational Node Started");

            this.RegisterComponent();
            this.StartTimeoutTimer();

            while (true) Console.ReadLine();
        }

        #endregion


        #region Overrides

        protected override Status CurrentStatus()
        {
            Status status = new Status();
            status.Threads = this.CurrentStatusThreads();
            status.Id = this.ID;
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

        #endregion


        #region Private

        private StatusThreadsThread[] CurrentStatusThreads()
        {
            List<StatusThreadsThread> threads = new List<StatusThreadsThread>();

            foreach (NodeWorker worker in this.Workers)
                threads.Add(ComputationalNode.StatusThreadsThreadFromNodeWorker(worker));

            return threads.ToArray();
        }

        private static StatusThreadsThread StatusThreadsThreadFromNodeWorker(NodeWorker worker)
        {
            StatusThreadsThread thread = new StatusThreadsThread();
            thread.State = worker.StateString;
            if (worker.State == true) {
                if (thread.HowLong > 0) {
                    thread.HowLong = worker.HowLong;
                    thread.HowLongSpecified = true;
                } else
                    thread.HowLongSpecified = false;
                thread.TaskIdSpecified = true;
                thread.TaskId = worker.TaskId;
                thread.ProblemInstanceId = worker.ProblemInstanceId;
                thread.ProblemInstanceIdSpecified = true;
                thread.ProblemType = worker.ProblemType;
            } else {
                thread.TaskIdSpecified = false;
                thread.HowLongSpecified = false;
                thread.ProblemInstanceIdSpecified = false;
            }

            return thread;
        }

        #endregion
    }
}
