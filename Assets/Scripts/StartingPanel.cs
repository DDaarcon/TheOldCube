using UnityEngine;
using UnityEngine.UI;

public class StartingPanel : MonoBehaviour
{
    /* 
        color, blur > camera, alpha
     */
    
    public GameScript gameManager;

    public SuperBlur.SuperBlurFast UICamera;
    public LevelMenu levelMenu;
    public Image blurImage;
    [Range(0.0f, 1.0f)] public float startBlur;
    private Color startColor;
    public bool removeBlur {get; private set;} = false;
    private bool removeColor = false;
    private bool alphaRemoved = false;
    public ScreenOrientationScript screenOrientationScript;

    private void Awake() {
        // blurMaterial = blurImage.material;
        // blurMaterial.SetFloat(blurIntensityProperity, startBlur);
        UICamera.interpolation = startBlur;

    #if !UNITY_EDITOR
        if (new AndroidJavaClass("android.os.Build$VERSION").GetStatic<int>("SDK_INT") >= 28) {
            UICamera.kernelSize = SuperBlur.BlurKernelSize.Medium;
            // Shader.EnableKeyword("HIGHER_RESOLUTION_SHADOW_ON_THEMES");
        } else {
            UICamera.kernelSize = SuperBlur.BlurKernelSize.Small;
        }
    #else 
        UICamera.kernelSize = SuperBlur.BlurKernelSize.Small;
    #endif

        // reveal starting image
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        LeanTween.alphaCanvas(canvasGroup, 1f, 1f);
    }
    private void Update() {
        if (GetComponent<CanvasGroup>().alpha > 0.8f || screenOrientationScript.screenOrientationHasChanged)
        if (Input.touchCount > 0 || screenOrientationScript.screenOrientationHasChanged
            #if UNITY_EDITOR
            || Input.GetMouseButtonDown(0)
            #endif
            ) {
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            LeanTween.alphaCanvas(canvasGroup, 0f, 1f).setOnComplete(() => {alphaRemoved = true;});

            startColor = blurImage.color;
            removeColor = true;
            removeBlur = true;

            // initialize game
            gameManager.InitializeApplication();
        }
        if (removeBlur) {
            UICamera.interpolation -= (startBlur * (Time.deltaTime / 1f));
            if (UICamera.interpolation <= 0f) {
                UICamera.interpolation = 0f;
                removeBlur = false;
            }
        }
        if (removeColor) {
            blurImage.color += (Color.white - startColor) * (Time.deltaTime / 1f);
            if (blurImage.color.r >= Color.white.r &&
                blurImage.color.g >= Color.white.g &&
                blurImage.color.b >= Color.white.b) {
                removeColor = false;
            }
        }
        if (!removeBlur && !removeColor && alphaRemoved && levelMenu.appearedOnStart) {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy() {
        // TutorialScript.TurnOnNote(TutorialScript.Notes.ChooseFirstPieceNote);
        // TutorialScript.TurnOnNote(TutorialScript.Notes.RotateWorkspaceNote);
    }

}
