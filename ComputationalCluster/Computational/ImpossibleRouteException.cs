using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Computational
{
    public class ImpossibleRouteException : ApplicationException
    {
        public ImpossibleRouteException(string message, IList<Request> impossibleRequests)
            : base(message)
        {
            ImpossibleRequests = impossibleRequests;
        }

        public IList<Request> ImpossibleRequests { get; private set; }
    }
}
