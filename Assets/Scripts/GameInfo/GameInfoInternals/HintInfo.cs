using System;

using UnityEngine;

using GameInfo.GameInfoInternals.HintInfoInternals;

namespace GameInfo.GameInfoInternals
{
    [Serializable]
    public class HintInfo
    {
        public HintPhysicalInfo Physical { get; set; } = new HintPhysicalInfo();

        public float ButtonAppearingDuration = 1f;
        public float TimeAdditionIfFinished = 10f;
        public float TimeAdditionIfSkipped = 5f;
        public float OfflineWaitTime = 20f;

        [HideInInspector]
        public readonly string GooglePlay_GameID = "4007741";
        [HideInInspector]
        public readonly string myPlacementID = "rewardedVideo";
        public bool IsInAdTestingMode = true;
        
        [HideInInspector]
        public bool HintReady = false;
        [HideInInspector]
        public bool TimerCounting = true;
        [HideInInspector]
        public float TimerValue = 0f;
        //public AdvSolution gameSolution { get; private set; } = new AdvSolution();
        //public AdvSolution genrSolution { get; private set; } = new AdvSolution();

    }
}
