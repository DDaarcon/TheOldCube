using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GameInfo.GameInfoInternals.CubeInfoInternals;
using static Enums;

namespace GameExtensions.Cube
{
    public static class CubePhysicalDataExtensions
    {
        public static void SetPieceBySide(this CubePhysicalData physicalData, Side side, GameObject piece)
        {
            switch (side)
            {
                case Side.Bottom:
                    physicalData.BottomPiece = piece;
                    break;
                case Side.Back:
                    physicalData.BackPiece = piece;
                    break;
                case Side.Left:
                    physicalData.LeftPiece = piece;
                    break;
                case Side.Right:
                    physicalData.RightPiece = piece;
                    break;
                case Side.Front:
                    physicalData.FrontPiece = piece;
                    break;
                case Side.Top:
                    physicalData.TopPiece = piece;
                    break;
                default:
                    break;
            }
        }

        public static GameObject GetPieceBySide(this CubePhysicalData physicalData, Side side)
        {
            switch (side)
            {
                case Side.Bottom:
                    return physicalData.BottomPiece;
                case Side.Back:
                    return physicalData.BackPiece;
                case Side.Left:
                    return physicalData.LeftPiece;
                case Side.Right:
                    return physicalData.RightPiece;
                case Side.Front:
                    return physicalData.FrontPiece;
                case Side.Top:
                    return physicalData.TopPiece;
                default:
                    return null;
            }
        }
    }
}