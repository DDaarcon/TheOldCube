using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Enums;
using GameInfo.GameInfoInternals.SolutionInfoInternals;

namespace GameInfo.GameInfoInternals
{
    [Serializable]
    public class SolutionInfo<TData>
    {
        public SolutionInfo(Variant variant)
        {
            LeftSide = new SideData<TData>(variant);
            RightSide = new SideData<TData>(variant);
            FrontSide = new SideData<TData>(variant);
            BackSide = new SideData<TData>(variant);
            TopSide = new SideData<TData>(variant);
            BottomSide = new SideData<TData>(variant);

            Variant = variant;
        }

        public SideData<TData> LeftSide { get; set; }
        public SideData<TData> RightSide { get; set; }
        public SideData<TData> FrontSide { get; set; }
        public SideData<TData> BackSide { get; set; }
        public SideData<TData> TopSide { get; set; }
        public SideData<TData> BottomSide { get; set; }

        public Variant Variant { get; }

        public SideData<TData>[] FullArray
        {
            get => new SideData<TData>[]
            {
                BottomSide,
                BackSide,
                LeftSide,
                RightSide,
                FrontSide,
                TopSide
            };
        }

        public IList<SideData<TData>> FullList
        {
            get => new List<SideData<TData>>
            {
                BottomSide,
                BackSide,
                LeftSide,
                RightSide,
                FrontSide,
                TopSide
            };
        }
    }
}
