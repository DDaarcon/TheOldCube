using GameInfo.GameInfoInternals.SolutionInfoInternals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using static Enums;

namespace GameInfo.GameInfoInternals.InterfaceInfoInternals.PiecesButtonsInfoInternals
{
    [Serializable]
    public class PieceButtonInfo
    {
        public Button PhysicalObject;
        public Camera CellCamera;
        public Image BackgroundImage;
        public GameObject Piece;
        public bool IsEnabled;
        [HideInInspector]
        public Variant Variant;
        [HideInInspector]
        public Side? SideFromCurrentSolution;
        [HideInInspector]
        public Side? SideFromGeneratedSolution;
        [HideInInspector]
        public SideData<bool> SettingForPiece;
        public Piece PieceComponent => Piece.GetComponent<Piece>();

    }
}
