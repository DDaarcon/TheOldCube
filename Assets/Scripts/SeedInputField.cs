using UnityEngine;
using UnityEngine.UI;
using System;

public class SeedInputField : MonoBehaviour
{
    public GameObject debugLevelPanel;
    public GameScript gameManager;
    public bool enableLevelDebuging = true;
    private bool pointerIsDown = false;
    private bool pointerWasDown = false;
    private bool changedAtThisTouch = false;
    private float timeOfStart = 0f;
    public float lengthOfPress = 2f;
    public bool[] sides;
    public Toggle[] sidesToggles;

    public void PointerDown() {
        pointerIsDown = true;
    }
    public void PointerAway() {
        pointerIsDown = false;
    }

    public void InsertSeed() {
        int seedInt = Int32.Parse(debugLevelPanel.GetComponentInChildren<InputField>().text);
        gameManager.DebugLevel(seedInt, sides);
        gameManager.GetComponent<SoundScript>().PlayRandomDiceSound();
    }

    public void ToggleSides() {
        for (int i = 0; i < 6; i++) {
            sides[i] = sidesToggles[i].isOn;
        }
    }

    private void SetInputFieldActive() {
        if (enableLevelDebuging) {
            Debug.Log("onoff");
            bool a = debugLevelPanel.activeSelf;
            debugLevelPanel.SetActive(!debugLevelPanel.activeSelf);
            if (!a) RenewData();
        }
    }
    public void RenewData() {
        debugLevelPanel.GetComponentInChildren<InputField>().text = SolutionGenerationAlgorithm.SeedOfLast.ToString();
        sides = new bool[6];
        ToggleSides();
    }

    void Update()
    {
        if (pointerIsDown && !pointerWasDown) {
            timeOfStart = Time.time;
            changedAtThisTouch = false;
        }
        if (pointerIsDown && pointerWasDown) {
            if (Time.time - timeOfStart >= lengthOfPress && !changedAtThisTouch) {
                SetInputFieldActive();
                changedAtThisTouch = true;
            }
        }
        if (!pointerIsDown && pointerWasDown) {
            if (Time.time - timeOfStart < lengthOfPress) {
                gameManager.GetComponent<SoundScript>().PlayRandomDiceSound();
                gameManager.StartNewRandomGame();
            }
        }
        pointerWasDown = pointerIsDown;
    }
}
