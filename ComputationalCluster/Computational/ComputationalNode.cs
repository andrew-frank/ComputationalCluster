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
using AL_FOR_DVRP;
using System.IO;
using ComputationalCluster.Shared.Messages.SolvePartialProblemsNamespace;
using ComputationalCluster.Shared.Messages.SolutionRequestNamespace;
using ComputationalCluster.Shared.Messages.DivideProblemNamespace;
using ComputationalCluster.Shared.Messages.SolutionsNamespace;

namespace ComputationalCluster.Nodes
{
    public class ComputationalNode : Node
    {

        #region Properties/ivars

        private ProblemType[] _solvableProblems;
        public ProblemType[] SolvableProblems { get { return _solvableProblems; } }


        private List<NodeWorker> _workers = new List<NodeWorker>();
        public List<NodeWorker> Workers {get {return _workers; } }

        private string globalProblem;

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

            //if (properExample.Requests.Count == 0) {   /*some exception*/ }

            //Here fire the algorithm for example.
            this.RegisterComponent();
            //this.StartTimeoutTimer();

            
            //ExampleObject properExample = ProperProblemLoader.LoadProblemFromString("");

            ////ALGORITHM//
            //var watch = Stopwatch.StartNew();
            //TryServe(properExample.Depots, properExample.vehicleInfo, properExample.Requests);
            //watch.Stop();
            //var elapsedMs = watch.ElapsedMilliseconds;
            //Console.WriteLine("Algorith executed in: " + elapsedMs + "ms");

            //////////////ALOGIRTHM 2///////////////
      //    string a = File.ReadAllText("problem9.vrp");
      //     newExampleObject o = ProblemLoader.loadnewExampleString(a);
       //   double d=  AlgorithmSolution.Find_Solution(o.Location, o.DeliverDemand, o.ServiceBegin, o.ServiceEnd, o.ServiceDuration, o.DistanceMatrix, o.DriveTimeMatrix, o.VehicleName, o.VehicleCapacity);
            //    Console.WriteLine("This is the answer from algorithm: " + d);


            while(true) Console.ReadLine();
        }

        private void TryServe(IList<Depot> Depots, VehicleInfo vehicleInfo, List<Request> requests)
        {

            try {
                var builder = new RouteBuilder(vehicleInfo);
                var routes = builder.Build(Depots, vehicleInfo, requests).ToList();

                String foundRoutes = "";
                String totalDistance = "";
                foundRoutes += string.Format("Found {0} routes", routes.Count()) + Environment.NewLine + Environment.NewLine;
                foreach (var route in routes) {
                    foundRoutes += string.Join(" -> " + Environment.NewLine, route.GetTimeTable().Select(checkPoint => "[Ar = " + checkPoint.ArrivalTime + " Loc = " + checkPoint.Location.X + " " + checkPoint.Location.Y + "[" + checkPoint.LocationID +  "]" + " Route ID = " + route.Venicle.Id + "]")) + Environment.NewLine;
                    foundRoutes += "vehicle distance: " + route.GetTotalDistance() + Environment.NewLine;
                    foundRoutes += Environment.NewLine;
                }
                Console.WriteLine(foundRoutes);
                totalDistance += "Total distance: " + routes.Sum(r => r.GetTotalDistance()) + Environment.NewLine;
                totalDistance += Environment.NewLine;
                Console.WriteLine(totalDistance);

            } catch (ImpossibleRouteException exception) {
                String failedServeRequest = "";
                String routesWithoutRequests = ""; ;
                failedServeRequest = "Can not serve requests:" + Environment.NewLine;
                foreach (var request in exception.ImpossibleRequests) {
                    failedServeRequest += "Loc = " + request.Location.X + " " + request.Location.Y + Environment.NewLine;
                }
                failedServeRequest += Environment.NewLine;
                Console.WriteLine(failedServeRequest);

                routesWithoutRequests += "Routes without these requests:" + Environment.NewLine;
                Console.WriteLine(routesWithoutRequests);
                TryServe(Depots, vehicleInfo, requests.Except(exception.ImpossibleRequests).ToList());
            } catch (Exception exception) {
                Console.WriteLine(exception.Message);
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



        #region Overrides


        protected override string ReceivedSolvePartialProblems(SolvePartialProblems solvePartialProblems)
        {
            ///// Hey here is the problem to solve! (undivided, everything is in CommonData)
            string problem = Utilities.Base64Decode(solvePartialProblems.CommonData);

            globalProblem = problem;
            //solvePartialProblems.Id;
            NodeWorker worker = new NodeWorker(solvePartialProblems.Id);
            worker.problemObject = ProblemLoader.loadnewExampleString(globalProblem);
            worker.calculateAlgorithm();

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

        protected override string ReceivedDivideProblem(DivideProblem divideProblem)
        {
            Debug.Assert(false, "Should not be here");
            return null;
        }

        protected override string ReceivedSolutionRequest(SolutionRequest solutionRequest)
        {
            Debug.Assert(false, "Should not be here");
            return null;
        }

        protected override string ReceivedSolveRequest(SolveRequest solveRequest)
        {
            Debug.Assert(false, "Should not be here");
            return null;
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
