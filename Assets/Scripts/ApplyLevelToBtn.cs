using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static Enums;

public class ApplyLevelToBtn : MonoBehaviour
{
    public LevelSettings lS {get; set;} = new LevelSettings();
    public TMP_Text labelText;
    public Image backgroundImage;
    public Image statusImage;
    public Sprite completedImage;
    public Sprite openedImage;
    public Variant variant {get; private set;}
    public GameScript GameManager {get; set;}
    // public Color colorForFinished;
    public Material[] difficultyMaterialPresets;
    public Sprite[] backgroundSprites;
    
    public void PrepareButton() {
        StrToArrayPlacedSides();
        labelText.fontMaterial = difficultyMaterialPresets[lS.difficulty];
        if (lS.finished) {
            statusImage.gameObject.SetActive(true);
            statusImage.sprite = completedImage;
        } else {
            statusImage.gameObject.SetActive(false);
        }
        // GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => GameManager.StartNewGame(lS.level, lS.seed, lS.placedSides, lS.finished));
        labelText.text = (lS.level + 1).ToString();
    }

    public void SetToFinished() {
        lS.finished = true;
        statusImage.gameObject.SetActive(true);
        statusImage.sprite = completedImage;
    }
    public void SetToOpened() {
        statusImage.gameObject.SetActive(true);
        statusImage.sprite = openedImage;
    }


    private void StrToArrayPlacedSides() {
        string str = lS.placedSiedsStr;
        for (int i = 0; i < str.Length; i++) {
            switch (str[i]) {
                case 'O':
                case 'f':
                case '0':
                default:
                    lS.placedSides[i] = false;
                    break;
                case 'I':
                case 't':
                case '1':
                    lS.placedSides[i] = true;
                    break;
            }
        }
    }

    // public void ChangeTransparency(float trans) {
    //     Color color = GetComponent<Image>().color;
    //     color.a = trans;
    //     GetComponent<Image>().color = color;
    // }

    // public float GetTransparency() {
    //     return GetComponent<Image>().color.a;
    // }

    void Awake()
    {
        int imageIdx = Random.Range(0, backgroundSprites.Length);
        int rotationMultiplier = Random.Range(0, 4);
        backgroundImage.sprite = backgroundSprites[imageIdx];
        Vector3 eulerRotation = new Vector3(0f, 0f, 90f * rotationMultiplier);
        backgroundImage.transform.localEulerAngles = eulerRotation;
    }

    
    void Update()
    {
        
    }
}
