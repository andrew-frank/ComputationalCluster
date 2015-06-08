using System;
using System.Linq;
//using Microsoft.Office.Interop.Excel;
//using System.Reflection;
//using System.IO;
//using System.Text;
//using System.Drawing;
using System.Globalization;

namespace AL_FOR_DVRP
{
	
	internal class Chromosome
	{
		
		private readonly int[,] route_population = new int[Population.population_size,AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly int[,] vehicle_allocated = new int[Population.population_size, AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly int[,] route_end_index = new int[Population.population_size,AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly double[,] arrival_at_customer = new double[Population.population_size,AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly double[,] ArrivalAtDepot = new double[Population.population_size,AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly double[,] actual_loading = new double[Population.population_size,AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly double[,] drivetime_to_customer = new double[Population.population_size,AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly double[,] DrivetimeToDepot = new double[Population.population_size,AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly double[,] distance_to_customer = new double[Population.population_size,AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly double[,] DistanceToDepot = new double[Population.population_size,AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly double[,] total_route_drivetime = new double[Population.population_size,AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly double[,] total_route_distance = new double[Population.population_size,AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly int[] NumberOfVehiclesUsed = new int[Population.population_size];
		private readonly double[] total_distance = new double[Population.population_size];
		private readonly double max_auto_created_vehicle_capacity = AlgorithmSolution.GASVechicleCapacity[0];
		private readonly double[] fitness = new double[Population.population_size];
		private double sum_fitness;
		private readonly Random random = new Random();
		private double random_number;
		private readonly int[] parent_selection = new int[Population.population_size];
		private readonly int[,] offspring1 = new int[Population.population_size/2,Population.population.GetLength(1)];
		private readonly int[,] offspring2 = new int[Population.population_size/2,Population.population.GetLength(1)];
		private readonly int[,] store_best_route_population = new int[TerminationCriteria.max_generation,AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly int[,] store_best_route_end_index = new int[TerminationCriteria.max_generation,AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly double[,] store_best_total_route_distance = new double[TerminationCriteria.max_generation,AlgorithmSolution.GASLocation.GetLength(0)-1];
		private readonly double [] store_best_total_distance = new double [TerminationCriteria.max_generation];
		private readonly double [] store_best_fitness = new double[TerminationCriteria.max_generation];
		
		internal void initialize_fields()
		{
			int i,j;
			
			sum_fitness = 0;
			random_number = 0;
			
			for(i=0;i<Population.population_size;i++)
			{
				
				NumberOfVehiclesUsed[i] = 0;
				total_distance[i] = 0;
				
				for(j=0;j<AlgorithmSolution.GASLocation.GetLength(0)-1;j++)
				{
					route_population[i,j] = 0;
					vehicle_allocated[i,j] = 0;
					route_end_index[i,j] = 0;
					arrival_at_customer[i,j] = 0;
					ArrivalAtDepot[i,j] = 0;
					actual_loading[i,j] = 0;
					total_route_drivetime[i,j] = 0;
					total_route_distance[i,j] = 0;
					drivetime_to_customer[i,j] = 0;
					DrivetimeToDepot[i,j] = 0;
					distance_to_customer[i,j] = 0;
					DistanceToDepot[i,j] = 0;
				}
			}
			
			for(i=0;i<Population.population_size;i++)
			{
				fitness[i] = 0;
				parent_selection[i] = 0;
			}
			
			for(i=0;i<Population.population_size/2;i++)
			{
				for(j=0;j<Population.population.GetLength(1);j++)
				{
					offspring1[i,j]=0;
					offspring2[i,j]=0;
				}
			}
		}
		
		internal void routing_scheme()
		{
			for(int i=0;i<Population.population_size;i++)
			{
				double DriveTime = 0;
				double TotalRouteDriveTime = 0;
				double Distance = 0;
				double TotalRouteDistance = 0;
				double Arrival = 0;
				double Previous_Arrival = 0;
				double Departure = 0;
				double DriveTime_to_depot = 0;
				double Distance_to_depot = 0;
				double Departure_to_depot = 0;
				double Arrival_at_depot=0;
				double RouteDeliveryDemand = 0;
				bool current_customer_found = false;
				int y = 0;
				int p = 0;
				int route = 1;
				int old_route = 1;
				int new_route = 0;
				double [] GASVechicleCapacityCopy = AlgorithmSolution.GASVechicleCapacity.ToArray();
				double MaxCapacityOfAvailableVehicles = GASVechicleCapacityCopy[0];
				int auto_created_vehicle_start_index = GASVechicleCapacityCopy.GetLength(0);
				
				for(int j=0;j<Population.population.GetLength(1);j++)
				{
					
					int current_customer=Population.population[i,j];
					
					for(int x=0;x<Population.population.GetLength(1);x++)
					{
						if(route_population[i,x] == current_customer)
						{
							current_customer_found = true;
						}
					}
					
					if(current_customer_found == true)
					{
						current_customer_found = false;
						
						if(j==Population.population.GetLength(1)-1)
							
						{
							old_route = route;
							if(route > AlgorithmSolution.GASVechicleCapacity.GetLength(0))
							{
								vehicle_allocated[i,auto_created_vehicle_start_index++] = route++;
							}
							else
							{
								int AllVechicleCapacityValueGreater = 0;
								for(int z=0;z<GASVechicleCapacityCopy.GetLength(0);z++)
								{
									
									if(GASVechicleCapacityCopy[z].Equals(-1) || GASVechicleCapacityCopy[z] > RouteDeliveryDemand)
									{
										AllVechicleCapacityValueGreater++;
										continue;
									}
									
									else if(GASVechicleCapacityCopy[z].Equals(RouteDeliveryDemand))
									{
										vehicle_allocated[i,z] = route++;
										GASVechicleCapacityCopy[z] = -1;
										break;
									}
									
									else if(GASVechicleCapacityCopy[z] < RouteDeliveryDemand)
									{
										
										for(int r=z-1;r>-1;r--)
										{
											if(GASVechicleCapacityCopy[r] > RouteDeliveryDemand)
											{
												vehicle_allocated[i,r] = route++;
												GASVechicleCapacityCopy[r] = -1;
												break;
											}
										}
										break;
									}
								}
								
								if(AllVechicleCapacityValueGreater == GASVechicleCapacityCopy.GetLength(0))
								{
									for(int d= GASVechicleCapacityCopy.GetLength(0)-1;d>-1;d--)
									{
										if(GASVechicleCapacityCopy[d].Equals(-1))
										{
											continue;
										}
										else
										{
											vehicle_allocated[i,d] = route++;
											GASVechicleCapacityCopy[d] = -1;
											break;
										}
										
									}
								}
								
								MaxCapacityOfAvailableVehicles = GASVechicleCapacityCopy.Max();
								
								if(route > AlgorithmSolution.GASVechicleCapacity.GetLength(0))
								{
									MaxCapacityOfAvailableVehicles = max_auto_created_vehicle_capacity;
								}
							}
							if(old_route == route)
							{
								Console.WriteLine("\n");
								Console.WriteLine("1:    {0}    {1}", MaxCapacityOfAvailableVehicles,RouteDeliveryDemand);
								Console.WriteLine("\n");
							}
							
							new_route=1;
							
							actual_loading[i,p] = RouteDeliveryDemand;
							
							DistanceToDepot[i,p] = Distance_to_depot;
							
							DrivetimeToDepot[i,p] = DriveTime_to_depot;
							
							TotalRouteDistance = TotalRouteDistance + Distance_to_depot;
							
							TotalRouteDriveTime = TotalRouteDriveTime + DriveTime_to_depot;
							
							total_route_distance[i,p] = TotalRouteDistance;
							
							total_route_drivetime[i,p] = TotalRouteDriveTime;
							
							ArrivalAtDepot[i,p] = Arrival_at_depot;
							
							route_end_index[i,p++] = y-1;
							
							j=-1;
						}
						
						continue;
						
					}
					
					if(j==0||new_route==1)
					{
						DriveTime=0;
						TotalRouteDriveTime=0;
						Distance=0;
						TotalRouteDistance = 0;
						Arrival=0;
						Departure=0;
						RouteDeliveryDemand=0;
						
						Distance = AlgorithmSolution.GASDistanceMatrix[0, current_customer];
						DriveTime = AlgorithmSolution.GASDriveTimeMatrix[0, current_customer];
						Arrival = AlgorithmSolution.GASServiceBegin[0] + DriveTime;
						if(Arrival<AlgorithmSolution.GASServiceBegin[current_customer])
						{
							Departure = AlgorithmSolution.GASServiceBegin[current_customer] + AlgorithmSolution.GASServiceDuration[current_customer-1];
						}
						else
						{
							Departure = Arrival + AlgorithmSolution.GASServiceDuration[current_customer-1];
						}
						RouteDeliveryDemand = AlgorithmSolution.GASDeliverDemand[current_customer-1];
						
						if(Arrival>AlgorithmSolution.GASServiceEnd[current_customer])
						{
							Console.WriteLine("\n" + "ServiceEnd time of {0} customer is too short.",AlgorithmSolution.GASLocation[current_customer]);
							Console.WriteLine("\nPress any key to exit application.");
							Console.ReadKey(true);
							Environment.Exit(1);
						}
						
						
						
						else if(AlgorithmSolution.GASDeliverDemand[current_customer-1]>MaxCapacityOfAvailableVehicles)
						{
							if(!MaxCapacityOfAvailableVehicles.Equals(-1) && route <= AlgorithmSolution.GASVechicleCapacity.GetLength(0))
							{
								Console.WriteLine("Delivery demand of {0} customer is too high",AlgorithmSolution.GASLocation[current_customer]);
								Console.WriteLine("Max capacity = {0} route = {1}",MaxCapacityOfAvailableVehicles,route);
								Console.WriteLine("\nPress any key to exit application and run application again.");
								Console.ReadKey(true);
								Environment.Exit(1);
							}
						}
						
						
						else
						{
							Distance_to_depot = AlgorithmSolution.GASDistanceMatrix[current_customer,0];
							DriveTime_to_depot = AlgorithmSolution.GASDriveTimeMatrix[current_customer,0];
							
							if(Arrival<AlgorithmSolution.GASServiceBegin[current_customer])
							{
								Departure_to_depot = AlgorithmSolution.GASServiceBegin[current_customer] + AlgorithmSolution.GASServiceDuration[current_customer-1];
							}
							else
							{
								Departure_to_depot = Arrival + AlgorithmSolution.GASServiceDuration[current_customer-1];
							}
							
							Arrival_at_depot=Departure_to_depot+DriveTime_to_depot;
							if(Arrival_at_depot<=AlgorithmSolution.GASServiceEnd[0])
							{
								distance_to_customer[i,y] = Distance;
								drivetime_to_customer[i,y] = DriveTime;
								TotalRouteDistance = TotalRouteDistance + Distance;
								TotalRouteDriveTime = TotalRouteDriveTime + DriveTime;
								arrival_at_customer[i,y] = Arrival;
								route_population[i,y] = current_customer;
								y = y + 1;
								new_route = 0;
								if(j==Population.population.GetLength(1)-1 || y==Population.population.GetLength(1))
								{
									old_route = route;
									if(route > AlgorithmSolution.GASVechicleCapacity.GetLength(0))
									{
										vehicle_allocated[i,auto_created_vehicle_start_index++] = route++;
									}
									else
									{
										int AllVechicleCapacityValueGreater = 0;
										for(int z=0;z<GASVechicleCapacityCopy.GetLength(0);z++)
										{
											
											if(GASVechicleCapacityCopy[z].Equals(-1) || GASVechicleCapacityCopy[z] > RouteDeliveryDemand)
											{
												AllVechicleCapacityValueGreater++;
												continue;
											}
											
											else if(GASVechicleCapacityCopy[z].Equals(RouteDeliveryDemand))
											{
												vehicle_allocated[i,z] = route++;
												GASVechicleCapacityCopy[z] = -1;
												break;
											}
											
											else if(GASVechicleCapacityCopy[z] < RouteDeliveryDemand)
											{
												
												for(int r=z-1;r>-1;r--)
												{
													if(GASVechicleCapacityCopy[r] > RouteDeliveryDemand)
													{
														vehicle_allocated[i,r] = route++;
														GASVechicleCapacityCopy[r] = -1;
														break;
													}
												}
												break;
											}
										}
										
										if(AllVechicleCapacityValueGreater == GASVechicleCapacityCopy.GetLength(0))
										{
											for(int d= GASVechicleCapacityCopy.GetLength(0)-1;d>-1;d--)
											{
												if(GASVechicleCapacityCopy[d].Equals(-1))
												{
													continue;
												}
												else
												{
													vehicle_allocated[i,d] = route++;
													GASVechicleCapacityCopy[d] = -1;
													break;
												}
												
											}
										}
										
										MaxCapacityOfAvailableVehicles = GASVechicleCapacityCopy.Max();
										
										if(route > AlgorithmSolution.GASVechicleCapacity.GetLength(0))
										{
											MaxCapacityOfAvailableVehicles = max_auto_created_vehicle_capacity;
										}
									}
									if(old_route == route)
									{
										Console.WriteLine("\n");
										Console.WriteLine("1:    {0}    {1}", MaxCapacityOfAvailableVehicles,RouteDeliveryDemand);
										Console.WriteLine("\n");
									}
									
									j=-1;
									
									new_route = 1;
									
									actual_loading[i,p] = RouteDeliveryDemand;
									
									DistanceToDepot[i,p] = Distance_to_depot;
									
									DrivetimeToDepot[i,p] = DriveTime_to_depot;
									
									TotalRouteDistance = TotalRouteDistance + Distance_to_depot;
									
									TotalRouteDriveTime = TotalRouteDriveTime + DriveTime_to_depot;
									
									total_route_distance[i,p] = TotalRouteDistance;
									
									total_route_drivetime[i,p] = TotalRouteDriveTime;
									
									ArrivalAtDepot[i,p] = Arrival_at_depot;
									
									route_end_index[i,p++] = y-1;
									
									if(y==Population.population.GetLength(1))
									{
										break;
									}
									else
									{
										continue;
									}
								}
							}
							
							else
							{
								Console.WriteLine("ServiceEnd time of depot{0} is too short",AlgorithmSolution.GASLocation[0]);
								Console.WriteLine("\nPress any key to exit application.");
								Console.ReadKey(true);
								Environment.Exit(1);
							}
						}
					}
					
					else
					{
						Distance = AlgorithmSolution.GASDistanceMatrix[route_population[i,y-1],current_customer];
						DriveTime = AlgorithmSolution.GASDriveTimeMatrix[route_population[i,y-1],current_customer];
						Previous_Arrival = Arrival;
						Arrival=Departure+DriveTime;
						RouteDeliveryDemand = RouteDeliveryDemand + AlgorithmSolution.GASDeliverDemand[current_customer-1];
						
						if(Arrival>AlgorithmSolution.GASServiceEnd[current_customer])
						{
							Arrival = Previous_Arrival;
							RouteDeliveryDemand = RouteDeliveryDemand - AlgorithmSolution.GASDeliverDemand[current_customer-1];
							if(j==Population.population.GetLength(1)-1)
							{
								old_route = route;
								if(route > AlgorithmSolution.GASVechicleCapacity.GetLength(0))
								{
									vehicle_allocated[i,auto_created_vehicle_start_index++] = route++;
								}
								else
								{
									int AllVechicleCapacityValueGreater = 0;
									for(int z=0;z<GASVechicleCapacityCopy.GetLength(0);z++)
									{
										
										if(GASVechicleCapacityCopy[z].Equals(-1) || GASVechicleCapacityCopy[z] > RouteDeliveryDemand)
										{
											AllVechicleCapacityValueGreater++;
											continue;
										}
										
										else if(GASVechicleCapacityCopy[z].Equals(RouteDeliveryDemand))
										{
											vehicle_allocated[i,z] = route++;
											GASVechicleCapacityCopy[z] = -1;
											break;
										}
										
										else if(GASVechicleCapacityCopy[z] < RouteDeliveryDemand)
										{
											
											for(int r=z-1;r>-1;r--)
											{
												if(GASVechicleCapacityCopy[r] > RouteDeliveryDemand)
												{
													vehicle_allocated[i,r] = route++;
													GASVechicleCapacityCopy[r] = -1;
													break;
												}
											}
											break;
										}
									}
									
									if(AllVechicleCapacityValueGreater == GASVechicleCapacityCopy.GetLength(0))
									{
										for(int d= GASVechicleCapacityCopy.GetLength(0)-1;d>-1;d--)
										{
											if(GASVechicleCapacityCopy[d].Equals(-1))
											{
												continue;
											}
											else
											{
												vehicle_allocated[i,d] = route++;
												GASVechicleCapacityCopy[d] = -1;
												break;
											}
											
										}
									}
									
									MaxCapacityOfAvailableVehicles = GASVechicleCapacityCopy.Max();
									
									if(route > AlgorithmSolution.GASVechicleCapacity.GetLength(0))
									{
										MaxCapacityOfAvailableVehicles = max_auto_created_vehicle_capacity;
									}
								}
								if(old_route == route)
								{
									Console.WriteLine("\n");
									Console.WriteLine("1:    {0}    {1}", MaxCapacityOfAvailableVehicles,RouteDeliveryDemand);
									Console.WriteLine("\n");
								}
								
								j=-1;
								
								new_route = 1;
								
								actual_loading[i,p] = RouteDeliveryDemand;
								
								DistanceToDepot[i,p] = Distance_to_depot;
								
								DrivetimeToDepot[i,p] = DriveTime_to_depot;
								
								TotalRouteDistance = TotalRouteDistance + Distance_to_depot;
								
								TotalRouteDriveTime = TotalRouteDriveTime + DriveTime_to_depot;
								
								total_route_distance[i,p] = TotalRouteDistance;
								
								total_route_drivetime[i,p] = TotalRouteDriveTime;
								
								ArrivalAtDepot[i,p] = Arrival_at_depot;
								
								route_end_index[i,p++] = y-1;
								
								continue;
							}
						}
						
						else if(RouteDeliveryDemand>MaxCapacityOfAvailableVehicles)
						{
							Arrival = Previous_Arrival;
							RouteDeliveryDemand = RouteDeliveryDemand - AlgorithmSolution.GASDeliverDemand[current_customer-1];
							old_route = route;
							if(route > AlgorithmSolution.GASVechicleCapacity.GetLength(0))
							{
								vehicle_allocated[i,auto_created_vehicle_start_index++] = route++;
							}
							else
							{
								int AllVechicleCapacityValueGreater = 0;
								for(int z=0;z<GASVechicleCapacityCopy.GetLength(0);z++)
								{
									
									if(GASVechicleCapacityCopy[z].Equals(-1) || GASVechicleCapacityCopy[z] > RouteDeliveryDemand)
									{
										AllVechicleCapacityValueGreater++;
										continue;
									}
									
									else if(GASVechicleCapacityCopy[z].Equals(RouteDeliveryDemand))
									{
										vehicle_allocated[i,z] = route++;
										GASVechicleCapacityCopy[z] = -1;
										break;
									}
									
									else if(GASVechicleCapacityCopy[z] < RouteDeliveryDemand)
									{
										
										for(int r=z-1;r>-1;r--)
										{
											if(GASVechicleCapacityCopy[r] > RouteDeliveryDemand)
											{
												vehicle_allocated[i,r] = route++;
												GASVechicleCapacityCopy[r] = -1;
												break;
											}
										}
										break;
									}
								}
								
								if(AllVechicleCapacityValueGreater == GASVechicleCapacityCopy.GetLength(0))
								{
									for(int d= GASVechicleCapacityCopy.GetLength(0)-1;d>-1;d--)
									{
										if(GASVechicleCapacityCopy[d].Equals(-1))
										{
											continue;
										}
										else
										{
											vehicle_allocated[i,d] = route++;
											GASVechicleCapacityCopy[d] = -1;
											break;
										}
										
									}
								}
								
								MaxCapacityOfAvailableVehicles = GASVechicleCapacityCopy.Max();
								
								if(route > AlgorithmSolution.GASVechicleCapacity.GetLength(0))
								{
									MaxCapacityOfAvailableVehicles = max_auto_created_vehicle_capacity;
								}
							}
							if(old_route == route)
							{
								Console.WriteLine("\n");
								Console.WriteLine("1:    {0}    {1}", MaxCapacityOfAvailableVehicles,RouteDeliveryDemand);
								Console.WriteLine("\n");
							}
							
							actual_loading[i,p] = RouteDeliveryDemand;
							
							DistanceToDepot[i,p] = Distance_to_depot;
							
							DrivetimeToDepot[i,p] = DriveTime_to_depot;
							
							TotalRouteDistance = TotalRouteDistance + Distance_to_depot;
							
							TotalRouteDriveTime = TotalRouteDriveTime + DriveTime_to_depot;
							
							total_route_distance[i,p] = TotalRouteDistance;
							
							total_route_drivetime[i,p] = TotalRouteDriveTime;
							
							ArrivalAtDepot[i,p] = Arrival_at_depot;
							
							route_end_index[i,p++] = y-1;
							
							j = -1;
							
							new_route = 1;
							
							continue;
						}
						
						else
						{
							Distance_to_depot = AlgorithmSolution.GASDistanceMatrix[current_customer,0];
							DriveTime_to_depot = AlgorithmSolution.GASDriveTimeMatrix[current_customer,0];
							
							if(Arrival<AlgorithmSolution.GASServiceBegin[current_customer])
							{
								Departure_to_depot = AlgorithmSolution.GASServiceBegin[current_customer] + AlgorithmSolution.GASServiceDuration[current_customer-1];
							}
							else
							{
								Departure_to_depot = Arrival + AlgorithmSolution.GASServiceDuration[current_customer-1];
							}
							
							Arrival_at_depot=Departure_to_depot+DriveTime_to_depot;
							if(Arrival_at_depot<=AlgorithmSolution.GASServiceEnd[0])
							{
								distance_to_customer[i,y] = Distance;
								drivetime_to_customer[i,y] = DriveTime;
								TotalRouteDistance = TotalRouteDistance + Distance;
								TotalRouteDriveTime = TotalRouteDriveTime + DriveTime;
								arrival_at_customer[i,y] = Arrival;
								route_population[i,y] = current_customer;
								y = y + 1;
								
								if(Arrival<AlgorithmSolution.GASServiceBegin[current_customer])
								{
									Departure = AlgorithmSolution.GASServiceBegin[current_customer] + AlgorithmSolution.GASServiceDuration[current_customer-1];
								}
								else
								{
									Departure = Arrival + AlgorithmSolution.GASServiceDuration[current_customer-1];
								}
								
								if(j==Population.population.GetLength(1)-1 || y==Population.population.GetLength(1))
								{
									old_route = route;
									if(route > AlgorithmSolution.GASVechicleCapacity.GetLength(0))
									{
										vehicle_allocated[i,auto_created_vehicle_start_index++] = route++;
									}
									else
									{
										int AllVechicleCapacityValueGreater = 0;
										for(int z=0;z<GASVechicleCapacityCopy.GetLength(0);z++)
										{
											
											if(GASVechicleCapacityCopy[z].Equals(-1) || GASVechicleCapacityCopy[z] > RouteDeliveryDemand)
											{
												AllVechicleCapacityValueGreater++;
												continue;
											}
											
											else if(GASVechicleCapacityCopy[z].Equals(RouteDeliveryDemand))
											{
												vehicle_allocated[i,z] = route++;
												GASVechicleCapacityCopy[z] = -1;
												break;
											}
											
											else if(GASVechicleCapacityCopy[z] < RouteDeliveryDemand)
											{
												
												for(int r=z-1;r>-1;r--)
												{
													if(GASVechicleCapacityCopy[r] > RouteDeliveryDemand)
													{
														vehicle_allocated[i,r] = route++;
														GASVechicleCapacityCopy[r] = -1;
														break;
													}
												}
												break;
											}
										}
										
										if(AllVechicleCapacityValueGreater == GASVechicleCapacityCopy.GetLength(0))
										{
											for(int d= GASVechicleCapacityCopy.GetLength(0)-1;d>-1;d--)
											{
												if(GASVechicleCapacityCopy[d].Equals(-1))
												{
													continue;
												}
												else
												{
													vehicle_allocated[i,d] = route++;
													GASVechicleCapacityCopy[d] = -1;
													break;
												}
												
											}
										}
										
										MaxCapacityOfAvailableVehicles = GASVechicleCapacityCopy.Max();
										
										if(route > AlgorithmSolution.GASVechicleCapacity.GetLength(0))
										{
											MaxCapacityOfAvailableVehicles = max_auto_created_vehicle_capacity;
										}
									}
									if(old_route == route)
									{
										Console.WriteLine("\n");
										Console.WriteLine("1:    {0}    {1}", MaxCapacityOfAvailableVehicles,RouteDeliveryDemand);
										Console.WriteLine("\n");
									}
									
									actual_loading[i,p] = RouteDeliveryDemand;
									
									DistanceToDepot[i,p] = Distance_to_depot;
									
									DrivetimeToDepot[i,p] = DriveTime_to_depot;
									
									TotalRouteDistance = TotalRouteDistance + Distance_to_depot;
									
									TotalRouteDriveTime = TotalRouteDriveTime + DriveTime_to_depot;
									
									total_route_distance[i,p] = TotalRouteDistance;
									
									total_route_drivetime[i,p] = TotalRouteDriveTime;
									
									ArrivalAtDepot[i,p] = Arrival_at_depot;
									
									route_end_index[i,p++] = y-1;
									
									new_route=1;
									
									j=-1;
									
									if(y==Population.population.GetLength(1))
									{
										break;
									}
									else
									{
										continue;
									}
								}
								
							}
							
							else
							{
								old_route = route;
								if(route > AlgorithmSolution.GASVechicleCapacity.GetLength(0))
								{
									vehicle_allocated[i,auto_created_vehicle_start_index++] = route++;
								}
								else
								{
									int AllVechicleCapacityValueGreater = 0;
									for(int z=0;z<GASVechicleCapacityCopy.GetLength(0);z++)
									{
										
										if(GASVechicleCapacityCopy[z].Equals(-1) || GASVechicleCapacityCopy[z] > RouteDeliveryDemand)
										{
											AllVechicleCapacityValueGreater++;
											continue;
										}
										
										else if(GASVechicleCapacityCopy[z].Equals(RouteDeliveryDemand))
										{
											vehicle_allocated[i,z] = route++;
											GASVechicleCapacityCopy[z] = -1;
											break;
										}
										
										else if(GASVechicleCapacityCopy[z] < RouteDeliveryDemand)
										{
											
											for(int r=z-1;r>-1;r--)
											{
												if(GASVechicleCapacityCopy[r] > RouteDeliveryDemand)
												{
													vehicle_allocated[i,r] = route++;
													GASVechicleCapacityCopy[r] = -1;
													break;
												}
											}
											break;
										}
									}
									
									if(AllVechicleCapacityValueGreater == GASVechicleCapacityCopy.GetLength(0))
									{
										for(int d= GASVechicleCapacityCopy.GetLength(0)-1;d>-1;d--)
										{
											if(GASVechicleCapacityCopy[d].Equals(-1))
											{
												continue;
											}
											else
											{
												vehicle_allocated[i,d] = route++;
												GASVechicleCapacityCopy[d] = -1;
												break;
											}
											
										}
									}
									
									MaxCapacityOfAvailableVehicles = GASVechicleCapacityCopy.Max();
									
									if(route > AlgorithmSolution.GASVechicleCapacity.GetLength(0))
									{
										MaxCapacityOfAvailableVehicles = max_auto_created_vehicle_capacity;
									}
								}
								if(old_route == route)
								{
									Console.WriteLine("\n");
									Console.WriteLine("1:    {0}    {1}", MaxCapacityOfAvailableVehicles,RouteDeliveryDemand);
									Console.WriteLine("\n");
								}
								
								actual_loading[i,p] = RouteDeliveryDemand;
								
								DistanceToDepot[i,p] = Distance_to_depot;
								
								DrivetimeToDepot[i,p] = DriveTime_to_depot;
								
								TotalRouteDistance = TotalRouteDistance + Distance_to_depot;
								
								TotalRouteDriveTime = TotalRouteDriveTime + DriveTime_to_depot;
								
								total_route_distance[i,p] = TotalRouteDistance;
								
								total_route_drivetime[i,p] = TotalRouteDriveTime;

								ArrivalAtDepot[i,p] = Arrival_at_depot;
								
								route_end_index[i,p++] = y-1;
								
								new_route=1;
								
								j=-1;
								
								continue;
							}
							
							
						}
						
						
					}
					
					
				}
				
				NumberOfVehiclesUsed[i] = route-1;
				
				for(int z=0;z<AlgorithmSolution.GASLocation.GetLength(0)-1;z++)
				{
					total_distance[i] = total_distance[i] + total_route_distance[i,z];
				}
				
				
				
			}
			
		}
		
		internal void calculate_fitness()
		{
			for(int i=0;i<Population.population_size;i++)
			{
				fitness[i] = 1/total_distance[i];
				sum_fitness = sum_fitness + fitness[i];
			}
		}
		
		internal void Print_best_chromosome()
		{
			double maxValue;
			int i,maxIndex;
			maxValue = fitness.Max();
			maxIndex = fitness.ToList().IndexOf(maxValue);
			
			for(i=0;i<route_population.GetLength(1);i++)
			{
				
				store_best_route_population[TerminationCriteria.generation_completed,i] = route_population[maxIndex,i];
				
				
				store_best_route_end_index[TerminationCriteria.generation_completed,i] = route_end_index[maxIndex,i];
				
			
				store_best_total_route_distance[TerminationCriteria.generation_completed,i] = total_route_distance[maxIndex,i];
				
				
			}
			store_best_fitness[TerminationCriteria.generation_completed] = maxValue;
			store_best_total_distance[TerminationCriteria.generation_completed] = total_distance[maxIndex];
		}
		
		internal void Selection()
		{
			double sum_of_fitness;
			int i,j;
			for(i=0;i<Population.population_size;i++)
			{
				sum_of_fitness = 0;
				random_number = random.NextDouble();
				random_number = random_number * sum_fitness;
				for(j=0;j<Population.population_size;j++)
				{
					sum_of_fitness = sum_of_fitness + fitness[j];
					if(sum_of_fitness > random_number)
					{
						parent_selection[i] = j;
						break;
					}
				}
			}
		}
		
		internal void Crossover()
		{
			int   i, j, k = 0;
			int[,] parent1 = new int[Population.population_size/2, Population.population.GetLength(1)];
			int[,] parent2 = new int[Population.population_size/2, Population.population.GetLength(1)];

			int firstCrossoverPoint = random.Next(1,Population.population.GetLength(1)/4);
			int secondCrossoverPoint = firstCrossoverPoint + (Population.population.GetLength(1)/2);
			
			for (i = 0; i < Population.population_size/2; i++)
			{
				for (j = 0; j < Population.population.GetLength(1); j++)
					parent1[i, j] = route_population[parent_selection[i], j];
			}

			for (i = Population.population_size/2; i < Population.population_size; i++)
			{
				for (j = 0; j < Population.population.GetLength(1); j++)
					parent2[k, j] = route_population[parent_selection[i], j];

				k = k + 1;
			}

			for (i = 0; i < Population.population_size/2; i++)
			{
				int copyindexoffspring1 = secondCrossoverPoint;
				int copyindexoffspring2 = secondCrossoverPoint;
				random_number = random.NextDouble();

				for (j = firstCrossoverPoint; j < secondCrossoverPoint; j++)
				{
					offspring1[i, j] = parent1[i, j];
					offspring2[i, j] = parent2[i, j];
				}

				bool copytooffspring1;
				bool copytooffspring2;

				for (j = secondCrossoverPoint; j < Population.population.GetLength(1); j++)
				{
					copytooffspring1 = true;
					copytooffspring2 = true;

					for (k = 0; k < Population.population.GetLength(1); k++)
					{
						if (parent2[i, j] == offspring1[i, k])
							copytooffspring1 = false;

						if (parent1[i, j] == offspring2[i, k])
							copytooffspring2 = false;
					}

					if (copytooffspring1)
					{
						offspring1[i, copyindexoffspring1] = parent2[i, j];
						copyindexoffspring1 = copyindexoffspring1 + 1;
					}
					
					if (copytooffspring2)
					{
						offspring2[i, copyindexoffspring2] = parent1[i, j];
						copyindexoffspring2 = copyindexoffspring2 + 1;
					}
				}

				for (j = 0; j < secondCrossoverPoint; j++)
				{
					copytooffspring1 = true;
					copytooffspring2 = true;
					
					for (k = 0; k < Population.population.GetLength(1); k++)
					{
						if (parent2[i, j] == offspring1[i, k])
							copytooffspring1 = false;

						if (parent1[i, j] == offspring2[i, k])
							copytooffspring2 = false;
					}

					if (copytooffspring1)
					{
						if (copyindexoffspring1 == Population.population.GetLength(1))
							copyindexoffspring1 = 0;
						
						offspring1[i, copyindexoffspring1] = parent2[i, j];
						copyindexoffspring1 = copyindexoffspring1 + 1;
					}

					if (copytooffspring2)
					{
						if (copyindexoffspring2 == Population.population.GetLength(1))
							copyindexoffspring2 = 0;

						offspring2[i, copyindexoffspring2] = parent1[i, j];
						copyindexoffspring2 = copyindexoffspring2 + 1;
					}
				}
			}
		}
		
		internal void Mutation()
		{
			int mutationPosition1 = random.Next(0,Population.population.GetLength(1));
			int mutationPosition2 = random.Next(0,Population.population.GetLength(1));
			
			int i;
			int k = 0;

			for (i = 0; i < Population.population_size/2; i++)
			{
				int temp = offspring1[i, mutationPosition1];
				offspring1[i, mutationPosition1] = offspring1[i, mutationPosition2];
				offspring1[i, mutationPosition2] = temp;

				temp = offspring2[i, mutationPosition1];
				offspring2[i, mutationPosition1] = offspring2[i, mutationPosition2];
				offspring2[i, mutationPosition2] = temp;
				
				int j;
				
				for (j = 0; j < Population.population.GetLength(1); j++)
				{
					Population.new_population[k, j] = offspring1[i, j];
				}
				
				k = k + 1;

				for (j = 0; j < Population.population.GetLength(1); j++)
				{
					
					Population.new_population[k, j] = offspring2[i, j];
				}
				
				k = k + 1;
			}
		}
		
		public void Replace()
		{
			int i,j,k,l=0;
			int NumberOfTenPercentageBestChromosomes = Convert.ToInt32(Population.population_size*0.10);
			int NumberOfNinetyPercentageBestChromosomes = Population.population_size-NumberOfTenPercentageBestChromosomes;
			double[] fitness_copy = fitness.ToArray();
			Array.Sort(fitness_copy);
			Array.Reverse(fitness_copy);

			for(i=0;i<NumberOfTenPercentageBestChromosomes;i++)
			{
				for(j=0;j<fitness.GetLength(0);j++)
				{
					if(fitness_copy[i].Equals(fitness[j]))
					{
						for(k=0;k<Population.population.GetLength(1);k++)
						{
							Population.population[l, k] = route_population[j,k];
						}
						l = l + 1;
						break;
					}
				}
			}

			for (i = 0; i < Population.population_size; i++)
			{

				for (j = 0; j < Population.population.GetLength(1); j++)
				{

					Population.population[l, j] = Population.new_population[i, j];
				}
				l = l + 1;
				if(l == Population.population_size)
				{
					break;
				}
			}
		}
		
		public double GenerateOutputFile()
		{
			
			double maxValue = store_best_fitness.Max();
			int maxIndex = store_best_fitness.ToList().IndexOf(maxValue);
			
			//Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

			//if (xlApp == null)
			//{
			//	Console.WriteLine("EXCEL could not be started. Check that your office installation and project references are correct.");
			//	return;
			//}
			
			//xlApp.Visible = true;

			//Workbook wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
			
			//Worksheet ws = (Worksheet)wb.Worksheets[1];

			//if (ws == null)
			//{
			//	Console.WriteLine("Worksheet could not be created. Check that your office installation and project references are correct.");
			//}
			
			//int row=1;
			//int column=2;
			Console.WriteLine();
				
			int j=0;
						
			for(int i=0;i<AlgorithmSolution.GASLocation.GetLength(0)-1;i++)
			{
				if(i==store_best_route_end_index[maxIndex,j]+1)
				{
					Console.WriteLine("{0,30}",store_best_total_route_distance[maxIndex,j]);
					//ws.Cells[row,column] = total_route_distance[maxIndex,j];
					//row = row + 1;
					//column = 2;
					j = j + 1;
				}
				
				Console.Write("{0,3}",store_best_route_population[maxIndex,i] + "    ");
				//ws.Cells[row,column] = route_population[maxIndex,i];
				//column = column+1;
				
			}
			Console.WriteLine("{0,30}",store_best_total_route_distance[maxIndex,j]);
			Console.WriteLine();
			Console.WriteLine("Total distance travelled by all cars: " + store_best_total_distance[maxIndex]);

            return store_best_total_distance[maxIndex];
			//ws.Cells[row,column] = total_route_distance[maxIndex,j];
			//ws.Cells[row+1,column] = total_distance[maxIndex];
			
			
			//string filename = string.Format(CultureInfo.InvariantCulture,"DVRP_SOLUTION  {0:dd-MM-yyyy  hh-mm-ss tt}.xlsx",DateTime.Now);
			
			
			//wb.SaveAs (Directory.GetCurrentDirectory() + @"\OutputFile\" + filename);

		}
		
	}

}


