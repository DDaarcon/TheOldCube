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
        private readonly GameplayInitialization gameplayService = new GameplayInitialization();
        private readonly GameVariantService variantService = new GameVariantService();

        // TODO: refactor
        public void Hide()
        {
            CanvasGroup canvasGroup = info.Object.GetComponent<CanvasGroup>();
            info.AlphaValue = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public void HideAndStartNewGame()
        {
            CanvasGroup canvasGroup = info.Object.GetComponent<CanvasGroup>();
            info.AlphaValue = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            if (info.Variant != editorInfo.Variant)
            {
                variantService.ChooseVariant(info.Variant);
            }

            if (!gameplayInfo.Level.IndexOfCurrentlyOpened.HasValue)
                throw new LevelIsRandomException();

            LevelSettings lS = levelMenu.GetLevelSettings(gameplayInfo.Level.IndexOfCurrentlyOpened.Value + 1, editorInfo.Variant);
            gameplayService.StartNewGame(gameplayInfo.Level.IndexOfCurrentlyOpened.Value + 1, lS.seed, lS.placedSides, lS.finished);
        }

        public class LevelIsRandomException : Exception
        {
            public override string Message => "You are trying to access level index, but level is random";
        }
    }
}
