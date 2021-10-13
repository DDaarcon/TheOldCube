using GameExtensions.Cube.PlacedSides;
using GameExtensions.Interface;
using GameExtensions.Solution;
using GameExtensions.Cube;
using GameInfo.GameInfoInternals.CubeInfoInternals;
using GameInfo.GameInfoInternals.EditorEnvironmentInfoInternals;
using GameServices.Clock;
using GameServices.Editor;
using GameServices.Interface;
using GameServices.PlacedPieces;
using GameServices.Themes;
using System;
using UnityEngine;
using static Enums;
using GameExtensions.Theme;

namespace GameServices.Gameplay
{
    public class GameplayInitialization : BaseService
	{
		public GameplayInitialization() { }

        private readonly PlacedPiecesService placedPiecesService = new PlacedPiecesService();
        private readonly EditorEvents editorEvents = new EditorEvents();
        private readonly TimeService timeService = new TimeService();
        private readonly ClockVisibilityService clockVisibilityService = new ClockVisibilityService();
        private readonly ThemesService themesService = new ThemesService();
        private readonly NextLevelButtonService nextLevelButtonService = new NextLevelButtonService();
        private readonly PiecesButtonsService piecesButtonsService = new PiecesButtonsService();

        private WorkspaceInfo workspaceInfo => editorInfo.Workspace;

        // TODO: refactor
        private void CommonBeginning()
        {
            placedPiecesService.RemoveAll();
            
            if (generalInfo.EditorEnvironment.DuringPlacing)
            {
                editorEvents.AbortPlacing();
            }

            generalInfo.EditorEnvironment.CurrentSolution.Clear();

            timeService.Reset();

            //hintScript.RenewHint();
            nextLevelButtonService.Hide();

            gameplayInfo.IsFinished = false;
            gameplayInfo.IsRestartedAfterFinish = false;
            workspaceInfo.IsRotating = false;

            themesService.SetDefaultIfIsTrying();

            debugInfo.SeedInputFieldScript.RenewData();

            LeanTween.cancel(workspaceInfo.PhysicalPosition.gameObject);
            workspaceInfo.PhysicalPosition.LeanScale(Vector3.one, 0f);
        }
        private void StartGameSuffix(bool autoHideMenu)
        {
            if (workspaceInfo.DefaultRotation.HasValue)
                throw new MissingDefaultWorkspaceRotationException();
            workspaceInfo.PhysicalPosition.rotation = workspaceInfo.DefaultRotation.Value;

            //if (autoHideMenu) levelMenu.ToggleMenus(0, true);
        }
        public void StartNewRandomGame(bool autoHideMenu = true)
        {
            CommonBeginning();

            editorInfo.GeneratedSolution.SetUsingGenerationAlgorithm();
            gameplayInfo.IsLevelRandom = true;
            gameplayInfo.HasStarted = false;
            clockVisibilityService.MakeWellVisible();

            piecesButtonsService.RenewButtons();

            StartGameSuffix(autoHideMenu);
        }
        public void StartNewRandomGame(int seed, bool autoHideMenu = true)
        {
            CommonBeginning();

            editorInfo.GeneratedSolution.SetUsingGenerationAlgorithm(seed);
            gameplayInfo.IsLevelRandom = true;
            gameplayInfo.HasStarted = false;
            clockVisibilityService.MakeWellVisible();

            piecesButtonsService.RenewButtons();

            StartGameSuffix(autoHideMenu);
        }
        // TODO: change placedSides_ type to PlacedSidesInfo
        public void StartNewGame(int level, int seed, PlacedSidesInfo placedSides, bool finished)
        {
            CommonBeginning();

            editorInfo.GeneratedSolution.SetUsingGenerationAlgorithm(seed);
            gameplayInfo.Level.IndexOfCurrentlyOpened = level;
            gameplayInfo.Level.PiecesPlacedOnStart = placedSides;
            gameplayInfo.IsLevelRandom = false;
            clockVisibilityService.MakeBarelyVisible();

            piecesButtonsService.RenewButtons();

            gameplayInfo.IsFinished = finished;

            var sides = SideArray;
            foreach (var side in sides)
            {
                if (placedSides.GetBySide(side) || finished)
                {
                    // do zaimplementowania - powinno dodatkowo przypisywać wartość SideForCurrentSolution odpowiedzniemu buttonowi
                    // ewentualnie - powinno pobierać od razu buttona, ale raczej nie, bo to wyjątkowa sytuacja, gdzie sciana jest od razu wstawina i pomija etap PlacedPiece
                    //PlacePieceAt();
                    var button = piecesButtonsService.GetButtonBySideOnGeneratedSolution(side);
                    button.SetEnableState(false);
                }
            }

            if (gameplayInfo.Level.IndexOfCurrentlyOpened == 0)
            {
                TutorialScript.TurnOnNote(TutorialScript.Notes.ChooseFirstPieceNote);
            }
            if (gameplayInfo.Level.IndexOfCurrentlyOpened == 1)
            {
                TutorialScript.TurnOnNote(TutorialScript.Notes.RotateWorkspaceNote);
            }

            StartGameSuffix(true);
        }

        /*public void DebugLevel(int seed, bool[] placedSides_)
        {
            CommonBeginning();

            editorInfo.GeneratedSolution.SetUsingGenerationAlgorithm(seed);
            placedSidesFromSolution = placedSides_.Clone() as bool[];
            levelNotRandom = false;
            clockText.CrossFadeAlpha(0.25f, 1f, true);

            RenewButtons();

            for (int i = 0; i < 6; i++)
            {
                if (placedSides_[i])
                {
                    PlacePieceAt(genrSolution[i], (Side)i, true);
                    int b = 0;
                    while (buttonOrder[b] != i) b++;
                    piecesButtonsIndexes[i] = b;
                    EnabledButton(b, false);
                }
            }

            StartGameSuffix(true);
        }*/

        //#if UNITY_EDITOR
        //        public void DebugNewGame(bool[][,] solution)
        //        {
        //            CommonBeginning();

        //            levelNotRandom = true;
        //            finishedGame = true;

        //            OverrideGeneratedSolution(solution);
        //            VisualizeDataFromSolution(genrSolution, "GenrSolution before applying (DebugNewGame)");

        //            RenewButtons();

        //            for (int i = 0; i < 6; i++)
        //            {
        //                if (PlacePieceAt(genrSolution[i], (Side)i, true))
        //                {
        //                    int b = 0;
        //                    while (buttonOrder[b] != i) b++;
        //                    piecesButtonsIndexes[i] = b;
        //                    EnabledButton(b, false);
        //                }
        //            }
        //            VisualizeDataFromSolution(gameSolution, "GameSolution after applying genrSolution (DNG)");
        //        }
        //#endif
        public void RestartGame()
        {
            bool finishedAndRestarted = false;
            if (gameplayInfo.IsFinished)
            {
                timeService.Resume();
                finishedAndRestarted = true;
            }
            CommonBeginning();

            gameplayInfo.IsRestartedAfterFinish = finishedAndRestarted;

            piecesButtonsService.SetEnableStateForAll(true);

            if (!gameplayInfo.IsLevelRandom)
            {
                var sides = SideArray;
                foreach (var side in sides)
                {
                    if (!gameplayInfo.Level.PiecesPlacedOnStart.GetBySide(side))
                        continue;

                    // wyjaśnienie wyżej
                    //PlacePieceAt();
                    var button = piecesButtonsService.GetButtonBySideOnGeneratedSolution(side);
                    button.SetEnableState(false);
                }
            }

            //levelMenu.ToggleMenus(0, false);
        }

        // TODO: refactor
        private void FinishedLevel()
        {
            Debug.Log("Finish");

            CubePhysicalData piecesCopy = new CubePhysicalData();
            var sides = SideArray;
            foreach (var side in sides)
            {
                var piece = cubeInfo.PhysicalData.GetPieceBySide(side);

                piecesCopy.SetPieceBySide(side, UnityEngine.Object.Instantiate(piece, piece.transform.position, piece.transform.rotation));
                var copiedPiece = piecesCopy.GetPieceBySide(side);

                if (copiedPiece.TryGetComponent(out PiecePG piecePG))
                {
                    piecePG.SetTheme(themeInfo.CurrentTheme, (int)side);
                }

                copiedPiece.GetComponent<Piece>().ChangeSetting(editorInfo.CurrentSolution.GetBySide(side).Data);
                copiedPiece.GetComponent<Piece>().ChangeColor(piece.GetComponent<Piece>().color);
                copiedPiece.GetComponent<Piece>().ChangeColorInTime(themeInfo.ColorsForPieces.GetBySide(side), 1f);
                //PlayVibrateAnimation(copiedPiece, 1f);

                piece.GetComponent<Piece>().ChangeTransparency(0f, true);
                piece.transform.localScale = Vector3.one * 0.0001f;
                int i2 = i;
                LeanTween.delayedCall(1f, () => { copiedPiece.GetComponent<Piece>().InitializeDecay(1f); });
                LeanTween.delayedCall(2f, () => { piece.GetComponent<Piece>().ChangeTransparencyInTime(1f, 1f); });
                LeanTween.scale(piece, Vector3.one, 1f).delay = 2f;
            }
            SoundScript thisSoundScript = GetComponent<SoundScript>();
            LeanTween.delayedCall(0.9f, thisSoundScript.PlayCubeDecaySound);
            workspace.transform.localScale = Vector3.one * 0.5f;
            GetComponent<SoundScript>().PlayCubeVibrateSound();

            duringRotationWorkspace = true;

            if (levelNotRandom)
            {
                levelMenu.LevelPassed(openedLevel);
                alertSpawner.SpawnText(openedLevel + 1);

                if (openedLevel == 1)
                {
                    TutorialScript.TurnOnNote(TutorialScript.Notes.LevelsExpl);
                    TutorialScript.TurnOnNote(TutorialScript.Notes.ThemesExpl);
                }

                // show next level button if current level isn't last
                if (openedLevel < levelMenu.GetAmountOfLevels(variant) - 1)
                {
                    CanvasGroup nextLvlCG = nextLevelBtn.GetComponent<CanvasGroup>();
                    nextLvlBtnAlpha = 1;
                    nextLvlCG.interactable = true;
                    nextLvlCG.blocksRaycasts = true;
                    nextLvlBtnVariant = variant;

                    nextLevelBtn.onClick.RemoveAllListeners();
                    nextLevelBtn.onClick.AddListener(() => HideNextLevelButton(false));
                }
            }
            else if (!gameFinishedAndRestarted)
            {
                SaveInfoState sis;
                switch (variant)
                {
                    case Variant.x3:
                        sis = saveScript.saveInfo3State;
                        break;
                    default:
                    case Variant.x4:
                        sis = saveScript.saveInfo4State;
                        break;
                }
                timeOfGame = Time.time - timeOfStart;
                if (sis.randomAverageTime != 0f)
                    sis.randomAverageTime = ((sis.randomAverageTime * (float)sis.randomGamesWon) + timeOfGame) / (float)(sis.randomGamesWon + 1);
                else
                    sis.randomAverageTime = timeOfGame;
                sis.randomGamesWon++;

                Material clockTextMaterial = clockText.fontMaterial;
                clockDefaultColor = clockTextMaterial.GetColor("_OutlineColor");
                clockDefaultThick = clockTextMaterial.GetFloat("_OutlineWidth");
                if (sis.randomShortestTime > timeOfGame || sis.randomShortestTime == 0f)
                {
                    sis.randomShortestTime = timeOfGame;
                    LeanTween.value(gameObject, 0f, 2f, shineTime).setOnUpdate((float val) => { PlayClockShineAnimation(clockTextMaterial, val, true); });
                }
                else
                {
                    LeanTween.value(gameObject, 0f, 2f, shineTime).setOnUpdate((float val) => { PlayClockShineAnimation(clockTextMaterial, val, false); });
                }

                saveScript.SaveInfoData();
                infoPanel.UpdateInfo();

            }

            finishedGame = true;
        }

        public class MissingDefaultWorkspaceRotationException : Exception
        {
            public override string Message => "Default workspace rotation hasn't been assigned, but is trying to be accessed";
        }
    }
}
