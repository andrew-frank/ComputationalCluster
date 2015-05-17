using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace ComputationalCluster.ComputationalNode
{
    public class Depot
    {
        public Depot(Point _location, int _start, int _end)
        {
            Location = _location;
            Start = _start;
            End = _end;
            wHours = _end - _start;
        }


        Point Location { get; set; }
        int Start { get; set; }
        int End {get; set;}

        int wHours { get; set; }

    }
 
}
