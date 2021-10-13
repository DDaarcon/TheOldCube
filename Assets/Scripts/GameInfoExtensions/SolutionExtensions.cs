using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameInfo.GameInfoInternals;
using GameInfo.GameInfoInternals.SolutionInfoInternals;
using GameExtensions.SideData;
using static Enums;

namespace GameExtensions.Solution
{
    public static class SolutionExtensions
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
            if (data.Length != 6)
                throw new InvalidSolutionDataException();
            solution.Set(data[0], data[1], data[2], data[3], data[4], data[5]);
        }

        public static void SetDefaultValueForSideData<TData>(this SolutionInfo<TData> solution, TData defaultValue)
        {
            foreach (var side in solution.FullList)
                side.DefaultValue = defaultValue;
        }

        public static void SetUsingGenerationAlgorithm(this SolutionInfo<bool> solutionWithBools, int? seed = null)
        {
            var generatedFromAlgorithm = seed.HasValue
                ? SolutionGenerationAlgorithm.GetNewSolution(seed.Value, solutionWithBools.Variant)
                : SolutionGenerationAlgorithm.GetNewSolution(solutionWithBools.Variant);

            SideData<bool>[] sideDatas = new SideData<bool>[generatedFromAlgorithm.Length];
            for (int i = 0; i < generatedFromAlgorithm.Length; i++)
                sideDatas[i] = SideData<bool>.CreateFromArray(generatedFromAlgorithm[i], solutionWithBools.Variant);

            solutionWithBools.Set(sideDatas);
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

        public static SideData<TData> GetBySide<TData>(this SolutionInfo<TData> solution, Side side)
        {
            switch (side)
            {
                case Side.Bottom:
                    return solution.BottomSide;
                case Side.Back:
                    return solution.BackSide;
                case Side.Left:
                    return solution.LeftSide;
                case Side.Right:
                    return solution.RightSide;
                case Side.Front:
                    return solution.FrontSide;
                case Side.Top:
                    return solution.TopSide;
                default:
                    throw new NotImplementedException();
            }
        }

        public class InvalidSolutionDataException : Exception
        {
            public override string Message => "Data passed to fill solution is invalid. Array size isn't exactly 6.";
        }
    }
}
