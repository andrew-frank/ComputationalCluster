using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace ComputationalCluster.Computational
{
  



    
    public class DVRP
    {
        public List<Depot> Depots;
        public List<DVRPClient> Clients;
        public List<Vehicle> Vehicles;
        public List<Point> Locations;
        public double getDistance(Point a, Point b)
        {

            double x= b.X - a.X;
            double y = b.Y - b.Y;
            double Distance = Math.Sqrt(x * x + y * y);
            return Distance;
        }

        public void Example()
        {
          Depots.Clear();
            Clients.Clear();
            Vehicles.Clear();

            Depot dp= new Depot(new Point (0,0), 0, 560);
            Depots.Add(dp);

            Point A = new Point(0, 0);
            Point B = new Point(-39, 97);
            Point C = new Point(77, -20);
            Point D = new Point(-34, -99);
            Point E = new Point(75, -43);
            Point F = new Point(87, -66);
            Point G = new Point(-55, 86);
            Point H = new Point(-93, -3);
            Locations.Add(A); Locations.Add(B); Locations.Add(C); Locations.Add(D); Locations.Add(E); Locations.Add(F); Locations.Add(G); Locations.Add(H);

            for(int i=0; i<8; i++)
            {
                Vehicle V = new Vehicle(1,1);
                Vehicles.Add(V);
            }
            foreach(Point L in Locations)
            {
                DVRPClient client = new DVRPClient(L);
                Clients.Add(client);
            }
        }

        

        public void Solve()
        {
            List<int> ClientIds = new List<int>();
            for (int i = 0; i < Clients.Count; i++)
                ClientIds.Add(i+1);

            while(true)
            {

            }



        }

        private double TotalDistance(List<int> Ids, List<Point> locations, double length, Depot D)
        {
            length = 0;
            length = getDistance(D.Location, locations[0]);
            for (int i = 0; i < locations.Count() - 1; i++) //przez wszystkie 
            {
                length += getDistance(locations[i], locations[i + 1]);
            }

            length += getDistance(locations[locations.Count - 1], D.Location);
            return length;
        }


    }
}
