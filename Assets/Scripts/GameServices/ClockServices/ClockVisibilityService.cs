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
    public class ClockVisibilityService : BaseService
    {
        public const float HIDE_CLOCK_TIME = 1f;
        public const float SHOW_CLOCK_TIME = 1f;
        private ClockInfo info => interfaceInfo.ClockInfo;
        private TimeInfo timeInfo => info.TimeInfo;
        private ClockPhysicalInfo physicalInfo => info.ClockPhysicalInfo;

        public void SetTimeOnClockDisplay(float? time = null)
        {
            time ??= timeInfo.Passed;
            physicalInfo.ClockText.text = "<mspace=0.7em>" + (time / 60 >= 10 ? "" : "0") + (int)(time / 60) + ":" + (time % 60 >= 10 ? "" : "0") + (int)(time % 60);
        }

        public void MakeWellVisible()
        {
            physicalInfo.ClockText.CrossFadeAlpha(1f, SHOW_CLOCK_TIME, false);
        }

        public void MakeBarelyVisible()
        {
            physicalInfo.ClockText.CrossFadeAlpha(.25f, HIDE_CLOCK_TIME, false);
        }
    }
}
