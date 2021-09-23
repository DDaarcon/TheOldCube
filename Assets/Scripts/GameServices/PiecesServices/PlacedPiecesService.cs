using UnityEngine;

using GameInfo;
using GameInfo.GameInfoInternals.CubeInfoInternals;
using GameExtensions.Cube.PlacedSides;

namespace GameServices.PlacedPieces
{
    public class PlacedPiecesService
    {
        private GameInformation Information => GameInfoHolder.Information;
        private PlacedSidesInfo PlacedSides => Information.Cube.PlacedSides;
        private CubePhysicalData CubePhysicalData => Information.Cube.PhysicalData;

        public void RemoveAll()
        {
            PlacedSides.Clear();

            foreach (var piece in CubePhysicalData.Pieces)
            {
                if (piece != null) Object.Destroy(piece);
            }
        }
    }
}
