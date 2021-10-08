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
    public class ClockVisibilityService
    {
        private GameInformation Information => GameInfoHolder.Information;
        private ClockInfo General => Information.Interface.ClockInfo;
        private TimeInfo Time => General.TimeInfo;
        private ClockPhysicalInfo Physical => General.ClockPhysicalInfo;

        public void SetTimeOnClockDisplay()
        {
            float time = Time.Passed;
            Physical.ClockText.text = "<mspace=0.7em>" + (time / 60 >= 10 ? "" : "0") + (int)(time / 60) + ":" + (time % 60 >= 10 ? "" : "0") + (int)(time % 60);
        }
    }
}
