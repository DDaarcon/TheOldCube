using System.Linq;

namespace GameInfo.GameInfoInternals.CubeInfoInternals
{
    public class PlacedSidesInfo
    {
        public bool LeftSideIsPlaced { get; set; }
        public bool RightSideIsPlaced { get; set; }
        public bool FrontSideIsPlaced { get; set; }
        public bool BackSideIsPlaced { get; set; }
        public bool TopSideIsPlaced { get; set; }
        public bool BottomSideIsPlaced { get; set; }

        public bool[] ArrayRepresentation
        {
            get => new bool[] {
                BottomSideIsPlaced,
                BackSideIsPlaced,
                LeftSideIsPlaced,
                RightSideIsPlaced,
                TopSideIsPlaced,
                FrontSideIsPlaced};
        }

        public int AmoutOfPlaced
        {
            get => ArrayRepresentation.Count(x => x);
        }
    }
}
