using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace GameInfo.GameInfoInternals.ThemeInfoInternals
{
    [Serializable]
    public class ColorsForPiecesInfo
    {
        public Color BottomPiece;
        public Color BackPiece;
        public Color LeftPiece;
        public Color RightPiece;
        public Color FrontPiece;
        public Color TopPiece;

        public Color PlacingPiece;
        public Color MistakesInPlacingPiece;
    }
}
