using System.Windows;

namespace VehicleRouting
{
    public class Request
    {
        public int Id { get; set; }

        public Point Location { get; set; }

        public double Start { get; set; }

        public double End { get; set; }

        public double Unload { get; set; }

        public double Size { get; set; }
    }
}
