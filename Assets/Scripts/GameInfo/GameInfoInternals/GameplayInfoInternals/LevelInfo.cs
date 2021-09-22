using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameInfo.GameInfoInternals.CubeInfoInternals;

namespace GameInfo.GameInfoInternals.GameplayInfoInternals
{
    public class LevelInfo
    {
        public PlacedSidesInfo PicesPlacedOnStart { get; set; }
        public int? IndexOfCurrentlyOpened { get; set; }
        
    }
}
