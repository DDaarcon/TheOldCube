using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameInfo;
using GameInfo.GameInfoInternals;

namespace GameServices.Themes
{
    public class ThemesService : BaseService
    {
        private ThemeInfo info => generalInfo.Theme;

        public void SetDefaultIfIsTrying()
        {
            if (info.IsTrying)
            {
                info.IsTrying = false;
                info.CurrentTheme = ThemeInfo.DefaultTheme;
            }
        }
    }
}
