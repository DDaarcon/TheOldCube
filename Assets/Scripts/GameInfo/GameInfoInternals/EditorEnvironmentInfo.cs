using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using static Enums;
using GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals;

namespace GameInfo.GameInfoInternals
{
    public class EditorEnvironmentInfo
    {
        public EditorEnvironmentInfo()
        {
            Solution = new(this.Variant);
        }

        public PlacedPieceInfo PlacedPiece { get; set; } = new();
        public WorkspaceInfo Workspace { get; set; } = new();
        public SolutionInfo<bool> Solution { get; set; }

        public Variant Variant { get; set; } = Variant.x4;
        public bool DuringPlacing { get; set; }

    }
}
