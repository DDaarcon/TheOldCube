using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameInfo.GameInfoInternals.CubeInfoInternals;
using static Enums;

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

        public static bool GetBySide(this PlacedSidesInfo placedSides, Side side)
        {
            return side switch
            {
                Side.Bottom => placedSides.BottomSideIsPlaced,
                Side.Back => placedSides.BackSideIsPlaced,
                Side.Left => placedSides.LeftSideIsPlaced,
                Side.Right => placedSides.RightSideIsPlaced,
                Side.Front => placedSides.FrontSideIsPlaced,
                Side.Top => placedSides.TopSideIsPlaced,
                _ => throw new NotImplementedException(),
            }
        }
    }
}
