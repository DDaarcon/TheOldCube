using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameInfo;
using GameInfo.GameInfoInternals.InterfaceInfoInternals;
using GameInfo.GameInfoInternals.InterfaceInfoInternals.ClockInfoInternals;

namespace GameServices.Clock
{
    public class ClockTimeService : BaseService
    {
        private ClockInfo info => interfaceInfo.ClockInfo;
        private TimeInfo timeInfo => info.TimeInfo;

        private readonly ClockVisibilityService ClockVisually = new ClockVisibilityService();

        public void Reset()
        {
            timeInfo.Passed = 0f;
            timeInfo.StartedAt = 0f;
            timeInfo.IsPaused = true;

            ClockVisually.SetTimeOnClockDisplay();
        }
    }
}
