using AL_FOR_DVRP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Computational
{
    public class NodeWorker
    {
        #region Properties/ivars

        private bool _state = false;
        private ulong _howLong;
        private ulong _problemInstanceId;
        private ulong _taskId;
        private string _problemType;

        /// 0 = idle, 1 = busy
        public bool State { get { return _state; } }
        public string StateString {
            get {
                switch (this.State) {
                    case false: return "Idle";
                    case true: return "Busy";
                }
                return null;
            }
        }

        public ulong HowLong { get { return _howLong; } }
        public ulong ProblemInstanceId { get { return _problemInstanceId; } }
        public ulong TaskId { get { return _taskId; } }
        public string ProblemType { get { return _problemType; } }

        public newExampleObject problemObject { get; set; }

        #endregion

        public NodeWorker(ulong id)
        {
            this._taskId = id;
        }


        public double calculateAlgorithm()
        {
            _problemType= "DVRP";

            double shortestPath = 0;
            var watch = Stopwatch.StartNew();
            if (watch.IsRunning)
                Console.WriteLine("Stopwatch running");
            else Console.WriteLine("Stopwatch NOT RUNNING");
            this._problemInstanceId = this._taskId;
            
            AlgorithmSolution.Find_Solution(problemObject.Location, problemObject.DeliverDemand, problemObject.ServiceBegin, 
                problemObject.ServiceEnd, problemObject.ServiceDuration, problemObject.DistanceMatrix, 
                problemObject.DriveTimeMatrix, problemObject.VehicleName, problemObject.VehicleCapacity);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            _howLong = (ulong)elapsedMs;
            Console.WriteLine("Finished!\a\nTime = " + watch.Elapsed);
            
            return shortestPath;
        }



        //thread stuff here

    }
}
