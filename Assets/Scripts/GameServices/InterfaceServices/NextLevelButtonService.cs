using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GameInfo;
using GameInfo.GameInfoInternals.InterfaceInfoInternals;
using GameServices.Gameplay;

namespace GameServices.Interface
{
    public class NextLevelButtonService : BaseService
    {
        private NextLevelButtonInfo info => generalInfo.Interface.NextLevelButton;
        private GameplayInitialization gameplay = new GameplayInitialization();

        // TODO: refactor
        private void HideNextLevelButton(bool justHide)
        {
            CanvasGroup canvasGroup = info.Object.GetComponent<CanvasGroup>();
            info.AlphaValue = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            if (!justHide)
            {
                if (info.Variant != generalInfo.EditorEnvironment.Variant)
                {
                    ChooseVariant(info.Variant, false);
                }

                if (!generalInfo.Gameplay.Level.IndexOfCurrentlyOpened.HasValue)
                    throw new LevelIsRandomException();

                LevelSettings lS = levelMenu.GetLevelSettings(openedLevel + 1, generalInfo.EditorEnvironment.Variant);
                gameplay.StartNewGame(generalInfo.Gameplay.Level.IndexOfCurrentlyOpened.Value + 1, lS.seed, lS.placedSides, lS.finished);
            }
        }

        public class LevelIsRandomException : Exception
        {
            public override string Message => "You are trying to access level index, but level is random";
        }
    }
}
