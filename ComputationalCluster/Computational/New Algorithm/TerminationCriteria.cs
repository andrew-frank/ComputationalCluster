using System;

namespace AL_FOR_DVRP
{
	internal class TerminationCriteria
	{
		internal static int generation_completed;
		internal const int max_generation = 20;
		
		internal bool TerminationCriteriaCheck() 
		{
		
			if(generation_completed==max_generation)
				return true;
			else
				return false;
		}
		
		internal void  Increment_Generation_Completed_Byone()
		{
			generation_completed = generation_completed + 1;
		}  
	}
}
