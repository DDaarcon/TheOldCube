﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameInfo.GameInfoInternals;
using GameInfo.GameInfoInternals.SolutionInfoInternals;
using GameExtensions.SideData;

namespace GameExtensions.Solution
{
    public static class SolutionExtension
    {
        public static void Set<TData>(this SolutionInfo<TData> solution,
            SideData<TData> bottom,
            SideData<TData> back,
            SideData<TData> left,
            SideData<TData> right,
            SideData<TData> front,
            SideData<TData> top)
        {
            solution.BottomSide = bottom;
            solution.BackSide = back;
            solution.LeftSide = left;
            solution.RightSide = right;
            solution.FrontSide = front;
            solution.TopSide = top;
        }

        public static void Set<TData>(this SolutionInfo<TData> solution, SideData<TData>[] data)
        {
            solution.Set(data[0], data[1], data[2], data[3], data[4], data[5]);
        }

        public static void SetDefaultValueForSideData<TData>(this SolutionInfo<TData> solution, TData defaultValue)
        {
            foreach (var side in solution.FullList)
                side.DefaultValue = defaultValue;
        }

        public static void Clear<TData>(this SolutionInfo<TData> solution)
        {
            foreach (var side in solution.FullList)
                side.Clear();
        }

        public static void Clear<TData>(this SolutionInfo<TData> solution, TData clearValue)
        {
            foreach (var side in solution.FullList)
                side.Clear(clearValue);
        }
    }
}
