using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameInfo.GameInfoInternals.InterfaceInfoInternals.ClockInfoInternals
{
    [Serializable]
    public class TimeInfo
    {
        [HideInInspector]
        public float StartedAt;
        [HideInInspector]
        public float Passed;
        [HideInInspector]
        public bool IsPaused;
    }
}
