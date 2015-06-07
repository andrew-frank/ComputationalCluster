using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using AL_FOR_DVRP;

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
			
			StreamReader InputFile = new System.IO.StreamReader(Directory.GetCurrentDirectory() + @"\InputFiles\io2_9_plain_e_D.txt");
			
			using(InputFile)
			{
				string InputLine = InputFile.ReadLine();
				InputLine = InputFile.ReadLine();
				
				string[] InputData = InputLine.Split(' ');
				LocationList.Add(Convert.ToInt32(InputData[0]));
				
				InputLine = InputFile.ReadLine();
				InputLine = InputFile.ReadLine();
				
				InputData = InputLine.Split(' ');
				for(int i=0;i<InputData.Length;i++)
				{
					if(i%2==0)
					{
						LocationList.Add(Convert.ToInt32(InputData[i]));
						VehicleNameList.Add(InputData[i]);
					}
					else
						DeliverDemandList.Add(-Convert.ToDouble(InputData[i],CultureInfo.InvariantCulture));
				}
				
				
				InputLine = InputFile.ReadLine();
				InputLine = InputFile.ReadLine();
				
				InputData = InputLine.Split(' ');
				for(int i=0;i<InputData.Length;i++)
				{
					if(i%3==0)
					{
						
					}
					else if(i%3==1)
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
				for(int i=0;i<InputData.Length;i++)
				{
					if(i%2!=0)
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
				for(int i=0;i<InputData.Length;i++)
				{
					
					if(i%2!=0)
					{
						const double cutoff = 0.5;
						if(Convert.ToInt32(InputData[i])>ServiceEndDepot*cutoff)
							ServiceBeginList.Add(0);
						else
							ServiceBeginList.Add(Convert.ToInt32(InputData[i]));
						ServiceEndList.Add(ServiceEndDepot);
						const double AllVehicleCapacity = 100;
						VehicleCapacityList.Add(AllVehicleCapacity);
					}
					
				}
				
			}
			
			int [] Location = LocationList.ToArray();
			double [] DeliverDemand = DeliverDemandList.ToArray();
			int [] ServiceDuration = ServiceDurationList.ToArray();
			int [] ServiceBegin = ServiceBeginList.ToArray();
			int [] ServiceEnd = ServiceEndList.ToArray();
			string [] VehicleName = VehicleNameList.ToArray();
			double [] VehicleCapacity = VehicleCapacityList.ToArray();
			int [] X_Coordinate = X_CoordinateList.ToArray();
			int [] Y_Coordinate = Y_CoordinateList.ToArray();
			
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
