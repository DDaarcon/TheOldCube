using System;
using UnityEngine;
using UnityEngine.UI;

using GameInfo;
using GameServices.Editor;
using GameServices.PlacedPieces;

using static Enums;

namespace GameServices.Gameplay
{
	public class GameplayInitialization
	{
		public GameplayInitialization() { }

        private GameInformation Information = GameInfoHolder.Information;

        [Header("Game and levels:")]
        public LevelMenu levelMenu;
        public HintScript hintScript;
        public AlertSpawner alertSpawner;
        public SeedInputField seedInputField;
        public Button nextLevelBtn;
        private byte nextLvlBtnAlpha = 0;
        private Variant nextLvlBtnVariant;
        private int openedLevel = 0;
        private bool levelNotRandom = false;
        private bool[] placedSidesFromSolution;
        private SaveScript saveScript;
        private bool randomGameBeforeStart = false;
        public bool finishedGame { get; private set; } = false; // might be useful for displaying time of game
        private bool gameFinishedAndRestarted = false;
        private bool tryingTheme = false;
        // private int sideToRotateTowardsIndex = 0;
        /**
        <value>Random rotation used for animating solved puzzle</value>
        **/
        private Quaternion randomRotationForWorkspace;

        private readonly PlacedPiecesService placedPieces = new();
        private readonly EditorEvents editorEvents = new();

        private void CommonGameStartBeginning()
        {
            placedPieces.RemoveAll();
            
            if (Information.EditorEnvironment.DuringPlacing)
            {
                editorEvents.AbortPlacing();
            }

            SetEmptyGameSolution();
            ResetClock();
            hintScript.RenewHint();
            HideNextLevelButton(true);

            finishedGame = false;
            timeOfGame = 0f;
            gameFinishedAndRestarted = false;
            duringRotationWorkspace = false;

            if (tryingTheme)
            {
                tryingTheme = false;
                gameTheme = Themes.BasicStone;
            }

            seedInputField.RenewData();

            LeanTween.cancel(workspace.gameObject);
            workspace.LeanScale(Vector3.one, 0f);
        }
        private void StartGameSuffix(bool autoHideMenu)
        {
            seedInputField.RenewData();
            workspace.rotation = defaultRotation;
            if (autoHideMenu) levelMenu.ToggleMenus(0, true)
        }
        public void StartNewRandomGame(bool autoHideMenu = true)
        {
            CommonGameStartBeginning();

            genrSolution = SolutionGenerator.GetNewSolution(variant);
            levelNotRandom = false;
            randomGameBeforeStart = true;
            clockText.CrossFadeAlpha(1f, 1f, false);

            RenewButtons();

            StartGameSuffix(autoHideMenu);
        }
        public void StartNewRandomGame(int seed, bool autoHideMenu = true)
        {
            CommonGameStartBeginning();

            genrSolution = SolutionGenerator.GetNewSolution(seed, variant);
            levelNotRandom = false;
            randomGameBeforeStart = true;
            clockText.CrossFadeAlpha(1f, 1f, false);

            RenewButtons();

            StartGameSuffix(autoHideMenu);
        }
        public void StartNewGame(int level, int seed, bool[] placedSides_, bool finished_)
        {
            CommonGameStartBeginning();

            genrSolution = SolutionGenerator.GetNewSolution(seed, variant);
            openedLevel = level;
            placedSidesFromSolution = placedSides_.Clone() as bool[];
            levelNotRandom = true;
            clockText.CrossFadeAlpha(0.25f, 1f, false);

            RenewButtons();

            finishedGame = finished_;

            for (int i = 0; i < 6; i++)
            {
                if (placedSides_[i] || finished_)
                {
                    PlacePieceAt(genrSolution[i], (Side)i, true);
                    int b = 0;
                    while (buttonOrder[b] != i) b++;
                    piecesButtonsIndexes[i] = b;
                    EnabledButton(b, false);
                }
            }

            if (openedLevel == 0)
            {
                TutorialScript.TurnOnNote(TutorialScript.Notes.ChooseFirstPieceNote);
            }
            if (openedLevel == 1)
            {
                TutorialScript.TurnOnNote(TutorialScript.Notes.RotateWorkspaceNote);
            }

            StartGameSuffix(true);
        }

        public void DebugLevel(int seed, bool[] placedSides_)
        {
            CommonGameStartBeginning();

            genrSolution = SolutionGenerator.GetNewSolution(seed, variant);
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
        }

#if UNITY_EDITOR
        public void DebugNewGame(bool[][,] solution)
        {
            CommonGameStartBeginning();

            levelNotRandom = true;
            finishedGame = true;

            OverrideGeneratedSolution(solution);
            VisualizeDataFromSolution(genrSolution, "GenrSolution before applying (DebugNewGame)");

            RenewButtons();

            for (int i = 0; i < 6; i++)
            {
                if (PlacePieceAt(genrSolution[i], (Side)i, true))
                {
                    int b = 0;
                    while (buttonOrder[b] != i) b++;
                    piecesButtonsIndexes[i] = b;
                    EnabledButton(b, false);
                }
            }
            VisualizeDataFromSolution(gameSolution, "GameSolution after applying genrSolution (DNG)");
        }
#endif
        public void RestartGame()
        {
            // neccessary when game is finished and then restarted
            bool finishedAndRestarted = false;
            if (finishedGame)
            {
                timeOfStart = Time.time - timeOfGame;
                finishedAndRestarted = true;
            }
            CommonGameStartBeginning();
            gameFinishedAndRestarted = finishedAndRestarted;

            for (int i = 0; i < 6; i++)
                EnabledButton(i, true);

            for (int i = 0; i < 6; i++)
            {
                if (levelNotRandom && placedSidesFromSolution[i])
                {
                    PlacePieceAt(genrSolution[i], (Side)i);
                    int b = 0;
                    while (buttonOrder[b] != i) b++;
                    EnabledButton(b, false);
                }
            }

            levelMenu.ToggleMenus(0, false);
        }
        private void FinishedLevel()
        {
            Debug.Log("Finish");

            GameObject[] finalPiecesCopy = new GameObject[6];
            for (int i = 0; i < 6; i++)
            {
                // make a copy, start decay animation
                finalPiecesCopy[i] = Instantiate(finalPieces[i], finalPieces[i].transform.position, finalPieces[i].transform.rotation);

                PiecePG piecePG;
                if (finalPiecesCopy[i].TryGetComponent<PiecePG>(out piecePG))
                {
                    piecePG.SetTheme(gameTheme, i);
                }

                finalPiecesCopy[i].GetComponent<Piece>().ChangeSetting(gameSolution[i]);
                finalPiecesCopy[i].GetComponent<Piece>().ChangeColor(finalPieces[i].GetComponent<Piece>().color);
                finalPiecesCopy[i].GetComponent<Piece>().ChangeColorInTime(colors[i], 1f);
                PlayVibrateAnimation(finalPiecesCopy[i], 1f);

                finalPieces[i].GetComponent<Piece>().ChangeTransparency(0f, true);
                finalPieces[i].transform.localScale = Vector3.one * 0.0001f;
                int i2 = i;
                LeanTween.delayedCall(1f, () => { finalPiecesCopy[i2].GetComponent<Piece>().InitializeDecay(1f); });
                LeanTween.delayedCall(2f, () => { finalPieces[i2].GetComponent<Piece>().ChangeTransparencyInTime(1f, 1f); });
                LeanTween.scale(finalPieces[i], Vector3.one, 1f).delay = 2f;
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
    }
}
