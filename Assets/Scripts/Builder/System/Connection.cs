using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Builder.System
{
    public class Connection
    {
        readonly FixedJoint2D selfJoint;
        readonly FixedJoint2D targetJoint;

        public event Action OnDisconnected;

        public Connection(FixedJoint2D selfJoint, FixedJoint2D targetJoint)
        {
            this.selfJoint = selfJoint;
            this.targetJoint = targetJoint;
        }

        public void Disconnect()
        {
            Object.Destroy(selfJoint);
            Object.Destroy(targetJoint);
            OnDisconnected?.Invoke();
            OnDisconnected = null;
        }
    }
}