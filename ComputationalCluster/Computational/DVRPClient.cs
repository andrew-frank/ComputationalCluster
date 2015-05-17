using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace ComputationalCluster.Computational
{
  public class DVRPClient
    {
      public DVRPClient(Point location, double time, double unloadTime, int size)
      {
          this.Location = location;
          this.Time = time;
          this.UnloadTime = unloadTime;
          this.Size = size;
      }

      public DVRPClient(Point location)
      {
          this.Location = location;
      }


      public Point Location { get; protected set; }
      public double Time { get; protected set; }
      public double UnloadTime { get; protected set; }
      public int Size { get; protected set; }
     
    }
}
