using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals.WorkspaceInfoInternals;

namespace GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals
{
    [Serializable]
    public class WorkspaceInfo
    {
        public Transform PhysicalPosition;
        public RelativePiecesPlacingPositionsInfo RelativePiecesPlacingPositions { get; set; } = new RelativePiecesPlacingPositionsInfo();
        [HideInInspector]
        public Quaternion RandomRotationForAnimation;
        [HideInInspector]
        public bool IsRotating;
    }
}