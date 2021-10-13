using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace GameInfo.GameInfoInternals.CubeInfoInternals
{
    [SerializeField]
    public class CubePhysicalData
    {
        [HideInInspector]
        public GameObject BottomPiece;
        [HideInInspector]
        public GameObject BackPiece;
        [HideInInspector]
        public GameObject LeftPiece;
        [HideInInspector]
        public GameObject RightPiece;
        [HideInInspector]
        public GameObject FrontPiece;
        [HideInInspector]
        public GameObject TopPiece;

        public GameObject[] Pieces => new GameObject[]
        {
            BottomPiece, BackPiece, LeftPiece,
            RightPiece, FrontPiece, TopPiece
        };
    }
}
