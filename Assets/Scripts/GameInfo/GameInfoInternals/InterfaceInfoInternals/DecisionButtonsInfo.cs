using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace GameInfo.GameInfoInternals.InterfaceInfoInternals
{
    [Serializable]
    public class DecisionButtonsInfo
    {

        public CanvasGroup Panel;
        [HideInInspector]
        public LTDescr PanelAnimation;
    }
}
