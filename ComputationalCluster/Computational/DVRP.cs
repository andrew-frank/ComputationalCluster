
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

        public static double getDistance(Point a, Point b)
        {

            double x = b.X - a.X;
            double y = b.Y - b.Y;
            double Distance = Math.Sqrt(x * x + y * y);
            return Distance;
        }

        public static void Example(List<Depot> Depots, List<DVRPClient> Clients, List<Vehicle> Vehicles, List<Point> Locations)
        {
            Depots.Clear();
            Clients.Clear();
            Vehicles.Clear();


            Depot dp = new Depot(new Point(0, 0), 0, 560);
            Depots.Add(dp);

            int unloadTime = 560;
            Point A = new Point(0, 0);
            Point B = new Point(-39, 97);
            Point C = new Point(77, -20);
            Point D = new Point(-34, -99);
            Point E = new Point(75, -43);
            Point F = new Point(87, -66);
            Point G = new Point(-55, 86);
            Point H = new Point(-93, -3);
            Locations.Add(A); Locations.Add(B); Locations.Add(C); Locations.Add(D); Locations.Add(E); Locations.Add(F); Locations.Add(G); Locations.Add(H);

            for (int i = 0; i < 8; i++)
            {
                Vehicle V = new Vehicle(1, 1);
                Vehicles.Add(V);
            }
            foreach (Point L in Locations)
            {
                DVRPClient client = new DVRPClient(L, unloadTime);
                Clients.Add(client);
            }
        }



        public static double Solve(List<DVRPClient> Clients, List<Depot> Depots)
        {

            for (int i = 0; i < Clients.Count; i++)
            {
                Clients[i].ID = i + 1;
            }
            double length = 1000000;

            do
            {
                double MinLength;
                MinLength = TotalDistance(Clients, Depots);
                if (MinLength < length)
                {
                    length = MinLength;
                }
  
            } while (next_permutation(Clients));

            return length;
        }

        private static double TotalDistance(List<DVRPClient> Clients, List<Depot> Depots)
        {

            double length = 10000;
            double time = 0;
            for (int i = 0; i < Depots.Count; i++)
            {
                double minlength = getDistance(Depots[i].Location, Clients[0].Location);
                if (minlength < length)
                    length = minlength;

            }
            length = getDistance(Depots[0].Location, Clients[0].Location);
            time += Clients[0].UnloadTime;

            for (int i = 0; i < Clients.Count() - 1; i++) //przez wszystkie 
            {
                length += getDistance(Clients[i].Location, Clients[i + 1].Location);
                time += Clients[i].UnloadTime;
            }

            length += getDistance(Clients[Clients.Count - 1].Location, Depots[0].Location);
            return length;
        }

        // https://42bits.wordpress.com/2010/04/12/generating_all_possible_permutations_of_a_sequence/
        public static bool next_permutation(List<DVRPClient> Clients)
        {
            int i, j, l;
            for (j = Clients.Count - 2; j >= 0; j--) //get maximum index j for which arr[j+1] > arr[j]
                if (Clients[j + 1].ID > Clients[j].ID)
                    break;
            if (j == -1) //has reached it's lexicographic maximum value, No more permutations left 
                return false;

            for (l = Clients.Count - 1; l > j; l--) //get maximum index l for which arr[l] > arr[j]
                if (Clients[l].ID > Clients[j].ID)
                    break;

            DVRPClient swap = Clients[j]; //Swap arr[i],arr[j] 
            Clients[j] = Clients[l];
            Clients[l] = swap;

            for (i = j + 1; i < Clients.Count; i++) //reverse array present after index : j+1 
            {
                if (i > Clients.Count - i + j)
                    break;
                swap = Clients[i];
                Clients[i] = Clients[Clients.Count - i + j];
                Clients[Clients.Count - i + j] = swap;
            }
            return true;
        }

  
    }


}
