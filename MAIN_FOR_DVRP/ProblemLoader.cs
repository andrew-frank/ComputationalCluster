using ComputationalCluster.Computational;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputationalCluster.Client
{
    public static class ProblemLoader
    {
     

        public static newExampleObject loadnewExample(string filename)
        {


            #region
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


            List<int> LocationList = new List<int>();
            List<double> DeliverDemandList = new List<double>();
            List<int> ServiceDurationList = new List<int>();

            List<int> ServiceBeginList = new List<int>();
            List<int> ServiceEndList = new List<int>();
            List<string> VehicleNameList = new List<string>();

            List<double> VehicleCapacityList = new List<double>();
            List<int> X_CoordinateList = new List<int>();
            List<int> Y_CoordinateList = new List<int>();
            double[,] DistanceMatrix = new Double[4,4];
            double[,] DriveTimeMatrix = new Double[4,4];
            #endregion
            if (filename != null)
            {

                try
                {
                    System.IO.StreamReader file =
                    new System.IO.StreamReader(filename);

                    String[] linesOfFile = { };
                    try
                    {
                        linesOfFile = File.ReadAllLines(filename);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("EXCEPTION: " + e.Message);
                    }  


                    String[] words;
                    String[] separators = { ":" };
              
                 //  while ((line = file.ReadLine()) != null)
                    for (int j = 0; j < linesOfFile.Count(); j++ )
                    {
                        line=linesOfFile[j];
           
                        words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        if (line.Contains("NUM_DEPOTS"))
                        {

                            n_depots = Int32.Parse(words[1]);

                        }
                        if (line.Contains("NUM_CAPACITIES"))
                        {
                            n_cap = Int32.Parse(words[1]);
                        }
                        if (line.Contains("NUM_VISITS"))
                        {
                            n_visits = Int32.Parse(words[1]);
                        }
                        if (line.Contains("NUM_LOCATIONS"))
                        {
                            n_locations = Int32.Parse(words[1]);
                        }
                        if (line.Contains("NUM_VEHICLES"))
                        {
                            n_vehicles = Int32.Parse(words[1]);
                        }

                        if (line.Contains("CAPACITIES"))
                        {

                            capacity = Int32.Parse(words[1]);
                        }

                        if (line.Contains("DEMAND_SECTION"))
                        {
                            for (int i = n_depots; i < n_locations; i++)
                            {
                              //  line = file.ReadLine();
                                j++;
                                line = linesOfFile[j];
                                string[] Words = line.Split(' ');
                                demands.Add(int.Parse(Words[Words.Count() - 1]));
                            }
                        }
                        if (line.Contains("LOCATION_COORD_SECTION"))
                        {
                            for (int i = 0; i < n_locations; i++)
                            {
                            //    line = file.ReadLine();
                                j++;
                                line = linesOfFile[j];
                                string[] Words = line.Split(' ');
                                Point p = new Point(int.Parse(Words[3]), int.Parse(Words[4]));
                                locations.Add(p);
                            }
                        }
                        if (line.Contains("DURATION_SECTION"))
                        {
                            for (int i = 0; i < n_depots; i++)
                            {
                           //     line = file.ReadLine();
                                j++;
                                line = linesOfFile[j];
                                string[] Words = line.Split(' ');
                                durations.Add(int.Parse(Words[Words.Count() - 1]));
                            }
                        }
                        if (line.Contains("DEPOT_TIME_WINDOW_SECTION")) //depot
                        {
                            for (int i = 0; i < n_depots; i++)
                            {
                             //   line = file.ReadLine();
                                j++;
                                line = linesOfFile[j];
                                string[] Words = line.Split(' ');
                                TimeWindows.Add(int.Parse(Words[Words.Count() - 1]));///
                            }
                        }
                        if (line.Contains("TIME_AVAIL_SECTION")) //klienci
                        {
                            for (int i = 0; i < demands.Count(); i++)
                            {
                              //  line = file.ReadLine();
                                j++;
                                line = linesOfFile[j];
                                string[] Words = line.Split(' ');
                                TimeAvailable.Add(int.Parse(Words[Words.Count() - 1]));
                            }
                        }
                    } //end of reading

                    for (int i = 0; i < n_locations; i++)
                    {
                        LocationList.Add(i);
                        X_CoordinateList.Add((int)locations[i].X);
                        Y_CoordinateList.Add((int)locations[i].Y);

                    }
                    for (int i = 0; i < n_vehicles; i++)
                    {
                        VehicleNameList.Add((i + 1).ToString());
                        VehicleCapacityList.Add(100);
                    }
                    for (int i = 0; i < demands.Count+1; i++)
                    {
                        ServiceDurationList.Add(20);
                        ServiceEndList.Add(TimeWindows[0]);
                       // ServiceBeginList.Add(TimeAvailable[i]);
                        if(i+1<demands.Count +1)
                        DeliverDemandList.Add(-Convert.ToDouble(demands[i], CultureInfo.InvariantCulture));
                    }


                    for (int i = 0; i < demands.Count + 1; i++)
                    {
                        if (i == 0)
                            ServiceBeginList.Add(0);
                        else
                            ServiceBeginList.Add(TimeAvailable[i-1]);
                    }

                    for (int i = 0; i < demands.Count+1; i++)
                    {
                     int ServiceEndDepot = TimeWindows[0];
                       const double cutoff = 0.5;
                    if (Convert.ToInt32(ServiceBeginList[i]) > ServiceEndDepot * cutoff)
                    ServiceBeginList[i]=0;

                    }
                    int[] Location = LocationList.ToArray(); 
                     DistanceMatrix = new double[Location.GetLength(0), Location.GetLength(0)];
                     DriveTimeMatrix = new double[Location.GetLength(0), Location.GetLength(0)];
                    int[] X_Coordinate = X_CoordinateList.ToArray();
                    int[] Y_Coordinate = Y_CoordinateList.ToArray(); 
                    for (int i = 0; i < X_Coordinate.Length; i++)
                    {
                        for (int j = i; j < X_Coordinate.Length; j++)
                        {
                            if (i == j)
                            {
                                DistanceMatrix[i, j] = 0;
                                DriveTimeMatrix[i, j] = 0;

                            }
                            else
                            {
                                DistanceMatrix[i, j] = Math.Sqrt((X_Coordinate[i] - X_Coordinate[j]) * (X_Coordinate[i] - X_Coordinate[j]) + (Y_Coordinate[i] - Y_Coordinate[j]) * (Y_Coordinate[i] - Y_Coordinate[j]));
                                DriveTimeMatrix[i, j] = DistanceMatrix[i, j];
                                DistanceMatrix[j, i] = DistanceMatrix[i, j];
                                DriveTimeMatrix[j, i] = DriveTimeMatrix[i, j];
                            }

                        }
                    }

                }

                catch (Exception a)
                { 
                }


            }
           
            newExampleObject o = new newExampleObject(LocationList.ToArray(), DeliverDemandList.ToArray(), ServiceDurationList.ToArray(),
                     ServiceBeginList.ToArray(), ServiceEndList.ToArray(), VehicleNameList.ToArray(), VehicleCapacityList.ToArray(), X_CoordinateList.ToArray(), Y_CoordinateList.ToArray(), DistanceMatrix, DriveTimeMatrix);
            return o;

        }

    }
}
