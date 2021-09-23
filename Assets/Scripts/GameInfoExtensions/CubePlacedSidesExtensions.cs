using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameInfo.GameInfoInternals.CubeInfoInternals;

namespace GameExtensions.Cube.PlacedSides
{
    public static class CubePlacedSidesExtensions
    {
        public static void Clear(this PlacedSidesInfo placedSides)
        {
            placedSides.LeftSideIsPlaced = false;
            placedSides.RightSideIsPlaced = false;
            placedSides.FrontSideIsPlaced = false;
            placedSides.BackSideIsPlaced = false;
            placedSides.TopSideIsPlaced = false;
            placedSides.BottomSideIsPlaced = false;
        }
    }
}
