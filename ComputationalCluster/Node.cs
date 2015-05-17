using ComputationalCluster.Shared.Connection;
using ComputationalCluster.Shared.Messages.RegisterNamespace;
using ComputationalCluster.Shared.Messages.RegisterResponseNamespace;
using ComputationalCluster.Shared.Messages.StatusNamespace;
using ComputationalCluster.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace ComputationalCluster.Nodes
{
    public abstract class Node : Object
    {
        #region Properties

        protected Timer TimeoutTimer { get; private set; }


        public uint Timeout { get; set; }
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


        //the number of threads that could be efficiently run in parallel
        private int _parallelThreads = 1;
        public int ParallelThreads { get { return Convert.ToByte(_parallelThreads); } }

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


        #region Overrides/Cluster

        protected abstract Status CurrentStatus();
        protected abstract Register GenerateRegister();

        #endregion


        #region Common/Cluster

        /// GenerateRegister() needs to be overriden at this point.
        protected void RegisterComponent()
        {
            Register register = this.GenerateRegister();

            String message = register.SerializeToXML();

            Debug.Assert(message != null);
            if (message == null)
                return;

            string response = CMSocket.Instance.SendMessage(this.Port, this.HostName, message);
            Object obj = response.DeserializeXML();

            if (!(obj is RegisterResponse))
                throw new Exception("Invalid response");

            RegisterResponse registerResponse = (RegisterResponse)obj;
            this.ID = registerResponse.Id;
            this.Timeout = registerResponse.Timeout;
            this.StartTimeoutTimer();
        }


        /// CurrentStatus() needs to be overriden at this point.
        protected void TimeoutTimerCallback(object sender, System.Timers.ElapsedEventArgs e)
        {
            String message = "";
            Status status = this.CurrentStatus();
            message = status.SerializeToXML();
            CMSocket.Instance.SendMessage(this.Port, this.HostName, message);
        }


        /// Starts timer using Timeout value as interval.
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
