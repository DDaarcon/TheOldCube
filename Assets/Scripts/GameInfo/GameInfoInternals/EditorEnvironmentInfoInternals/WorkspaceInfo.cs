using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals
{
    [Serializable]
    public class WorkspaceInfo
    {
        public Transform PhysicalPosition;
        [HideInInspector]
        public Quaternion RandomRotationForAnimation;
        [HideInInspector]
        public bool IsRotating;
    }
}