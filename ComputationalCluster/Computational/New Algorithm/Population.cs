using System;

namespace AL_FOR_DVRP
{
	internal class Population
	{
		internal static int population_size = 32000;  
		
		internal static int[,] population = new int[population_size,AlgorithmSolution.GASLocation.GetLength(0)-1];
		internal static int[,] new_population = new int[population_size,AlgorithmSolution.GASLocation.GetLength(0)-1];
		
				
		internal static void generate_initial_population()
			
		{
			
			int[] initial_population = new int[AlgorithmSolution.GASLocation.GetLength(0)-1];
			
			for(int i=0;i<AlgorithmSolution.GASLocation.GetLength(0)-1;i++)
			{
				initial_population[i] = i+1;
			}
			
			Random r = new Random();
			int x = 0;
			while(x<population_size)
			{
				for(int j=initial_population.Length; j>0; j--)
				{
					int k = r.Next(j);
					int l = initial_population[k];
					initial_population[k] = initial_population[j - 1];
					initial_population[j - 1]  = l;
				}
				int m;
				

				for(m=0;m<AlgorithmSolution.GASLocation.GetLength(0)-1;m++)
				{
					population[x,m] = initial_population[m];
				}
				x=x+1;
				//}
			}
		}

		
		 static public void sortchar(char [] buffer, int len)

        {

            for (int i = 1; i < len; i++)

            {

                for (int j = 0; j < len - i; j++)

                {

                    if (buffer[j] > buffer[j+1])

                    {

                        char temp = buffer[j];

                        buffer[j] = buffer[j + 1];

                        buffer[j + 1] = temp;

                    }

                }

            }

        }

 

        static public bool NextPermuation(char[] p, int len)

        {

            for(int k = len - 1; k > 0; k--)

            {

                if(p[k - 1] >= p[k])

                    continue;

                else

                {

                    if(k <= len - 3)

                    {

                        char newchar = p[k-1];

                        int anchor = -1;

                        for(int j = len - 1; j >= k; j--)

                        {

                            if(newchar < p[j])

                            {

                                anchor = j;

                                break;

                            }

                        }

                        if(anchor == -1)

                            return false;

                        char ch = p[k-1];

                        p[k-1] = p[anchor];

                        p[anchor] = ch;

 

                        char[] tbuffer = new char[len - k];

                        for (int m = 0; m < len - k; m++)

                            tbuffer[m] = p[k + m];

                        sortchar(tbuffer, len - k);

                        for (int n = 0; n < len - k; n++)

                            p[k + n] = tbuffer[n];

                        return true;

                    }

                    else

                    {

                        char[] tempptr = new char[3];

                        tempptr[0] = p[p.Length - 3];

                        tempptr[1] = p[p.Length - 2];

                        tempptr[2] = p[p.Length - 1]; 

 

                        int count = 3;   

                        for(int i = count - 1; i > 0; i--)

                        {

                            if(tempptr[i - 1] >= tempptr[i])

                                continue;

                            else

                            {

                                if(i <= count - 2)

                                {

                                    if(tempptr[i+1] > tempptr[i-1])

                                    {

                                        char ch = tempptr[i+1];

                                        tempptr[i+1] = tempptr[i];

                                        tempptr[i] = tempptr[i-1];

                                        tempptr[i-1] = ch;

                                    }

                                    else

                                    {

                                        char ch = tempptr[i-1];

                                        tempptr[i-1] = tempptr[i];

                                        tempptr[i] = tempptr[i+1];

                                        tempptr[i+1] = ch;

                                    }

                                }

                                else

                                {

                                    char ch = tempptr[i];

                                    tempptr[i] = tempptr[i-1];

                                    tempptr[i-1] = ch;

                                }

                                p[p.Length - 3] = tempptr[0];

                                p[p.Length - 2] = tempptr[1];

                                p[p.Length - 1] = tempptr[2];

                                return true;

                            }

                        }

                        return false;

                    }

                }

            }

            return false;

        }

			
		internal static void show_population() 
		{
						
			for(int i=0;i<population_size;i++)
			{
				for(int j=0;j<AlgorithmSolution.GASLocation.GetLength(0)-1;j++)
				{
					Console.Write("{0,4}",population[i,j]);
				}
				Console.WriteLine("\n");
			}
			
			Console.WriteLine("Initial population was successfully generated.");
			
			Console.WriteLine();
			
			//Console.WriteLine("Press any key to continue...");
		
		}
	}
}
 