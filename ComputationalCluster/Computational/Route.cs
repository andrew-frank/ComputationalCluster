using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputationalCluster.Computational
{
    public class Route
    {
        private readonly VenicleInfo _venicleInfo;

        public Route(VenicleInfo venicleInfo)
        {
            _venicleInfo = venicleInfo;
            Requests = new List<Request>();
        }

        public Venicle Venicle { get; set; }

        public IList<Request> Requests { get; set; }

        public double GetSize()
        {
            return Requests.Sum(r => r.Size);
        }

        public bool IsCanAdd(Request request)
        {
            if (GetSize() + request.Size > _venicleInfo.Capacity)
                return false;

            var tempRequests = Requests.ToList();
            tempRequests.Add(request);
            var timeTable = GetTimeTable(tempRequests).ToList();
            var lastRequest = timeTable[timeTable.Count() - 2];

            if (Math.Max(lastRequest.ArrivalTime, request.Start) + request.Unload > request.End)
                return false;

            return timeTable.Last().ArrivalTime < Venicle.Deport.End;
        }

        public IEnumerable<CheckPoint> GetTimeTable()
        {
            return GetTimeTable(Requests);
        }

        private IEnumerable<CheckPoint> GetTimeTable(IEnumerable<Request> requests)
        {
            var result = new List<CheckPoint>
            {
                new CheckPoint
                {
                    ArrivalTime = 0,
                    Location = Venicle.Deport.Location
                }
            };
            double totalTime = Venicle.Deport.Start;
            foreach (var request in requests)
            {
                totalTime += CalculateDistance(result.Last().Location, request.Location) / _venicleInfo.Speed;
                result.Add(new CheckPoint
                {
                    ArrivalTime = totalTime,
                    Location = request.Location
                });
                totalTime = Math.Max(totalTime, request.Start) + request.Unload;
            }

            totalTime += CalculateDistance(result.Last().Location, Venicle.Deport.Location) / _venicleInfo.Speed;
            result.Add(new CheckPoint
            {
                ArrivalTime = totalTime,
                Location = Venicle.Deport.Location
            });
            return result;
        }

        public double GetTotalDistance()
        {
            var path = GetTimeTable().ToList();

            if (path.Count() < 2)
                return 0;

            double totalDistance = 0;
            var lastPoint = path[0].Location;
            for (var i = 1; i < path.Count(); i++)
            {
                totalDistance += CalculateDistance(lastPoint, path[i].Location);
                lastPoint = path[i].Location;
            }
            return totalDistance;
        }

        private static double CalculateDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public class CheckPoint
        {
            public double ArrivalTime { get; set; }

            public Point Location { get; set; }
        }
    }
}
