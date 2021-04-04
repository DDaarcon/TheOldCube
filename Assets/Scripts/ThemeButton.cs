using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using static Enums;

[RequireComponent(typeof(Button))]
public class ThemeButton : MonoBehaviour
{
    // private Rect rect;
    
    public GameScript gameManager;
    private SaveScript saveScript;
    public Themes themeToApply;
    [HideInInspector] public RenderTexture renderTextureShadow;
    // public Camera cameraShadow;
    public Camera cellCamera;
    public Image backgroundImage;
    // public RawImage rowImageShadow;
    public Image imageShadow;
    // public int shadowTextureDimension = 256;
    // private const int cellCameraDisplayedLayer = 8;
    public GameObject piecePG;
    public PiecePG pieceComponent {get; private set;}
    [HideInInspector] public bool randomRotationIsOn = false;
    private bool postFirstAppearance = false;
    private Quaternion randomRotationForPiece;
    // private Texture2D textureShadow;

    public static List<ThemeButton> instancesList = new List<ThemeButton>();

    public TMP_Text noteContent;
    private readonly string WHEN_ON = "On";
    private readonly string WHEN_LOCKED = "Locked";
    private readonly string WHEN_TRY_READY = "Try";
    private readonly string WHEN_TRYING = "Trying";
    private readonly string WHEN_READY = "Off";
    private readonly Vector2 noteSizeNormal = new Vector2(70f, 50f);
    private readonly Vector2 noteSizeLong = new Vector2(110f, 50f);
    [SerializeField] private string toUnlockMsg;


    public void Enabled(bool isEnabled) {
        if (!isEnabled) {
            pieceComponent.ChangeTransparency(0.2f);
            this.GetComponent<Button>().interactable = false;
            Color color = backgroundImage.color;
            color.a = 0.2f;
            backgroundImage.color = color;

        } else {
            pieceComponent.ChangeTransparency(1f);
            this.GetComponent<Button>().interactable = true;
            Color color = backgroundImage.color;
            color.a = 1f;
            backgroundImage.color = color;
        }
    }

    void Awake()
    {
        
    }
    public void RecalculateUI(int delayBy = 1) {
        GUIset = 1 - delayBy;
    }

    private void Start() {
        instancesList.Add(this);
        
        piecePG.transform.localScale = Vector3.one * 0.001f;
        pieceComponent = piecePG.GetComponent<PiecePG>();
        pieceComponent.ChangeSetting(new bool[,] {{I}});

        saveScript = gameManager.GetComponent<SaveScript>();

        randomRotationForPiece = Random.rotation;
    }

    private void OnDestroy() {
        instancesList.Remove(this);
    }

    // public void RenderIntoTexture() {
    //     if (renderTextureShadow == null) renderTextureShadow = new RenderTexture(shadowTextureDimension, shadowTextureDimension, 16, RenderTextureFormat.ARGB32);
    //     // if (!captureOfCube.IsCreated()) captureOfCube.Create();
    //     Destroy(textureShadow);

    //     cameraShadow.targetTexture = renderTextureShadow;
    //     cameraShadow.Render();
    //     RenderTexture.active = renderTextureShadow;

    //     textureShadow = new Texture2D(shadowTextureDimension, shadowTextureDimension, TextureFormat.ARGB32, false);
    //     textureShadow.ReadPixels(new Rect(0, 0, shadowTextureDimension, shadowTextureDimension), 0, 0, false);
    //     textureShadow.Apply();

    //     cameraShadow.targetTexture = null;
    //     RenderTexture.active = null;
        

    //     Destroy(renderTextureShadow);
    //     rowImageShadow.texture = textureShadow;
    // }

    public void SetProperNote() {
        SaveGameDataState dataState = saveScript.saveGameDataState;
        Button button = GetComponent<Button>();

        bool unlocked;
        switch (themeToApply) {
            default:
            case Themes.BasicStone:
                unlocked = true;
                break;
            case Themes.FancyStone:
                unlocked = true;
                break;
            case Themes.Gold:
                unlocked = dataState.goldTheme.unlocked;
                // postTry = dataState.goldThemePostTry;
                if (!unlocked) {
                    if (saveScript.saveInfo4State.randomGamesWon >= 50) {
                        unlocked = true;
                        dataState.goldTheme.Unlock();
                    }
                }
                break;
            case Themes.Minecraft:
                unlocked = dataState.minecrafTheme.unlocked;
                // postTry = dataState.minecraftThemePostTry;
                if (!unlocked) {
                    if (LevelMenu.finishedLevelsCounter >= 120) {
                        unlocked = true;
                        dataState.minecrafTheme.Unlock();
                    }
                }
                break;
            case Themes.RedShiny:
                unlocked = dataState.redShinyTheme.unlocked;
                // postTry = dataState.redShinyThemePostTry;
                if (!unlocked) {
                    if (saveScript.saveInfo3State.randomGamesWon >= 50) {
                        unlocked = true;
                        dataState.redShinyTheme.Unlock();
                    }
                }
                break;
            case Themes.Copper:
                unlocked = dataState.copperTheme.unlocked;
                // postTry = dataState.copperThemePostTry;
                if (!unlocked) {
                    if (saveScript.saveInfo4State.randomGamesWon > 0 &&
                        saveScript.saveInfo4State.randomShortestTime < 40f) {
                        unlocked = true;
                        dataState.copperTheme.Unlock();
                    }
                }
                break;
            case Themes.BlueShiny:
                unlocked = dataState.blueShinyTheme.unlocked;
                // postTry = dataState.blueShinyThemePostTry;
                if (!unlocked) {
                    if (dataState.blueShinyTheme.adsLeft <= 0) {
                        unlocked = true;
                        dataState.blueShinyTheme.Unlock();
                    }
                }
                break;
            case Themes.DarkElement:
                unlocked = dataState.darkElementTheme.unlocked;
                // postTry = dataState.darkElementThemePostTry;
                if (!unlocked) {
                    if (dataState.darkElementTheme.adsLeft <= 0) {
                        unlocked = true;
                        dataState.darkElementTheme.Unlock();
                    }
                }
                break;
            case Themes.TicTacToe:
                unlocked = dataState.ticTacToeTheme.unlocked;
                // postTry = dataState.ticTacToeThemePostTry;
                if (!unlocked) {
                    if (dataState.ticTacToeTheme.adsLeft <= 0) {
                        unlocked = true;
                        dataState.ticTacToeTheme.Unlock();
                    }
                }
                break;
        }

        button.onClick.RemoveAllListeners();

        if (unlocked) {
            if (gameManager.gameTheme == themeToApply) { // current active
                noteContent.text = WHEN_ON;
                noteContent.transform.parent.GetComponent<RectTransform>().sizeDelta = noteSizeNormal;
            } else { // ready to apply
                noteContent.text = WHEN_READY;
                noteContent.transform.parent.GetComponent<RectTransform>().sizeDelta = noteSizeNormal;
                button.onClick.AddListener(() => {
                    gameManager.ChangeTheme(themeToApply);
                    foreach (ThemeButton tb in instancesList) {
                        tb.SetProperNote();
                    }
                });
            }
        } else {
            if (themeToApply == Themes.BlueShiny || themeToApply == Themes.DarkElement || themeToApply == Themes.TicTacToe) { // show add on touch
                if (themeToApply == Themes.BlueShiny)
                    noteContent.text = dataState.blueShinyTheme.adsLeft + " Ads";
                if (themeToApply == Themes.DarkElement)
                    noteContent.text = dataState.darkElementTheme.adsLeft + " Ads";
                if (themeToApply == Themes.TicTacToe)
                    noteContent.text = dataState.ticTacToeTheme.adsLeft + " Ads";
                noteContent.transform.parent.GetComponent<RectTransform>().sizeDelta = noteSizeLong;
                button.onClick.AddListener(() => gameManager.GetComponent<UnlockingThemesViaAds>().ShowAd(this));
            } else { // is locked
                noteContent.text = WHEN_LOCKED;
                noteContent.transform.parent.GetComponent<RectTransform>().sizeDelta = noteSizeLong;
                button.onClick.AddListener(() => ToastScript.AddMsg(toUnlockMsg, 1f));
            }

        }
        // saveScript.SaveGameData();
    }

    void OnGUI() {
        
    }
    private int GUIset = 0;
    void Update()
    {
        if (randomRotationIsOn) {
            if (!postFirstAppearance) {
                postFirstAppearance = true;
                SetProperNote();
            }

            // RenderIntoTexture();

            Quaternion preedit = piecePG.transform.rotation;
            Quaternion destinedRotation = randomRotationForPiece;

            piecePG.transform.rotation = Quaternion.SlerpUnclamped(piecePG.transform.rotation, destinedRotation, Time.deltaTime / (Quaternion.Angle(piecePG.transform.rotation, destinedRotation) + 1) * 20f);
            if (piecePG.transform.rotation == preedit) {
                randomRotationForPiece = Random.rotation;
            }
        } else {
            postFirstAppearance = false;
        }
        
        if (GUIset < 2) {
            if (GUIset == 1) {

                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                Canvas canvas = GetComponentInParent<Canvas>();
                screenPos -= (new Vector3(125f - 12.5f, 125f - 12.5f, 0f) * canvas.scaleFactor);
                cellCamera.rect = new Rect(screenPos.x / Screen.width, 
                    (screenPos.y + 10f)/ Screen.height, 
                    (250f - 25f)/ Screen.width * canvas.scaleFactor, 
                    (250f - 25f)/ Screen.height * canvas.scaleFactor);
                
            }
            GUIset++;
        }
    }
}
