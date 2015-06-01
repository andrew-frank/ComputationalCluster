using System;
using System.Collections.Generic;

namespace VehicleRouting
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
