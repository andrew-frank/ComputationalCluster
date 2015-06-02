using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputationalCluster.Computational
{
    public static class ProperProblemLoader
    {
        public static ExampleObject LoadProblem(string filename)
        {
            String _name = "";
            int _num_depots = 0;
            int  num_capacities = 0;
            int _num_visits = 0;
            int _num_locations = 0;
            int _num_vehicles = 0;
            int _capacities = 0;
            String[] linesOfFile = {};
            try {
                linesOfFile = File.ReadAllLines(filename);
            } catch (Exception e) {
                Console.WriteLine("EXCEPTION: " + e.Message);
            }  
            String tempValue = "";
            String[] aboveData;
            String[] separators = { ":" };
            for (int i = 0; i < linesOfFile.Count(); i++) {
                aboveData = linesOfFile[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if(aboveData.Count() > 1)
                    tempValue = aboveData[1];
                switch (aboveData[0]) {
                    case "NAME":
                        _name = tempValue;
                        break;
                    case "NUM_DEPOTS":
                        _num_depots = Int32.Parse(tempValue);
                        break;
                    case "NUM_CAPACITIES":
                        num_capacities = Int32.Parse(tempValue);
                        break;
                    case "NUM_VISITS":
                        _num_visits = Int32.Parse(tempValue);
                        break;
                    case "NUM_LOCATIONS":
                        _num_locations = Int32.Parse(tempValue);
                        break;
                    case "NUM_VEHICLES":
                        _num_vehicles = Int32.Parse(tempValue);
                        break;
                    case "CAPACITIES":
                        _capacities = Int32.Parse(tempValue);
                        break;   
                    default:
                        continue;
                }
            }

            //Parsing DATA SECTION
            
            IList<Depot> listOfDepots = new List<Depot>();
            List<Request> listOfRequests = new List<Request>();
            
            for (int j = 0; j < linesOfFile.Count(); j++) {
                if (linesOfFile[j] == "DATA_SECTION") {
                    for(int z = 1; z <= linesOfFile.Count() - (j+1); z++){
                        switch(linesOfFile[j+z].ToString()){
                            case "DEPOTS":
                                for (int i = 1; i <= _num_depots; i++) {
                                    Depot newDepot = new Depot();
                                    newDepot.Name = linesOfFile[j + z + i].ToString();
                                    newDepot.Vehicles = _num_vehicles;
                                    listOfDepots.Add(newDepot);
                                }
                                    break;
                            case "DEMAND_SECTION":
                                    String[] demandSectionLine;
                                    String[] demandSectionSeparator = {" "};
                                    for (int i = 1; i <= _num_visits; i++) {
                                        Request newRequest = new Request();
                                        demandSectionLine = linesOfFile[j + z + i].Split(demandSectionSeparator, StringSplitOptions.RemoveEmptyEntries);
                                        newRequest.Id = Int32.Parse(demandSectionLine[0]);
                                        newRequest.Size = -Int32.Parse(demandSectionLine[1]);
                                        listOfRequests.Add(newRequest);
                                    }
                                    break;
                            case "LOCATION_COORD_SECTION":
                                String[] locationCoordSectionLine;
                                String[] locationCoordSectionSeparator = { " " };
                                for (int i = 1; i <= _num_depots; i++) {
                                    locationCoordSectionLine = linesOfFile[j + z + i].Split(locationCoordSectionSeparator, StringSplitOptions.RemoveEmptyEntries);
                                    Point depoLocation = new Point(double.Parse(locationCoordSectionLine[1]), double.Parse(locationCoordSectionLine[2]));
                                    listOfDepots.ElementAt(Int32.Parse(locationCoordSectionLine[0])).Location = depoLocation;
                                }                               
                                for (int i = (_num_depots+1); i <= _num_visits+1; i++) {
                                    locationCoordSectionLine = linesOfFile[j + z + i].Split(locationCoordSectionSeparator, StringSplitOptions.RemoveEmptyEntries);
                                    Point requestLocation = new Point(double.Parse(locationCoordSectionLine[1]), double.Parse(locationCoordSectionLine[2]));
                                    listOfRequests.ElementAt(i - 2).Location = requestLocation;
                                }
                                    break;
                            case "DEPOT_LOCATION_SECTION":
                                //fucking skip it
                                break;
                            case "VISIT_LOCATION_SECTION":
                                //no fucking clue dude
                                break;
                            case "DURATION_SECTION":
                                String[] durationSectionLine;
                                String[] durationSectionSeparator = { " " };
                                for (int i = 1; i <= _num_visits; i++) {
                                    durationSectionLine = linesOfFile[j + z + i].Split(durationSectionSeparator, StringSplitOptions.RemoveEmptyEntries);
                                    listOfRequests.ElementAt(i - 1).Unload = Int32.Parse(durationSectionLine[1]);
                                }
                                break;
                            case "DEPOT_TIME_WINDOW_SECTION":
                                String[] depotTimeWidnowSectionLine;
                                String[] depotTimeWidnowSectionSeparator = { " " };
                                int requestEnd = 0;
                                for (int i = 1; i <= _num_depots; i++) {
                                    depotTimeWidnowSectionLine = linesOfFile[j + z + i].Split(depotTimeWidnowSectionSeparator, StringSplitOptions.RemoveEmptyEntries);
                                    listOfDepots.ElementAt(i - 1).Start = 0;
                                    listOfDepots.ElementAt(i - 1).End = Int32.Parse(depotTimeWidnowSectionLine[2]);
                                    requestEnd = Int32.Parse(depotTimeWidnowSectionLine[2]);
                                }
                                for (int i = 1; i <= _num_visits; i++) {
                                    listOfRequests.ElementAt(i - 1).End = requestEnd; //tutaj chujnia, bedzie 0
                                }
                                break;
                            case "TIME_AVAIL_SECTION":
                                String[] timeAvailSectionLine;
                                String[] timeAvailSectionSeparator = { " " };
                                for (int i = 1; i <= _num_visits; i++) {
                                    timeAvailSectionLine = linesOfFile[j + z + i].Split(timeAvailSectionSeparator, StringSplitOptions.RemoveEmptyEntries);
                                    listOfRequests.ElementAt(i - 1).Start = Int32.Parse(timeAvailSectionLine[1]);
                                }
                                break;
                            default:
                                continue;
                        }
                    }                 
                }
            }
            VehicleInfo vehicleInfo = new VehicleInfo();
            vehicleInfo.Speed = 1;
            vehicleInfo.Capacity = _capacities;

            ExampleObject Example = new ExampleObject();
            Example.Depots = listOfDepots;
            Example.vehicleInfo = vehicleInfo;
            Example.Requests = listOfRequests;
            return Example;            
        }
    }
}
