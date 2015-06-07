using System;
using System.IO;

namespace AL_FOR_DVRP
{
	public class AlgorithmSolution
	{
		public static int [] GASLocation;
		public static double [] GASDeliverDemand;
		public static int [] GASServiceBegin;
		public static int [] GASServiceEnd;
		public static int [] GASServiceDuration;
		public static double[,] GASDistanceMatrix;
		public static double[,] GASDriveTimeMatrix;
		public static string[] GASVechicleName;
		public static double [] GASVechicleCapacity;
		
		public static void Find_Solution(int [] Location, double [] DeliverDemand, int [] ServiceBegin, int [] ServiceEnd, int [] ServiceDuration, double [,] DistanceMatrix, double [,] DriveTimeMatrix,string [] VechicleName, double [] VechicleCapacity)
		{
			Console.WriteLine("\n" + "Please wait until a solution will be displyed on screen.");
			
			//Console.WriteLine("\n" + "Generating excel file may take several seconds.");
			
			GASLocation = Location;
			GASDeliverDemand = DeliverDemand;
			GASServiceBegin = ServiceBegin;
			GASServiceEnd = ServiceEnd;
			GASServiceDuration = ServiceDuration;
			GASDistanceMatrix = DistanceMatrix;
			GASDriveTimeMatrix = DriveTimeMatrix;
			GASVechicleName = VechicleName;
			GASVechicleCapacity = VechicleCapacity;
			
			AlgorithmSolution AS = new AlgorithmSolution();
			
			AS.Execute_Algorithm();
			
			//Console.WriteLine("\n" + "Excel file has been successfully generated and saved at location: " + Directory.GetCurrentDirectory() + @"\OutputFile");
		}
		
		internal void Execute_Algorithm()
		{

			Population.generate_initial_population();
 			
 			Chromosome c = new Chromosome();
 			
 			TerminationCriteria tc = new TerminationCriteria();
			
			while(tc.TerminationCriteriaCheck() == false)
        	{
        		c.initialize_fields();
        		c.routing_scheme();
	            c.calculate_fitness();
	        	c.Print_best_chromosome();
	        	c.Selection();
	        	c.Crossover();
	        	c.Mutation();
	        	c.Replace();
	        	tc.Increment_Generation_Completed_Byone();
        	}
        	
			c.GenerateOutputFile();
			
		}
		
	}
}





