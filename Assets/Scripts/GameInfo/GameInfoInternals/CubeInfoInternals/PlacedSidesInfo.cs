using System.Linq;

namespace GameInfo.GameInfoInternals.CubeInfoInternals
{
    public class PlacedSidesInfo
    {

        public PlacedSidesInfo() { }
        public PlacedSidesInfo(bool[] data)
        {
            BottomSideIsPlaced = data[0];
            BackSideIsPlaced = data[1];
            LeftSideIsPlaced = data[2];
            RightSideIsPlaced = data[3];
            TopSideIsPlaced = data[4];
            FrontSideIsPlaced = data[5];
        }
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
