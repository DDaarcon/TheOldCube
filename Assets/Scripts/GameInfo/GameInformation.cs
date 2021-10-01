using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GameInfo.GameInfoInternals;

namespace GameInfo
{
    
    [Serializable]
    public class GameInformation : UnityEngine.Object
    {
        public GameInformation() { }

        public CubeInfo Cube { get; set; } = new CubeInfo();
        public EditorEnvironmentInfo EditorEnvironment { get; set; } = new EditorEnvironmentInfo();
        public InterfaceInfo Interface { get; set; } = new InterfaceInfo();
        public ThemeInfo Theme { get; set; } = new ThemeInfo();
        public bool testField;
    }
}
