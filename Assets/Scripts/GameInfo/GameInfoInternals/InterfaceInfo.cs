using UnityEngine;

using GameInfo.GameInfoInternals.InterfaceInfoInternals;
using System;

namespace GameInfo.GameInfoInternals
{
    [Serializable]
    public class InterfaceInfo
    {
        public PiecesButtonsInfo PiecesButtons { get; set; } = new PiecesButtonsInfo();
        public DecisionButtonsInfo DecisionButtons { get; set; } = new DecisionButtonsInfo();
        public ClockInfo ClockInfo { get; set; } = new ClockInfo();
        public NextLevelButtonInfo NextLevelButton { get; set; } = new NextLevelButtonInfo();

        [HideInInspector]
        public ScreenOrientation ScreenOrientation;
    }
}
