using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInfo
{
    public static class GameInfoHolder
    {
        private static GameInformation gameInformation;
        public static GameInformation Information
        {
            get
            {
                if (gameInformation == null)
                    gameInformation = new();
                return gameInformation;
            }
        }
    }
}
