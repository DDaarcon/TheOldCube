using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameInfo.GameInfoInternals.SolutionInfoInternals;

namespace GameExtensions.SideData
{
    public static class SideDataExtensions
    {
        public static void Clear<TData>(this SideData<TData> side, TData defaultValue)
        {
            side.DefaultValue = defaultValue;
            side.Clear();
        }

        public static void Clear<TData>(this SideData<TData> side)
        {
            int xLength = side.Data.GetLength(0), yLength = side.Data.GetLength(1);
            for (int x = 0; x < xLength; x++)
                for (int y = 0; y < yLength; y++)
                    side.Data[x, y] = side.DefaultValue;
        }
    }
}
