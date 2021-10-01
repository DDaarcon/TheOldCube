using UnityEngine;

using GameInfo.GameInfoInternals.InterfaceInfoInternals;

namespace GameInfo.GameInfoInternals
{
    public class InterfaceInfo : UnityEngine.Object
    {
        public PiecesButtonsInfo PiecesButtons { get; set; } = new PiecesButtonsInfo();
        public DecisionButtonsInfo DecisionButtons { get; set; } = new DecisionButtonsInfo();
        public ClockInfo ClockInfo { get; set; } = new ClockInfo();

        public ScreenOrientation ScreenOrientation { get; set; }
    }
}
