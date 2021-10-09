using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GameInfo.GameInfoInternals.InterfaceInfoInternals.SeedInputInfoInternals;

namespace GameInfo.GameInfoInternals.InterfaceInfoInternals
{
    [Serializable]
    public class SeedInputInfo
    {
        public SeedInputPhysicalInfo Physical { get; set; } = new SeedInputPhysicalInfo();
        public bool IsLevelDebugingEnabled = true;
        [HideInInspector]
        public bool IsPointerDown = false;
        [HideInInspector]
        public bool WasPointerDawnInPreviousFrame = false;
        [HideInInspector]
        public float TimeWhenTouchHappened;
        [HideInInspector]
        public float DurationOfTouch = 2f;
        [HideInInspector]
        public bool PlaceBottomSide;
        [HideInInspector]
        public bool PlaceBackSide;
        [HideInInspector]
        public bool PlaceLeftSide;
        [HideInInspector]
        public bool PlaceRightSide;
        [HideInInspector]
        public bool PlaceFrontSide;
        [HideInInspector]
        public bool PlaceTopSide;
        //private bool changedAtThisTouch = false;
    }
}
