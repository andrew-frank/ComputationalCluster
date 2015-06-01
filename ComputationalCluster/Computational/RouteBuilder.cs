using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Computational
{
    public class RouteBuilder
    {
        private readonly VenicleInfo _venicleInfo;

        public RouteBuilder(VenicleInfo venicleInfo)
        {
            _venicleInfo = venicleInfo;
        }

        public IList<Route> Build(IList<Deport> deports, VenicleInfo venicleInfo, List<Request> requests)
        {
            BruteForceCounter.MaximumOption = 0;
            BruteForceCounter.CurrentOption = 0;
            BruteForceCounter.MinPathDistance = 0;
            BruteForceCounter.MinPath = null;

            while (true)
            {
                var routes = new List<Route>();
                var unservicedRequests = requests;
                var venicles = CreateVenicles(deports);
                foreach (var venicle in venicles)
                {
                    var route = BuildRoute(venicle, unservicedRequests);
                    if (route == null)
                        continue;
                    unservicedRequests = unservicedRequests.Except(route.Requests).ToList();
                    routes.Add(route);
                }

                BruteForceCounter.PossibleRequests = BruteForceCounter.PossibleRequests.Concat(routes.SelectMany(r => r.Requests)).ToList();

                if (unservicedRequests.Count() == requests.Count())
                    if (BruteForceCounter.MinPath == null)
                        throw new ImpossibleRouteException("Can service all requests", requests.Except(BruteForceCounter.PossibleRequests).ToList());
                    else
                        return BruteForceCounter.MinPath;

                BruteForceCounter.MaximumOption++;
                BruteForceCounter.CurrentOption = 0;
                var totalDistance = routes.Sum(r => r.GetTotalDistance());
                if (unservicedRequests.Any() || !(BruteForceCounter.MinPathDistance > totalDistance || BruteForceCounter.MinPath == null))
                    continue;
                BruteForceCounter.MinPathDistance = totalDistance;
                BruteForceCounter.MinPath = routes;
            }
        }

        //private static IEnumerable<Venicle> CreateVenicles(IEnumerable<Deport> deports)
        public static IEnumerable<Venicle> CreateVenicles(IEnumerable<Deport> deports)
        {
            var result = new List<Venicle>();
            var id = 1;
            foreach (var deport in deports)
            {
                for (var i = 0; i < deport.Venicles; i++)
                {
                    result.Add(new Venicle
                    {
                        Id = id++,
                        Deport = deport
                    });
                }
            }
            return result;
        }

        private Route BuildRoute(Venicle venicle, List<Request> requests)
        {
            if (!requests.Any())
                return null;

            var result = new Route(_venicleInfo) { Venicle = venicle };
            foreach (var request in requests)
            {
                if (result.IsCanAdd(request))
                {
                    if (BruteForceCounter.CurrentOption == BruteForceCounter.MaximumOption)
                    {
                        result.Requests.Add(request);
                    }
                    else
                    {
                        BruteForceCounter.CurrentOption++;
                    }
                }
            }
            return result.Requests.Any() ? result : null;
        }
    }
}
