using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameInfo;
using GameInfo.GameInfoInternals;

namespace GameServices
{
    public class BaseService
    {
        protected GameInformation generalInfo => GameInfoHolder.Information;

        #region First level information classes

        protected CubeInfo cubeInfo => generalInfo.Cube;
        protected EditorEnvironmentInfo editorInfo => generalInfo.EditorEnvironment;
        protected GameplayInfo gameplayInfo => generalInfo.Gameplay;
        protected ThemeInfo themeInfo => generalInfo.Theme;
        protected InterfaceInfo interfaceInfo => generalInfo.Interface;

        #endregion
    }
}
