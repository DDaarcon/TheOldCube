using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals
{
    public class WorkspaceInfo
    {
        public Transform PhysicalPosition { get; set; }
        public Quaternion RandomRotationForAnimation { get; set; }
        public bool IsRotating { get; set; }
    }
}
