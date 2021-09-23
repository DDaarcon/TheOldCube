using UnityEngine;

using GameInfo.GameInfoInternals.InterfaceInfoInternals;

namespace GameInfo.GameInfoInternals
{
    public class InterfaceInfo
    {
        public PiecesButtonsInfo PiecesButtons { get; set; }
        public DecisionButtonsInfo DecisionButtons { get; set; }

        public ScreenOrientation ScreenOrientation { get; set; }
    }
}
