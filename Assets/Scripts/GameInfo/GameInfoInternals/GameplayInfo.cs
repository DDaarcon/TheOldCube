using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using GameInfo.GameInfoInternals.GameplayInfoInternals;

namespace GameInfo.GameInfoInternals
{
    [Serializable]
    public class GameplayInfo
    {
        public LevelInfo Level { get; set; }
        public TimeInfo Time { get; set; }

        public bool IsLevelRandom;
        [HideInInspector]
        public bool HasStarted;
        [HideInInspector]
        public bool IsFinished;
        [HideInInspector]
        public bool IsRestartedAfterFinish;
    }
}
