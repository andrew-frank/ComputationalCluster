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


        }






    }
}
