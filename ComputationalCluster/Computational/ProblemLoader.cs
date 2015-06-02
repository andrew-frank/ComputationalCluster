using ComputationalCluster.Computational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputationalCluster.Client
{
    public  static class ProblemLoader
    {
    public    static  ExampleObject LoadProblem(string filename)
        { 
            string line;
            //local variables
            int n_depots = 0;
            int n_cap = 1;
            int n_visits = 1;
            int n_locations = 1;
            int n_vehicles = 1;
            int capacity = 1;
            List<int> demands = new List<int>();
            List<Point> locations = new List<Point>();
            List<int> durations = new List<int>();
            List<int> TimeWindows = new List<int>();
            List<int> TimeAvailable = new List<int>();
            List<Request> requests = new List<Request>();
            List<VehicleInfo> vehicles = new List<VehicleInfo>();
            List<Depot> depots = new List<Depot>();
            if (filename != null)
            {

                try
                {
                    System.IO.StreamReader file =
                    new System.IO.StreamReader(filename);
                    while ((line = file.ReadLine()) != null)
                    {

                        if (line.Contains("NUM_DEPOTS"))
                        {
                            n_depots = (int)line.ElementAt(line.Count() - 1) - '0';
                            int x = 0;
                        }
                        if (line.Contains("NUM_CAPACITIES"))
                        {
                            n_cap = (int)line.ElementAt(line.Count() - 1) - '0';
                        }
                        if (line.Contains("NUM_VISITS"))
                        {
                            n_visits = (int)line.ElementAt(line.Count() - 1) - '0';
                        }
                        if (line.Contains("NUM_LOCATIONS"))
                        {
                            n_locations = (int)line.ElementAt(line.Count() - 1) - '0';
                        }
                        if (line.Contains("NUM_VEHICLES"))
                        {
                            n_vehicles = (int)line.ElementAt(line.Count() - 1) - '0';
                        }

                        if (line.Contains("CAPACITIES")) {

                            capacity = (int)line.ElementAt(line.Count() - 1) - '0'; ;
                        }
                        
                        if (line.Contains("DEMAND_SECTION"))
                        {
                            for (int i = n_depots; i < n_locations; i++)
                            {
                                line = file.ReadLine();
                                string[] words = line.Split(' ');
                                demands.Add(int.Parse(words[words.Count() - 1]));
                            }
                        }
                        if (line.Contains("LOCATION_COORD_SECTION"))
                        {
                            for (int i = 0; i < n_locations; i++)
                            {
                                line = file.ReadLine();
                                string[] words = line.Split(' ');
                                Point p = new Point(int.Parse(words[3]), int.Parse(words[4]));
                                locations.Add(p);
                            }
                        }
                        if (line.Contains("DURATION_SECTION"))
                        {
                            for (int i = 0; i < n_depots; i++)
                            {
                                line = file.ReadLine();
                                string[] words = line.Split(' ');
                                durations.Add(int.Parse(words[words.Count() - 1]));
                            }
                        }
                        if (line.Contains("DEPOT_TIME_WINDOW_SECTION"))
                        {
                            for (int i = 0; i < n_depots; i++)
                            {
                                line = file.ReadLine();
                                string[] words = line.Split(' ');
                                TimeWindows.Add(int.Parse(words[words.Count() - 1]));
                            }
                        }
                        if (line.Contains("TIME_AVAIL_SECTION"))
                        {
                            for (int i = 0; i < demands.Count(); i++)
                            {
                                line = file.ReadLine();
                                string[] words = line.Split(' ');
                                TimeAvailable.Add(int.Parse(words[words.Count() - 1]));
                            }
                        }
                    }

                    for (int i = 0; i < n_depots; i++)
                    {
                        Depot z = new Depot();
                        z.Location = locations[i];
                        z.Start = 0;
                        z.End = TimeWindows[i];
                        z.Vehicles = n_vehicles;
                        depots.Add(z);
                    }

                    for (int i = n_depots; i < locations.Count; i++)
                    {                     
                        Request r = new Request();
                        r.Location = locations[i];
                        requests.Add(r);
                    }

                    for (int i = 0; i < requests.Count; i++)
                    {
                        requests[i].Start = TimeAvailable[i];
                        requests[i].End = TimeWindows[0];
                        requests[i].Id = i + 1;                       
                        requests[i].Unload = -demands[i];
                    }
                   
                    VehicleInfo v = new VehicleInfo();
                    v.Speed = 1;
                    v.Capacity = 100;
                   


                    ExampleObject Example = new ExampleObject();
                    Example.Depots = depots;
                    Example.vehicleInfo = v;
                    Example.Requests = requests;
                    return Example;


                }

                catch (Exception )
                {
                 }

            }
            ExampleObject Ex = new ExampleObject();
            return Ex;
        }


    }
}
