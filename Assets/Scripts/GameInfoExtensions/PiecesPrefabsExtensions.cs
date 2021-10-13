using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals;
using static Enums;

namespace GameExtensions.Pieces
{
    public static class PiecesPrefabsExtensions
    {
        public static GameObject GetByVariant(this PiecesPrefabsInfo info, Variant variant)
        {
            switch (variant)
            {
                case Variant.x3:
                    return info.Piece3_3;
                case Variant.x4:
                    return info.Piece4_4;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
