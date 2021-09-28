using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameInfo.GameInfoInternals;

namespace GameInfo
{
    public class GameInformation
    {
        public CubeInfo Cube { get; set; } = new();
        public EditorEnvironmentInfo EditorEnvironment { get; set; } = new();
        public InterfaceInfo Interface { get; set; } = new();
        public ThemeInfo Theme { get; set; } = new();
    }
}
