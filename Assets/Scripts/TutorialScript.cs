using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class TutorialScript : MonoBehaviour
{
    public static bool firstApplicationLaunch = false;
    public static int howManyTimesLaunchedBefore {get; private set;}
    public static readonly string TUTORIAL_SHOWN_PPREF = "TUT_SHOWN";
    public static bool tutorialDeleted = false;
    public static readonly int AMOUNT_OF_NOTES = 9;
    public ScreenOrientationScript screenOrientationScript;
    private static TutorialScript instance = null;

    private static bool[] done;
    private byte[] alphas;

    public enum Notes {
        ChooseFirstPieceNote,
        RotateWorkspaceNote,
        HoldToRemoveNote,
        ChooseSecondPieceNote,
        MoveNote,
        SumbitCancelNote,
        RotateNote,
        LevelsExpl,
        ThemesExpl
    }

    [Header("Tutorial Notes:")]
    public RectTransform chooseFirstPieceNote;
    public RectTransform rotateWorkspaceNote;
    public RectTransform holdToRemoveNote;
    public RectTransform chooseSecondPieceNote;
    public RectTransform moveNote;
    public RectTransform submitCancelNote;
    public RectTransform rotateNote;

    [Header("Tutorial Explanations:")]
    public RectTransform levelsExpl;
    public RectTransform themesExpl;

    void Start()
    {

        firstApplicationLaunch = !PlayerPrefs.HasKey(TUTORIAL_SHOWN_PPREF);
        // PlayerPrefs.SetInt(TUTORIAL_SHOWN_PPREF, 0);

        if (!firstApplicationLaunch) {
            howManyTimesLaunchedBefore = PlayerPrefs.GetInt(TUTORIAL_SHOWN_PPREF);
            PlayerPrefs.SetInt(TUTORIAL_SHOWN_PPREF, howManyTimesLaunchedBefore + 1);

            // definitly remove tutorial after 4th app launch
            if (howManyTimesLaunchedBefore > 4) {
                tutorialDeleted = true;
                Destroy(this.gameObject);
                return;
            }
        } else {
            PlayerPrefs.SetInt(TUTORIAL_SHOWN_PPREF, 1);
            howManyTimesLaunchedBefore = 0;
        }

        done = new bool[AMOUNT_OF_NOTES];
        alphas = new byte[AMOUNT_OF_NOTES];

        for (int i = 0; i < AMOUNT_OF_NOTES; i++) {
            TurnOffNote((Notes)i);
            alphas[i] = 0;
            done[i] = false;
        }
        
        instance = this;

        EventTrigger.Entry entryLevels = new EventTrigger.Entry(), entryThemes = new EventTrigger.Entry(),
            entryRotateWS = new EventTrigger.Entry(), entryHold = new EventTrigger.Entry();
        entryLevels.eventID = entryThemes.eventID = entryRotateWS.eventID = entryHold.eventID = EventTriggerType.PointerDown;

        entryLevels.callback.AddListener((data) => { TurnOffNote(Notes.LevelsExpl); });
        entryThemes.callback.AddListener((data) => { TurnOffNote(Notes.ThemesExpl); });
        entryRotateWS.callback.AddListener((data) => { TurnOffNote(Notes.RotateWorkspaceNote); });
        entryHold.callback.AddListener((data) => { TurnOffNote(Notes.HoldToRemoveNote); });

        levelsExpl.GetComponent<EventTrigger>().triggers.Add(entryLevels);
        themesExpl.GetComponent<EventTrigger>().triggers.Add(entryThemes);
        rotateWorkspaceNote.GetComponent<EventTrigger>().triggers.Add(entryRotateWS);
        holdToRemoveNote.GetComponent<EventTrigger>().triggers.Add(entryHold);
    }


    public static void TurnOnNote(Notes note) {
        if (instance == null || done[(int) note]) return;

        done[(int) note] = true;
        RectTransform noteRT = instance.ReturnNoteRT(note);
        // noteRT.gameObject.SetActive(true);
        noteRT.GetComponent<CanvasGroup>().alpha = 0f;
        noteRT.GetComponent<CanvasGroup>().interactable = true;
        noteRT.GetComponent<CanvasGroup>().blocksRaycasts = true;
        // LeanTween.alphaCanvas(noteRT.GetComponent<CanvasGroup>(), 1f, 0.4f);
        instance.alphas[(int)note] = 1;
    }
    public static void TurnOffNote(Notes note) {
        if (instance == null) return;

        RectTransform noteRT = instance.ReturnNoteRT(note);
        if (!noteRT.GetComponent<CanvasGroup>().interactable) return;
        // noteRT.GetComponent<CanvasGroup>().alpha = 1f;
        noteRT.GetComponent<CanvasGroup>().interactable = false;
        noteRT.GetComponent<CanvasGroup>().blocksRaycasts = false;
        // LeanTween.alphaCanvas(noteRT.GetComponent<CanvasGroup>(), 0f, 0.4f);
        instance.alphas[(int)note] = 0;
    }

    public RectTransform ReturnNoteRT(Notes notes) {
        switch (notes) {
            default:
            case Notes.ChooseFirstPieceNote:
                return chooseFirstPieceNote;
            case Notes.RotateWorkspaceNote:
                return rotateWorkspaceNote;
            case Notes.HoldToRemoveNote:
                return holdToRemoveNote;
            case Notes.ChooseSecondPieceNote:
                return chooseSecondPieceNote;
            case Notes.MoveNote:
                return moveNote;
            case Notes.SumbitCancelNote:
                return submitCancelNote;
            case Notes.RotateNote:
                return rotateNote;
            case Notes.LevelsExpl:
                return levelsExpl;
            case Notes.ThemesExpl:
                return themesExpl;

        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < AMOUNT_OF_NOTES; i++) {
            CanvasGroup noteCG = ReturnNoteRT((Notes)i).GetComponent<CanvasGroup>();
            if (noteCG.alpha == (float)alphas[i]) continue;
            if (alphas[i] == 1) {
                float alpha = noteCG.alpha;
                alpha += Time.deltaTime / 0.4f;
                if (alpha > 1) alpha = 1f;
                noteCG.alpha = alpha;
            }
            if (alphas[i] == 0) {
                float alpha = noteCG.alpha;
                alpha -= Time.deltaTime / 0.4f;
                if (alpha < 0) alpha = 0f;
                noteCG.alpha = alpha;
            }
        }
    }
}
