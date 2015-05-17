using ComputationalCluster.Shared.Connection;
using ComputationalCluster.Shared.Messages.RegisterNamespace;
using ComputationalCluster.Shared.Messages.StatusNamespace;
using ComputationalCluster.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace ComputationalCluster.Nodes
{
    public abstract class Node : Object
    {
        #region Properties

        protected Timer TimeoutTimer { get; private set; }


        public Int32 Timeout { get; set; }
        public ulong ID { get; set; }
        public string TypeName {
            get {  return  Utilities.NodeNameForType(NodeType); }
            //set { }
        }
        public NodeType NodeType { get; set; }
        public System.Net.IPAddress LocalIP { get; set; } //?
        public System.Net.IPAddress IP {get; set;}
        public string HostName { get; set; }
        public int Port { get; set; }

        #endregion

        #region Public

        public override string ToString()
        {
            string msg;
            if (IP == null)
                msg = TypeName + ": " + ID;
            else
                msg = TypeName + ": " + ID + ", IP: " + IP.ToString();
            return msg;
        }


        #endregion

        #region Cluster

        #region Overrides

        protected abstract void RegisterComponent();
        protected abstract Status CurrentStatus();

        #endregion


        /// CurrentStatus() needs to be overriden at this point.
        protected void TimeoutTimerCallback(object sender, System.Timers.ElapsedEventArgs e)
        {
            String message = "";
            Status status = this.CurrentStatus();
            message = status.SerializeToXML();
            CMSocket.Instance.SendMessage(this.Port, this.HostName, message);
        }

        /// <summary>
        /// Starts timer using Timeout value as interval.
        /// </summary>
        protected void StartTimeoutTimer()
        {
            if (this.Timeout <= 0)
                throw new Exception("Unspecified timeout");

            this.TimeoutTimer = new Timer();
            this.TimeoutTimer.Interval = 1000 * this.Timeout;
            this.TimeoutTimer.Elapsed += this.TimeoutTimerCallback;
            this.TimeoutTimer.Start();
        }

        #endregion
    }

    public enum NodeType
    {
        Server = 0,
        Client,
        ComputationalNode,
        TaskManager
    }
}
