using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace ComputationalCluster.ComputationalNode
{
  public class Client
    {
      public Client(Point _location, double _time, double _unloadTime, int _size)
      {
          Location = _location;
          Time = _time;
          _unloadTime = unloadTime;
          Size = _size;
      }

      Point Location { get; set; }
      double Time;
      double unloadTime;
      int Size;
     
    }



}
