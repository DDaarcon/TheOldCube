using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using static Enums;

[RequireComponent (typeof (Button))]
public class HintScript : MonoBehaviour, IUnityAdsListener
{
    public class AdvSolution {
        public short variantInt;
        public Side placingTarget;
        public bool[,] bottom;
        public bool[,] back;
        public bool[,] left;
        public bool[,] right;
        public bool[,] front;
        public bool[,] top;
        public bool[,] GetBySide(Side side_) {
            switch (side_) {
                case Side.bottom:
                    return bottom;
                case Side.back:
                    return back;
                case Side.left:
                    return left;
                case Side.right:
                    return right;
                case Side.front:
                    return front;
                case Side.top:
                    return top;
                default:
                    return null;
            }
        }
        public void SetBySide(Side side_, bool[,] settingPerSide) {
            switch (side_) {
                case Side.bottom:
                    bottom = (bool[,])settingPerSide.Clone();
                    break;
                case Side.back:
                    back = (bool[,])settingPerSide.Clone();
                    break;
                case Side.left:
                    left = (bool[,])settingPerSide.Clone();
                    break;
                case Side.right:
                    right = (bool[,])settingPerSide.Clone();
                    break;
                case Side.front:
                    front = (bool[,])settingPerSide.Clone();
                    break;
                case Side.top:
                    top = (bool[,])settingPerSide.Clone();
                    break;
                default:
                    break;
            }
        }
        public void Set(AdvSolution solution) {
            variantInt = solution.variantInt;
            placingTarget = solution.placingTarget;
            SetBySide(Side.bottom, solution.bottom);
            SetBySide(Side.back, solution.back);
            SetBySide(Side.left, solution.left);
            SetBySide(Side.right, solution.right);
            SetBySide(Side.front, solution.front);
            SetBySide(Side.top, solution.top);
        }
        public void Set(bool[][,] solution) {
            variantInt = (short)solution[0].GetLength(0);
            SetBySide(Side.bottom, solution[0]);
            SetBySide(Side.back, solution[1]);
            SetBySide(Side.left, solution[2]);
            SetBySide(Side.right, solution[3]);
            SetBySide(Side.front, solution[4]);
            SetBySide(Side.top, solution[5]);

        }
        private void RotateClockwiseBySide(Side side_, int times) {
            bool[,] vertices = GetBySide(side_);
            times %= 4;

            if (times % 2 == 1) {
                for (int a = 0; a < variantInt / 2; a++) { // loop for each perimeter (square of vertices discarding the fill)
                    for (int b = 0; b < variantInt - 1 - a; b++) { // loop for each element on side of perimeter except of last one
                        bool temp = vertices[a + b, a];
                        // x increasing with a and b, y increasing with a
                        vertices[a + b, a] = vertices[variantInt - 1 - a, a + b];
                        // x decreasing with a, y increasing with a and b
                        vertices[variantInt - 1 - a, a + b] = vertices[variantInt - 1 - a - b, variantInt - 1 - a];
                        // x decreasing with a and b, y decreasing with a
                        vertices[variantInt - 1 - a - b, variantInt - 1 - a] = vertices[a, variantInt - 1 - a - b];
                        // x increasing with a, y decreasing with a and b
                        vertices[a, variantInt - 1 - a - b] = temp;
                    }
                }
                times--;
            }
            if (times == 2) {
                for (int a = 0; a < variantInt / 2; a++) { // loop for each perimeter
                    for (int b = 0; b < variantInt - 1 - a; b++) { // loop for each element on side of perimeter except of last one
                        bool temp = vertices[a + b, a];
                        // x increasing with a and b, y increasing with a
                        vertices[a + b, a] = vertices[variantInt - 1 - a - b, variantInt - 1 - a];
                        // x decreasing with a and b, y decreasing with a
                        vertices[variantInt - 1 - a - b, variantInt - 1 - a] = temp;
                        temp = vertices[variantInt - 1 - a, a + b];
                        // x decreasing with a, y increasing with a and b
                        vertices[variantInt - 1 - a, a + b] = vertices[a, variantInt - 1 - a - b];
                        // x increasing with a, y decreasing with a and b
                        vertices[a, variantInt - 1 - a - b] = temp;
                    }
                }
            }
        }
        public AdvSolution GetCopy() {
            AdvSolution rtn = new AdvSolution();
            rtn.Set(this);
            return rtn;
        }
        public bool[][,] GetArrayCopy() {
            bool[][,] rtn = new bool[6][,];
            rtn[0] = (bool[,])bottom.Clone();
            rtn[1] = (bool[,])back.Clone();
            rtn[2] = (bool[,])left.Clone();
            rtn[3] = (bool[,])right.Clone();
            rtn[4] = (bool[,])front.Clone();
            rtn[5] = (bool[,])top.Clone();
            return rtn;
        }
        /**
        <summary>Rotate solution clockwise around axis (only one axis per method call)</summary>
        <param name="timesX">Amount of rotations around X axis looking from right side</param>
        <param name="timesY">Amount of rotations around Y axis looking from top side</param>
        <param name="timesZ">Amount of rotations around Z axis looking from front side</param>
        **/
        public AdvSolution RotateClockwiseByCube(int timesX = 0, int timesY = 0, int timesZ = 0) {
            if ( (timesX != 0 && timesY != 0) || (timesY != 0 && timesZ != 0) || (timesX != 0 && timesZ != 0)) {
                Debug.Log("more than one axis for rotation");
                return this;
            }
            timesX %= 4; timesY %= 4; timesZ %= 4;

            if (timesZ % 2 == 1) {
                RotateClockwiseBySide(Side.front, 3);
                // RotateClockwiseBySide(Side.right, 1);
                bool[,] rightBackup = right;
                // RotateClockwiseBySide(Side.top, 1);
                right = top;
                // RotateClockwiseBySide(Side.left, 1);
                top = left;
                // RotateClockwiseBySide(Side.bottom, 1);
                left = bottom;
                bottom = rightBackup;
                RotateClockwiseBySide(Side.back, 1);
                if (placingTarget == Side.right) placingTarget = Side.bottom;
                else if (placingTarget == Side.bottom) placingTarget = Side.left;
                else if (placingTarget == Side.left) placingTarget = Side.top;
                else if (placingTarget == Side.top) placingTarget = Side.right;
                timesZ--;
            }
            if (timesZ == 2) {
                RotateClockwiseBySide(Side.front, 2);
                RotateClockwiseBySide(Side.back, 2);
                // RotateClockwiseBySide(Side.right, 2);
                bool[,] temp = right;
                // RotateClockwiseBySide(Side.left, 2);
                right = left;
                left = temp;
                // RotateClockwiseBySide(Side.top, 2);
                temp = top;
                // RotateClockwiseBySide(Side.bottom, 2);
                top = bottom;
                bottom = temp;
                if (placingTarget == Side.right) placingTarget = Side.left;
                else if (placingTarget == Side.left) placingTarget = Side.right;
                else if (placingTarget == Side.top) placingTarget = Side.bottom;
                else if (placingTarget == Side.bottom) placingTarget = Side.top;
            }

            if (timesY % 2 == 1) {
                RotateClockwiseBySide(Side.top, 3);
                RotateClockwiseBySide(Side.right, 1);
                bool[,] rightBackup = right;
                RotateClockwiseBySide(Side.back, 1);
                right = back;
                RotateClockwiseBySide(Side.left, 1);
                back = left;
                RotateClockwiseBySide(Side.front, 1);
                left = front;
                front = rightBackup;
                RotateClockwiseBySide(Side.bottom, 1);
                if (placingTarget == Side.right) placingTarget = Side.front;
                else if (placingTarget == Side.front) placingTarget = Side.left;
                else if (placingTarget == Side.left) placingTarget = Side.back;
                else if (placingTarget == Side.back) placingTarget = Side.right;
                timesY--;
            }
            if (timesY == 2) {
                RotateClockwiseBySide(Side.top, 2);
                RotateClockwiseBySide(Side.bottom, 2);
                RotateClockwiseBySide(Side.right, 2);
                bool[,] temp = right;
                RotateClockwiseBySide(Side.left, 2);
                right = left;
                left = temp;
                RotateClockwiseBySide(Side.front, 2);
                temp = front;
                RotateClockwiseBySide(Side.back, 2);
                front = back;
                back = temp;
                if (placingTarget == Side.right) placingTarget = Side.left;
                else if (placingTarget == Side.left) placingTarget = Side.right;
                else if (placingTarget == Side.front) placingTarget = Side.back;
                else if (placingTarget == Side.back) placingTarget = Side.front;
            }

            if (timesX % 2 == 1) {
                RotateClockwiseBySide(Side.right, 3);
                RotateClockwiseBySide(Side.top, 2);
                bool[,] topBackup = top;
                RotateClockwiseBySide(Side.front, 2);
                top = front;
                RotateClockwiseBySide(Side.bottom, 0);
                front = bottom;
                RotateClockwiseBySide(Side.back, 0);
                bottom = back;
                back = topBackup;
                RotateClockwiseBySide(Side.left, 1);
                if (placingTarget == Side.front) placingTarget = Side.top;
                else if (placingTarget == Side.bottom) placingTarget = Side.front;
                else if (placingTarget == Side.back) placingTarget = Side.bottom;
                else if (placingTarget == Side.top) placingTarget = Side.back;
                timesX--;
            }
            if (timesX == 2) {
                RotateClockwiseBySide(Side.right, 2);
                RotateClockwiseBySide(Side.left, 2);
                RotateClockwiseBySide(Side.front, 0);
                bool[,] temp = front;
                RotateClockwiseBySide(Side.back, 0);
                front = back;
                back = temp;
                RotateClockwiseBySide(Side.top, 2);
                temp = top;
                RotateClockwiseBySide(Side.bottom, 2);
                top = bottom;
                bottom = temp;
                if (placingTarget == Side.top) placingTarget = Side.bottom;
                else if (placingTarget == Side.bottom) placingTarget = Side.top;
                else if (placingTarget == Side.front) placingTarget = Side.back;
                else if (placingTarget == Side.back) placingTarget = Side.front;
            }
            return this;
        }
        /**
        <summary>Compare two AdvSolutions, return amount of sides that are the same</summary>
        **/
        public int Compare(AdvSolution solution) {
            int amount = 0;
            if (bottom.Cast<bool>().SequenceEqual(solution.bottom.Cast<bool>())) amount++;
            if (back.Cast<bool>().SequenceEqual(solution.back.Cast<bool>())) amount++;
            if (left.Cast<bool>().SequenceEqual(solution.left.Cast<bool>())) amount++;
            if (right.Cast<bool>().SequenceEqual(solution.right.Cast<bool>())) amount++;
            if (front.Cast<bool>().SequenceEqual(solution.front.Cast<bool>())) amount++;
            if (top.Cast<bool>().SequenceEqual(solution.top.Cast<bool>())) amount++;
            return amount;
        }
        public void DebugPrint(Side side_) {
            bool[,] vertices = GetBySide(side_);
            string log = "";
            for (int i = 0; i < variantInt; i ++) {
                for (int j = 0; j < variantInt; j++) {
                    log += vertices[i, j];
                    log += " ";
                }
                log += '\n';
            }
            Debug.Log(log);
        }
    }

    public GameScript gameManager;
    public ScreenOrientationScript screenOrientationScript;
    public TMP_Text hintTimer;
    public float timeForBottonToShow = 1f;
    public float timeAdditionIfFinished = 10f;
    public float timeAdditionIfSkipped = 5f;
    public float offlineHintWaitTime = 20f;

    private readonly string GooglePlay_GameID = "4007741";
    private readonly string myPlacementID = "rewardedVideo";
    [SerializeField] private bool adTestMode = true;
    
    public AdvSolution gameSolution {get; private set;} = new AdvSolution();
    public AdvSolution genrSolution {get; private set;} = new AdvSolution();
    private bool hintReady = false;
    private bool timerOn = true;
    private float offlineHintTimer = 0f;

    private Button button;

    private void Start() {
        button = GetComponent<Button>();
        SetButtonActiveState(!Advertisement.IsReady());
        Advertisement.Initialize(GooglePlay_GameID, adTestMode);
        Advertisement.AddListener(this);
    }

    private void ShowRewardedVideo() {
        if (Advertisement.IsReady(myPlacementID)) {
            gameManager.PauseTimer();
            Advertisement.Show(myPlacementID);
        } else {
            ApplyHintOffline();
        }
    }
    private void ApplyHintOffline() {
        GetHint();
        ApplyHint();
        SetButtonTimerState(true);
    }

    /*
    <summary>Apply a hint to game solution</summary>
    */
    private void GetHint() {
        if (gameManager.duringPlacing) {
            gameSolution.Set(gameManager.gameSolution);
            genrSolution.Set(gameManager.genrSolution);
            genrSolution.placingTarget = (Side)gameManager.buttonOrder[gameManager.placingPieceButtonIndex];
            // Debug.Log("Target side before searching: " + genrSolution.placingTarget);
            // gameManager.VisualizeDataFromSetting(genrSolution.GetBySide(genrSolution.placingTarget), "Piece being placed");


            List<AdvSolution> solutions = new List<AdvSolution>();
            List<int> similarities = new List<int>();
            bool foundBestMatch = false;
            int highestSimilarity = 0;

            // rotate generated solution every possible way or till found perfect match
            for (int i = 0; i < 6; i++) {
                // testing solution
                int similarity = gameSolution.Compare(genrSolution);
                if (highestSimilarity < similarity) highestSimilarity = similarity;
                solutions.Add(genrSolution.GetCopy());
                similarities.Add(similarity);
                if (similarity == gameManager.placedSides) {
                    foundBestMatch = true;
                    break;
                }
                
                for (int j = 0; j < 3; j++) {
                    genrSolution.RotateClockwiseByCube(timesZ: 1);
                    // testing solution
                    similarity = gameSolution.Compare(genrSolution);
                    if (highestSimilarity < similarity) highestSimilarity = similarity;
                    solutions.Add(genrSolution.GetCopy());
                    similarities.Add(similarity);
                    if (similarity == gameManager.placedSides) {
                        foundBestMatch = true;
                        break;
                    }
                }
                if (foundBestMatch) break;
                if (i != 2) genrSolution.RotateClockwiseByCube(timesX: 1);
                else genrSolution.RotateClockwiseByCube(timesY: 2);
            }

            // get index of best solution
            int finalIndex = 0;

            if (foundBestMatch) {
                finalIndex = similarities.Count - 1;
            } else {
                while (similarities[finalIndex] != highestSimilarity) {
                    finalIndex++;
                }
            }

            hintReady = true;
            genrSolution.Set(solutions[finalIndex]);

            // gameManager.VisualizeDataFromSolution(gameManager.gameSolution, "GameSolution");
            // gameManager.VisualizeDataFromSolution(solutions[finalIndex].GetArrayCopy(), "GenrSolution found by hint script");
            // Debug.Log("Similarities: " + similarities[finalIndex]);
            // Debug.Log("Amount of rotations: " + finalIndex);
            // Debug.Log("Target side: " + solutions[finalIndex].placingTarget);

            // gameManager.DebugNewGame(solutions[finalIndex].GetArrayCopy());
            // Side side = solutions[finalIndex].placingTarget;

            // gameManager.ForcePlacePieceAt(solutions[finalIndex].GetBySide(side), side);

        }
    }
    private void ApplyHint() {
        if (hintReady) {
            hintReady = false;
            Side side = genrSolution.placingTarget;
            gameManager.ForcePlacePieceAt(genrSolution.GetBySide(side), side);
        }
    }

    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
        
        if (showResult == ShowResult.Finished) {
            ApplyHint();
            gameManager.ResumeTimer(timeAdditionIfFinished);
            SetButtonActiveState(false);
        } else if (showResult == ShowResult.Skipped) {
            ApplyHint();
            gameManager.ResumeTimer(timeAdditionIfSkipped);
            SetButtonTimerState(false);
            ToastScript.AddMsg("Ad skipped.", 1f);
        } else if (showResult == ShowResult.Failed) {
            Debug.LogWarning("The ad did not finish due to an error.");
            ApplyHint();
            gameManager.ResumeTimer(timeAdditionIfFinished);
            SetButtonTimerState(false);
        }
    }

    public void OnUnityAdsReady (string placementId) {
        if (placementId == myPlacementID) {
            if (!timerOn) {
                SetButtonActiveState(false);
            }
        }
    }

    public void OnUnityAdsDidError (string message) {
        hintReady = false;
        gameManager.ResumeTimer();
    }

    public void OnUnityAdsDidStart (string placementId) {
        GetHint();
    } 

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy() {
        Advertisement.RemoveListener(this);
    }


    public void ShowHintButton() {
        GetComponent<CanvasGroup>().alpha = 0f;
        if (screenOrientationScript.screenOrientation == ScreenOrientation.Portrait) {
            LeanTween.moveX(this.GetComponent<RectTransform>(), 90f, timeForBottonToShow);
        }
        if (screenOrientationScript.screenOrientation == ScreenOrientation.Landscape) {
            LeanTween.moveY(this.GetComponent<RectTransform>(), 110f, timeForBottonToShow);
        }
        LeanTween.alphaCanvas(this.GetComponent<CanvasGroup>(), 1f, timeForBottonToShow);
        
    }

    public void RenewHint() {
        if (button == null) {
            button = GetComponent<Button>();
        }
        SetButtonActiveState(!Advertisement.IsReady());
    }
    private void SetButtonActiveState(bool offline) {
        timerOn = false;
        button.interactable = true;
        button.onClick.RemoveAllListeners();
        if (offline) {
            button.onClick.AddListener(ApplyHintOffline);
        } else {
            button.onClick.AddListener(ShowRewardedVideo);
        }
    }
    private void SetButtonTimerState(bool offline) {
        timerOn = true;
        button.interactable = false;
        offlineHintTimer = offlineHintWaitTime;
        button.onClick.RemoveAllListeners();
    }

    public void HideHintButton() {
        if (screenOrientationScript.screenOrientation == ScreenOrientation.Portrait) {
            LeanTween.moveX(this.GetComponent<RectTransform>(), -60f, timeForBottonToShow);
        }
        if (screenOrientationScript.screenOrientation == ScreenOrientation.Landscape) {
            LeanTween.moveY(this.GetComponent<RectTransform>(), -60f, timeForBottonToShow);
        }
        LeanTween.alphaCanvas(this.GetComponent<CanvasGroup>(), 0f, timeForBottonToShow);
    }

    private void Update() {
        if (timerOn) {
            if (offlineHintTimer > 0f) {
                hintTimer.gameObject.SetActive(true);
                offlineHintTimer -= Time.deltaTime;
                hintTimer.text = ((int)offlineHintTimer).ToString();
            } else {
                hintTimer.gameObject.SetActive(false);
                SetButtonActiveState(!Advertisement.IsReady());
            }
        }
    }
}
