using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class WrenchStampedListener : Listener<Messages.Geometry.WrenchStamped>
    {
    }
}
