using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Enums;

namespace GameInfo.GameInfoInternals
{
    [Serializable]
    public class ThemeInfo
    {
        public static readonly Themes DefaultTheme = Themes.BasicStone;
        public Themes CurrentTheme = DefaultTheme;
        public bool IsTrying;
    }
}
