using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using AL_FOR_DVRP;
using ComputationalCluster.Computational;
using ComputationalCluster.Client;

public class Program
{
	static void Main(string[] args)
	{
		try
		{
			List<int> LocationList = new List<int>();
			List<double> DeliverDemandList = new List<double>();
			List<int> ServiceDurationList = new List<int>();
			List<int> ServiceBeginList = new List<int>();
			List<int> ServiceEndList = new List<int>();
			List<string> VehicleNameList = new List<string>();
			List<double> VehicleCapacityList = new List<double>();
			List<int> X_CoordinateList = new List<int>();
			List<int> Y_CoordinateList = new List<int>();

            StreamReader InputFile = new System.IO.StreamReader(Directory.GetCurrentDirectory() + @"\d.txt");

            using (InputFile)
            {
                string InputLine = InputFile.ReadLine();
                InputLine = InputFile.ReadLine();

                string[] InputData = InputLine.Split(' ');
                LocationList.Add(Convert.ToInt32(InputData[0]));

                InputLine = InputFile.ReadLine();
                InputLine = InputFile.ReadLine();

                InputData = InputLine.Split(' ');
                for (int i = 0; i < InputData.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        LocationList.Add(Convert.ToInt32(InputData[i]));
                        VehicleNameList.Add(InputData[i]);
                    }
                    else
                        DeliverDemandList.Add(-Convert.ToDouble(InputData[i], CultureInfo.InvariantCulture));
                }


                InputLine = InputFile.ReadLine();
                InputLine = InputFile.ReadLine();

                InputData = InputLine.Split(' ');
                for (int i = 0; i < InputData.Length; i++)
                {
                    if (i % 3 == 0)
                    {

                    }
                    else if (i % 3 == 1)
                    {
                        X_CoordinateList.Add(Convert.ToInt32(InputData[i]));
                    }
                    else
                    {
                        Y_CoordinateList.Add(Convert.ToInt32(InputData[i]));
                    }
                }

                InputLine = InputFile.ReadLine();
                InputLine = InputFile.ReadLine();

                InputData = InputLine.Split(' ');
                for (int i = 0; i < InputData.Length; i++)
                {
                    if (i % 2 != 0)
                        ServiceDurationList.Add(Convert.ToInt32(InputData[i]));

                }

                InputLine = InputFile.ReadLine();
                InputLine = InputFile.ReadLine();

                InputData = InputLine.Split(' ');
                ServiceBeginList.Add(Convert.ToInt32(InputData[1]));
                ServiceEndList.Add(Convert.ToInt32(InputData[2]));
                int ServiceEndDepot = Convert.ToInt32(InputData[2]);
                InputLine = InputFile.ReadLine();
                InputLine = InputFile.ReadLine();

                InputData = InputLine.Split(' ');
                for (int i = 0; i < InputData.Length; i++)
                {

                    if (i % 2 != 0)
                    {
                        const double cutoff = 0.5;
                        if (Convert.ToInt32(InputData[i]) > ServiceEndDepot * cutoff)
                            ServiceBeginList.Add(0);
                        else
                            ServiceBeginList.Add(Convert.ToInt32(InputData[i]));
                        ServiceEndList.Add(ServiceEndDepot);
                        const double AllVehicleCapacity = 100;
                        VehicleCapacityList.Add(AllVehicleCapacity);
                    }

                }

            }

            int[] Location = LocationList.ToArray(); //ilosc lokacji, po prostu id
            double[] DeliverDemand = DeliverDemandList.ToArray(); //zawsze dodatnie demand
            int[] ServiceDuration = ServiceDurationList.ToArray(); //stala 20
            int[] ServiceBegin = ServiceBeginList.ToArray();
            int[] ServiceEnd = ServiceEndList.ToArray(); //available
            string[] VehicleName = VehicleNameList.ToArray(); //depot end
            double[] VehicleCapacity = VehicleCapacityList.ToArray(); //stala 100
            int[] X_Coordinate = X_CoordinateList.ToArray(); //wszystko
            int[] Y_Coordinate = Y_CoordinateList.ToArray(); //wszystko 
            newExampleObject o = ProblemLoader.loadnewExample("problem16.vrp");
         
            //int[] Location = o.Location;
            //double[] DeliverDemand = o.DeliverDemand;
            //int[] ServiceDuration = o.ServiceDuration;
            //int[] ServiceBegin = o.ServiceBegin;
            //int[] ServiceEnd = o.ServiceEnd;
            //string[] VehicleName = o.VehicleName;
            //double[] VehicleCapacity = o.VehicleCapacity;
            //int[] X_Coordinate = o.X_Coordinate;
            //int[] Y_Coordinate = o.Y_Coordinate;


        Location = o.Location;
        DeliverDemand = o.DeliverDemand;
        ServiceDuration = o.ServiceDuration;
        ServiceBegin = o.ServiceBegin;
        ServiceEnd = o.ServiceEnd;
        VehicleName = o.VehicleName;
        VehicleCapacity = o.VehicleCapacity;
        X_Coordinate = o.X_Coordinate;
        Y_Coordinate = o.Y_Coordinate;





			double[,] DistanceMatrix = new double[Location.GetLength(0), Location.GetLength(0)];
			double[,] DriveTimeMatrix = new double[Location.GetLength(0), Location.GetLength(0)];
			
			for(int i=0;i<X_Coordinate.Length;i++)
			{
				for(int j=i;j<X_Coordinate.Length;j++)
				{
					if(i==j)
					{
						DistanceMatrix[i,j] = 0;
						DriveTimeMatrix[i,j] = 0;
						
					}
					else
					{
						DistanceMatrix[i,j]=Math.Sqrt((X_Coordinate[i] - X_Coordinate[j])*(X_Coordinate[i] - X_Coordinate[j]) + (Y_Coordinate[i] - Y_Coordinate[j])*(Y_Coordinate[i] - Y_Coordinate[j]));
						DriveTimeMatrix[i,j] = DistanceMatrix[i,j];
						DistanceMatrix[j,i] = DistanceMatrix[i,j];
						DriveTimeMatrix[j,i] = DriveTimeMatrix[i,j];
					}
					
				}
			}
			
			Console.WriteLine("\n" + "Input data files have been successfully read.");

			AlgorithmSolution.Find_Solution(Location,DeliverDemand,ServiceBegin,ServiceEnd,ServiceDuration,DistanceMatrix,DriveTimeMatrix,VehicleName,VehicleCapacity);

		}
		
		catch(FormatException)
		{
			Console.WriteLine("\n" + "Sorry, You have not entered valid integer number.");
		}
		
		catch(FileNotFoundException FNFE)
		{
			Console.WriteLine(FNFE.Message);
		}
		
		catch(IOException IOE)
		{
			Console.WriteLine(IOE.Message);
		}
		
		Console.ReadKey(true);
		
	}
}
