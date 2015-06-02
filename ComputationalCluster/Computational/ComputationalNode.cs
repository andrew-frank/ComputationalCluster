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
using ComputationalCluster.Client;
using ComputationalCluster.Shared.Messages.SolveRequestNamespace;
using ComputationalCluster.Shared.Messages.SolveRequestResponseNamespace;

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
            string filename = "problem.vrp";
            ExampleObject example = ProblemLoader.LoadProblem(filename);

          if(example.Requests.Count==0)
          {   /*some exception*/ }
           
            //Here fire the algorithm for example.
            this.RegisterComponent();
            this.StartTimeoutTimer();

            while (true) {
                Console.WriteLine("Possible actions:\n1.Send problem\n2.Check for solution\n>");
                string str = Console.ReadLine();
                ulong number = 0;
                if (ulong.TryParse(str, out number)) {
                    switch (number) {
                        case 1:
                            Console.WriteLine("Specify name of the problem:\n>");
                            Console.WriteLine("Sending example problem");
                            this.SendExampleProblem();
                            break;
                        case 2:
                            Console.WriteLine("Specify ID of the problem:\n>");
                            str = Console.ReadLine();
                            number = 0;
                            if(ulong.TryParse(str, out number)) {

                            }
                            break;
                    }
                } else {
                    Console.WriteLine("Unrecognized action");
                }
            }
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

        private void CheckForSolution(ulong problemID)
        {
            //TODO:
            Console.WriteLine("Unimplemented");
        }


        private void SendExampleProblem()
        {
            SolveRequest solveRequest = new SolveRequest();
            String problem = "VRPTEST io2_8a\r\nCOMMENT: \r\nNAME: io2_8a\r\nNUM_DEPOTS: 1\r\nNUM_CAPACITIES: 1\r\nNUM_VISITS: 8\r\nNUM_LOCATIONS: 9\r\nNUM_VEHICLES: 8\r\nCAPACITIES: 100\r\nDATA_SECTION\r\nDEPOTS\r\n  0\r\nDEMAND_SECTION\r\n  1 -29\n  2 -21\n  3 -28\n  4 -20\n  5 -8\n  6 -31\n  7 -13\n  8 -29\nLOCATION_COORD_SECTION\r\n  0 0 0\r\n  1 -39 97\n  2 34 -45\n  3 77 -20\n  4 -34 -99\n  5 75 -43\n  6 87 -66\n  7 -55 86\n  8 -93 -3\nDEPOT_LOCATION_SECTION\r\n  0 0\r\nVISIT_LOCATION_SECTION\r\n  1 1\n  2 2\n  3 3\n  4 4\n  5 5\n  6 6\n  7 7\n  8 8\nDURATION_SECTION\r\n  1 20\n  2 20\n  3 20\n  4 20\n  5 20\n  6 20\n  7 20\n  8 20\nDEPOT_TIME_WINDOW_SECTION\r\n  0 0 560\r\nCOMMENT: TIMESTEP: 7\r\nTIME_AVAIL_SECTION\r\n  1 276\n  2 451\n  3 171\n  4 365\n  5 479\n  6 546\n  7 376\n  8 289\nEOF";
            var base64 = problem.Base64Encode();
            solveRequest.Data = base64;
            solveRequest.ProblemType = Utilities.ProblemNameForType(ProblemType.DVRP);
            String message = solveRequest.SerializeToXML();
            string response = CMSocket.Instance.SendMessage(this.Port, this.IP, message);

            Object obj = response.DeserializeXML();
            Debug.Assert(obj is SolveRequestResponse, "Wrong response object");
            SolveRequestResponse requestResopnse = (SolveRequestResponse)obj;
            Debug.Assert(requestResopnse.IdSpecified, "No ID in solve request response");
            if (!requestResopnse.IdSpecified)
                return;
            Console.WriteLine("ID of the problem" + requestResopnse.Id);
        }

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
