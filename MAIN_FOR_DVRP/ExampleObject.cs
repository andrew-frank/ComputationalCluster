using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Computational
{


    public class newExampleObject
    {

     public   int[] Location;
     public  double[] DeliverDemand;
     public   int[] ServiceDuration;
     public  int[] ServiceBegin;
     public  int[] ServiceEnd;
     public   string[] VehicleName;
     public   double[] VehicleCapacity;
     public   int[] X_Coordinate;
     public   int[] Y_Coordinate;

        public newExampleObject()
        {

        }
        public newExampleObject(int[] locations, double[] deliverdemand, int[] serviceduration, int[] servicebegin, int[] serviceend, string[] vehiclename,
            double[] vehiclecapacity, int[] x_Coordinate, int[] y_Coordinate)
        {
            Location = locations;
            DeliverDemand = deliverdemand;
            ServiceDuration=serviceduration;
            ServiceBegin = servicebegin;
            ServiceEnd = serviceend;
            VehicleName = vehiclename;
            VehicleCapacity = vehiclecapacity;
            X_Coordinate = x_Coordinate;
            Y_Coordinate = y_Coordinate;

        }





    }


}