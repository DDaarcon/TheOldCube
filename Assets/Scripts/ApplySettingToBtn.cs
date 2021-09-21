using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class ApplySettingToBtn : MonoBehaviour
{
    
    private Variant currentVariant = Variant.x4;
    // private Rect rect;
    private bool[,] setting = new bool[,] {
        {I, I, I, I},
        {I, I, I, I},
        {I, I, I, I},
        {I, I, I, I}
    };
    /*
    <value>Index assigned to button, probably unnecessary</value>
    */
    public int index {get; set;}

    public Camera cellCamera;
    public Image backgroundImage;
    private const int cellCameraDisplayedLayer = 8;
    public GameObject piece;
    public GameObject copyPiece3;
    public GameObject copyPiece4;
    public Piece pieceComponent {get; private set;}

    public bool[,] GetSetting() {
        return setting;
    }
    public void ChangeSetting(bool[,] setting_) {
        setting = setting_;
        pieceComponent.ChangeSetting(setting);
    }
    public void ChangeVariant(Variant variant) {
        if (currentVariant != variant) {
            currentVariant = variant;
            Vector3 position = piece.transform.position;
            Quaternion rotation = piece.transform.rotation;
            Vector3 scale = piece.transform.localScale;
            Color color = piece.GetComponent<Piece>().color;
            Destroy(piece);
            if (variant == Variant.x4) {
                setting = new bool[,] {
                    {I, I, I, I},
                    {I, I, I, I},
                    {I, I, I, I},
                    {I, I, I, I}
                };
                piece = Instantiate(copyPiece4, position, rotation);
            }
            if (variant == Variant.x3) {
                setting = new bool[,] {
                    {I, I, I},
                    {I, I, I},
                    {I, I, I}
                };
                piece = Instantiate(copyPiece3, position, rotation);
            }
            piece.transform.parent = this.transform;
            piece.transform.localScale = scale;
            pieceComponent = piece.GetComponent<Piece>();
            pieceComponent.ChangeLayer(cellCameraDisplayedLayer);
            pieceComponent.ChangeColor(color);

        }
    }

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
        pieceComponent = piece.GetComponent<Piece>();

    }
    public void RecalculateUI(int delayBy = 1) {
        GUIset = 1 - delayBy;
    }


    void OnGUI() {
        
    }
    private int GUIset = 0;
    void Update()
    {
        if (GUIset < 2) {
            if (GUIset == 1) {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                // Canvas canvas = GameObject.Find("Canvas of Main UI").GetComponent<Canvas>();
                Canvas canvas = GetComponentInParent<Canvas>();
                screenPos -= (new Vector3(125, 125, 0) * canvas.scaleFactor);
                cellCamera.rect = new Rect(screenPos.x / Screen.width, 
                    screenPos.y / Screen.height, 
                    250f / Screen.width * canvas.scaleFactor, 
                    250f / Screen.height * canvas.scaleFactor);
                
            }
            GUIset++;
        }

        
    }
}
