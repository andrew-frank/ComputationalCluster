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
using ComputationalCluster.Shared.Messages.SolvePartialProblemsNamespace;
using ComputationalCluster.Shared.Messages.SolutionRequestNamespace;
using ComputationalCluster.Shared.Messages.DivideProblemNamespace;
using ComputationalCluster.Shared.Messages.SolveRequestResponseNamespace;
using ComputationalCluster.Shared.Messages.SolutionsNamespace;



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
            this.IP = server;
            Console.WriteLine("Task Manager Started");

            this.RegisterComponent();
            this.StartTimeoutTimer();

            while (true) Console.ReadLine();
        }


        protected override Status CurrentStatus()
        {
            Status status = new Status();
            status.Id = this.ID;
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


        #region Overrides


        protected override string ReceivedDivideProblem(DivideProblem divideProblem)
        {
            /* Divide Problem is sent to TM to start the action of dividing the problem instance to smaller tasks. 
             * TM is provided with information about the computational power of the cluster in terms of total number 
             * of available threads. The same message is used to relay information for synchronizing info with Backup CS.
             */

            //Debug.Assert(false, "Unimplemented");

            //!!!!!!!!!!!!!!!!!!!
            ////we are not dividing yet - inserting everything into CommonData
            ////the same should be done in the ComputationalNode
            SolvePartialProblems solvePartialProblems = new SolvePartialProblems();
            solvePartialProblems.CommonData = divideProblem.Data;

            solvePartialProblems.Id = divideProblem.Id;
            solvePartialProblems.SolvingTimeoutSpecified = false;
            if(divideProblem.ProblemType != null)
                solvePartialProblems.ProblemType = divideProblem.ProblemType;

            CMSocket.Instance.SendMessage(this.Port, this.IP, solvePartialProblems.SerializeToXML(), this);

            return null;
        }

        protected override string ReceivedSolveRequest(SolveRequest solveRequest)
        {
            Debug.Assert(false, "Should not be here");
            return null;
        }

        protected override string ReceivedSolvePartialProblems(SolvePartialProblems solvePartialProblems)
        {
            Debug.Assert(false, "Should not be here");
            return null;
        }

        protected override string ReceivedSolutions(Solutions solution)
        {
            Debug.Assert(false, "Should not be here");
            return null;
        }

        protected override string ReceivedSolveRequestResponse(SolveRequestResponse solveRequestResponse)
        {
            Debug.Assert(false, "Should not be here");
            return null;
        }


        protected override string ReceivedRegister(Register register, IPAddress senderAddr)
        {
            Debug.Assert(false, "Should not be here");
            return null;
        }

        protected override string ReceivedStatus(Status status)
        {
            Debug.Assert(false, "Should not be here");
            return null;
        }


        protected override string ReceivedSolutionRequest(SolutionRequest solutionRequest)
        {
            Debug.Assert(false, "Should not be here");
            return null;
        }


        #endregion
    }
}
