using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace ComputationalCluster.Computational
{
    public class Depot
    {
        public Depot(Point location, int start, int end)
        {
            this.Location = location;
            this.Start = start;
            this.End = end;
            this.workingHours = end - start;
        }


        public Point Location { get; protected set; }
        public int Start { get; protected set; }
        public int End { get; protected set; }
        public int workingHours { get; protected set; }
    }
 
}
