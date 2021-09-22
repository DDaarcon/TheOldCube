using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Enums;

namespace GameDebug
{
    public static class CustomDebug
    {
        public static void VisualizeDataFromSolution(bool[][,] solution, string precedingMsg = "")
        {
            string et = precedingMsg;
            et += " (solution): \n";
            for (int s = 0; s < 6; s++)
            {
                et += "Side: " + ((Side)s).ToString() + '\n';
                for (int i = 0; i < solution.GetLength(0); i++)
                {
                    for (int j = 0; j < solution.GetLength(0); j++)
                    {
                        et += (solution[s][i, j] ? "I" : "O") + "\t";
                    }
                    et += '\n';
                }
            }
            UnityEngine.Debug.Log(et);
        }
        public static void VisualizeDataFromSetting(bool[,] setting, string precedingMsg = "")
        {
            string et = precedingMsg;
            et += " (setting): \n";
            for (int i = 0; i < setting.GetLength(0); i++)
            {
                for (int j = 0; j < setting.GetLength(0); j++)
                {
                    et += (setting[i, j] ? "I" : "O") + "\t";
                }
                et += '\n';
            }
            UnityEngine.Debug.Log(et);
        }
    }
}
