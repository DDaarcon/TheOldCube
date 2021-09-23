using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using static Enums;

using GameInfo.GameInfoInternals.InterfaceInfoInternals.PiecesButtonsInfoInternals;

namespace GameInfo.GameInfoInternals.InterfaceInfoInternals
{
    public class PiecesButtonsInfo
    {
        public PiecesButtonsPhysicalData PhysicalData { get; set; }

        public Dictionary<Side, int> GeneratedSolutionPieceSideToButtonIndex {get; set; }
        public Dictionary<Side, int> PlacedPieceToButtonIndex { get; set; }
        public int ButtonIndexForCurrentlyPlacedPiece { get; set; }

        public Color Color { get; set; }
    }
}
