using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.Shared.Utilities;
using ComputationalCluster.Shared.Messages.SolveRequestNamespace;
using ComputationalCluster.Shared.Messages.DivideProblemNamespace;
using ComputationalCluster.Shared.Messages.RegisterNamespace;
using System.Threading;
using ComputationalCluster.Shared.Messages.StatusNamespace;
using ComputationalCluster.Shared.Connection;
using System.Diagnostics;
using ComputationalCluster.Misc;
using ComputationalCluster.Shared.Messages.SolveRequestResponseNamespace;
using ComputationalCluster.Client;
using ComputationalCluster.Computational;

namespace ComputationalCluster.Nodes
{
    public sealed class Client : Node
    {
        #region Public

        public Client()
        {
            this.CommonInit();
        }

        private void CommonInit()
        {
            this.NodeType = NodeType.Client;
        }


        public void startInstance(UInt16 port, IPAddress server)
        {
            this.Port = port;
            this.IP = server;
            Console.WriteLine("Client Started");

            Console.Write("Debug? [y/n] \n>");
            string debug = Console.ReadLine();

            if (debug == "n") {
                this.Port = 0;
                port = 0;
                this.HostName = "";
            }

            while (port == 0) {
                Console.WriteLine(" Parameters Syntax: [-address [IPv4 address or IPv6 address or host name]] [-port[port number]]");
                Console.Write("> ");

                String parameters;
                parameters = Console.ReadLine();
                parameters = parameters.Replace(" ", string.Empty);
                //Shared.Connection.ConnectionHelpers.CheckInputSyntax(parameters, port, hostName);
            }

            Console.WriteLine("Specify name of the problem file");
            String filename = Console.ReadLine();

            //ExampleObject exampleFromFile = ProblemLoader.LoadProblem(filename + ".vrp");

            ExampleObject properExample = ProperProblemLoader.LoadProblem(filename + ".vrp");
            //ALGORITHM//
            var watch = Stopwatch.StartNew();
            TryServe(properExample.Depots, properExample.vehicleInfo, properExample.Requests);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Algorith executed in: " + elapsedMs + "ms");         
            
            Console.ReadLine();
            //string problem = System.IO.File.ReadAllText(filename);
     

            //byte[] base64Problem = problem.Base64Encode();

            //SolveRequest request = new SolveRequest();
            //request.Data = base64Problem;
            //request.ProblemType = Utilities.ProblemNameForType(ProblemType.DVRP);
            //request.SolvingTimeoutSpecified = false;
            //request.IdSpecified = false;
        }
        //
        private void TryServe(IList<Depot> Depots, VehicleInfo vehicleInfo, List<Request> requests)
        {
            
            try {
                var builder = new RouteBuilder(vehicleInfo);
                var routes = builder.Build(Depots, vehicleInfo, requests).ToList();

                String foundRoutes = "";
                String totalDistance =  "";
                foundRoutes += string.Format("Found {0} routes", routes.Count()) + Environment.NewLine + Environment.NewLine;
                foreach (var route in routes) {
                    foundRoutes += string.Join(" -> " + Environment.NewLine, route.GetTimeTable().Select(checkPoint => "[Ar = " + checkPoint.ArrivalTime + " Loc = " + checkPoint.Location.X + " " + checkPoint.Location.Y + "]")) + Environment.NewLine;
                    foundRoutes += "vehicle distance: " + route.GetTotalDistance() + Environment.NewLine;
                    foundRoutes += Environment.NewLine;
                }
                Console.WriteLine(foundRoutes);
                totalDistance += "Total distance: " + routes.Sum(r => r.GetTotalDistance()) + Environment.NewLine;
                totalDistance += Environment.NewLine;
                Console.WriteLine(totalDistance);

            } catch (ImpossibleRouteException exception) {
                String failedServeRequest = "";
                String routesWithoutRequests = "";;
                failedServeRequest = "Can not serve requests:" + Environment.NewLine;
                foreach (var request in exception.ImpossibleRequests) {
                    failedServeRequest += "Loc = " + request.Location.X + " " + request.Location.Y + Environment.NewLine;
                }
                failedServeRequest += Environment.NewLine;
                Console.WriteLine(failedServeRequest);

                routesWithoutRequests += "Routes without these requests:" + Environment.NewLine;
                Console.WriteLine(routesWithoutRequests);
                TryServe(Depots, vehicleInfo, requests.Except(exception.ImpossibleRequests).ToList());
            } catch (Exception exception) {
                Console.WriteLine(exception.Message);
            }
        }

        
        #endregion

        #region Overrides

        //Invalid:
        protected override Status CurrentStatus()
        {
            throw new Exception("Client does not implement CurrentStatus()");
        }

        protected override Register GenerateRegister()
        {
            throw new Exception("Client does not implement GenerateRegister()");
        }

        #endregion


        #region Private

        private void SendSolveRequest(SolveRequest solveRequest)
        {
            string message = solveRequest.SerializeToXML();
            string res = CMSocket.Instance.SendMessage(this.Port, this.IP, message);
            Object obj = res.DeserializeXML();
            if (!(obj is SolveRequestResponse))
                throw new Exception("Wrong type");

            SolveRequestResponse response = (SolveRequestResponse)obj;
            if (response.IdSpecified) {
                ulong id = response.Id;
            }
        }

        #endregion
    }

}
