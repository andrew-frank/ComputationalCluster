using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputationalCluster.Computational
{
    class Request
    {
        public int Id { get; set; }

        public Point Location { get; set; }

        public double Start { get; set; }

        public double End { get; set; }

        public double Unload { get; set; }

        public double Size { get; set; }
    }
}
