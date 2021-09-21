using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Enums;

public class InfoPanel : MonoBehaviour
{
    public ScreenOrientationScript screenOrientationScript;
    public SaveScript saveScript;
    public RectTransform clockPanel;
    public float time = 0.3f;
    public bool infoPanelVisible {get; private set;} = false;

    /**
    <summary>Show and hide info panel</summary>
    **/
    public void ToggleInfoVisibility() {
        float posx = this.GetComponent<RectTransform>().anchoredPosition.x;
        float posy = this.GetComponent<RectTransform>().anchoredPosition.y;
        float width = this.GetComponent<RectTransform>().rect.width;
        float height = this.GetComponent<RectTransform>().rect.height;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        ScreenOrientation orientation = screenOrientationScript.screenOrientation;
        if (!infoPanelVisible) {
            infoPanelVisible = true;
            UpdateInfo();
            if (orientation == ScreenOrientation.Portrait) {
                LeanTween.moveX(this.GetComponent<RectTransform>(), 25f, time);
                LeanTween.scale(clockPanel.gameObject, Vector3.one * 0.4f, time);
                LeanTween.moveY(clockPanel, -height, time);
            }
            if (orientation == ScreenOrientation.LandscapeLeft ||
                orientation == ScreenOrientation.LandscapeRight) {
                LeanTween.moveY(this.GetComponent<RectTransform>(), -30f, time);
            }
            LeanTween.alphaCanvas(canvasGroup, 1f, time);
        } else {
            infoPanelVisible = false;
            if (orientation == ScreenOrientation.Portrait) {
                LeanTween.moveX(this.GetComponent<RectTransform>(), -width, time);
                LeanTween.scale(clockPanel.gameObject, Vector3.one, time);
                LeanTween.moveY(clockPanel, 0f, time);
            }
            if (orientation == ScreenOrientation.LandscapeLeft ||
                orientation == ScreenOrientation.LandscapeRight) {
                LeanTween.moveY(this.GetComponent<RectTransform>(), height, time);
            }
            LeanTween.alphaCanvas(canvasGroup, 0f, time / 2f);
        }
    }
    /**
    <summary>Force show or hide of info panel</summary>
    <param name="whichStateToForce">true for force show, false for force hide</param>
    **/
    public void ToggleInfoVisibility(bool whichStateToForce) {
        if (whichStateToForce) {
            if (!infoPanelVisible) {
                ToggleInfoVisibility();
            }
        } else {
            if (infoPanelVisible) {
                ToggleInfoVisibility();
            }
        }
    }
    
    /**
    <summary>Fixing boolean representation of info panel visibility (used only when screen orientation is changed)</summary>
    **/
    public void FixBoolState() {
        infoPanelVisible = false;
    }

    /**
    <summary>Update information in info panel</summary>
    **/
    public void UpdateInfo() {
        TMP_Text variantText = transform.GetChild(0).GetComponent<TMP_Text>();
        Text[] texts = transform.GetChild(1).GetComponentsInChildren<Text>();
        SaveInfoState sis;

        switch (saveScript.GetComponent<GameScript>().variant) {
            case Variant.x3:
                sis = saveScript.saveInfo3State;
                break;
            default:
            case Variant.x4:
                sis = saveScript.saveInfo4State;
                break;
        }
        int variantInt = (int)saveScript.GetComponent<GameScript>().variant;
        variantText.text = variantInt + "x" + variantInt;
        texts[0].text =     "Random Games Won:   \t" + sis.randomGamesWon;
        if (sis.randomGamesWon > 0) {
            texts[1].text = "Random Game Record: \t" + (sis.randomShortestTime / 60 < 10 ? " " : "") + (int)(sis.randomShortestTime / 60) + ":" +
                            (sis.randomShortestTime % 60 < 10 ? "0" : "") + (int)sis.randomShortestTime % 60;
            texts[2].text = "Random Game Average:\t" + (sis.randomAverageTime / 60 < 10 ? " " : "") + (int)(sis.randomAverageTime / 60) + ":" +
                            (sis.randomAverageTime % 60 < 10 ? "0" : "") + (int)sis.randomAverageTime % 60;
        } else {
            texts[1].text = texts[2].text = "";
        }
    }

    void Awake() {
        saveScript.LoadInfoData();
    }
}
