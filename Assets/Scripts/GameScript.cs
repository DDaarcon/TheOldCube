using UnityEngine;
using UnityEngine.UI;
using TMPro;
// using System;
using static Enums;

[RequireComponent (typeof (SoundScript))]
[RequireComponent (typeof (SaveScript))]
[RequireComponent (typeof (ScreenOrientationScript))]
public class GameScript : MonoBehaviour
{

    /* TODO:
        //1. Obrót kostki poprzez swobodne przesuwanie 
        //2. Usuwanie ścian 
        // 3. UI i wyglad
            //a. --> przyciski zatwierdzanie i anulowanie pokazuja sie w trakcie umieszczania sciany, znajduja sie nad przyciskami wyboru sciany
                //przyciski wyboru sciany (lub caly panel) przyciemnione (blur? / transp?)
            // b. design steeringwheel
            // c. --> kolorystyka do zmiany
            //d. --> dodatkowe informacje na ekranie (czas?, jakas punktacja?)
            // e. --> tło do zmiany
            //f. wiecej animacji (partycje przy usuwaniu?, obrot siany przy dopasowywaniu? przesuniecie przy zmianie
                //pozycji przy dopasowywaniu?)
            //g. animacja otwierania menu
            // h. --> animacja końca gry (zmniejszająca sie/ zwiekszajaca kostka; inne tło; plaszczyzna ze wzrorem w pieces)
            // g. blur przy starcie, podczas wyświetlania ikony i nazwy gry

        // 4. --> Generowanie rozwiązań
            // a. ??> losowe seedy oparte o cos innego niz zegar systemowy
        //5. zablokowany obrót do poprawy
        //6. nieobracanie się scian przy zmianie pozycji (nalezy dostosowac obrot workspace do rotacji scian (setting))
        //7. przy wstawianiu nowej sciany pierwsza pozycja powinna byc tą ustawioną do kamery
        //8. zmiana na sciany zlozone z quadów
        //9. Ustawienie ścian w przyciskach przy rozdzielczosci 2960x1440
        //10. Czytelniejsze kolory ścian
        // 11. --> Oświetlenie
        //13. poprawienie znikania quadu kiedy 2 quady nakładają się na siebie podczas dopasowywania
            //a. ukrywanie kolidujących elementów w finalPieces
        //14. ustawianie przyciskow ze scianami w bardziej losowej kolejnosci (tak zeby lewa nie byla przypisana zawsze do tego samego przycisku itd)
        //15. --> przy obracaniu czerwona sciana nadal zle sie wyświetla jesli koliduje z inna
        //16. przy pierwszym wlaczaniu gry nie laduja sie poziomy
        //17. --> wprowadzenie statystyk i zapisywanie ich
        //18. restart button
        19. ??> SearchForMistakes() do poprawy (wraca do gameSolution za kazdym razem - wolne)
        20. ??> wersja dla zmiejszonego ekrau (podział)
        //21. ??> panel lewy dostosowywany do zawartości ????
        //22. canvas scaling 50/50
        //23. --> wersja dla 3x3
        //24. przy poziomie 3 znika niepoprawny klocek ze ściany (front przy dopasowywanym top)
        //25. --> zmiana działania steering wheel (naciskanie w srodek, przeciąganie)
        //26. przy usuwaniu sciany partycje zeby to wyglądalo
        // 28. obracanie workspace powinno byc bardzie smooth
        //29. ??> zegar bugi
        // 33. ??> fancy shader for pieces
        //34. random btn -> canvas2btns close
        //35. --> po ulozeniu nadal liczy i gdy wcisnie sie restart dodany jest czas ktorego zlicznie nie było widoczne (do sprawdzenia)
        // 36. ??> 5x5 
        //37. restart nie dziala
        // 38. ??> zegar w poziomach (przejście w przeźroczysty, inne rozwiązanie?)
        //39. animacja oddalania układanej ściany w momencie obracania
        //40. przy aktywnym levelmenu dotkniecie w obszarze touchablearea powinno chowac menu
        //41. obracanie układanej ściany, bloki sasiednich scian powinny znikac na koncu animacji przemieszczania ukladanej sciany?
        //42. --> better multitouch management (do sprawdzenia steeringwheel)
        //43. przy obracaniu dwoma palcami opcja obracania jak jednym
        //45. udoskonalenie zmiany orientacji ekranu (przejście portrait -> landscaperight bug z viewportami)
        //46. --> animacja zakończenia, większe elementy
        //47. start aplikacji set cameras viewports
        //48. --> chowanie infoPanel przejście do przezroczystosci
        //49. --> pojawianie się pieceSelectPanel, pojawianie się ścian przed/równocześnie z pojawieniem się rightPanel
        // 50. ??> animacja rozkładania kostki na siatkę po ukończeniu gry
        //51. ##> napis "Level X passed", wyświetlanie przed kostką 
        //52. --> miejsca wstawiania finishparticles bardziej dopracowane (podzielenie touchablearea na części)
        //53. obrazek i napis w miejscu workspace po uruchomienu aplikacji
        //54. ##> właczenie aplikacji z orientacją landscape, pozostaje portrait
        //55. ??> kwadratowe operation buttons, operationbuttons na panelu, kolor kostki z kolorem czerwonym (bledy w wyswietlaniu), przyciski 3x3 4x4 (mikołajczyk)
        //56. ##> level, restart pressed, side placed automaticly
        //57. ##> restart pressed during workspace scalling, workspace stays small
        //58. level cell font
        // 59. ??> level cell press and hold shows pieces from level
        //60. hiden ui at start, shows on first touch
        // 61. --> krótsze particles on finish / inny sposob wstawiania
        //62. upgrade savelevelsetting serialization
        // 64. --> Rozpadająca się kostka pod koniec (tylko Piece)
        //65. Automatyczna rotacja w losowym kierunku pod koniec
        // 66. PiecePG skalowanie dodawanej ściany powoduje błąd wyświetlania
        //67. PiecePG nie ma mesh collidera, przebudowanie funkcji GetDataFromTouch
        // 68. ??> PiecePG Dodecahedron
        // 69. PiecePG zmiana koloru poszczególnych kostek (mesh.color)
        // 72. --> PiecePG ChangeColorInTime ustawia początkowy kolor czarny
        // 73. po ulozeniu gra sie nie konczy
        // 75. auto hide info panel
        //76. usuwanie ściany która była wsadzona przez level nie zwalnia przycisku dodania tej ściany
        // 77. ??> PiecePG material color (nie działa)
        // 78. PiecePG ChangeTransparencyInTime nie usuwa ściany
        // 79. ##> nie wstawia ścian
        // 81. ##> particles on delete doesnt show every time
        // 82. ??> PiecePG Decay optimalize
        // 63. --> better finished levels marking
        // 85. on finish before dacay run vibrate animation on whole workspace
        // 86. --> problem with removing (in some cases wrong button becomes interactable)
        // 87. --> SearchForMistakes() somewhere after applying hint
        // 89. --> wprowadzenie reklam
        // 90. placing last side by hint does something strange
        // 91. 3x3 lv1 wstawienie pelnej sciany naprzeciwko juz wstawionej powoduje jej znikniecie
        // 93. --> sometimes on one side (bottom?) piece's cubes doesn't shift
        // 92. --> tutorial/samouczek
        // 98. hide wheels panel while not placing option
        // 99. deleting doesnt work properly
        // 100. --> starting screen fix
        // 102. themes
        // 103. --> tutorial (on portrait mode)
        12. Poziomy
            a. --> łatwe poziomy z ułożoną częściowo kostką
        27. ??> wiecej statystyk
        30. ??> info panel text na text mesh pro
        31. audio
            a. dzwieki przyciskow
            b. dzwieki dzialania na kostce
            c. muzyka?
            // d. wyciszanie (zapisywane w playerpref?)
            e. dopracowanie dzwiekow
            f. missing sounds:
                menu hide/show
                piece rotate
                piece change position
        32. ??> zbyt wolne animacje?
        44. ??> oznaczenie dla otwartego poziomu
        70. DOKUMENTACJA
        71. ??> PiecePG ograniczenie częstości wyliczania danych pod mesh (przy rotacji zachowanie poprzedniego mesha?)
        74. --> nadpisywanie pozycji vertices w PiecePartMeshGenerate podczas rotacji Piece lub zatwierdzania Piece
        80. ??> dopracowac animacje wstawiania i usuwania scian dla PiecePG
        83. ??> PiecePartMeshGenerate - instead of storing whole Vector3 store just z value
        84. ??> PiecePG vertices shift on rotation need fixing
        88. --> mozliwosc schowania ekranu startowego dopiero przy opacity > 0.8/0.9, wypisanie na ekranie startowym creditsow pociechy
        94. doesn't reshift on cancel placing?
        95. more piece rotation sounds (depending on rotation distance/speed)
        96. resizing operation buttons on some resolutions?
        97. deleting pieces during placing
        101. ads skipping made better, ad filters
        104. pierwsze odpalenie aplikacji - 1 level, dostosowanie tutorialu
        // 105. ##> load level and themes panel in diffrent moment
        106. --> next level button stays visible
        // 107. next themes panel


        For future updates:
        1. add motion to decaying cube while rotation workspace
        2. more animations of dacaying cubes
        




        
        

        SteeringWheel:
        // 1. ??> Programowe wyznaczanie wiecej niż 4 pozycji (podzielenie kola na wiecej czesci) (czy na pewno bedzie to db rozwiazanie?)

        WorkspaceRotation:
        //1. dotkniecie poza obszarem i najechanie palcem na obszar nie powinno pozwalac na obrot workspace

        Dodatkowe:
        1. ??> obracanie SCIANY W PRZYCISKU WYBORU SCIANY ruchem palca, powrot do pozycji domyslnej po puszczeniu
        //2. zmiana koloru szescianu (quadów), kiedy nie pasuje i nacisnieto przycisk wstawiania (OK)/ zmiana koloru przez caly czas dopasowywania
        3. ??> inny sposob umieszczania scian
        4. ??> inne rozwiazanie dla dopasowywania scian (bardziej czytelne)
        //5. długie nacisniecie na przycisk losowania pozwala wprowadzic seed

        Legend:
        1 ??> might add
        2 --> partly finished
        3 ##> to check

        Errors:
        1.  reproduce: open level menu on portrait, change to landscape, try to open level menu
            what: "shadow" of pieces buttons instead of level menu
        2.  reproduce: finish level, open themes menu, press next level button
            what: opening pieces menu is delayed
        3.  reproduce: open level menu, before animation finished open themes menu
            what: bugs while displaying menus
        4.  reproduce: rotate device to landscape or just start app in this position
            what: right panel displays dispite
        5.  reproduce: while system's autorotation is of and device stays in landscape mode, lock device, rotate device to portrait and unlock
            waht: screen orientation goes crazy
    */

    
    // gameSolution SETTING AND OPERATIONS
    /**
    <value>Representation of whole puzzle present on screen</value>
    **/
    public bool[][,] gameSolution {get; private set;}

    #if TRUE

    /**
    <summary>Debug method: Print in console settings of all pieces of present puzzle</summary>
    **/
    public void VisualizeDataFromSolution(bool[][,] solution, string precedingMsg = "") {
        string et = precedingMsg;
        et += " (solution): \n";
        for (int s = 0; s < 6; s++) {
            et += "Side: " + ((Side)s).ToString() + '\n';
            for (int i = 0; i < variantInt; i++) {
                for (int j = 0; j < variantInt; j++) {
                    et += (solution[s][i, j] ? "I" : "O") + "\t";
                }
                et += '\n';
            }
        }
        Debug.Log(et);
    }
    public void VisualizeDataFromSetting(bool[,] setting, string precedingMsg = "") {
        string et = precedingMsg;
        et += " (setting): \n";
        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                et += (setting[i, j] ? "I" : "O") + "\t";
            }
            et += '\n';
        }
        Debug.Log(et);
    }
    #endif
    private void SetEmptyGameSolution() {
        for (int o = 0; o < 6; o++){
            SetEmptySideOnGameSolution((Side)o);
        }
    }
    private void SetEmptySideOnGameSolution(Side side_) {
        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                gameSolution[(int)side_][i, j] = O;
            }
        }
    }
    
    // direction aNd side
    private struct dNs {
        public Side s;
        public Dir d;
        public dNs(Side s_, Dir d_) {
            s = s_;
            d = d_;
        }
    }
    // representation of data below should be in some file for information purposes
    /**
    <value>Data for correct calculating if side which is being placed fits with others.
    For certain [side, direction] get side it touches in this direction</value>
    **/
    private dNs[,] properEdge = new dNs[6, 4] {
        {new dNs(Side.back, Dir.D), new dNs(Side.left, Dir.R), new dNs(Side.front, Dir.U), new dNs(Side.right, Dir.L)},     //bottom
        {new dNs(Side.top, Dir.Un), new dNs(Side.left, Dir.U), new dNs(Side.bottom, Dir.U), new dNs(Side.right, Dir.Un)},   //back
        {new dNs(Side.back, Dir.L), new dNs(Side.top, Dir.R), new dNs(Side.front, Dir.Ln), new dNs(Side.bottom, Dir.L)},    //left
        {new dNs(Side.back, Dir.Rn), new dNs(Side.bottom, Dir.R), new dNs(Side.front, Dir.R), new dNs(Side.top, Dir.L)},    //right
        {new dNs(Side.bottom, Dir.D), new dNs(Side.left, Dir.Dn), new dNs(Side.top, Dir.Dn), new dNs(Side.right, Dir.D)},   //front
        {new dNs(Side.back, Dir.Un), new dNs(Side.right, Dir.R), new dNs(Side.front, Dir.Dn), new dNs(Side.left, Dir.L)}    //top
        // up direction             left direction              down direction              right direction
    };

    /**
    <value>GameSolution excluding shifted cubes (SearchForMistakes)</value>
    **/
    private bool[][,] shiftedGameSolution;

    // put edge in 1d array
    private bool[] GetEdge(Side side_, Dir edge){
        bool[] rtnValue = new bool[variantInt];
        int l = variantInt - 1;
        if (edge == Dir.U)
            for (int i = 0; i < variantInt; i++)
                rtnValue[i] = gameSolution[(int)side_][0, i];
        if (edge == Dir.L)
            for (int i = 0; i < variantInt; i++)
                rtnValue[i] = gameSolution[(int)side_][i, 0];
        if (edge == Dir.D)
            for (int i = 0; i < variantInt; i++)
                rtnValue[i] = gameSolution[(int)side_][l, i];
        if (edge == Dir.R)
            for (int i = 0; i < variantInt; i++)
                rtnValue[i] = gameSolution[(int)side_][i, l];
        if (edge == Dir.Un)
            for (int i = 0; i < variantInt; i++)
                rtnValue[l - i] = gameSolution[(int)side_][0, i];
        if (edge == Dir.Ln)
            for (int i = 0; i < variantInt; i++)
                rtnValue[l - i] = gameSolution[(int)side_][i, 0];
        if (edge == Dir.Dn)
            for (int i = 0; i < variantInt; i++)
                rtnValue[l - i] = gameSolution[(int)side_][l, i];
        if (edge == Dir.Rn)
            for (int i = 0; i < variantInt; i++)
                rtnValue[l - i] = gameSolution[(int)side_][i, l];
        return rtnValue;
    }
    // though 4 edges of selected side
    private bool CheckSideInEveryDirection(Side side_, bool force = false) {
        for (int i = 0; i < 4; i++) {
            if (!Check2Sides(side_, (Dir)i)) {
                if (!force) return false;
                Debug.Log("Remove Piece at side: " + properEdge[(int)side_, i].s);
                RemovePieceAt(properEdge[(int)side_, i].s);
            }
        } 
        return true;
    }
    // find proper edge to check
    private bool Check2Sides(Side s1, Dir dir_) {
        bool[] edge1, edge2;
        edge1 = GetEdge(s1, dir_);
        dNs dns = properEdge[(int)s1, (int)dir_];
        edge2 = GetEdge(dns.s, dns.d);
        for (int i = 0; i < variantInt; i++) {
            if (edge1[i] && edge2[i]){
                return false;
            }
        }
        return true;
    }
    // versions of above methods with detection of incorrectness in side being placed
    // and adjacent sides
    private bool CheckSideInEveryDirection(Side side_, bool[,] setting_, out bool[,] arrang) {
        // arrangment of incorrect cubes
        if (variant == Variant.x4) arrang = new bool[,] {
            {O, O, O, O},
            {O, O, O, O},
            {O, O, O, O},
            {O, O, O, O}
        };
        else /* if (variant == Variant.x3) */ arrang = new bool[,] {
            {O, O, O},
            {O, O, O},
            {O, O, O}
        };
        // setting settingDuringPlacing as copy of GameSolution
        for (int i = 0; i < 6; i++) {
            shiftedGameSolution[i] = (bool[,])gameSolution[i].Clone();
        }

        // check if any incorrectness are spotted
        bool detectedIncorrectness = false;
        for (int i = 0; i < 4; i++) {
            if (!Check2Sides(side_, (Dir)i, setting_, ref arrang))
                detectedIncorrectness = true;
        }
        return !detectedIncorrectness;
    }
    private bool Check2Sides(Side s1, Dir dir_, bool[,] setting_, ref bool[,] arrang) {
        bool detectedIncorrectness = false;
        bool[] edge1, edge2;
        // get proper edge of side being placed
        edge1 = new bool[variantInt];
        int l = variantInt - 1;
        if (dir_ == Dir.U)
            for (int i = 0; i < variantInt; i++)
                edge1[i] = setting_[0, i];
        if (dir_ == Dir.L)
            for (int i = 0; i < variantInt; i++)
                edge1[i] = setting_[i, 0];
        if (dir_ == Dir.D)
            for (int i = 0; i < variantInt; i++)
                edge1[i] = setting_[l, i];
        if (dir_ == Dir.R)
            for (int i = 0; i < variantInt; i++)
                edge1[i] = setting_[i, l];
        // get proper edge of adjacent side
        dNs dns = properEdge[(int)s1, (int)dir_];
        edge2 = GetEdge(dns.s, dns.d);
        // loop though edge
        for (int i = 0; i < variantInt; i++) {
            if (edge1[i] && edge2[i]){
                // set arrang for placing Piece (colored cubes)
                if (dir_ == Dir.U) arrang[0, i] = I;
                if (dir_ == Dir.L) arrang[i, 0] = I;
                if (dir_ == Dir.R) arrang[i, l] = I;
                if (dir_ == Dir.D) arrang[l, i] = I;

                // set settingDuringPlacing for other already positioned Pieces
                int r = 0, c = 0;
                switch (dns.d) {
                    case Dir.U: r = 0; c = i; break;
                    case Dir.Un: r = 0; c = l - i; break;
                    case Dir.D: r = l; c = i; break;
                    case Dir.Dn: r = l; c = l - i; break;
                    case Dir.L: r = i; c = 0; break;
                    case Dir.Ln: r = l - i; c = 0; break;
                    case Dir.R: r = i; c = l; break;
                    case Dir.Rn: r = l - i; c = l; break;
                }
                shiftedGameSolution[(int)dns.s][r, c] = O;

                detectedIncorrectness = true;
            }
        }
        return !detectedIncorrectness;
    }

    /**
    <summary>Put settings to data representaion of puzzle</summary>
    **/
    public bool PlaceSideOnGameSolution(Side side_, bool[,] setting_, bool force = false) {
        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                gameSolution[(int)side_][i, j] = setting_[i, j];
            }
        }

        if (CheckSideInEveryDirection(side_, force)) 
            return true;
        else {
            SetEmptySideOnGameSolution(side_);
            return false;
        }
    }







    // GENERATION OF RANDOM SOLUTION
    /**
    <value>Data representation of solved puzzle (in one of ways at least, shouldn't be used in checking correction of placed side)</value>
    **/
    public bool[][,] genrSolution {get; private set;}

    public void OverrideGeneratedSolution(bool[][,] solution) {
        genrSolution = (bool[][,])solution.Clone();
    }

    // BUTTONS 
    [Header("Buttons:")]
    /**
    <value>GameObject which contain buttons with pieces</value>
    **/
    public GameObject containerOfButtons;
    /**
    <value>Array of buttons with pieces</value>
    **/
    private Button[] buttons;
    /**
    <value>Information about which button corresponds to which already placed piece (necessary for removing).
    It is somehow reversed to what buttonOrder array do</value>
    **/
    private int[] piecesButtonsIndexes = new int[6];
    /**
    <value>Info to which button correnspods currently placed piece</value>
    **/
    public int placingPieceButtonIndex {get; private set;}
    /**
    <value>Color in which button pieces should appear</value>
    **/
    public Color colorForButtonPieces = new Color();
    /**
    <value>GameObject of two buttons - accept and cancel</value>
    **/
    // public GameObject canvasOf2Btns;
    public CanvasGroup yesNoButtonsPanel;
    private LTDescr yesNoPanelAnimation;
    /**
    <value>Order in which buttons should be organised in container (index for array is button, int stored at this index - side from solution)</value>
    **/
    public int[] buttonOrder {get; private set;}
    private void SetButtons(){
        buttons = new Button[6];

        for (int i = 0; i < 6; i++) {
            if (buttons[i] == null)
                buttons[i] = containerOfButtons.transform.GetChild(i).GetComponent<Button>();
            buttons[i].GetComponent<ApplySettingToBtn>().index = i;
            buttons[i].GetComponent<ApplySettingToBtn>().ChangeVariant(variant);
            buttons[i].GetComponent<ApplySettingToBtn>().pieceComponent.ChangeColor(colorForButtonPieces);
        }
        
    }
    private void RenewButtons() {
        if (buttons[0] == null)
            SetButtons();
        buttonOrder = RandomizeButtonOrder();
        for (int i = 0; i < 6; i++)
        {
            // piecesButtonsIndexes[i] = -1;
            int i2 = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => PlacePiece(genrSolution[buttonOrder[i2]], i2));
            buttons[i].GetComponent<ApplySettingToBtn>().ChangeSetting(genrSolution[buttonOrder[i]]);
            buttons[i].GetComponent<ApplySettingToBtn>().Enabled(true);
        }
    }
    public void RecalculateButtonsCams(int delayBy = 1) {
        // bool makeActive = false;
        // if (!containerOfButtons.activeSelf) {makeActive = true; containerOfButtons.SetActive(true);}
        foreach (Button button in buttons) {
            button.GetComponent<ApplySettingToBtn>().RecalculateUI(delayBy);
        }
        // if (makeActive) containerOfButtons
    }
    private int[] RandomizeButtonOrder(bool randomize = true) {
        int[] order = new int[6] {0, 1, 2, 3, 4, 5};
        if (randomize) {
            System.Random random = new System.Random();
            for (int i = 0; i < 3; i++) {
                int first = random.Next(6), second;
                do {
                    second = random.Next(6);
                } while (first == second);
                int backup = order[first];
                order[first] = order[second];
                order[second] = backup;

            }
        }
        return order;
    }
    private void EnabledButton(int index, bool toEnable) {
        buttons[index].GetComponent<ApplySettingToBtn>().Enabled(toEnable);
    }
    private void EnabledYesNoButtons(bool toEnable) {
        if (yesNoPanelAnimation != null) {
            LeanTween.cancel(yesNoPanelAnimation.id);
        }
        yesNoPanelAnimation = LeanTween.alphaCanvas(yesNoButtonsPanel, toEnable ? 1f : 0f, 0.2f);
        yesNoButtonsPanel.interactable = toEnable;
        yesNoButtonsPanel.blocksRaycasts = toEnable;
    }






    // ROTATING WORKSPACE

    private Quaternion defaultRotation;
    // private Vector3[] rotationOfWorkspace = new Vector3[6] {
    //     new Vector3(-90, 0, 0),  // bottom
    //     new Vector3(0, 0, 0),   // back
    //     new Vector3(0, 90, 0),  // left
    //     new Vector3(0, -90, 0), // right
    //     new Vector3(0, 180, 0), // front
    //     new Vector3(90, 0, 0)  // top
    // };







    // DELETING PIECES

    private RaycastHit hit;

    public void GetDataFromTouch(Vector2 touchPos) {
        if (!duringPlacing) {
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            Piece piece;

            if (Physics.Raycast(ray, out hit, 5000)) {
                if (hit.transform.TryGetComponent(out piece) ||
                    (hit.transform.parent != null && hit.transform.parent.parent != null && hit.transform.parent.parent.TryGetComponent(out piece))) {

                    switch (piece.transform.name) {
                        case "bottom":
                            RemovePieceAt(Side.bottom);
                            break;
                        case "back":
                            RemovePieceAt(Side.back);
                            break;
                        case "left":
                            RemovePieceAt(Side.left);
                            break;
                        case "right":
                            RemovePieceAt(Side.right);
                            break;
                        case "front":
                            RemovePieceAt(Side.front);
                            break;
                        case "top":
                            RemovePieceAt(Side.top);
                            break;
                        default:
                            Debug.Log("Error while trying to detect piece to delete ;(");
                        break;
                    }

                }
            }
        }
    }








    // APPEARANCE

    public Themes gameTheme {get; private set;} = Themes.BasicStone;
    public int placedSides {get; private set;} = 0;
    public bool[] placedSidesArray {get; private set;} = {O, O, O, O, O, O};
    private GameObject[] finalPieces;
    private GameObject placedPiece;
    private Side placedSide;
    private bool duringRotationPiece;
    // private bool rotationPieceJustStarted;
    private int pieceRotationInInt = 0;
    private float pieceDestinedRotationY;
    private bool rotationPieceJustStarted = true;
    private LTDescr rotationPieceAwayMovement = new LTDescr();
    public bool duringPlacing {get; private set;} = false;
    /**
    <value>Rotation of workspace after game is finished</value>
    **/
    private bool duringRotationWorkspace = false;
    private bool placingJustStarted = true;
    private int posIndex = 0;
    private int rotIndex = 0;
    private bool acceptButtonPressed = false;
    private bool cancelButtonPressed = false;
    private Side[] available;
    private int currentPositionFromAvailable = 0;
    IndexStateChange posState = IndexStateChange.stay;
    IndexStateChange rotState = IndexStateChange.stay;
    public static float lengthOfSide {get; private set;} = 20f;
    private static float s3Len;
    public Vector3[] positionForSides {get; private set;}
    public Vector3[] rotationForSides {get; private set;} = new Vector3[6] {
        new Vector3(180, 0, 0),   // bottom
        new Vector3(90, 0, 0), // back
        new Vector3(0, 180, -90), // left
        new Vector3(0, 180, 90),  // right
        new Vector3(-90, 0, 0),  // front
        new Vector3(0, 180, 0), // top
    };

    [Header("Appearance:")]
    public GameObject copyOfPiece3;
    public GameObject copyOfPiece4;
    public GameObject copyOfPiecePG;
    public bool proceduralGeneratedMesh;
    private GameObject copyOfPiece;
    public Transform workspace;
    public Color[] colors = new Color[6];
    #if UNITY_EDITOR
    public bool debugColorsOn = false;
    public Color[] debugColors = new Color[6];
    #endif
    public Color colorForPlacing = new Color();
    public Color colorForMistakes = new Color();
    public float movePlacedPieceForwardFor = 0f;

    public GameObject[] GetFinalPieces() {
        return finalPieces;
    }
    /**
    <summary>Method responsible for interprating index (how the pointer on wheel is positioned right now) into specific change of rotation/position</summary>
    <param name="index">Position of pointer on wheel</param>
    <param name="rotOrPos_">Manipulating rotation or position</param>
    <param name="assingIndexesWithoutChanges">Do not set state (increased, decreased, stayed), just assign index</param>
    **/
    public void GetDataFromWheel(int index, RotOrPos rotOrPos_, bool assingIndexesWithoutChanges = false){
        if (assingIndexesWithoutChanges) {
            if (rotOrPos_ == RotOrPos.position)
                posIndex = index;
            if (rotOrPos_ == RotOrPos.rotation)
                rotIndex = index;
            return;
        }
        if (rotOrPos_ == RotOrPos.position){
            if (posIndex == 3 && index == 0)
                posState = IndexStateChange.increase;
            else if (posIndex == 0 && index == 3)
                posState = IndexStateChange.decrease;
            else if (posIndex > index)
                posState = IndexStateChange.decrease;
            else if (posIndex < index)
                posState = IndexStateChange.increase;
            else if (posIndex == index)
                posState = IndexStateChange.stay;
            posIndex = index;
        }

        if (rotOrPos_ == RotOrPos.rotation){
            if (rotIndex == 3 && index == 0)
                rotState = IndexStateChange.increase;
            else if (rotIndex == 0 && index == 3)
                rotState = IndexStateChange.decrease;
            else if (rotIndex > index)
                rotState = IndexStateChange.decrease;
            else if (rotIndex < index)
                rotState = IndexStateChange.increase;
            else if (rotIndex == index)
                rotState = IndexStateChange.stay;
            rotIndex = index;
        }
    }

    public void ClickedActionButton(bool accept) {
        if (duringPlacing) {
            if (accept)
                acceptButtonPressed = true;
            else
                cancelButtonPressed = true;
        }

        if (levelNotRandom && openedLevel == 0 && !accept) {
            TutorialScript.TurnOnNote(TutorialScript.Notes.ChooseFirstPieceNote);
        }
        if (levelNotRandom && openedLevel == 1 && accept) {
            TutorialScript.TurnOffNote(TutorialScript.Notes.RotateWorkspaceNote);
            TutorialScript.TurnOnNote(TutorialScript.Notes.HoldToRemoveNote);
        }
    }
    /**
    <summary>Insert a placing (moveable) piece into scene</summary>
    **/
    public void PlacePiece(bool[,] setting_, int buttonIndex) {
        if (!duringPlacing) {
            // canvasOf2Btns.SetActive(true);
            EnabledYesNoButtons(true);

            EnabledButton(buttonIndex, false);
            placingPieceButtonIndex = buttonIndex;

            TutorialScript.TurnOffNote(TutorialScript.Notes.ChooseFirstPieceNote);
            if (placedSides == 0) {
                PlacePieceAt(setting_, Side.bottom);
                if (randomGameBeforeStart) StartTimer();
                return;
            }
            if (openedLevel == 0) {
                TutorialScript.TurnOnNote(TutorialScript.Notes.RotateNote);
                TutorialScript.TurnOnNote(TutorialScript.Notes.SumbitCancelNote);
            }
            if (openedLevel == 1) {
                TutorialScript.TurnOnNote(TutorialScript.Notes.MoveNote);
            }

            GetComponent<SoundScript>().PlayChoosedSideSound();

            placedPiece = Instantiate(copyOfPiece, workspace.position, workspace.rotation);
            float delay = 0f;
            PiecePG piecePG;
            if (placedPiece.TryGetComponent<PiecePG>(out piecePG)) {
                delay = piecePG.shiftTime;
                piecePG.ChangeVariant(variant);
            }
            placedPiece.GetComponent<Piece>().ChangeSetting(setting_);
            placedPiece.transform.parent = workspace;
            duringPlacing = true;
            levelMenu.ToggleRightPanelHideFeatureOn(false);
            // duringRotationWorkspace = true;
            available = SetAvailableSides();

            Vector3 prevScale = placedPiece.transform.localScale;
            placedPiece.name = "PLACING";
            placedPiece.transform.localScale = Vector3.one * 0.01f;

            placedPiece.LeanScale(prevScale, 0.1f).delay = delay;
            placedPiece.transform.SetAsFirstSibling();
            placedPiece.GetComponent<Piece>().ChangeColor(colorForPlacing);
            placedPiece.GetComponent<Piece>().ChangeTransparency(0.7f);
            
            SetTransparencyOfAllPlacedSides(0.5f);

            infoPanel.ToggleInfoVisibility(false);
            hintScript.ShowHintButton();

        }
    }

    private void SetTransparencyOfAllPlacedSides(float trans) {
        for (int i = 0; i < 6; i++) {
            if (placedSidesArray[i] == true) {
                finalPieces[i].GetComponent<Piece>().ChangeTransparencyInTime(trans, 0.5f);
            }
        }
    }

    private void RemovePieceAt(Side side_) {
        placedSides--;
        placedSidesArray[(int)side_] = O;

        VisualizeDataFromSetting(finalPieces[(int)side_].GetComponent<Piece>().GetSetting(), "Piece being deleted");

        finalPieces[(int)side_].GetComponent<Piece>().ChangeTransparencyInTime(0f, 0.3f, true);
        LeanTween.scale(finalPieces[(int)side_], Vector3.zero, 0.3f);

        // LeanTween.delayedCall(0.15f, () => ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screenshots/" + "Removing_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png"));

        GetComponent<SoundScript>().PlayDestroySideSound();
        SetEmptySideOnGameSolution(side_);

        Debug.Log("Released button index:" + piecesButtonsIndexes[(int)side_]);
        EnabledButton(piecesButtonsIndexes[(int)side_], true);

        TutorialScript.TurnOffNote(TutorialScript.Notes.HoldToRemoveNote);
    }

    private bool PlacePieceAt(bool[,] setting_, Side side_, bool silent = false) {
        if (PlaceSideOnGameSolution(side_, setting_)) {
            // canvasOf2Btns.SetActive(false);
            EnabledYesNoButtons(false);

            placedSides++;
            placedSidesArray[(int)side_] = I;

            piecesButtonsIndexes[(int)side_] = placingPieceButtonIndex;
            // float placedPieceTransparency = 1f;
            bool fromPlacedPiece = false;
            Color placedPieceColor = new Color();
            if (placedPiece != null) {
                fromPlacedPiece = true;
                placedPieceColor = placedPiece.GetComponent<Piece>().color;
                // placedPieceTransparency = placedPiece.GetComponent<Piece>().GetTransparency();
                Destroy(placedPiece);
            }
            
            if (GetComponent<ScreenOrientationScript>().screenOrientation == ScreenOrientation.Portrait) levelMenu.ToggleRightPanelHideFeatureOn(true);
            duringPlacing = false;
            duringRotationWorkspace = false;
            currentPositionFromAvailable = 0;

            finalPieces[(int)side_] = Instantiate(copyOfPiece, workspace.position, workspace.rotation);
            PiecePG piecePG;
            if (finalPieces[(int)side_].TryGetComponent<PiecePG>(out piecePG)) {
                piecePG.ChangeVariant(variant);
                piecePG.SetTheme(gameTheme, (int)side_);
            }
            finalPieces[(int)side_].name = side_.ToString(); // essiential for removing
            finalPieces[(int)side_].GetComponent<Piece>().ChangeSetting(setting_);
            if (fromPlacedPiece) {
                finalPieces[(int)side_].GetComponent<Piece>().ChangeColor(placedPieceColor);
                finalPieces[(int)side_].GetComponent<Piece>().ChangeColorInTime(colors[(int)side_], 1f);
                // finalPieces[(int)side_].GetComponent<Piece>().ChangeTransparency(placedPieceTransparency);
                // finalPieces[(int)side_].GetComponent<Piece>().ChangeTransparencyInTime(1f, 0.5f);
            } else {
                Vector3 prevScale = finalPieces[(int)side_].transform.localScale;
                finalPieces[(int)side_].transform.localScale = Vector3.one * 0.001f;
                LeanTween.scale(finalPieces[(int)side_], prevScale, 0.1f);
                finalPieces[(int)side_].GetComponent<Piece>().ChangeColor(colors[(int)side_]);
            }
            finalPieces[(int)side_].transform.parent = workspace;
            finalPieces[(int)side_].transform.localPosition = positionForSides[(int)side_];
            finalPieces[(int)side_].transform.localEulerAngles = rotationForSides[(int)side_];

            SetTransparencyOfAllPlacedSides(1f);

            infoPanel.ToggleInfoVisibility(false);
            hintScript.HideHintButton();

            if (placedSides == 6 && !finishedGame) {
                FinishedLevel();
            } else if (!silent){
                GetComponent<SoundScript>().PlayPlaceSideSound();
            }

            return true;
        }
        Debug.Log("Piece can not be placed. At side: " + side_);
        return false;
    }
    /**
    <summary>Place a piece on solution/workspace even if it collides with already placed pieces. Warning:
        Placing piece by this method while duringPlacing==false won't assign placed piece to any button.</summary>
    **/
    public void ForcePlacePieceAt(bool[,] setting_, Side side_, bool silent = false) {
        if (placedSidesArray[(int)side_] == I) {
            RemovePieceAt(side_);
        }

        if (duringPlacing) {
            placedPiece.GetComponent<Piece>().ChangeTransparencyInTime(0f, 0.1f, true);
            LeanTween.scale(placedPiece, Vector3.zero, 0.1f);

            currentPositionFromAvailable = 0;
            piecesButtonsIndexes[(int)side_] = placingPieceButtonIndex;
            if (GetComponent<ScreenOrientationScript>().screenOrientation == ScreenOrientation.Portrait) levelMenu.ToggleRightPanelHideFeatureOn(true);
            duringPlacing = false;
            
            SearchForMistakes();
        }

        PlaceSideOnGameSolution(side_, setting_, true);

        // canvasOf2Btns.SetActive(false);
        EnabledYesNoButtons(false);

        placedSides++;
        placedSidesArray[(int)side_] = I;
        
        duringRotationWorkspace = false;
        currentPositionFromAvailable = 0;

        finalPieces[(int)side_] = Instantiate(copyOfPiece, workspace.position, workspace.rotation);
        PiecePG piecePG;
        if (finalPieces[(int)side_].TryGetComponent<PiecePG>(out piecePG)) {
            piecePG.ChangeVariant(variant);
            piecePG.SetTheme(gameTheme, (int) side_);
        }
        finalPieces[(int)side_].name = side_.ToString(); // essiential for removing
        finalPieces[(int)side_].GetComponent<Piece>().ChangeSetting(setting_);

        finalPieces[(int)side_].GetComponent<Piece>().ChangeColor(colorForPlacing);
        finalPieces[(int)side_].GetComponent<Piece>().ChangeColorInTime(colors[(int)side_], 1f);
        
        finalPieces[(int)side_].transform.parent = workspace;
        finalPieces[(int)side_].transform.localPosition = positionForSides[(int)side_];
        finalPieces[(int)side_].transform.localEulerAngles = rotationForSides[(int)side_];

        // VisualizeDataFromSolution(gameSolution, "GameSolution after ForcePlacePieceAt " + side_);

        SetTransparencyOfAllPlacedSides(1f);

        infoPanel.ToggleInfoVisibility(false);
        hintScript.HideHintButton();

        if (placedSides == 6 && !finishedGame) {
            FinishedLevel();
        } else {
            Vector3 prevScale = finalPieces[(int)side_].transform.localScale;
            finalPieces[(int)side_].transform.localScale = Vector3.one * 0.001f;
            LeanTween.scale(finalPieces[(int)side_], prevScale, 0.1f);

            if (!silent) GetComponent<SoundScript>().PlayPlaceSideSound();
        }
    }

    // public void ChangeTheme(int themeInt) {
    //     ChangeTheme((Themes)themeInt);
    // }
    public void ChangeTheme(Themes theme, bool forOneGame = false) {
        if (gameTheme == theme) return;
        gameTheme = theme;

        tryingTheme = forOneGame;
        if (!tryingTheme) {
            saveScript.saveGameDataState.SetTheme = theme;
            // saveScript.SaveGameData();
        }

        Debug.Log("Theme: " + theme + (tryingTheme ? " - trying" : ""));
        for (int i = 0; i < 6; i++) {
            if (placedSidesArray[i]) {
                PiecePG piecePG;
                if (finalPieces[i].TryGetComponent<PiecePG>(out piecePG)) {
                    LeanTween.scale(finalPieces[i], Vector3.one * 0.0001f, 0.1f);
                    int i2 = i;
                    LeanTween.delayedCall(0.1f, () => piecePG.SetTheme(theme, i2));
                    LeanTween.scale(finalPieces[i], Vector3.one, 0.2f).setDelay(0.101f);
                } else return;
            }
        }

    }

    private Side[] SetAvailableSides() {
        if (placedSides == 1 && placedSidesArray[(int)Side.bottom])
            return new Side[4] {Side.left, Side.back, Side.right, Side.front};
        
        Side[] rtn = new Side[6 - placedSides];
        int ind = 0;
        for (int i = 1; i < 6; i++) {
            if (placedSidesArray[i] == O){
                rtn[ind] = (Side)i;
                ind++;
            }
        }
        return rtn;
    }

    // two methods below are responsible for fixing placedPiece when changing its place
    private void NormalizePlacedSideApperance(Side side_) {
        switch (side_) {
            case Side.bottom:
            case Side.back:
                placedPiece.GetComponent<Piece>().RotateClockwise(2);
                break;
            case Side.left:
                placedPiece.GetComponent<Piece>().RotateClockwise(3);
                break;
            case Side.right:
                placedPiece.GetComponent<Piece>().RotateClockwise(1);
                break;
            default:
                break;
        }
    }
    private enum turn {left, upside, right, none};
    private void NormalizePlacedSideApperance(Side sideStart, Side sideEnd) {
        turn[] turns = new turn[6] {turn.upside, turn.upside, turn.left, turn.right, turn.none, turn.none};
        if (turns[(int)sideStart] == turn.none){
            switch (turns[(int)sideEnd]) {
                case turn.left:
                    placedPiece.GetComponent<Piece>().RotateClockwise(1);
                    break;
                case turn.right:
                    placedPiece.GetComponent<Piece>().RotateClockwise(3);
                    break;
                case turn.upside:
                    placedPiece.GetComponent<Piece>().RotateClockwise(2);
                    break;
            }
        }
        if (turns[(int)sideStart] == turn.left){
            switch (turns[(int)sideEnd]) {
                case turn.none:
                    placedPiece.GetComponent<Piece>().RotateClockwise(3);
                    break;
                case turn.right:
                    placedPiece.GetComponent<Piece>().RotateClockwise(2);
                    break;
                case turn.upside:
                    placedPiece.GetComponent<Piece>().RotateClockwise(1);
                    break;
            }
        }
        if (turns[(int)sideStart] == turn.right){
            switch (turns[(int)sideEnd]) {
                case turn.left:
                    placedPiece.GetComponent<Piece>().RotateClockwise(2);
                    break;
                case turn.none:
                    placedPiece.GetComponent<Piece>().RotateClockwise(1);
                    break;
                case turn.upside:
                    placedPiece.GetComponent<Piece>().RotateClockwise(3);
                    break;
            }
        }
        if (turns[(int)sideStart] == turn.upside){
            switch (turns[(int)sideEnd]) {
                case turn.left:
                    placedPiece.GetComponent<Piece>().RotateClockwise(3);
                    break;
                case turn.right:
                    placedPiece.GetComponent<Piece>().RotateClockwise(1);
                    break;
                case turn.none:
                    placedPiece.GetComponent<Piece>().RotateClockwise(2);
                    break;
            }
        }
    }
    
    // two methods below are responsible for getting side faced closest towards camera
    private Side GetClosestSide() {

        // z niewiadomych powodów odwraca pewne kierunki
        Vector3[] vects = new Vector3[6] {
            workspace.up,
            -workspace.forward,
            Vector3.Cross(workspace.up, workspace.forward),
            -(Vector3.Cross(workspace.up, workspace.forward)),
            workspace.forward,
            -workspace.up
        };

        Vector3 toCam = workspace.position - Camera.main.transform.position;

        float[] angles = new float[6];

        for (int i = 0; i < 6; i++) {
            angles[i] = Vector3.Angle(toCam, vects[i]);
        }

        int index = 0;
        for (int i = 1; i < 6; i++) {
            if (angles[i] < angles[index]){
                index = i;
            }
        }
        return (Side)index;
    }
    private void ApplyClosestToAvailable(){
        Side side_ = GetClosestSide();
        for (int i = 0; i < available.Length; i++) {
            if (available[i] == side_){
                currentPositionFromAvailable = i;
                return;
            }
        }
    }

    private void SearchForMistakes(bool forceDefault = false) {
        for (int i = 0; i < 6; i++) {
            if (placedSidesArray[i]) {
                finalPieces[i].GetComponent<Piece>().ChangeSettingForShift();
            }
        }
        if (duringPlacing && !forceDefault) {
            bool[,] arrang;
            placedPiece.GetComponent<Piece>().RetrieveColor();
            if (!CheckSideInEveryDirection(available[currentPositionFromAvailable], placedPiece.GetComponent<Piece>().GetSetting(), out arrang)) {
                placedPiece.GetComponent<Piece>().ChangeColorFor(colorForMistakes, 1, arrang);
                /* LTSeq seq = LeanTween.sequence();
                seq.append(0.07f); */
                for (int i = 0; i < 6; i++) {
                    if (placedSidesArray[i]) {
                        int i2 = i;
                        /* seq.append(() =>  */finalPieces[i2].GetComponent<Piece>().ChangeSettingForShift(shiftedGameSolution[i2]);
                    }
                }
            }
        }

    }







    // 3x3, 4x4 VARIANTS

    public Variant variant {get; private set;} = Variant.x4;
    private int variantInt;

    private void InitVariant(bool autoStart = false) {
        variantInt = (int)variant;
        if (proceduralGeneratedMesh) {
            copyOfPiece = copyOfPiecePG;
            // copyOfPiece.GetComponent<PiecePG>().ChangeVariant(variant);
            s3Len = (variant == Variant.x3 ? 1f : 1.5f) * lengthOfSide;
        } else {
            if (variant == Variant.x3) {
                s3Len = 1f * lengthOfSide;
                copyOfPiece = copyOfPiece3;
            }
            if (variant == Variant.x4) {
                s3Len = 1.5f * lengthOfSide;
                copyOfPiece = copyOfPiece4;
            }
        }

        gameSolution = new bool[6][,] {
            new bool[variantInt, variantInt], new bool[variantInt, variantInt], new bool[variantInt, variantInt], 
            new bool[variantInt, variantInt], new bool[variantInt, variantInt], new bool[variantInt, variantInt]
        };
        shiftedGameSolution = new bool[6][,] {
            new bool[variantInt, variantInt], new bool[variantInt, variantInt], new bool[variantInt, variantInt], 
            new bool[variantInt, variantInt], new bool[variantInt, variantInt], new bool[variantInt, variantInt]
        };

        positionForSides = new Vector3[6] {
            new Vector3(0, -s3Len, 0),  // bottom
            new Vector3(0, 0, s3Len),   // back
            new Vector3(-s3Len, 0, 0),  // left
            new Vector3(s3Len, 0, 0),   // right
            new Vector3(0, 0, -s3Len),  // front
            new Vector3(0, s3Len, 0),   // top
        };
        for (int i = 0; i < 6; i++)
            buttons[i].GetComponent<ApplySettingToBtn>().ChangeVariant(variant);

        infoPanel.UpdateInfo();
        
        if (TutorialScript.firstApplicationLaunch || !levelMenu.IsLevelPassed(0, Variant.x4)) {
            LevelSettings lS = levelMenu.GetLevelSettings(0, Variant.x4);
            StartNewGame(0, lS.seed, lS.placedSides, lS.finished);
            return;
        }
        if (autoStart) StartNewRandomGame(false);
    }

    public void ChooseVariant(Variant variant_, bool autoStart = false) {
        if (variant != variant_) {
            variant = variant_;
            InitVariant(autoStart);
        }
    }
    public void ChooseVariant(int var) {
        Variant variant_ = Variant.x4;
        if (var == 0) variant_ = Variant.x3;
        if (var == 1) variant_ = Variant.x4;
        ChooseVariant(variant_, true);
    }







    // GAME AND LEVELS

    [Header("Game and levels:")]
    public LevelMenu levelMenu;
    public HintScript hintScript;
    public AlertSpawner alertSpawner;
    public SeedInputField seedInputField;
    public Button nextLevelBtn;
    private byte nextLvlBtnAlpha = 0;
    private Variant nextLvlBtnVariant;
    private int openedLevel = 0;
    private bool levelNotRandom = false;
    private bool[] placedSidesFromSolution;
    private SaveScript saveScript;
    private bool randomGameBeforeStart = false;
    public bool finishedGame {get; private set;} = false; // might be useful for displaying time of game
    private bool gameFinishedAndRestarted = false;
    private bool tryingTheme = false;
    // private int sideToRotateTowardsIndex = 0;
    /**
    <value>Random rotation used for animating solved puzzle</value>
    **/
    private Quaternion randomRotationForWorkspace;

    private void StartGamePrefix() {
        placedSides = 0;
        for (int i = 0; i < 6; i++) {
            placedSidesArray[i] = O;
            if (finalPieces[i] != null) Destroy(finalPieces[i]);
        }
        if (duringPlacing) {
            // canvasOf2Btns.SetActive(false);
            EnabledYesNoButtons(false);

            currentPositionFromAvailable = 0;
            if (GetComponent<ScreenOrientationScript>().screenOrientation == ScreenOrientation.Portrait) levelMenu.ToggleRightPanelHideFeatureOn(true);
            duringPlacing = false;
            duringRotationWorkspace = false;
            Destroy(placedPiece);
        }
        SetEmptyGameSolution();
        ResetClock();
        hintScript.RenewHint();
        HideNextLevelButton(true);

        finishedGame = false;
        timeOfGame = 0f;
        gameFinishedAndRestarted = false;
        duringRotationWorkspace = false;

        if (tryingTheme) {
            tryingTheme = false;
            gameTheme = Themes.BasicStone;
        }

        seedInputField.RenewData();
        
        LeanTween.cancel(workspace.gameObject);
        workspace.LeanScale(Vector3.one, 0f);
    }
    private void StartGameSuffix(bool autoHideMenu) {
        seedInputField.RenewData();
        workspace.rotation = defaultRotation;
        if (autoHideMenu) levelMenu.ToggleMenus(0, true);
    }
    public void StartNewRandomGame(bool autoHideMenu = true) {
        StartGamePrefix();

        genrSolution = SolutionGenerator.GetNewSolution(variant);
        levelNotRandom = false;
        randomGameBeforeStart = true;
        clockText.CrossFadeAlpha(1f, 1f, false);

        RenewButtons();

        StartGameSuffix(autoHideMenu);
    }
    public void StartNewRandomGame(int seed, bool autoHideMenu = true) {
        StartGamePrefix();

        genrSolution = SolutionGenerator.GetNewSolution(seed, variant);
        levelNotRandom = false;
        randomGameBeforeStart = true;
        clockText.CrossFadeAlpha(1f, 1f, false);

        RenewButtons();

        StartGameSuffix(autoHideMenu);
    }
    public void StartNewGame(int level, int seed, bool[] placedSides_, bool finished_) {
        StartGamePrefix();

        genrSolution = SolutionGenerator.GetNewSolution(seed, variant);
        openedLevel = level;
        placedSidesFromSolution = placedSides_.Clone() as bool[];
        levelNotRandom = true;
        clockText.CrossFadeAlpha(0.25f, 1f, false);

        RenewButtons();

        finishedGame = finished_;

        for (int i = 0; i < 6; i++) {
            if (placedSides_[i] || finished_) {
                PlacePieceAt(genrSolution[i], (Side)i, true);
                int b = 0;
                while (buttonOrder[b] != i) b++;
                piecesButtonsIndexes[i] = b;
                EnabledButton(b, false);
            }
        }

        if (openedLevel == 0) {
            TutorialScript.TurnOnNote(TutorialScript.Notes.ChooseFirstPieceNote);
        }
        if (openedLevel == 1) {
            TutorialScript.TurnOnNote(TutorialScript.Notes.RotateWorkspaceNote);
        }

        StartGameSuffix(true);
    }

    public void DebugLevel(int seed, bool[] placedSides_) {
        StartGamePrefix();

        genrSolution = SolutionGenerator.GetNewSolution(seed, variant);
        placedSidesFromSolution = placedSides_.Clone() as bool[];
        levelNotRandom = false;
        clockText.CrossFadeAlpha(0.25f, 1f, true);

        RenewButtons();

        for (int i = 0; i < 6; i++) {
            if (placedSides_[i]) {
                PlacePieceAt(genrSolution[i], (Side)i, true);
                int b = 0;
                while (buttonOrder[b] != i) b++;
                piecesButtonsIndexes[i] = b;
                EnabledButton(b, false);
            }
        }

        StartGameSuffix(true);
    }

    #if UNITY_EDITOR
    public void DebugNewGame(bool[][,] solution) {
        StartGamePrefix();

        levelNotRandom = true;
        finishedGame = true;

        OverrideGeneratedSolution(solution);
        VisualizeDataFromSolution(genrSolution, "GenrSolution before applying (DebugNewGame)");

        RenewButtons();

        for (int i = 0; i < 6; i++) {
            if (PlacePieceAt(genrSolution[i], (Side)i, true)) {
                int b = 0;
                while (buttonOrder[b] != i) b++;
                piecesButtonsIndexes[i] = b;
                EnabledButton(b, false);
            }
        }
        VisualizeDataFromSolution(gameSolution, "GameSolution after applying genrSolution (DNG)");
    }
    #endif
    public void RestartGame() {
        // neccessary when game is finished and then restarted
        bool finishedAndRestarted = false;
        if (finishedGame) {
            timeOfStart = Time.time - timeOfGame;
            finishedAndRestarted = true;
        }
        StartGamePrefix();
        gameFinishedAndRestarted = finishedAndRestarted;

        for (int i = 0; i < 6; i++)
            EnabledButton(i, true);

        for (int i = 0; i < 6; i++) {
            if (levelNotRandom && placedSidesFromSolution[i]) {
                PlacePieceAt(genrSolution[i], (Side)i);
                int b = 0;
                while (buttonOrder[b] != i) b++;
                EnabledButton(b, false);
            }
        }

        levelMenu.ToggleMenus(0, false);
    }
    private void FinishedLevel() {
        Debug.Log("Finish");
        
        GameObject[] finalPiecesCopy = new GameObject[6];
        for (int i = 0; i < 6; i++) {
            // make a copy, start decay animation
            finalPiecesCopy[i] = Instantiate(finalPieces[i], finalPieces[i].transform.position, finalPieces[i].transform.rotation);

            PiecePG piecePG;
            if (finalPiecesCopy[i].TryGetComponent<PiecePG>(out piecePG)) {
                piecePG.SetTheme(gameTheme, i);
            }

            finalPiecesCopy[i].GetComponent<Piece>().ChangeSetting(gameSolution[i]);
            finalPiecesCopy[i].GetComponent<Piece>().ChangeColor(finalPieces[i].GetComponent<Piece>().color);
            finalPiecesCopy[i].GetComponent<Piece>().ChangeColorInTime(colors[i], 1f);
            PlayVibrateAnimation(finalPiecesCopy[i], 1f);

            finalPieces[i].GetComponent<Piece>().ChangeTransparency(0f, true);
            finalPieces[i].transform.localScale = Vector3.one * 0.0001f;
            int i2 = i;
            LeanTween.delayedCall(1f, () => {finalPiecesCopy[i2].GetComponent<Piece>().InitializeDecay(1f);});
            LeanTween.delayedCall(2f, () => {finalPieces[i2].GetComponent<Piece>().ChangeTransparencyInTime(1f, 1f);});
            LeanTween.scale(finalPieces[i], Vector3.one, 1f).delay = 2f;
        }
        SoundScript thisSoundScript = GetComponent<SoundScript>();
        LeanTween.delayedCall(0.9f, thisSoundScript.PlayCubeDecaySound);
        workspace.transform.localScale = Vector3.one * 0.5f;
        GetComponent<SoundScript>().PlayCubeVibrateSound();
        
        duringRotationWorkspace = true;

        if (levelNotRandom) {
            levelMenu.LevelPassed(openedLevel);
            alertSpawner.SpawnText(openedLevel + 1);

            if (openedLevel == 1) {
                TutorialScript.TurnOnNote(TutorialScript.Notes.LevelsExpl);
                TutorialScript.TurnOnNote(TutorialScript.Notes.ThemesExpl);
            }

            // show next level button if current level isn't last
            if (openedLevel < levelMenu.GetAmountOfLevels(variant) - 1) {
                CanvasGroup nextLvlCG = nextLevelBtn.GetComponent<CanvasGroup>();
                nextLvlBtnAlpha = 1;
                nextLvlCG.interactable = true;
                nextLvlCG.blocksRaycasts = true;
                nextLvlBtnVariant = variant;
                
                nextLevelBtn.onClick.RemoveAllListeners();
                nextLevelBtn.onClick.AddListener(() => HideNextLevelButton(false));
            }
        } else if (!gameFinishedAndRestarted) {
            SaveInfoState sis;
            switch (variant) {
                case Variant.x3:
                    sis = saveScript.saveInfo3State;
                    break;
                default:
                case Variant.x4:
                    sis = saveScript.saveInfo4State;
                    break;
            }
            timeOfGame = Time.time - timeOfStart;
            if (sis.randomAverageTime != 0f)
                sis.randomAverageTime = ((sis.randomAverageTime * (float)sis.randomGamesWon) + timeOfGame) / (float)(sis.randomGamesWon + 1);
            else
                sis.randomAverageTime = timeOfGame;
            sis.randomGamesWon++;

            Material clockTextMaterial = clockText.fontMaterial;
            clockDefaultColor = clockTextMaterial.GetColor("_OutlineColor");
            clockDefaultThick = clockTextMaterial.GetFloat("_OutlineWidth");
            if (sis.randomShortestTime > timeOfGame || sis.randomShortestTime == 0f){
                sis.randomShortestTime = timeOfGame;
                LeanTween.value(gameObject, 0f, 2f, shineTime).setOnUpdate((float val) => {PlayClockShineAnimation(clockTextMaterial, val, true);});
            } else {
                LeanTween.value(gameObject, 0f, 2f, shineTime).setOnUpdate((float val) => {PlayClockShineAnimation(clockTextMaterial, val, false);});
            }
            
            saveScript.SaveInfoData();
            infoPanel.UpdateInfo();

        }

        finishedGame = true;
    }
    private void PlayClockShineAnimation(Material material, float value, bool record) {
        if (value > 1f) {
            value = 2f - value;
        }
        material.SetColor("_OutlineColor", Color.Lerp(clockDefaultColor, record ? clockRecordShineColor : clockRegularFinishShineColor, value));
        material.SetFloat("_OutlineWidth", Mathf.Lerp(clockDefaultThick, record ? clockRecordShineThick : clockRegularFinishShineThick, value));
    }
    private void PlayVibrateAnimation(GameObject target, float time/* , LeanTweenType easeType = LeanTweenType.easeInCubic, float delay = 0f */) {
        Vector3 v = target.transform.position;
        float m = 2f;
        LTSpline spline = new LTSpline(new Vector3[] { 
            v + new Vector3(0f, 0f, 0f) * m, v + new Vector3(0f, 0f, 0f) * m,
            v + new Vector3(1f, 0f, 0f) * m,
            v + new Vector3(-1f, 1f, 1f) * m,
            v + new Vector3(0f, -1f, 1f) * m,
            v + new Vector3(1f, 1f, -1f) * m,
            v + new Vector3(-1f, 0f, 0f) * m,
            v + new Vector3(1f, 1f, -1f) * m,
            v + new Vector3(0f, -1f, 1f) * m,
            v + new Vector3(-1f, 0f, -1f) * m,
            v + new Vector3(0f, 0f, 0f) * m, v + new Vector3(0f, 0, 0f) * m
        });

        LeanTween.moveSpline(target, spline.pts, time).setEase(LeanTweenType.easeInCubic);
    }

    private void HideNextLevelButton(bool justHide) {
        CanvasGroup nextLvlCG = nextLevelBtn.GetComponent<CanvasGroup>();
        nextLvlBtnAlpha = 0;
        nextLvlCG.interactable = false;
        nextLvlCG.blocksRaycasts = false;

        if (!justHide) {
            if (nextLvlBtnVariant != variant) {
                ChooseVariant(nextLvlBtnVariant, false);
            }

            LevelSettings lS = levelMenu.GetLevelSettings(openedLevel + 1, variant);
            StartNewGame(openedLevel + 1, lS.seed, lS.placedSides, lS.finished);
        }
    }
    
    
    
    
    
    
    
    // CLOCK

    [Header("Clock:")]
    public InfoPanel infoPanel;
    public TMP_Text clockText;
    public Color clockRecordShineColor;
    public float clockRecordShineThick;
    public Color clockRegularFinishShineColor;
    public float clockRegularFinishShineThick;
    public float shineTime;
    private Color clockDefaultColor;
    private float clockDefaultThick;
    private bool clockPaused = false;
    private float timeOfStart;
    private float timeOfGame; // neccessary when game is finished and then restarted

    private void ResetClock() {
        clockText.text = "<mspace=0.7em>00:00";
    }

    private void StartTimer() {
        timeOfStart = Time.time;
        randomGameBeforeStart = false;
    }
    /**
    <summary>Pauses the clock, returns false if clock is already paused</summary>
    **/
    public bool PauseTimer() {
        if (!clockPaused) {
            clockPaused = true;
            timeOfGame = Time.time - timeOfStart;
            return true;
        }
        return false;
    }
    /**
    <sumary>Resumes the clock</summary>
    <param name="addition">Time added to time of game</param>
    **/
    public void ResumeTimer(float addition = 0f) {
        if (clockPaused) {
            clockPaused = false;
            timeOfStart = Time.time - timeOfGame - addition;
        }
    }

    
    
    
    
    
    // MONOBEHAVIOUR METHODS
    void Start()
    {
        //InitializeGame();

        // GUIset = 0;
        SetButtons();

    }

    public void InitializeApplication() {
        SetEmptyGameSolution();
        if (finalPieces == null) finalPieces = new GameObject[6];
        Vector3 dir = -(workspace.position - Camera.main.transform.position);
        defaultRotation = workspace.rotation = Quaternion.LookRotation(dir);
        // canvasOf2Btns.SetActive(false);
        EnabledYesNoButtons(false);

        randomRotationForWorkspace = Random.rotation;
        saveScript = GetComponent<SaveScript>();
        #if UNITY_EDITOR
        if (debugColorsOn) colors = debugColors;
        #endif

        saveScript.LoadGameData();

        // SetButtons();
        InitVariant(true);
    }


    void Update()
    {
        if (!levelNotRandom && !randomGameBeforeStart && !finishedGame && !clockPaused) {
            float time = Time.time - timeOfStart;
            clockText.text = "<mspace=0.7em>" + (time / 60 >= 10 ? "" : "0") + (int)(time / 60) + ":" + (time % 60 >= 10 ? "" : "0") + (int)(time % 60);
        }


        CanvasGroup nextLvlBtnCG = nextLevelBtn.GetComponent<CanvasGroup>();
        if (nextLvlBtnCG.alpha != (float)nextLvlBtnAlpha) {
            if (nextLvlBtnAlpha == 1) {
                float alpha = nextLvlBtnCG.alpha;
                alpha += Time.deltaTime / 0.4f;
                if (alpha > 1) alpha = 1f;
                nextLvlBtnCG.alpha = alpha;
            }
            if (nextLvlBtnAlpha == 0) {
                float alpha = nextLvlBtnCG.alpha;
                alpha -= Time.deltaTime / 0.4f;
                if (alpha < 0) alpha = 0f;
                nextLvlBtnCG.alpha = alpha;
            }
        }


        if (!duringPlacing) {
            placingJustStarted = true;
            TutorialScript.TurnOffNote(TutorialScript.Notes.MoveNote);
            TutorialScript.TurnOffNote(TutorialScript.Notes.RotateNote);
            TutorialScript.TurnOffNote(TutorialScript.Notes.SumbitCancelNote);
        }


        if (duringPlacing) {
            if (placingJustStarted) {
                NormalizePlacedSideApperance(available[currentPositionFromAvailable]);
                ApplyClosestToAvailable();

                placedPiece.transform.localPosition = positionForSides[(int)available[currentPositionFromAvailable]];
                placedPiece.transform.localEulerAngles = rotationForSides[(int)available[currentPositionFromAvailable]];
                placedPiece.transform.position += placedPiece.transform.up * movePlacedPieceForwardFor;

                SearchForMistakes();
                placingJustStarted = false;
            }


            // positioning
            if (posState == IndexStateChange.decrease){
                int prevCPFA = currentPositionFromAvailable--;
                if (currentPositionFromAvailable < 0) currentPositionFromAvailable = available.Length - 1;
                // duringRotationWorkspace = true;

                placedPiece.transform.localPosition = positionForSides[(int)available[currentPositionFromAvailable]];
                placedPiece.transform.localEulerAngles = rotationForSides[(int)available[currentPositionFromAvailable]];
                placedPiece.transform.position += placedPiece.transform.up * movePlacedPieceForwardFor;

                NormalizePlacedSideApperance(available[prevCPFA], available[currentPositionFromAvailable]);
                SearchForMistakes();
            }
            if (posState == IndexStateChange.increase){
                int prevCPFA = currentPositionFromAvailable++;
                if (currentPositionFromAvailable == available.Length) currentPositionFromAvailable = 0;
                // duringRotationWorkspace = true;

                placedPiece.transform.localPosition = positionForSides[(int)available[currentPositionFromAvailable]];
                placedPiece.transform.localEulerAngles = rotationForSides[(int)available[currentPositionFromAvailable]];
                placedPiece.transform.position += placedPiece.transform.up * movePlacedPieceForwardFor;

                NormalizePlacedSideApperance(available[prevCPFA], available[currentPositionFromAvailable]);
                SearchForMistakes();
            }
            

            // rotating
            if (rotState == IndexStateChange.increase || rotState == IndexStateChange.decrease) {
                if (rotationPieceJustStarted){
                    Debug.Log("Delayed sound");
                    SoundScript soundScript = GetComponent<SoundScript>();
                    // LeanTween.delayedCall(gameObject, 0.1f, () => {soundScript.PlayPieceRotationSound();});
                    soundScript.PlayPieceRotationSound(0.1f);
                }
                else GetComponent<SoundScript>().PlayPieceRotationSound();
            }
            if (rotState == IndexStateChange.decrease) {
                rotState = IndexStateChange.stay;
                duringRotationPiece = true;
                pieceDestinedRotationY -= 90f;
                pieceRotationInInt += 1;
            }
            if (rotState == IndexStateChange.increase) {
                rotState = IndexStateChange.stay;
                duringRotationPiece = true;
                pieceDestinedRotationY += 90f;
                pieceRotationInInt += 3;
            }


            // placing
            if (acceptButtonPressed) {
                if (PlacePieceAt(placedPiece.GetComponent<Piece>().GetSetting(), available[currentPositionFromAvailable])) {
                    
                }else {
                    GetComponent<SoundScript>().PlayWrongPlaceSound();
                }
            }
            acceptButtonPressed = false;


            // canceling
            if (cancelButtonPressed) {
                // canvasOf2Btns.SetActive(false);
                EnabledYesNoButtons(false);

                GetComponent<SoundScript>().PlayCancelPlacingSound();

                hintScript.HideHintButton();

                placedPiece.GetComponent<Piece>().ChangeTransparencyInTime(0f, 0.1f, true);
                LeanTween.scale(placedPiece, Vector3.zero, 0.1f);
                SetTransparencyOfAllPlacedSides(1f);
                currentPositionFromAvailable = 0;
                if (GetComponent<ScreenOrientationScript>().screenOrientation == ScreenOrientation.Portrait) levelMenu.ToggleRightPanelHideFeatureOn(true);
                duringPlacing = false;
                // duringRotationWorkspace = false;
                EnabledButton(placingPieceButtonIndex, true);
                SearchForMistakes();
            }
            cancelButtonPressed = false;
        }


        if (duringRotationPiece) {
            // once every started rotation return sides settings
            if (rotationPieceJustStarted) {
                for (int i = 0; i < 6; i++) {
                    if (placedSidesArray[i]) {
                        finalPieces[i].GetComponent<Piece>().ChangeSetting(gameSolution[i]);
                    }
                    LeanTween.cancel(rotationPieceAwayMovement.uniqueId);
                    rotationPieceAwayMovement = LeanTween.move(placedPiece, 
                        workspace.TransformPoint(positionForSides[(int)available[currentPositionFromAvailable]]) + (placedPiece.transform.up * 20f), 
                        0.1f).setOnComplete(() => {SearchForMistakes(true);});
                }
                placedPiece.GetComponent<Piece>().RetrieveColor();
                // if (!placedPiece.LeanIsTweening())
                rotationPieceJustStarted = false;
            }

            float rotationOfY = pieceDestinedRotationY * Time.deltaTime * 8f;
            pieceDestinedRotationY -= rotationOfY;

            placedPiece.transform.Rotate(new Vector3(
                0,
                rotationOfY,
                0
            ), Space.Self);

            if (pieceDestinedRotationY < 1f && pieceDestinedRotationY > -1f) {
                placedPiece.GetComponent<Piece>().RotateClockwise(pieceRotationInInt);
                SearchForMistakes();
                placedPiece.transform.localEulerAngles = rotationForSides[(int)available[currentPositionFromAvailable]];

                float delay = 0f;
                PiecePG piecePG;
                if (placedPiece.TryGetComponent<PiecePG>(out piecePG)) {
                    delay = piecePG.shiftTime;
                }
                LeanTween.cancel(rotationPieceAwayMovement.uniqueId);
                rotationPieceAwayMovement = LeanTween.moveLocal(placedPiece, positionForSides[(int)available[currentPositionFromAvailable]], 0.1f).setDelay(delay);
                pieceDestinedRotationY = 0f;
                pieceRotationInInt = 0;
                duringRotationPiece = false;
                rotationPieceJustStarted = true;
            }
        }


        // Rotation of workspace after game finished
        if (duringRotationWorkspace) {
            Quaternion preedit = workspace.rotation;
            // Quaternion destinedRotation = defaultRotation * Quaternion.Euler(rotationOfWorkspace[sideToRotateTowardsIndex]);
            Quaternion destinedRotation = randomRotationForWorkspace;
            WorkspaceRotation workspaceRotation = workspace.GetComponent<WorkspaceRotation>();
            if (!workspaceRotation.oneFingerOn && !workspaceRotation.twoFingersOn)
                workspace.rotation = Quaternion.SlerpUnclamped(workspace.rotation, destinedRotation, Time.deltaTime / (Quaternion.Angle(workspace.rotation, destinedRotation) + 1) * 20f);
            if (workspace.rotation == preedit) {
                // sideToRotateTowardsIndex++;
                // if (sideToRotateTowardsIndex > 5) {
                //     sideToRotateTowardsIndex = 0;
                // }
                randomRotationForWorkspace = Random.rotation;
            }
        }


        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.O)) {
            VisualizeDataFromSolution(gameSolution, "GameSolution on demand");
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            FinishedLevel();
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            hintScript.genrSolution.Set(gameSolution);
            DebugNewGame(hintScript.genrSolution.RotateClockwiseByCube(timesX: 1).GetArrayCopy());
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            hintScript.genrSolution.Set(gameSolution);
            DebugNewGame(hintScript.genrSolution.RotateClockwiseByCube(timesY: 1).GetArrayCopy());
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            hintScript.genrSolution.Set(gameSolution);
            DebugNewGame(hintScript.genrSolution.RotateClockwiseByCube(timesZ: 1).GetArrayCopy());
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            string str = "Visualize piecesButtonsIndexes[]:\n";
            for(int i = 0; i < piecesButtonsIndexes.Length; i++) {
                str += "side " + (Side)i + ": btn " + piecesButtonsIndexes[i] + '\n';
            }
            Debug.Log(str);
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screenshots/" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png");
        }

        #endif
    }
}
