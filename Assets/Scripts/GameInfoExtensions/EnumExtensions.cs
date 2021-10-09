using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Enums;

namespace GameExtensions
{
    public static class EnumExtensions
    {
        public static int Int(this Variant variant)
        {
            return (int)variant;
        }
    }
}
