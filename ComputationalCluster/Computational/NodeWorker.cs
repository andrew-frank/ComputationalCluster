using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Computational
{
    class NodeWorker
    {
        #region Properties/ivars

        private bool _state = false;
        private ulong _howLong;
        private ulong _problemInstanceId;
        private ulong _taskId;
        private string _problemType;

        /// 0 = idle, 1 = busy
        public bool State { get { return _state; } }
        public string StateString {
            get {
                switch (this.State) {
                    case false: return "Idle";
                    case true: return "Busy";
                }
                return null;
            }
        }

        public ulong HowLong { get { return _howLong; } }
        public ulong ProblemInstanceId { get { return _problemInstanceId; } }
        public ulong TaskId { get { return _taskId; } }
        public string ProblemType { get { return _problemType; } }

        #endregion


        //thread stuff here

    }
}
