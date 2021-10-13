﻿using System;
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
            switch (side)
            {
                case Side.Bottom:
                    return placedSides.BottomSideIsPlaced;
                case Side.Back:
                    return placedSides.BackSideIsPlaced;
                case Side.Left:
                    return placedSides.LeftSideIsPlaced;
                case Side.Right:
                    return placedSides.RightSideIsPlaced;
                case Side.Front:
                    return placedSides.FrontSideIsPlaced;
                case Side.Top:
                    return placedSides.TopSideIsPlaced;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
