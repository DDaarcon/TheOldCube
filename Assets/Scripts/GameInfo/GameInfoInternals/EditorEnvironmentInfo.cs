using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals;

namespace GameInfo.GameInfoInternals
{
    public class EditorEnvironmentInfo
    {
        public PlacedPieceInfo PlacedPiece { get; set; }
        public WorkspaceInfo Workspace { get; set; }

        public bool DuringPlacing { get; set; }

    }
}
