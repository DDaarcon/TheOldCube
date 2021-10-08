using System;
using UnityEngine;

using GameInfo.GameInfoInternals.InterfaceInfoInternals.ClockInfoInternals;

namespace GameInfo.GameInfoInternals.InterfaceInfoInternals
{
    [Serializable]
    public class ClockInfo
    {
        public ClockPhysicalInfo ClockPhysicalInfo { get; set; }
        public TimeInfo TimeInfo { get; set; }

        public Color RecordShineColor;
        public float RecordShineFontThickness;
        public Color RegularFinishShineColor;
        public float RegularFinishShineFontThickness;
        public float ShineTime;
        public Color DefaultColor;
        public float DefaultFontThickness;
    }
}
