using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GameInfo.GameInfoInternals.ThemeInfoInternals;
using static Enums;

namespace GameInfo.GameInfoInternals
{
    [Serializable]
    public class ThemeInfo
    {
        public ColorsForPiecesInfo ColorsForPieces = new ColorsForPiecesInfo();
        [HideInInspector]
        public static readonly Themes DefaultTheme = Themes.BasicStone;
        public Themes CurrentTheme = DefaultTheme;
        public bool IsTrying;
    }
}
