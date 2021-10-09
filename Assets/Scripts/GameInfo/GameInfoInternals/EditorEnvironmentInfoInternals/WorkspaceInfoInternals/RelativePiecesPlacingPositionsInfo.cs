using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals.WorkspaceInfoInternals
{
    [Serializable]
    public class RelativePiecesPlacingPositionsInfo
    {
        [HideInInspector]
        public Vector3 Bottom;
        [HideInInspector]
        public Vector3 Back;
        [HideInInspector]
        public Vector3 Left;
        [HideInInspector]
        public Vector3 Right;
        [HideInInspector]
        public Vector3 Front;
        [HideInInspector]
        public Vector3 Top;

    }
}
