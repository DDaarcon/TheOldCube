using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using static Enums;

namespace Assets.Scripts.GameBase
{
    public class TempAllVar
    {

        public bool[][,] gameSolution { get; private set; }

        private struct dNs
        {
            public Side s;
            public Dir d;
            public dNs(Side s_, Dir d_)
            {
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

        /**
        <value>Data representation of solved puzzle (in one of ways at least, shouldn't be used in checking correction of placed side)</value>
        **/
        public bool[][,] genrSolution { get; private set; }

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
        public int placingPieceButtonIndex { get; private set; }
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
        public int[] buttonOrder { get; private set; }



        private Quaternion defaultRotation;

        public Themes gameTheme { get; private set; } = Themes.BasicStone;
        public int placedSides { get; private set; } = 0;
        public bool[] placedSidesArray { get; private set; } = { O, O, O, O, O, O };
        private GameObject[] finalPieces;
        private GameObject placedPiece;
        private Side placedSide;
        private bool duringRotationPiece;
        // private bool rotationPieceJustStarted;
        private int pieceRotationInInt = 0;
        private float pieceDestinedRotationY;
        private bool rotationPieceJustStarted = true;
        private LTDescr rotationPieceAwayMovement = new LTDescr();
        public bool duringPlacing { get; private set; } = false;
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
        public static float lengthOfSide { get; private set; } = 20f;
        private static float s3Len;
        public Vector3[] positionForSides { get; private set; }
        public Vector3[] rotationForSides { get; private set; } = new Vector3[6] {
            new Vector3(180, 0, 0),   // bottom
            new Vector3(90, 0, 0), // back
            new Vector3(0, 180, -90), // left
            new Vector3(0, 180, 90),  // right
            new Vector3(-90, 0, 0),  // front
            new Vector3(0, 180, 0), // top
        };

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
        public bool finishedGame { get; private set; } = false; // might be useful for displaying time of game
        private bool gameFinishedAndRestarted = false;
        private bool tryingTheme = false;
        // private int sideToRotateTowardsIndex = 0;
        /**
        <value>Random rotation used for animating solved puzzle</value>
        **/
        private Quaternion randomRotationForWorkspace;

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
    }
}
