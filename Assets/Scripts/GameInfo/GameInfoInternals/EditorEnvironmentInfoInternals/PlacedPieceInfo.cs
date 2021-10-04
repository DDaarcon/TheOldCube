using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using static Enums;

namespace GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals
{
    [Serializable]
    public class PlacedPieceInfo
    {
        [HideInInspector]
        public GameObject Object;
        [HideInInspector]
        public Side? CurrentPositionRelativeToCube;
        [HideInInspector]
        public bool DuringRotationAnimation;
    }
}
