using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameBase.GameInfoInternals.CubeInfoInternals;

namespace GameBase.GameInfoInternals.GameplayInfoInternals
{
    public class LevelInfo
    {
        public PlacedSidesInfo PicesPlacedOnStart { get; set; }
        public int? IndexOfCurrentlyOpened { get; set; }
        
    }
}
