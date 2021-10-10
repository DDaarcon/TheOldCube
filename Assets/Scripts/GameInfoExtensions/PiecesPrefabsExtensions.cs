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
            return variant switch
            {
                Variant.x3 => info.Piece3_3,
                Variant.x4 => info.Piece4_4,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
