using System.Windows;

namespace VehicleRouting
{
    public class Depot
    {
        public string Name { get; set; }

        public Point Location { get; set; }

        public double Start { get; set; }

        public double End { get; set; }

        public int Venicles { get; set; }
    }
}
