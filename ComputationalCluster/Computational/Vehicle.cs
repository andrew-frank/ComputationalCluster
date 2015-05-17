using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Computational
{
    public class Vehicle
    {
        public Vehicle(int capacity, int speed)
        {
            Capacity = capacity;
            Speed = speed;
        }

        public int Capacity { get; protected set; }
        public int Speed { get; protected set; }
    }
}
