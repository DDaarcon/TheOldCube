using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using static Enums;
using GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals;
using GameExtensions.Solution;

namespace GameInfo.GameInfoInternals
{
    [Serializable]
    public class EditorEnvironmentInfo
    {
        public EditorEnvironmentInfo()
        {
            CurrentSolution = new SolutionInfo<bool>(this.Variant);
            CurrentSolution.SetDefaultValueForSideData(false);
        }

        public PlacedPieceInfo PlacedPiece { get; set; } = new PlacedPieceInfo();
        public WorkspaceInfo Workspace { get; set; } = new WorkspaceInfo();
        public SolutionInfo<bool> CurrentSolution { get; set; }

        public Variant Variant = Variant.x4;

        [HideInInspector]
        public bool DuringPlacing = false;

    }
}
