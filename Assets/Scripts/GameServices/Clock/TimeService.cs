using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GameInfo.GameInfoInternals.GameplayInfoInternals;

namespace GameServices.Clock
{
    public class TimeService : BaseService
    {
        private TimeInfo timeInfo => gameplayInfo.Time;

        private readonly ClockVisibilityService ClockVisually = new ClockVisibilityService();

        public void Reset()
        {
            timeInfo.Passed = 0f;
            timeInfo.StartedAt = 0f;
            timeInfo.IsPaused = true;

            ClockVisually.SetTimeOnClockDisplay();
        }

        public void Resume()
        {
            //Passed stays the same
            timeInfo.StartedAt = Time.time - timeInfo.Passed;
            timeInfo.IsPaused = false;
        }

        public void UpdateAndShowTime()
        {
            UpdateTime();
            ClockVisually.SetTimeOnClockDisplay();
        }

        public void UpdateTime()
        {
            if (timeInfo.IsPaused)
                return;

            timeInfo.Passed += Time.deltaTime;
        }
    }
}
