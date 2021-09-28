﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Enums;

namespace GameInfo.GameInfoInternals.SolutionInfoInternals
{
    public class SideData<TData>
    {
        public SideData(Variant variant)
        {
            int sideLength = (int)variant;
            Data = new TData[sideLength, sideLength];
        }

        public TData[,] Data { get; set; }
        public TData DefaultValue { get; set; }
    }
}