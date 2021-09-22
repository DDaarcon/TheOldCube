using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameBase.GameInfoInternals.GameplayInfoInternals;

namespace GameBase.GameInfoInternals
{
    public class GameplayInfo
    {
        public LevelInfo Level { get; set; }

        public bool IsRandom { get; set; }
    }
}
