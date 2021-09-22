using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameBase.GameInfoInternals;

namespace GameBase
{
    public class GameInfo
    {
        public CubeInfo Cube { get; set; }
        public EditorEnvironmentInfo EditorEnvironment { get; set; }
    }
}
