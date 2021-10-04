using System;
using UnityEngine;

namespace GameInfo.GameInfoInternals.InterfaceInfoInternals
{
    [Serializable]
    public class ClockInfo
    {
        public Color clockRecordShineColor;
        public float clockRecordShineThick;
        public Color clockRegularFinishShineColor;
        public float clockRegularFinishShineThick;
        public float shineTime;
        private Color clockDefaultColor;
        private float clockDefaultThick;
        private bool clockPaused = false;
        private float timeOfStart;
        private float timeOfGame;
    }
}
