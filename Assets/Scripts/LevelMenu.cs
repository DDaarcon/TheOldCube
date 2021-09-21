using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using static Enums;

public class LevelMenu : MonoBehaviour
{
    enum OnLeft {Off, Pieces, Levels, Themes} 
    private OnLeft currentOnLeft;
    enum OnRight {Off, Wheels, Variants} 
    private OnRight currentOnRight;
    public GameScript gameManager;
    public ScreenOrientationScript screenOrientationScript;
    public SaveScript saveScript;
    public SoundScript soundScript;
    public GameObject piecesPanel;
    public GameObject themesPanel;
    public GameObject levelScroll;
    public GameObject wheelsPanel;
    public GameObject variantPanel;
    public Button hidePanelButton;
    public Button showPanelButton;
    public CanvasGroup operationButtonsVertical;
    public CanvasGroup operationButtonsHorizontal;
    public StartingPanel startingPanel;
    public TMP_Text clockText;
    private GameObject levelPanelContent, level4Panel, level3Panel;
    public RectTransform rightPanelRect;
    public GameObject levelCellTemplate;
    public RectTransform themesPanelControl;
    public RectTransform[] themeSelectPanels;
    private int themeSelectPanelShown = 0;
    private bool rightPanelInHideMode = false;
    public bool RightPanelInHideMode {
        get{return rightPanelInHideMode;} 
        private set{rightPanelInHideMode = value;}
    }
    public float fadeTime = 1f;
    public bool duringAnimation {get; private set;} = false;
    private ApplySettingToBtn[] pieceButtons;
    // private ThemeButton[] themeButtons;
    private List<GameObject> allLevels4 = new List<GameObject>();
    private List<GameObject> allLevels3 = new List<GameObject>();
    public static int finishedLevelsCounter = 0;

    public bool appearedOnStart {get; private set;} = false;
    private int preloadMenus = 2;
    // private bool wheelsPanelAutoHide = false;

    public void ToggleRightPanelDisplay() {
        RightPanelInHideMode = !RightPanelInHideMode;
        ToggleRightPanelDisplay(RightPanelInHideMode);
    }
    private void ToggleRightPanelDisplay(bool hide) {
        LTSeq seq = LeanTween.sequence();
        seq.append(() => {duringAnimation = true;});

        soundScript.PlayRightMenuMoveSound();

        if (hide) {
            showPanelButton.interactable = true;
            seq.append(() => {LeanTween.alphaCanvas(rightPanelRect.GetComponent<CanvasGroup>(), 0f, fadeTime * 1.7f); LeanTween.moveX(rightPanelRect, rightPanelRect.rect.width, fadeTime * 2f);});

        } else {
            showPanelButton.interactable = false;
            seq.append(() => {LeanTween.moveX(rightPanelRect, -30f, fadeTime * 2f);});
            seq.append(() => {LeanTween.alphaCanvas(rightPanelRect.GetComponent<CanvasGroup>(), 1f, fadeTime * 1.7f);});
            
        }
        seq.append(() => {duringAnimation = false;});
    }
    public void ToggleRightPanelHideFeatureOn(bool on) {
        if (on) {
            hidePanelButton.interactable = true;
            if (rightPanelInHideMode) {
                ToggleRightPanelDisplay(true);
            }
        } else {
            hidePanelButton.interactable = false;
            showPanelButton.interactable = false;
            LeanTween.moveX(rightPanelRect, -30f, fadeTime);
            LeanTween.alphaCanvas(rightPanelRect.GetComponent<CanvasGroup>(), 1f, fadeTime);
            // rightPanelInHideMode = false;
        }
    }

    public void InstantToggleMenus(int menu) {
        if (menu == 0 && currentOnLeft == OnLeft.Pieces) return;
        if (menu == 1 && currentOnLeft == OnLeft.Levels) return;
        if (menu == 2 && currentOnLeft == OnLeft.Themes) return;

        // if (menu > -1) {
        //     ToggleActive(menu);
        // } else {
        //     ToggleActive();
        // }
        ToggleActive(menu);

        HidePiecesInButtons(OnLeft.Pieces, !piecesPanel.activeSelf, 0f);
    }

    // used in buttons listeners
    public void ToggleMenus(int menu) {
        ToggleMenus(menu, false);
    }
    /**
    <summary>Main method for changing menus</summary>
    <param name="menu">0 for change into default piece selection menu, 1 for change between level selection / piece selection menus, 2 for change between theme selection / piece selection menus</param>
    <param name="delayWhenLevelMenu">Delay for 0.4 sec before hiding level selection menu</param>
    **/
    public void ToggleMenus(int menu, bool delayWhenLevelMenu) {
        OnLeft destinedOnLeft = OnLeft.Off;
        OnRight destinedOnRight = OnRight.Off;
        if (menu == 0) { 
            if (currentOnLeft == OnLeft.Pieces) return;
            else {
                destinedOnLeft = OnLeft.Pieces;
                destinedOnRight = OnRight.Wheels;
            }
        }
        if (menu == 1) {
            if (currentOnLeft == OnLeft.Levels) {
                destinedOnLeft = OnLeft.Pieces;
                destinedOnRight = OnRight.Wheels;
            }
            else {
                TutorialScript.TurnOffNote(TutorialScript.Notes.LevelsExpl);
                destinedOnLeft = OnLeft.Levels;
                destinedOnRight = OnRight.Variants;
            }
        }
        if (menu == 2) {
            if (currentOnLeft == OnLeft.Themes) {
                destinedOnLeft = OnLeft.Pieces;
                destinedOnRight = OnRight.Wheels;
            }
            else {
                TutorialScript.TurnOffNote(TutorialScript.Notes.ThemesExpl);
                destinedOnLeft = OnLeft.Themes;
                destinedOnRight = OnRight.Wheels;
            }
        }
        bool hidePiecesInButtons = currentOnLeft == OnLeft.Pieces;
        bool hideCubesInThemeButtons = currentOnLeft == OnLeft.Themes;

        

        soundScript.PlayMenuMoveSound();
        
        LTSeq seq = LeanTween.sequence();
        RectTransform thisRectT = GetComponent<RectTransform>();
        float width = thisRectT.rect.width;

        // prepare for hide (cancel placing, hide pieces, shadow rendering stop for theme buttons)
        seq.append(() => {duringAnimation = true;});
        if (/* menu == 1 ||  */hidePiecesInButtons) {
            gameManager.ClickedActionButton(false);
            seq.append(() => HidePiecesInButtons(OnLeft.Pieces, true, 0.1f));
            seq.append(0.1f);
        }
        if (hideCubesInThemeButtons) {
            gameManager.ClickedActionButton(false);
            seq.append(() => {
                HidePiecesInButtons(OnLeft.Themes, true, 0.1f);
            });
            seq.append(0.1f);
        }
        if (destinedOnLeft == OnLeft.Pieces && delayWhenLevelMenu) seq.append(0.4f);


        // hide panels
        if (screenOrientationScript.screenOrientation == ScreenOrientation.Portrait) {
            seq.append(() => {LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0f, fadeTime * 0.7f); LeanTween.moveX(thisRectT, -width, fadeTime);});
            if (destinedOnRight != currentOnRight) seq.append(() => {LeanTween.alphaCanvas(rightPanelRect.GetComponent<CanvasGroup>(), 0f, fadeTime * 0.7f); LeanTween.moveX(rightPanelRect, rightPanelRect.rect.width, fadeTime);});
            seq.append(fadeTime);
        }
        if (screenOrientationScript.screenOrientation == ScreenOrientation.LandscapeLeft ||
            screenOrientationScript.screenOrientation == ScreenOrientation.LandscapeRight) {
            seq.append(() => {LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0f, fadeTime * 0.7f); LeanTween.moveX(thisRectT, -width, fadeTime);});
            if (destinedOnRight != currentOnRight) seq.append(() => {LeanTween.alphaCanvas(rightPanelRect.GetComponent<CanvasGroup>(), 0f, fadeTime * 0.7f); LeanTween.moveX(rightPanelRect, rightPanelRect.rect.width, fadeTime);});
            seq.append(fadeTime);
        }

        // menu change
        seq.append(() => {
            ToggleActive(destinedOnLeft);
            ToggleActive(destinedOnRight);
        });
        


        // show panels and everything related (show pieces etc.)
        if (screenOrientationScript.screenOrientation == ScreenOrientation.Portrait) {
            seq.append(() => {
                LTDescr tw = LeanTween.moveX(thisRectT, 30f, fadeTime);
                if (destinedOnLeft == OnLeft.Themes) {
                    tw.setOnComplete(() => RecalculateThemeButtonsCams());
                }
            });
            seq.append(() => {
                if (!rightPanelInHideMode || levelScroll.activeSelf) {
                    LeanTween.moveX(rightPanelRect, -30f, fadeTime);
                }
                if (destinedOnLeft == OnLeft.Levels) {
                    showPanelButton.interactable = false;
                    hidePanelButton.interactable = false;
                } else {
                    showPanelButton.interactable = rightPanelInHideMode;
                    hidePanelButton.interactable = true;
                }
            });
            seq.append(fadeTime * 0.3f);
            seq.append(() => {LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 1f, fadeTime * 0.7f); LeanTween.alphaCanvas(rightPanelRect.GetComponent<CanvasGroup>(), 1f, fadeTime * 0.7f);});
            seq.append(fadeTime * 0.7f);
            if (destinedOnLeft == OnLeft.Pieces) seq.append(() => HidePiecesInButtons(OnLeft.Pieces, false, 0.1f));
            if (destinedOnLeft == OnLeft.Themes) seq.append(() => {
                // RecalculateThemeButtonsCams();
                HidePiecesInButtons(OnLeft.Themes, false, 0.1f);
            });
        }
        if (screenOrientationScript.screenOrientation == ScreenOrientation.LandscapeLeft ||
            screenOrientationScript.screenOrientation == ScreenOrientation.LandscapeRight) {

            seq.append(() => {
                LTDescr tw = LeanTween.moveX(thisRectT, 50f, fadeTime);
                LeanTween.moveX(rightPanelRect, -50f, fadeTime);
                if (destinedOnLeft == OnLeft.Themes) {
                    tw.setOnComplete(() => RecalculateThemeButtonsCams());
                }
            });
            seq.append(fadeTime * 0.3f);
            seq.append(() => {
                LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 1f, fadeTime * 0.7f);
                LeanTween.alphaCanvas(rightPanelRect.GetComponent<CanvasGroup>(), 1f, fadeTime * 0.7f);
            });
            seq.append(fadeTime * 0.7f);
            if (destinedOnLeft == OnLeft.Pieces) seq.append(() => HidePiecesInButtons(OnLeft.Pieces, false, 0.1f));
            if (destinedOnLeft == OnLeft.Themes) seq.append(() => {
                HidePiecesInButtons(OnLeft.Themes, false, 0.1f);
            });
        }
        seq.append(() => {duringAnimation = false;});
    }

    // used in buttons listeners
    public void ToggleVariant(int variantInt) {
        // important to pass as parameter int coresponding to variant (3 = x3, 4 = x4)
        if (gameManager.variant != (Variant)variantInt) {
            Button[] variantButtons = variantPanel.GetComponentsInChildren<Button>();
            for (int i = 0; i < variantButtons.Length; i++) {
                variantButtons[i].interactable = true;
            }
            gameManager.ChooseVariant((Variant)variantInt, true);
            SetActiveVariant((Variant)variantInt);
            List<GameObject> tempAllLevels = allLevels4;
            if (gameManager.variant == Variant.x3) tempAllLevels = allLevels3;
            for (int i = 0; i < tempAllLevels.Count; i++) {
                tempAllLevels[i].transform.localScale = Vector3.zero;
                tempAllLevels[i].LeanScale(Vector3.one, 0.2f);
            }
            variantButtons[variantInt - 3].interactable = false;
        }
    }

    private void HidePiecesInButtons(OnLeft menu, bool hide, float time) {
        if (menu == OnLeft.Pieces)
        for (int i = 0; i < 6; i++) {
            // buttons[i].piece.transform.localScale = Vector3.one * (hide ? 2f : 0f);
            LeanTween.scale(pieceButtons[i].piece.gameObject, hide ? Vector3.zero : Vector3.one * 2f, time);
        }
        if (menu == OnLeft.Themes) {
            ThemeButton[] themeButtonsInCurrentPanel = themeSelectPanels[themeSelectPanelShown].GetComponentsInChildren<ThemeButton>();
            if (!hide) {
                foreach (ThemeButton tb in themeButtonsInCurrentPanel) {
                    tb.randomRotationIsOn = true;
                }
            }
            for (int i = 0; i < themeButtonsInCurrentPanel.Length; i++) {
                themeButtonsInCurrentPanel[i].piecePG.transform.localScale = !hide ? Vector3.zero : Vector3.one * 5f;
                themeButtonsInCurrentPanel[i].imageShadow.transform.localScale = !hide ? Vector3.zero : Vector3.one;
                LeanTween.scale(themeButtonsInCurrentPanel[i].piecePG.gameObject, hide ? Vector3.zero : Vector3.one * 5f, time);
                LeanTween.scale(themeButtonsInCurrentPanel[i].imageShadow.gameObject, hide ? Vector3.zero : Vector3.one, time);
            }
            if (hide) {
                foreach (ThemeButton tb in themeButtonsInCurrentPanel) {
                    tb.randomRotationIsOn = false;
                }
            }
        }
    }


    private void ToggleActive(int menu) {
        if (menu == 2) {
            ToggleActive(OnLeft.Themes);
            ToggleActive(OnRight.Wheels);
        }
        if (menu == 1) {
            ToggleActive(OnLeft.Levels);
            ToggleActive(OnRight.Variants);
        }
        if (menu == 0) {
            ToggleActive(OnLeft.Pieces);
            ToggleActive(OnRight.Wheels);
        }
    }

    private void ToggleActive(OnRight onRight) {
        if (currentOnRight == onRight) return;
        switch (onRight) {
            case OnRight.Variants:
                wheelsPanel.SetActive(false);
                variantPanel.SetActive(true);
                break;
            case OnRight.Wheels:
                wheelsPanel.SetActive(true);
                variantPanel.SetActive(false);
                break;
        }
        currentOnRight = onRight;
    }
    private void ToggleActive(OnLeft onLeft) {
        if (currentOnLeft == onLeft) return;
        switch (onLeft) {
            case OnLeft.Pieces:
                piecesPanel.SetActive(true);
                levelScroll.SetActive(false);
                themesPanel.SetActive(false);
                SetActiveVariant(gameManager.variant);
                break;
            case OnLeft.Levels:
                piecesPanel.SetActive(false);
                levelScroll.SetActive(true);
                themesPanel.SetActive(false);
                break;
            case OnLeft.Themes:
                piecesPanel.SetActive(false);
                levelScroll.SetActive(false);
                themesPanel.SetActive(true);
                break;
        }
        currentOnLeft = onLeft;
    }
    private void SetActiveVariant(Variant variant) {
        level3Panel.SetActive(variant == Variant.x3);
        level4Panel.SetActive(variant == Variant.x4);
    }

    public void RecalculateThemeButtonsCams(int delayBy = 1) {
        foreach (ThemeButton button in themeSelectPanels[themeSelectPanelShown].GetComponentsInChildren<ThemeButton>()) {
            button.RecalculateUI(delayBy);
        }
    }

    private void ShowNextThemes(bool next) {
        if (themeSelectPanels.Length == 1) return;

        CanvasGroup prevShownCG = themeSelectPanels[themeSelectPanelShown].GetComponent<CanvasGroup>();
        HidePiecesInButtons(OnLeft.Themes, true, 0.2f);
        LeanTween.alphaCanvas(prevShownCG, 0f, 0.2f);
        prevShownCG.interactable = false;
        prevShownCG.blocksRaycasts = false;

        if (next) {
            if (++themeSelectPanelShown == themeSelectPanels.Length) themeSelectPanelShown = 0;
        }
        else {
            if (--themeSelectPanelShown < 0) themeSelectPanelShown = themeSelectPanels.Length - 1;
        }
        
        LeanTween.delayedCall(0.2001f, () => {
            prevShownCG.gameObject.SetActive(false);

            CanvasGroup shownCG = themeSelectPanels[themeSelectPanelShown].GetComponent<CanvasGroup>();
            shownCG.gameObject.SetActive(true);
            HidePiecesInButtons(OnLeft.Themes, false, 0.2f);
            RecalculateThemeButtonsCams();
            LeanTween.alphaCanvas(shownCG, 1f, 0.2f);
            shownCG.interactable = true;
            shownCG.blocksRaycasts = true;
        });
        
    }




    private void AddLevel(LevelSettings levelSettings, Variant variant) {
        bool finished = false;
        if (variant == Variant.x4){
            allLevels4.Add(Instantiate(levelCellTemplate, level4Panel.transform));
            ApplyLevelToBtn settingBtn = allLevels4[allLevels4.Count - 1].GetComponent<ApplyLevelToBtn>();
            settingBtn.lS = levelSettings;
            finished = settingBtn.lS.finished = saveScript.saveLevel4State.levelList[levelSettings.level].finished;
            settingBtn.GameManager = gameManager;
            settingBtn.PrepareButton();
        } else if (variant == Variant.x3) {
            allLevels3.Add(Instantiate(levelCellTemplate, level3Panel.transform));
            ApplyLevelToBtn settingBtn = allLevels3[allLevels3.Count - 1].GetComponent<ApplyLevelToBtn>();
            settingBtn.lS = levelSettings;
            finished = settingBtn.lS.finished = saveScript.saveLevel3State.levelList[levelSettings.level].finished;
            settingBtn.GameManager = gameManager;
            settingBtn.PrepareButton();
        }
        if (finished) finishedLevelsCounter++;
    }

    public int GetAmountOfLevels(Variant variant) {
        switch (variant) {
            default:
            case Variant.x4:
                return allLevels4.Count;
            case Variant.x3:
                return allLevels3.Count;
        }
    }

    public void LevelPassed(int level) {
        if (gameManager.variant == Variant.x4){
            allLevels4[level].GetComponent<ApplyLevelToBtn>().SetToFinished();
            saveScript.saveLevel4State.levelList[level].finished = true;
        } else if (gameManager.variant == Variant.x3) {
            allLevels3[level].GetComponent<ApplyLevelToBtn>().SetToFinished();
            saveScript.saveLevel3State.levelList[level].finished = true;
        }
        saveScript.SaveLevelData();
    }

    public bool IsLevelPassed(int level, Variant variant) {
        if (variant == Variant.x4) {
            return saveScript.saveLevel4State.levelList[level].finished;
        } else if (variant == Variant.x3) {
            return saveScript.saveLevel3State.levelList[level].finished;
        }
        return false;
    }

    public LevelSettings GetLevelSettings(int level, Variant variant) {
        if (variant == Variant.x4) {
            return allLevels4[level].GetComponent<ApplyLevelToBtn>().lS;
        } else if (variant == Variant.x3) {
            return allLevels3[level].GetComponent<ApplyLevelToBtn>().lS;
        }
        return null;
    }

    void Awake() 
    {
        levelPanelContent = levelScroll.transform.GetChild(0).gameObject;
        level4Panel = levelPanelContent.transform.GetChild(0).gameObject;
        level3Panel = levelPanelContent.transform.GetChild(1).gameObject;
        pieceButtons = piecesPanel.GetComponentsInChildren<ApplySettingToBtn>();
        // themeButtons = themesPanel.GetComponentsInChildren<ThemeButton>();

        // AddLevelToPredefined(123,        new bool[]{O, O, I, O, O, O}, 
        // AddLevelToPredefined(456,        new bool[]{O, O, O, O, I, I}, 
        // AddLevelToPredefined(1334016831, new bool[]{O, O, O, O, O, O}, 
        // AddLevelToPredefined(685913430,  new bool[]{O, O, O, O, O, O}, 
        // AddLevelToPredefined(829488405,  new bool[]{O, O, O, O, O, O}, 
        // AddLevelToPredefined(1404534178, new bool[]{O, O, O, O, O, O}, 
        // AddLevelToPredefined(652967443,  new bool[]{O, O, O, O, O, O}, 
        // AddLevelToPredefined(2051549478, new bool[]{O, O, O, O, O, O}, 
        // AddLevelToPredefined(45634234,   new bool[]{O, O, O, O, O, O}, 
        // AddLevelToPredefined(453426,     new bool[]{O, O, O, O, O, O},
        // AddLevelToPredefined(456,        new bool[]{I, I, I, I, O, O},
        // AddLevelToPredefined(456,        new bool[]{O, O, O, O, O, O},
        // AddLevelToPredefined(456,        new bool[]{O, O, O, O, O, O},
        // AddLevelToPredefined(123123,     new bool[]{O, O, O, O, O, O},
        // AddLevelToPredefined(5322334,    new bool[]{O, O, O, O, O, O},

        
        saveScript.LoadLevelData();
        foreach (var x in saveScript.saveLevel4Setting.levelList) {
            AddLevel(x, Variant.x4);
        }
        foreach (var x in saveScript.saveLevel3Setting.levelList) {
            AddLevel(x, Variant.x3);
        }
    }

    void Start()
    {
        // ToggleActive(-1);
        HidePiecesInButtons(OnLeft.Pieces, true, 0f);
        GetComponent<CanvasGroup>().alpha = 0f;
        rightPanelRect.GetComponent<CanvasGroup>().alpha = 0f;
        if (operationButtonsHorizontal.gameObject.activeSelf) {
            operationButtonsHorizontal.alpha = 0f;
            operationButtonsHorizontal.interactable = false;
        }
        if (operationButtonsVertical.gameObject.activeSelf) {
            operationButtonsVertical.alpha = 0f;
            operationButtonsVertical.interactable = false;
        }
        clockText.CrossFadeAlpha(0f, 0f, false);

        showPanelButton.onClick.AddListener(ToggleRightPanelDisplay);
        hidePanelButton.onClick.AddListener(ToggleRightPanelDisplay);

        EventTrigger.Entry entryPrevThemesBtn = new EventTrigger.Entry(), entryNextThemesBtn = new EventTrigger.Entry();
        entryNextThemesBtn.eventID = entryPrevThemesBtn.eventID = EventTriggerType.PointerDown;
        entryPrevThemesBtn.callback.AddListener((data) => ShowNextThemes(false));
        entryNextThemesBtn.callback.AddListener((data) => ShowNextThemes(true));

        themesPanelControl.GetChild(0).GetComponent<EventTrigger>().triggers.Add(entryPrevThemesBtn);
        themesPanelControl.GetChild(1).GetComponent<EventTrigger>().triggers.Add(entryNextThemesBtn);
    }


    void Update()
    {
        if (levelScroll.activeSelf && !duringAnimation){
            if (Input.touchCount == 1) {
                Touch touch = Input.GetTouch(0);

                if (screenOrientationScript.TouchableAreaInput(touch.position) && 
                    touch.phase == TouchPhase.Began
                ) ToggleMenus(0, false);
            }
            #if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0)) {
                if (screenOrientationScript.TouchableAreaInput(Input.mousePosition))
                    ToggleMenus(0, false);
            }
            #endif
        }
        if (!appearedOnStart) {
            /* if (startingPanel.alpha > 0.8f || screenOrientationScript.screenOrientationHasChanged)
            if (Input.touchCount > 0 || screenOrientationScript.screenOrientationHasChanged
            #if UNITY_EDITOR
            || Input.GetMouseButtonDown(0)
            #endif
            )  */
            if (startingPanel.removeBlur) {
                appearedOnStart = true;
                ToggleActive(0);
                HidePiecesInButtons(OnLeft.Pieces, true, 0f);
                LeanTween.sequence().append(0.6f).append(() => {HidePiecesInButtons(OnLeft.Pieces, false, 0.1f);});
                LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 1f, 1f);
                LeanTween.alphaCanvas(rightPanelRect.GetComponent<CanvasGroup>(), 1f, 1f);
                gameManager.RecalculateButtonsCams(0);
                LeanTween.alphaCanvas(operationButtonsHorizontal, 1f, 1f);
                operationButtonsHorizontal.interactable = true;
                LeanTween.alphaCanvas(operationButtonsVertical, 1f, 1f);
                operationButtonsVertical.interactable = true;
                clockText.CrossFadeAlpha(1f, 1f, false);
            }
        }
        if (levelScroll.activeSelf) {
            Vector2 sizeDelta = levelPanelContent.GetComponent<RectTransform>().sizeDelta;
            if (gameManager.variant == Variant.x4)
                sizeDelta.y += level4Panel.GetComponent<RectTransform>().sizeDelta.y;
            if (gameManager.variant == Variant.x3)
                sizeDelta.y += level3Panel.GetComponent<RectTransform>().sizeDelta.y;
            levelPanelContent.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        }
    }

    private void LateUpdate() {
        // preloading menus to avoid delay when openning them
        if (!appearedOnStart) {
            if (preloadMenus >= 0) ToggleActive(preloadMenus--);
        }
    }
}
