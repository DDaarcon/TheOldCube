using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using static Enums;

namespace GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals
{
    public class PlacedPieceInfo
    {
        public GameObject Object { get; set; }
        public Side? CurrentPositionRelativeToCube { get; set; }

        public bool DuringRotationAnimation { get; set; }
    }
}
