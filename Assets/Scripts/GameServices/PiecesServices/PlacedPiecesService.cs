using UnityEngine;

using GameInfo;
using GameInfo.GameInfoInternals.CubeInfoInternals;
using GameExtensions.Cube.PlacedSides;

namespace GameServices.PlacedPieces
{
    public class PlacedPiecesService : BaseService
    {
        private PlacedSidesInfo placedSidesInfo => cubeInfo.PlacedSides;
        private CubePhysicalData cubePhysicalInfo => cubeInfo.PhysicalData;

        public void RemoveAll()
        {
            placedSidesInfo.Clear();

            foreach (var piece in cubePhysicalInfo.Pieces)
            {
                if (piece != null) Object.Destroy(piece);
            }
        }
    }
}
