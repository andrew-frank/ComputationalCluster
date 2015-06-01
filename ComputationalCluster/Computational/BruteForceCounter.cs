using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Computational
{
    class BruteForceCounter
    {
        static BruteForceCounter()
        {
            PossibleRequests = new List<Request>();
        }

        public static IList<Route> MinPath { get; set; }

        public static double MinPathDistance { get; set; }

        public static int MaximumOption { get; set; }

        public static int CurrentOption { get; set; }

        public static IList<Request> PossibleRequests { get; set; }
    }
}
