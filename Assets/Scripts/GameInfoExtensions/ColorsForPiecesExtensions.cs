using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GameInfo.GameInfoInternals.ThemeInfoInternals;
using static Enums;

namespace GameExtensions.Theme
{
    public static class ColorsForPiecesExtensions
    {
        public static Color GetBySide(this ColorsForPiecesInfo colorsInfo, Side side)
        {
            switch (side)
            {
                case Side.Bottom:
                    return colorsInfo.BottomPiece;
                case Side.Back:
                    return colorsInfo.BackPiece;
                case Side.Left:
                    return colorsInfo.LeftPiece;
                case Side.Right:
                    return colorsInfo.RightPiece;
                case Side.Front:
                    return colorsInfo.FrontPiece;
                case Side.Top:
                    return colorsInfo.TopPiece;
                default:
                    throw new InvalidSideException();
            }
        }
    }
}
