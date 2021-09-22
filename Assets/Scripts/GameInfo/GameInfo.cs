using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameInfo.GameInfoInternals;

namespace GameInfo
{
    public class GameInfo
    {
        public CubeInfo Cube { get; set; }
        public EditorEnvironmentInfo EditorEnvironment { get; set; }
        public InterfaceInfo Interface { get; set; }
        public ThemeInfo Theme { get; set; }
    }
}
