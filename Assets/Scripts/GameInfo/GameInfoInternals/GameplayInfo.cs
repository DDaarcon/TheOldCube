using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameInfo.GameInfoInternals.GameplayInfoInternals;

namespace GameInfo.GameInfoInternals
{
    public class GameplayInfo
    {
        public LevelInfo Level { get; set; }

        public bool IsRandom { get; set; }
    }
}
