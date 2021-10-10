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
    [Serializable]
    public class PiecesButtonsInfo
    {
        public PiecesButtonsPhysicalData PhysicalData { get; set; } = new PiecesButtonsPhysicalData();
        /*
         Żeby nie zapomnieć: dane buttonów powinny być uzupełniane w inicjalizacji danych info każdego komponentu ApplySettingToBtn, ten komponent powinien mieć przypisany index, po którym rozpozna, którym buttonem jest
         
         */
        public PieceButtonInfo Button1 { get; set; } = new PieceButtonInfo();
        public PieceButtonInfo Button2 { get; set; } = new PieceButtonInfo();
        public PieceButtonInfo Button3 { get; set; } = new PieceButtonInfo();
        public PieceButtonInfo Button4 { get; set; } = new PieceButtonInfo();
        public PieceButtonInfo Button5 { get; set; } = new PieceButtonInfo();
        public PieceButtonInfo Button6 { get; set; } = new PieceButtonInfo();

        public Color ColorForPieces;
        public int CellCameraDiplayLayer = 8;

        [HideInInspector]
        public int ButtonIndexForCurrentlyPlacedPiece;
        public PieceButtonInfo[] Buttons => new PieceButtonInfo[]
        {
            Button1, Button2, Button3,
            Button4, Button5, Button6
        };
    }
}
