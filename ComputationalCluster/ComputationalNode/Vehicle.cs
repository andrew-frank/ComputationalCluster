using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.ComputationalNode
{
    public class Vehicle
    {
        public Vehicle(int _capacity, int _speed)
        {
            Capacity = _capacity;
            Speed = _speed;

        }

        int Capacity { get; set; }
        int Speed { get; set; }


    }
}
