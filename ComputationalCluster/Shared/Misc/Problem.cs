using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Misc {

    public enum ProblemType
    {
        DVRP = 0
    }

    public class Problem 
    {
        public ProblemType Type { get; protected set; }
    }
}