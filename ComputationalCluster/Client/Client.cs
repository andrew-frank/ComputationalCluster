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
using System.IO;
using ComputationalCluster.Shared.Messages.SolvePartialProblemsNamespace;
using ComputationalCluster.Shared.Messages.SolutionRequestNamespace;
using ComputationalCluster.Shared.Messages.SolutionsNamespace;

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
                while (port == 0) {
                    Console.WriteLine(" Parameters Syntax: [-address [IPv4 address or IPv6 address or host name]] [-port[port number]]");
                    Console.Write("> ");

                    String parameters;
                    parameters = Console.ReadLine();
                    parameters = parameters.Replace(" ", string.Empty);
                    //Shared.Connection.ConnectionHelpers.CheckInputSyntax(parameters, port, hostName);
                }

            }

            while (true) {
                Console.WriteLine("Possible actions:\n1.Send problem\n2.Send multiple problems from folder\n3.Check for solution>");
                string str = Console.ReadLine();
                ulong number = 0;
                if (ulong.TryParse(str, out number)) {
                    switch (number) {
                        case 1:                           
                            Console.WriteLine("Specify name of the problem:\n>");
                            String fileName = Console.ReadLine();
                            String problem = File.ReadAllText(fileName + ".vrp");
                            List<String> singleProblemList = new List<string>();
                            singleProblemList.Add(problem);
                            Console.WriteLine("Sending example problem");
                            this.SendExampleProblem(singleProblemList);
                            break;
                        case 2:
                            Console.WriteLine("Specify name of the folder with problems:\n>");                            
                            String folderName = Console.ReadLine();
                            //String folderPath = "C:/PW/VI SEMESTR/SE2/SE2_Project/ComputationalCluster/bin/Debug/" + folderName;
                            String folderPath = AppDomain.CurrentDomain.BaseDirectory + folderName;
                            try {
                                String[] filePaths = Directory.GetFiles(@folderPath, "*.vrp");
                                List<String> problems = new List<String>();
                                foreach (String s in filePaths) {                                    
                                    problems.Add(File.ReadAllText(s));
                                }
                                Console.WriteLine("Sending example problems");
                                this.SendExampleProblem(problems);
                            } catch (System.IO.DirectoryNotFoundException e) {
                                Console.WriteLine("Folder not found\n");
                            }                         
                            break;
                        case 3:
                            Console.WriteLine("Specify ID of the problem:\n>");
                            str = Console.ReadLine();
                            number = 0;
                            if (ulong.TryParse(str, out number)) {

                            }
                            break;
                    }
                } else {
                    Console.WriteLine("Unrecognized action");
                }
            }

            Console.WriteLine("Specify name of the problem file");
            String problemContent = System.IO.File.ReadAllText(Console.ReadLine() + ".vrp");  
        }

        private void PrintMenuToConsole()
        {
            Console.WriteLine("Please define the operation you want to proceed:");
            Console.WriteLine("1 - load problem");
            Console.WriteLine("2 - check status of the problem");           
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


        #region Overrides


        protected override string ReceivedSolutions(Solutions solution)
        {
            Console.WriteLine("\n" + solution.SerializeToXML());
            return null;
        }

        protected override string ReceivedSolveRequestResponse(SolveRequestResponse solveRequestResponse)
        {
            Console.WriteLine("\n"+solveRequestResponse.SerializeToXML());
            return null;
        }

        protected override string ReceivedRegister(Register register, IPAddress senderAddr)
        {
            Debug.Assert(false, "Should not be here");
            return null;
        }

        protected override string ReceivedStatus(Status status)
        {
            Debug.Assert(false, "Should not be here");
            return null;
        }

        protected override string ReceivedDivideProblem(DivideProblem divideProblem)
        {
            Debug.Assert(false, "Should not be here"); 
            return null;
        }

        protected override string ReceivedSolutionRequest(SolutionRequest solutionRequest)
        {
            Debug.Assert(false, "Should not be here");
            return null;
        }

        protected override string ReceivedSolvePartialProblems(SolvePartialProblems solvePartialProblems)
        {
            Debug.Assert(false, "Should not be here");
            return null;
        }

        protected override string ReceivedSolveRequest(SolveRequest solveRequest)
        {
            Debug.Assert(false, "Should not be here");
            return null;
        }

        #endregion

        #endregion


        #region Private


        private void CheckForSolution(ulong problemID)
        {
            //TODO:
            Console.WriteLine("Unimplemented");
        }


        private void SendExampleProblem(List<String> problems)
        {
            SolveRequest solveRequest = new SolveRequest();
            //String problem = "VRPTEST io2_8a\r\nCOMMENT: \r\nNAME: io2_8a\r\nNUM_DEPOTS: 1\r\nNUM_CAPACITIES: 1\r\nNUM_VISITS: 8\r\nNUM_LOCATIONS: 9\r\nNUM_VEHICLES: 8\r\nCAPACITIES: 100\r\nDATA_SECTION\r\nDEPOTS\r\n  0\r\nDEMAND_SECTION\r\n  1 -29\n  2 -21\n  3 -28\n  4 -20\n  5 -8\n  6 -31\n  7 -13\n  8 -29\nLOCATION_COORD_SECTION\r\n  0 0 0\r\n  1 -39 97\n  2 34 -45\n  3 77 -20\n  4 -34 -99\n  5 75 -43\n  6 87 -66\n  7 -55 86\n  8 -93 -3\nDEPOT_LOCATION_SECTION\r\n  0 0\r\nVISIT_LOCATION_SECTION\r\n  1 1\n  2 2\n  3 3\n  4 4\n  5 5\n  6 6\n  7 7\n  8 8\nDURATION_SECTION\r\n  1 20\n  2 20\n  3 20\n  4 20\n  5 20\n  6 20\n  7 20\n  8 20\nDEPOT_TIME_WINDOW_SECTION\r\n  0 0 560\r\nCOMMENT: TIMESTEP: 7\r\nTIME_AVAIL_SECTION\r\n  1 276\n  2 451\n  3 171\n  4 365\n  5 479\n  6 546\n  7 376\n  8 289\nEOF";
            //String problem = File.ReadAllText("problem.vrp");
            foreach (String singleProblem in problems) {
                var base64 = singleProblem.Base64Encode();
                solveRequest.Data = base64;
                solveRequest.ProblemType = Utilities.ProblemNameForType(ProblemType.DVRP);

                this.SendSolveRequest(solveRequest);
            }
            
        }


        private void SendSolveRequest(SolveRequest solveRequest)
        {
            String message = solveRequest.SerializeToXML();
            CMSocket.Instance.SendMessage(this.Port, this.IP, message, this);

            //nowy comment
            //Object obj = response.DeserializeXML();
            //Debug.Assert(obj is SolveRequestResponse, "Wrong response object");
            //SolveRequestResponse requestResopnse = (SolveRequestResponse)obj;
            //Debug.Assert(requestResopnse.IdSpecified, "No ID in solve request response");
            //if (!requestResopnse.IdSpecified)
            //    return;
            //Console.WriteLine("ID of the problem" + requestResopnse.Id);


            //stary comment
            //string message = solveRequest.SerializeToXML();
            //string res = CMSocket.Instance.SendMessage(this.Port, this.IP, message, this);
            //Object obj = res.DeserializeXML();
            //if (!(obj is SolveRequestResponse))
            //    throw new Exception("Wrong type");

            //SolveRequestResponse response = (SolveRequestResponse)obj;
            //if (response.IdSpecified) {
            //    ulong id = response.Id;
            //}
        }

        #endregion
    }

}
