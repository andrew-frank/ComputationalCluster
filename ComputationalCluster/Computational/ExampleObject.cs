using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Computational
{
    public class ExampleObject {
        public   IList<Depot> Depots = new List<Depot>();
        public   VehicleInfo vehicleInfo = new VehicleInfo();
        public   List<Request> Requests     = new List<Request>();
    }
}