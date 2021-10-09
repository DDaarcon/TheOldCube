using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals.WorkspaceInfoInternals;

namespace GameExtensions.RelativePlacingPiecesPositions
{
    public static class RelativePlacingPiecesPositionsExtensions
    {
        public static void Calculate(this RelativePiecesPlacingPositionsInfo relativePos, float value)
        {
            relativePos.Bottom = new Vector3(0, -value, 0);
            relativePos.Back = new Vector3(0, 0, value);
            relativePos.Left = new Vector3(-value, 0, 0);
            relativePos.Right = new Vector3(value, 0, 0);
            relativePos.Front = new Vector3(0, 0, -value);
            relativePos.Top = new Vector3(0, value, 0);
        }
    }
}
