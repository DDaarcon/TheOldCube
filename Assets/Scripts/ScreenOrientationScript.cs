using UnityEngine;
using UnityEngine.UI;

public class ScreenOrientationScript : MonoBehaviour
{
    [Header("For Scripts:")]
    public GameScript gameManager;
    public CanvasScaler canvasOfMainUI;
    public CanvasScaler canvasOfTwoButtons;
    public LevelMenu levelMenu;
    public SeedInputField seedInputField;
    public SteeringWheel positionWheel;
    public SteeringWheel rotationWheel;
    public WorkspaceRotation workspaceRotation;
    public ParticleSpawnerForDelete particleSpawnerForDelete;
    public HintScript hintScript;
    public CanvasGroup tutorialPanel;



    private class RectTransformData {
        public Vector3 localPosition;
        public Vector2 anchoredPosition;
        public Vector2 sizeDelta;
        public Vector2 anchorMax;
        public Vector2 anchorMin;
        public Vector2 pivot;
        public Vector3 localScale;
        public Quaternion localRotation;

        public void GetFromRectTransform(RectTransform rectTransform) {
            if (rectTransform != null) {
                localPosition = rectTransform.localPosition;
                anchoredPosition = rectTransform.anchoredPosition;
                sizeDelta = rectTransform.sizeDelta;
                anchorMax = rectTransform.anchorMax;
                anchorMin = rectTransform.anchorMin;
                pivot = rectTransform.pivot;
                localScale = rectTransform.localScale;
                localRotation = rectTransform.localRotation;
            }
        }

        public void PushToRectTransform(RectTransform rectTransform) {
            if (rectTransform != null) {
                rectTransform.localPosition = localPosition;
                rectTransform.anchoredPosition = anchoredPosition;
                rectTransform.sizeDelta = sizeDelta;
                rectTransform.anchorMax = anchorMax;
                rectTransform.anchorMin = anchorMin;
                rectTransform.pivot = pivot;
                rectTransform.localScale = localScale;
                rectTransform.localRotation = localRotation;
            }
        }

        public void GetFromArgs(
            Vector3 localPosition_, Vector2 anchoredPosition_, Vector2 sizeDelta_, Vector2 anchorMax_,
            Vector2 anchorMin_, Vector2 pivot_, Vector3 localScale_, Quaternion localRotation_) 
        {
            localPosition = localPosition_;
            anchoredPosition = anchoredPosition_;
            sizeDelta = sizeDelta_;
            anchorMax = anchorMax_;
            anchorMin = anchorMin_;
            pivot = pivot_;
            localScale = localScale_;
            localRotation = localRotation_;
        }

        public void DebugPrintData() {
            Debug.Log(
                "new Vector3(" + localPosition.x.ToString().Replace(',', '.') + "f, " + localPosition.y.ToString().Replace(',', '.') + "f, " + localPosition.z.ToString().Replace(',', '.') + "f)" + ", " +
                "new Vector2(" + anchoredPosition.x.ToString().Replace(',', '.') + "f, " + anchoredPosition.y.ToString().Replace(',', '.') + "f)" + ", " +
                "new Vector2(" + sizeDelta.x.ToString().Replace(',', '.') + "f, " + sizeDelta.y.ToString().Replace(',', '.') + "f)" + ", " +
                "new Vector2(" + anchorMax.x.ToString().Replace(',', '.') + "f, " + anchorMax.y.ToString().Replace(',', '.') + "f)" + ", " +
                "new Vector2(" + anchorMin.x.ToString().Replace(',', '.') + "f, " + anchorMin.y.ToString().Replace(',', '.') + "f)" + ", " +
                "new Vector2(" + pivot.x.ToString().Replace(',', '.') + "f, " + pivot.y.ToString().Replace(',', '.') + "f)" + ", " +
                "new Vector3(" + localScale.x.ToString().Replace(',', '.') + "f, " + localScale.y.ToString().Replace(',', '.') + "f, " + localScale.z.ToString().Replace(',', '.') + "f)" + ", " +
                "new Quaternion(" + localRotation.x.ToString().Replace(',', '.') + "f, " + localRotation.y.ToString().Replace(',', '.') + "f, " + localRotation.z.ToString().Replace(',', '.') + "f, " + localRotation.w.ToString().Replace(',', '.') + "f)"
            );
        }
    }

    struct ParticleFinishSettings {
        public float camDis;
        public float partDis;
        public float camDisRange;
        public ParticleFinishSettings(float a, float b, float c) {camDis = a; partDis = b; camDisRange = c;}
    }

    [Space(40)]
    public RectTransform leftPanel;
    public RectTransform rightPanel;
    public RectTransform operationButtonsVertical;
    public RectTransform operationButtonsHorizontal;
    public RectTransform hintButton;
    public RectTransform touchableArea;
    public RectTransform seedInput;
    public RectTransform infoPanel;
    public RectTransform timerPanel;
    public RectTransform twoButtonsPanel;
    public RectTransform startingPanel;
    public RectTransform alertSpawner;
    public RectTransform toastMessage;
    public RectTransform nextLevelButton;
    public RectTransform background;
    public Transform workspace;
    public ParticleSpawnerForFinish particleSpawnerForFinish;


    private RectTransformData leftPanel_P = new RectTransformData(), 
    rightPanel_P = new RectTransformData(), 
    operationButtons_P = new RectTransformData(),
    hintButton_P = new RectTransformData(),
    touchableArea_P = new RectTransformData(), 
    seedInput_P = new RectTransformData(), 
    infoPanel_P = new RectTransformData(), 
    timerPanel_P = new RectTransformData(), 
    twoButtonsPanel_P = new RectTransformData(),
    // startingPanel_P = new RectTransformData(),
    alertSpawner_P = new RectTransformData(),
    toastMessage_P = new RectTransformData(),
    nextLevelButton_P = new RectTransformData(),
    background_P = new RectTransformData();
    private Vector3 workspacePosition_P;
    private ParticleFinishSettings particleFinishSettings_P;

    private RectTransformData leftPanel_L = new RectTransformData(), 
    rightPanel_L = new RectTransformData(), 
    operationButtons_L = new RectTransformData(), 
    hintButton_L = new RectTransformData(),
    touchableArea_L = new RectTransformData(), 
    seedInput_L = new RectTransformData(), 
    infoPanel_L = new RectTransformData(), 
    timerPanel_L = new RectTransformData(), 
    twoButtonsPanel_L = new RectTransformData(),
    // startingPanel_L = new RectTransformData(),
    alertSpawner_L = new RectTransformData(),
    toastMessage_L = new RectTransformData(),
    nextLevelButton_L = new RectTransformData(),
    background_L = new RectTransformData();
    private Vector3 workspacePosition_L;
    private ParticleFinishSettings particleFinishSettings_L;

    public int screenHeight {get; private set;}
    public int screenWidth {get; private set;}
    public DeviceOrientation deviceOrientation {get; private set;}
    public ScreenOrientation screenOrientation {get; private set;}
    public bool screenOrientationHasChanged {get; private set;} = false;
    private bool autoRotateOnAndroid = true;

    private void RectTransformsToData(ScreenOrientation orientation) {
        if (orientation == ScreenOrientation.Portrait) {
            leftPanel_P.GetFromRectTransform(leftPanel);
            rightPanel_P.GetFromRectTransform(rightPanel);
            operationButtons_P.GetFromRectTransform(operationButtonsVertical);
            touchableArea_P.GetFromRectTransform(touchableArea);
            seedInput_P.GetFromRectTransform(seedInput);
            infoPanel_P.GetFromRectTransform(infoPanel);
            timerPanel_P.GetFromRectTransform(timerPanel);
            twoButtonsPanel_P.GetFromRectTransform(twoButtonsPanel);
            // startingPanel_P.GetFromRectTransform(startingPanel);
            alertSpawner_P.GetFromRectTransform(alertSpawner);
        }
        if (orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.LandscapeRight) {
            leftPanel_L.GetFromRectTransform(leftPanel);
            rightPanel_L.GetFromRectTransform(rightPanel);
            operationButtons_L.GetFromRectTransform(operationButtonsHorizontal);
            touchableArea_L.GetFromRectTransform(touchableArea);
            seedInput_L.GetFromRectTransform(seedInput);
            infoPanel_L.GetFromRectTransform(infoPanel);
            timerPanel_L.GetFromRectTransform(timerPanel);
            twoButtonsPanel_L.GetFromRectTransform(twoButtonsPanel);
            // startingPanel_L.GetFromRectTransform(startingPanel);
            alertSpawner_L.GetFromRectTransform(alertSpawner);
        }
    }
    private void DataToRectTransforms(ScreenOrientation orientation) {
        if (orientation == ScreenOrientation.Portrait) {
            leftPanel_P.PushToRectTransform(leftPanel);
            rightPanel_P.PushToRectTransform(rightPanel);
            operationButtons_P.PushToRectTransform(operationButtonsVertical);
            hintButton_P.PushToRectTransform(hintButton);
            touchableArea_P.PushToRectTransform(touchableArea);
            seedInput_P.PushToRectTransform(seedInput);
            infoPanel_P.PushToRectTransform(infoPanel);
            timerPanel_P.PushToRectTransform(timerPanel);
            twoButtonsPanel_P.PushToRectTransform(twoButtonsPanel);
            // startingPanel_P.PushToRectTransform(startingPanel);
            alertSpawner_P.PushToRectTransform(alertSpawner);
            toastMessage_P.PushToRectTransform(toastMessage);
            nextLevelButton_P.PushToRectTransform(nextLevelButton);
        }
        if (orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.LandscapeRight) {
            leftPanel_L.PushToRectTransform(leftPanel);
            rightPanel_L.PushToRectTransform(rightPanel);
            operationButtons_L.PushToRectTransform(operationButtonsHorizontal);
            hintButton_L.PushToRectTransform(hintButton);
            touchableArea_L.PushToRectTransform(touchableArea);
            seedInput_L.PushToRectTransform(seedInput);
            infoPanel_L.PushToRectTransform(infoPanel);
            timerPanel_L.PushToRectTransform(timerPanel);
            twoButtonsPanel_L.PushToRectTransform(twoButtonsPanel);
            // startingPanel_L.PushToRectTransform(startingPanel);
            alertSpawner_L.PushToRectTransform(alertSpawner);
            toastMessage_L.PushToRectTransform(toastMessage);
            nextLevelButton_L.PushToRectTransform(nextLevelButton);
        }
    }
    private void DebugPrintData(ScreenOrientation orientation) {
        if (orientation == ScreenOrientation.Portrait) {
            leftPanel_P.DebugPrintData();
            rightPanel_P.DebugPrintData();
            operationButtons_P.DebugPrintData();
            touchableArea_P.DebugPrintData();
            seedInput_P.DebugPrintData();
            infoPanel_P.DebugPrintData();
            timerPanel_P.DebugPrintData();
            twoButtonsPanel_P.DebugPrintData();
            // startingPanel_P.DebugPrintData();
            alertSpawner_P.DebugPrintData();
        }
        if (orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.LandscapeRight) {
            leftPanel_L.DebugPrintData();
            rightPanel_L.DebugPrintData();
            operationButtons_L.DebugPrintData();
            touchableArea_L.DebugPrintData();
            seedInput_L.DebugPrintData();
            infoPanel_L.DebugPrintData();
            timerPanel_L.DebugPrintData();
            twoButtonsPanel_L.DebugPrintData();
            // startingPanel_L.DebugPrintData();
            alertSpawner_L.DebugPrintData();
        }
    }

    private void SetDefaultValues() {
        // Portrait
        leftPanel_P.GetFromArgs(
            new Vector3(-690f, -1450f, 0f), new Vector2(30f, 30f), new Vector2(790f, 1150f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        rightPanel_P.GetFromArgs(
            new Vector3(690f, -1450f, 0f), new Vector2(-30f, 30f), new Vector2(550f, 1150f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        operationButtons_P.GetFromArgs(
            new Vector3(695f, 1480f, 0f), new Vector2(-25f, 0f), new Vector2(120f, -1152f), new Vector2(1f, 1f), new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        hintButton_P.GetFromArgs(
            new Vector3(-780f, 12f, 0f), new Vector2(-60f, 1292f), new Vector2(120f, 120f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0.5f, 0.5f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        touchableArea_P.GetFromArgs(
            new Vector3(0f, 589.9861f, 0f), new Vector2(0f, 589.9861f), new Vector2(0f, -1180.028f), new Vector2(1f, 1f), new Vector2(0f, 0f), new Vector2(0.5f, 0.5f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        seedInput_P.GetFromArgs(
            new Vector3(-10f, -108f, 0f), new Vector2(-10f, 1172f), new Vector2(-220.0001f, 60f), new Vector2(1f, 0f), new Vector2(0f, 0f), new Vector2(0.5f, 0f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        infoPanel_P.GetFromArgs(
            new Vector3(-1560f, 1391.2f, 0f), new Vector2(-840.0001f, 0f), new Vector2(840f, 300f), new Vector2(0f, 0.97f), new Vector2(0f, 0.97f), new Vector2(0f, 1f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        timerPanel_P.GetFromArgs(
            new Vector3(-720f, 1203.2f, 0f), new Vector2(0f, 0f), new Vector2(500f, 250f), new Vector2(0f, 0.97f), new Vector2(0f, 0.97f), new Vector2(0f, 1f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        twoButtonsPanel_P.GetFromArgs(
            new Vector3(-510.0001f, -930f, 0f), new Vector2(30f, 30f), new Vector2(790f, 1150f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        // startingPanel_P.GetFromArgs(
        //     new Vector3(0f, 420f, 0f), new Vector2(0f, 1700f), new Vector2(1200f, 600f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        // );
        alertSpawner_P.GetFromArgs(
            new Vector3(0f, 270f, 0f), new Vector2(0f, 1550f), new Vector2(0f, 800f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        toastMessage_P.GetFromArgs(
            new Vector3(0f, 0f, 0f), new Vector2(0f, 1280f), new Vector2(1000f, 100f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        nextLevelButton_P.GetFromArgs(
            new Vector3(397.1f, 0f, 0f), new Vector2(-322.9f, 1280f), new Vector2(300f, 100f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0.5f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        workspacePosition_P = new Vector3(0f, 146f, 533f);
        particleFinishSettings_P = new ParticleFinishSettings(100f, 80f, 40f);

        // Landscape
        leftPanel_L.GetFromArgs(
            new Vector3(-1326.372f, -619.5863f, 0f), new Vector2(50f, 50f), new Vector2(750f, 1100f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        rightPanel_L.GetFromArgs(
            new Vector3(1326.372f, -619.5863f, 0f), new Vector2(-50f, 50f), new Vector2(500f, 950f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        operationButtons_L.GetFromArgs(
            new Vector3(1230f, 669.9999f, 0f), new Vector2(-50f, -50f), new Vector2(-1750f, -1100f), new Vector2(1f, 1f), new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        hintButton_L.GetFromArgs(
            new Vector3(-370f, -780f, 0f), new Vector2(910f, -60f), new Vector2(120f, 120f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0.5f, 0.5f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        touchableArea_L.GetFromArgs(
            new Vector3(0f, 25f, 0f), new Vector2(0f, 25f), new Vector2(-100f, -50f), new Vector2(1f, 1f), new Vector2(0f, 0f), new Vector2(0.5f, 0.5f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        seedInput_L.GetFromArgs(
            new Vector3(300f, 639.5863f, 0f), new Vector2(300f, -30f), new Vector2(-1200f, 60f), new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(0.5f, 1f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        infoPanel_L.GetFromArgs(
            new Vector3(-526.3719f, 969.5863f, 0f), new Vector2(850f, 300f), new Vector2(840f, 300f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        timerPanel_L.GetFromArgs(
            new Vector3(-1105f, 719.9999f, 0f), new Vector2(175f, 0f), new Vector2(500f, -1150f), new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        twoButtonsPanel_L.GetFromArgs(
            new Vector3(-1430f, -669.9999f, 0f), new Vector2(50f, 50f), new Vector2(750f, 1100f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        // startingPanel_L.GetFromArgs(
        //     new Vector3(125f, -19.99994f, 0f), new Vector2(125f, 700f), new Vector2(1200f, 600f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        // );
        alertSpawner_L.GetFromArgs(
            new Vector3(125f, -19.99994f, 0f), new Vector2(125f, 700f), new Vector2(0f, 800f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        toastMessage_L.GetFromArgs(
            new Vector3(100f, -600f, 0f), new Vector2(100f, 120f), new Vector2(1000f, 100f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        nextLevelButton_L.GetFromArgs(
            new Vector3(530f, -619.9999f, 0f), new Vector2(-750f, 100f), new Vector2(300f, 100f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0.5f), new Vector3(1f, 1f, 1f), new Quaternion(0f, 0f, 0f, 1f)
        );
        workspacePosition_L = new Vector3(30f, 0f, 250f);
        particleFinishSettings_L = new ParticleFinishSettings(60f, 80f, 40f);
    }
    
    void Start()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        deviceOrientation = Input.deviceOrientation;
        // screenOrientation = Screen.orientation;
        screenOrientation = ScreenOrientation.Portrait;
        #if UNITY_EDITOR
            if (screenHeight < screenWidth) {deviceOrientation = DeviceOrientation.LandscapeLeft; screenOrientation = ScreenOrientation.LandscapeLeft;}
            else {deviceOrientation = DeviceOrientation.Portrait; screenOrientation = ScreenOrientation.Portrait;}
        #endif

        SetDefaultValues();
        
        RearrangeUI(true);
    }


    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L)) {
            RearrangeUI(ScreenOrientation.LandscapeLeft);
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            RearrangeUI(ScreenOrientation.Portrait);
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            nextLevelButton_P.GetFromRectTransform(nextLevelButton);
            nextLevelButton_P.DebugPrintData();
        }
        #else

        #if UNITY_ANDROID && !UNITY_EDITOR
        using (var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            var context = actClass.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass systemGlobal = new AndroidJavaClass("android.provider.Settings$System");
            autoRotateOnAndroid = systemGlobal.CallStatic<int>("getInt", context.Call<AndroidJavaObject>("getContentResolver"), "accelerometer_rotation") == 1;
        }
        #endif

        Screen.autorotateToLandscapeLeft = autoRotateOnAndroid || Screen.orientation == ScreenOrientation.LandscapeLeft;
        Screen.autorotateToLandscapeRight = autoRotateOnAndroid || Screen.orientation == ScreenOrientation.LandscapeRight;
        Screen.autorotateToPortrait = autoRotateOnAndroid || Screen.orientation == ScreenOrientation.Portrait;
        Screen.orientation = autoRotateOnAndroid ? ScreenOrientation.AutoRotation : Screen.orientation;

        if (autoRotateOnAndroid && (screenHeight != Screen.height || screenWidth != Screen.width || screenOrientation != Screen.orientation))
        {
            RearrangeUI();
        }
        #endif
    }

    private void FixedUpdate() {
        screenOrientationHasChanged = false;
    }

    private Rect RectTransformToScreenSpace(RectTransform transform) {
        float scaleFactor = transform.GetComponentInParent<Canvas>().scaleFactor;
        Vector2 positionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 size = transform.rect.size * scaleFactor;
        positionOnScreen.x -= size.x * transform.pivot.x;
        positionOnScreen.y -= size.y * transform.pivot.y;
        positionOnScreen.y = Screen.height - positionOnScreen.y - (size.y);
        Rect rect = new Rect(positionOnScreen, size);
        return rect;

    }
    /**
    <summary>Version on current screen orientation</summary>
    **/
    public bool TouchableAreaInput(Vector2 point) {
        return TouchableAreaInput(point, screenOrientation);
    }
    public bool TouchableAreaInput(Vector2 point, ScreenOrientation orientation) {
        Vector2 iYPoint = new Vector2(point.x, Screen.height - point.y);
        Rect area = RectTransformToScreenSpace(touchableArea);
        if (orientation == ScreenOrientation.Portrait) {
            Rect areaException1 = RectTransformToScreenSpace(operationButtonsVertical);
            Rect areaException2 = RectTransformToScreenSpace(leftPanel);
            Rect areaException3 = RectTransformToScreenSpace(rightPanel);

            return area.Contains(iYPoint) && !areaException1.Contains(iYPoint) && !areaException2.Contains(iYPoint) 
                && !areaException3.Contains(iYPoint);
            
        }
        if (orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.LandscapeRight) {
            Rect areaException1 = RectTransformToScreenSpace(operationButtonsHorizontal);
            Rect areaException2 = RectTransformToScreenSpace(leftPanel);
            Rect areaException3 = RectTransformToScreenSpace(rightPanel);

            return area.Contains(iYPoint) && !areaException1.Contains(iYPoint) && !areaException2.Contains(iYPoint) 
                && !areaException3.Contains(iYPoint);
        }
        return false;
    }

    private void OnGUI() {

    }

    public void RearrangeUI(ScreenOrientation orientation, bool dontNotice = false) {
        screenHeight = Screen.height;
        screenWidth = Screen.width;


        if (orientation != screenOrientation) {
            if (orientation == ScreenOrientation.Portrait) {
                deviceOrientation = DeviceOrientation.Portrait;
                screenOrientation = ScreenOrientation.Portrait;
                DataToRectTransforms(screenOrientation);

                operationButtonsVertical.gameObject.SetActive(true);
                operationButtonsHorizontal.gameObject.SetActive(false);

                canvasOfMainUI.matchWidthOrHeight = 0f;
                canvasOfTwoButtons.matchWidthOrHeight = 0f;

                levelMenu.ToggleRightPanelHideFeatureOn(true);
                if (!TutorialScript.tutorialDeleted) {tutorialPanel.alpha = 1f; tutorialPanel.interactable = true; tutorialPanel.blocksRaycasts = true;}

                workspace.position = workspacePosition_P;
                particleSpawnerForFinish.ApplySettings(particleFinishSettings_P.camDis, particleFinishSettings_P.partDis, particleFinishSettings_P.camDisRange);
                particleSpawnerForDelete.distanceFromCamera = 450f;

            }
            if (orientation == ScreenOrientation.LandscapeLeft ||
                orientation == ScreenOrientation.LandscapeRight) 
            {
                screenOrientation = orientation;
                if (orientation == ScreenOrientation.LandscapeLeft) deviceOrientation = DeviceOrientation.LandscapeLeft;
                if (orientation == ScreenOrientation.LandscapeRight) deviceOrientation = DeviceOrientation.LandscapeRight;
                DataToRectTransforms(screenOrientation);

                operationButtonsVertical.gameObject.SetActive(false);
                operationButtonsHorizontal.gameObject.SetActive(true);

                canvasOfMainUI.matchWidthOrHeight = 0.5f;
                canvasOfTwoButtons.matchWidthOrHeight = 0.5f;

                levelMenu.ToggleRightPanelHideFeatureOn(false);
                if (!TutorialScript.tutorialDeleted) {tutorialPanel.alpha = 0f; tutorialPanel.interactable = false; tutorialPanel.blocksRaycasts = false;}

                workspace.position = workspacePosition_L;
                particleSpawnerForFinish.ApplySettings(particleFinishSettings_L.camDis, particleFinishSettings_L.partDis, particleFinishSettings_L.camDisRange);
                particleSpawnerForDelete.distanceFromCamera = 200f;

            }
        }

        positionWheel.ResetPositions();
        rotationWheel.ResetPositions();

        levelMenu.InstantToggleMenus(0);
        gameManager.RecalculateButtonsCams(1);
        levelMenu.RecalculateThemeButtonsCams();

        infoPanel.GetComponent<InfoPanel>().FixBoolState();

        if (gameManager.duringPlacing) {
            hintScript.ShowHintButton();
        }

        if (!dontNotice) screenOrientationHasChanged = true;

        // Debug.Log("RearrangeUI() is called");

    }
    public void RearrangeUI(bool dontNotice = false) {
        RearrangeUI(Screen.orientation, dontNotice);
    }
}
