using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine.UI;

using static Enums;

namespace GameInfo.GameInfoInternals.InterfaceInfoInternals
{
    [Serializable]
    public class NextLevelButtonInfo
    {
        public Button Object;
        public uint AlphaValue = 0;
        public Variant Variant;
    }
}
