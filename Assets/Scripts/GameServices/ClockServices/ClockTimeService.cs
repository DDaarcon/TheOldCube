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
    public class ClockTimeService
    {
        private GameInformation Information => GameInfoHolder.Information;
        private ClockInfo General => Information.Interface.ClockInfo;
        private TimeInfo Time => General.TimeInfo;

        private readonly ClockVisibilityService ClockVisually = new ClockVisibilityService();

        public void Reset()
        {
            Time.Passed = 0f;
            Time.StartedAt = 0f;
            Time.IsPaused = true;

            ClockVisually.SetTimeOnClockDisplay();
        }
    }
}
