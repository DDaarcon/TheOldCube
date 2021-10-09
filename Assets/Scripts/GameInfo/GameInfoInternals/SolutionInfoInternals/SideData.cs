using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Enums;

using GameExtensions;

namespace GameInfo.GameInfoInternals.SolutionInfoInternals
{
    public class SideData<TData>
    {
        public SideData(Variant variant)
        {
            int sideLength = variant.Int();
            Data = new TData[sideLength, sideLength];
        }

        public static SideData<TData> CreateFromArray(TData[,] data, Variant variantToVerifySize)
        {
            if (data.GetLength(0) != variantToVerifySize.Int()
                || data.GetLength(1) != variantToVerifySize.Int())
                throw new InvalidDataArraySizeException();
            return new SideData<TData>(data);
        }
        private SideData(TData[,] data)
        {
            Data = data;
        }

        public TData[,] Data { get; set; }
        public TData DefaultValue { get; set; }

        public class InvalidDataArraySizeException : Exception
        {
            public override string Message => "Data array passed to create SideData<TData> has invalid lengths compared to variant";
        }
    }
}
