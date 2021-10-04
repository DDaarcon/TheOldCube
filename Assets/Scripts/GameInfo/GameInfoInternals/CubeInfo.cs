using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameInfo.GameInfoInternals.CubeInfoInternals;

namespace GameInfo.GameInfoInternals
{
    [Serializable]
    public class CubeInfo
    {
        public PlacedSidesInfo PlacedSides { get; set; }
        public CubePhysicalData PhysicalData { get; set; }
    }
}
