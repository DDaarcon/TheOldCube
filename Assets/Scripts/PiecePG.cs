using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PiecePartMeshGenerate;
using static Enums;

public class PiecePG : Piece
{

    // to add: 
    // if vertices are already set don't set them again
    
    private List<Vector3> allVertices;
    private List<Vector2> allUVs;
    private List<int> allTriangles;
    private Mesh mesh;
    private Mesh colliderMesh;
    private bool uvIncluded;
    private int operationsCounter = 0; // for debug
    private bool decayFirstFrame;
    private bool shiftRenewTriangles = false;

    [Header("Mesh Procedural Generation:")]

    [SerializeField] private Themes pieceTheme = Themes.BasicStone;
    public bool printDebugMessages = true;
    public int meshPointsInRow;
    public float sizeOfSide;
    public float distanceBetweenSidesOfCube;
    public float distanceBetweenCubes;
    public float scopeForZValue;
    public int overrideVariant = 0;
    public Vector2 uvStart;
    public Vector2 uvEnd;
    public GameObject cubePrefab;
    public Color32 color32 {get; protected set;}
    public Material fancyStoneMaterialTemplate;
    public Material goldMaterialTemplate;
    public Material minecraftMaterialTemplate;
    public Material redShinyMaterialTemplate;
    public Material copperMaterialTemplate;
    public Material blueShinyMaterialTemplate;
    public Material darkElementMaterialTemplate;
    public Material ticTacToeMaterialTemplate;

    [Header("Cubes Offset")]
    /**
    <value>Time in which cubes are shifted</value>
    **/
    public float shiftTime = 0.1f;
    public float shiftDistance = 20f;
//     private float screenshotDelay = 0.4f;


    private readonly float[,] darkElementHeights = {
        {-1f, 0f, 1f, 1f, 1f, 0f, -1f},
        {0f, 1f, 2f, 2f, 2f, 1f, 0f},
        {1f, 2f, 2f, 3f, 2f, 2f, 1f},
        {1f, 2f, 3f, 3f, 3f, 2f, 1f},
        {1f, 2f, 2f, 3f, 2f, 2f, 1f},
        {0f, 1f, 2f, 2f, 2f, 1f, 0f},
        {-1f, 0f, 1f, 1f, 1f, 0f, -1f}
    };

    private readonly float[,] ticTacToeHeightsCircle = {
        {0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f},
        {0f, 0f, 0f, -2f, -2f, -2f, 0f, 0f, 0f},
        {0f, 0f, -2f, -2f, -2f, -2f, -2f, 0f, 0f},
        {0f, -2f, -2f, -2f, 0f, -2f, -2f, -2f, 0f},
        {0f, -2f, -2f, 0f, 0f, 0f, -2f, -2f, 0f},
        {0f, -2f, -2f, -2f, 0f, -2f, -2f, -2f, 0f},
        {0f, 0f, -2f, -2f, -2f, -2f, -2f, 0f, 0f},
        {0f, 0f, 0f, -2f, -2f, -2f, 0f, 0f, 0f},
        {0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f},
    };
    private readonly float[,] ticTacToeHeightsCross = {
        {0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f},
        {0f, -2f, -2f, 0f, 0f, 0f, -2f, -2f, 0f},
        {0f, -2f, -2f, -2f, 0f, -2f, -2f, -2f, 0f},
        {0f, 0f, -2f, -2f, -2f, -2f, -2f, 0f, 0f},
        {0f, 0f, 0f, -2f, -2f, -2f, 0f, 0f, 0f},
        {0f, 0f, -2f, -2f, -2f, -2f, -2f, 0f, 0f},
        {0f, -2f, -2f, -2f, 0f, -2f, -2f, -2f, 0f},
        {0f, -2f, -2f, 0f, 0f, 0f, -2f, -2f, 0f},
        {0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f},
    };


    protected override void Awake()
    {
        SetTheme(pieceTheme);
        color32 = new Color32(255, 255, 255, 255);
        
        FindChildren();

        // testings
        
        // ChangeSetting();
    }

    /**
    <summary>Change variant of Piece (3x3, 4x4)</summary>
    **/
    public void ChangeVariant(Variant variant) {
        if (this.variant != variant) {
            this.variant = variant;
            FindChildren();
        }
    }

    public void SetTheme(Themes theme, int textureIt = 0) {
        pieceTheme = theme;
        switch (theme) {
            default:
                material = new Material(materialTemplate);
                AssignStandardMaterial();
                break;
            case Themes.BasicStone:
                scopeForZValue = 1.5f;
                meshPointsInRow = 3;
                sizeOfSide = 16f;
                distanceBetweenSidesOfCube = 18f;
                distanceBetweenCubes = 20f;
                uvIncluded = false;
                material = new Material(materialTemplate);
                AssignStandardMaterial();
                break;
            case Themes.FancyStone:
                scopeForZValue = 2f;
                meshPointsInRow = 3;
                sizeOfSide = 12f;
                distanceBetweenSidesOfCube = 18f;
                distanceBetweenCubes = 20f;
                uvIncluded = false;
                material = new Material(fancyStoneMaterialTemplate);
                AssignStandardMaterial();
                break;
            case Themes.Gold:
                scopeForZValue = 0f;
                meshPointsInRow = 2;
                sizeOfSide = 16f;
                distanceBetweenSidesOfCube = 18f;
                distanceBetweenCubes = 20f;
                uvIncluded = false;
                material = new Material(goldMaterialTemplate);
                AssignStandardMaterial();
                break;
            case Themes.Minecraft:
                scopeForZValue = 0f;
                meshPointsInRow = 2;
                sizeOfSide = 19.9f;
                distanceBetweenSidesOfCube = 19.9f;
                distanceBetweenCubes = 20f;
                uvIncluded = true;
                uvStart = new Vector2((1f / 3f) * (textureIt % 3), 0.5f * (textureIt / 3));
                uvEnd = new Vector2((1f / 3f) * ((textureIt % 3) + 1), 0.5f * ((textureIt / 3) + 1));
                material = new Material(minecraftMaterialTemplate);
                AssignStandardMaterial();
                break;
            case Themes.RedShiny:
                scopeForZValue = 1f;
                meshPointsInRow = 3;
                sizeOfSide = 16f;
                distanceBetweenSidesOfCube = 18f;
                distanceBetweenCubes = 20f;
                uvIncluded = false;
                material = new Material(redShinyMaterialTemplate);
                AssignStandardMaterial();
                break;
            case Themes.Copper:
                scopeForZValue = 0.3f;
                meshPointsInRow = 3;
                sizeOfSide = 16f;
                distanceBetweenSidesOfCube = 18f;
                distanceBetweenCubes = 20f;
                uvIncluded = false;
                material = new Material(copperMaterialTemplate);
                AssignStandardMaterial();
                break;
            case Themes.BlueShiny:
                scopeForZValue = 0f;
                meshPointsInRow = 2;
                sizeOfSide = 12f;
                distanceBetweenSidesOfCube = 18f;
                distanceBetweenCubes = 20f;
                uvIncluded = false;
                material = new Material(blueShinyMaterialTemplate);
                AssignStandardMaterial();
                break;
            case Themes.DarkElement:
                scopeForZValue = 0f;
                meshPointsInRow = 7;
                sizeOfSide = 16f;
                distanceBetweenSidesOfCube = 18f;
                distanceBetweenCubes = 20f;
                uvIncluded = false;
                material = new Material(darkElementMaterialTemplate);
                AssignStandardMaterial();
                break;
            case Themes.TicTacToe:
                scopeForZValue = 0f;
                meshPointsInRow = 9;
                sizeOfSide = 16f;
                distanceBetweenSidesOfCube = 18f;
                distanceBetweenCubes = 20f;
                uvIncluded = false;
                material = new Material(ticTacToeMaterialTemplate);
                AssignStandardMaterial();
                break;

        }
        PositionChildren();
        PrepareSidesVisibility();
    }
    /**
    <summary>Find children (cubes)</summary>
    **/
	protected override void FindChildren()
	{
        if (children != null)
        if (children[0, 0] != null) {
            for (int i = 0; i < variantInt; i++) {
                for (int j = 0; j < variantInt; j++) {
                    Destroy(children[i, j].gameObject);
                }
            }
        }
		if (overrideVariant > 0) {
            variantInt = overrideVariant;
            setting = new bool[variantInt, variantInt];
            children = new GameObject[variantInt, variantInt];
        }
        else if (variant == Variant.x3) {
            variantInt = 3;
            setting = new bool[3, 3]{
                {O, O, O}, 
                {O, O, O},
                {O, O, O}};
            children = new GameObject[3, 3];
        } else if (variant == Variant.x4) {
            variantInt = 4;
            setting = new bool[4, 4]{
                {O, O, O ,O}, 
                {O, O, O, O},
                {O, O, O, O},
                {O, O, O, O}};
            children = new GameObject[4, 4];
        }

        // float a = distanceBetweenSidesOfCube / 2f;
        // cubePrefab.transform.GetChild(0).transform.localPosition = new Vector3(0f, -a, 0f);
        // cubePrefab.transform.GetChild(1).transform.localPosition = new Vector3(0f, 0f, a);
        // cubePrefab.transform.GetChild(2).transform.localPosition = new Vector3(-a, 0f, 0f);
        // cubePrefab.transform.GetChild(3).transform.localPosition = new Vector3(a, 0f, 0f);
        // cubePrefab.transform.GetChild(4).transform.localPosition = new Vector3(0f, 0f, -a);
        // cubePrefab.transform.GetChild(5).transform.localPosition = new Vector3(0f, a, 0f);
    

        PositionChildren();
	}
    /*
    <summary>Instantiate (if necessary) and position all children</summary>
    */
    private void PositionChildren() {
        float totalDistance = distanceBetweenCubes * variantInt;

        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                if (children[i, j] == null)
                children[i, j] = Instantiate(cubePrefab, transform);
                children[i, j].name = /* "Cube_" + */ i + ":" + j;
                children[i, j].GetComponent<PiecePartMeshGenerate>().CalculateOwnVertices();
                children[i, j].GetComponent<PiecePartMeshGenerate>().rename();
                Vector3 pos = Vector3.zero;
                pos.x = -(totalDistance / 2f) + (distanceBetweenCubes / 2f) + distanceBetweenCubes * j;
                pos.z = -(totalDistance / 2f) + (distanceBetweenCubes / 2f) + distanceBetweenCubes * i;
                children[i, j].transform.localPosition = pos;

                float a = distanceBetweenSidesOfCube / 2f;
                children[i, j].transform.GetChild(0).transform.localPosition = new Vector3(0f, -a, 0f);
                children[i, j].transform.GetChild(1).transform.localPosition = new Vector3(0f, 0f, a);
                children[i, j].transform.GetChild(2).transform.localPosition = new Vector3(-a, 0f, 0f);
                children[i, j].transform.GetChild(3).transform.localPosition = new Vector3(a, 0f, 0f);
                children[i, j].transform.GetChild(4).transform.localPosition = new Vector3(0f, 0f, -a);
                children[i, j].transform.GetChild(5).transform.localPosition = new Vector3(0f, a, 0f);
            }
        }
    }
	protected override void AssignStandardMaterial()
	{
        GetComponent<MeshRenderer>().material = material;
	}
    /*
    <summary>Set which cubes are shifted</summary>
    <param name="arrang">Array representing shifted cubes, 0 for shifted, 1 for normal</param>
    */
	public override void ChangeSettingForShift(bool[,] arrang = null)
	{
        if (arrang != null) {
            ShiftCubes(arrang);
            // PrepareSidesVisibilityShifted();
        } else {
            ShiftCubes(setting);
            // PrepareSidesVisibility();
        }
	}
    
    public override void RotateClockwise(int times) {
        times %= 4;
        int l = variantInt - 1;
        if (times % 2 == 1) {
            for (int b = 0; b < l; b++) {
                bool temp = setting[b, 0];
                // x increasing, y static min
                setting[b, 0] = setting[l, b];
                // x static max, y increasing
                setting[l, b] = setting[l - b, l];
                // x decreasing, y static max
                setting[l - b, l] = setting[0, l - b];
                // x static min, y decreasing
                setting[0, l - b] = temp;

                for (int a = 0; a < variantInt / 2; a++) {
                    if (b + a < l - a) {
                        Vertices tempV = children[b + a, a].GetComponent<PiecePartMeshGenerate>().vertices.GetCopy().RotateClockwiseByCube(1);
                        // x increasing with a and b, y increasing with a
                        children[b + a, a].GetComponent<PiecePartMeshGenerate>().vertices.Set(children[variantInt - 1 - a, b + a].GetComponent<PiecePartMeshGenerate>().vertices.RotateClockwiseByCube(1));
                        // x decreasing with a, y increasing with a and b
                        children[variantInt - 1 - a, b + a].GetComponent<PiecePartMeshGenerate>().vertices.Set(children[variantInt - 1 - b - a, variantInt - 1 - a].GetComponent<PiecePartMeshGenerate>().vertices.RotateClockwiseByCube(1));
                        // x decreasing with a and b, y decreasing with a
                        children[variantInt - 1 - b - a, variantInt - 1 - a].GetComponent<PiecePartMeshGenerate>().vertices.Set(children[a, variantInt - 1 - b - a].GetComponent<PiecePartMeshGenerate>().vertices.RotateClockwiseByCube(1));
                        // x increasing with a, y decreasing with a and b
                        children[a, variantInt - 1 - b - a].GetComponent<PiecePartMeshGenerate>().vertices.Set(tempV);
                        // Vertices tempV = children[b + a, a].GetComponent<PiecePartMeshGenerate>().vertices.GetCopy().RotateClockwiseByCube(1);
                        // // x increasing with a and b, y increasing with a
                        // children[b + a, a].GetComponent<PiecePartMeshGenerate>().vertices.Set(children[a, variantInt - 1 - b - a].GetComponent<PiecePartMeshGenerate>().vertices.RotateClockwiseByCube(1));
                        // // x increasing with a, y decreasing with a and b
                        // children[a, variantInt - 1 - b - a].GetComponent<PiecePartMeshGenerate>().vertices.Set(children[variantInt - 1 - b - a, variantInt - 1 - a].GetComponent<PiecePartMeshGenerate>().vertices.RotateClockwiseByCube(1));
                        // // x decreasing with a and b, y decreasing with a
                        // children[variantInt - 1 - b - a, variantInt - 1 - a].GetComponent<PiecePartMeshGenerate>().vertices.Set(children[variantInt - 1 - a, b + a].GetComponent<PiecePartMeshGenerate>().vertices.RotateClockwiseByCube(1));
                        // // x decreasing with a, y increasing with a and b
                        // children[variantInt - 1 - a, b + a].GetComponent<PiecePartMeshGenerate>().vertices.Set(tempV);
                        
                    }
                }
            }
            times--;
        }
        if (times == 2) {
            for (int b = 0; b < l; b++) {
                bool temp = setting[b, 0];
                // x increasing, y static min
                setting[b, 0] = setting[l - b, l];
                // x decreasing, y static max
                setting[l - b, l] = temp;
                temp = setting[l, b];
                // x static max, y increasing
                setting[l, b] = setting[0, l - b];
                // x static min, y decreasing
                setting[0, l - b] = temp;

                for (int a = 0; a < variantInt / 2; a++) {
                    if (b + a < l - a) {
                        Vertices tempV = children[b + a, a].GetComponent<PiecePartMeshGenerate>().vertices.GetCopy().RotateClockwiseByCube(2);
                        // x increasing with a and b, y increasing with a
                        children[b + a, a].GetComponent<PiecePartMeshGenerate>().vertices.Set(children[variantInt - 1 - b - a, variantInt - 1 - a].GetComponent<PiecePartMeshGenerate>().vertices.RotateClockwiseByCube(2));
                        // x decreasing with a and b, y decreasing with a
                        children[variantInt - 1 - b - a, variantInt - 1 - a].GetComponent<PiecePartMeshGenerate>().vertices.Set(tempV);
                        tempV = children[variantInt - 1 - a, b + a].GetComponent<PiecePartMeshGenerate>().vertices.GetCopy().RotateClockwiseByCube(2);
                        // x decreasing with a, y increasing with a and b
                        children[variantInt - 1 - a, b + a].GetComponent<PiecePartMeshGenerate>().vertices.Set(children[a, variantInt - 1 - b - a].GetComponent<PiecePartMeshGenerate>().vertices.RotateClockwiseByCube(2));
                        // x increasing with a, y decreasing with a and b
                        children[a, variantInt - 1 - b - a].GetComponent<PiecePartMeshGenerate>().vertices.Set(tempV);
                    }
                }
            }
        }
        ChangeSetting();
    }
    
    private Vector3[] gv(int i, int j, Side side_) {
        return children[i, j].GetComponent<PiecePartMeshGenerate>().GetVerticesBySide(side_);
    }
    private void sv(int i, int j, Side side_, Vector3[] vert) {
        children[i, j].GetComponent<PiecePartMeshGenerate>().SetVerticesBySide(side_, vert);
    }
    // private void rv(int i, int j, Side side_, int times) {
    //     children[i, j].GetComponent<PiecePartMeshGenerate>().RotateVerticesArrayClockwiseBySide(Side.front, times);
    // }
    protected override void ChangeMaterialFor(bool[,] arrang) {}
    public override void InitializeDecay(float time) {
        directionsForThrow = new Vector3[variantInt * variantInt];
        timeToDisappear = time;

        for (int i = 0; i < variantInt * variantInt; i++) {
            int x = i / variantInt, y = i % variantInt;
            directionsForThrow[i] = transform.up * (throwStrength + Random.Range(-throwStrengthScope, throwStrengthScope));
            Quaternion rotation = Quaternion.AngleAxis(Random.Range(-rangeForThrowDirections, rangeForThrowDirections), transform.up) *
                                    Quaternion.AngleAxis(Random.Range(-rangeForThrowDirections, rangeForThrowDirections), transform.right) *
                                    Quaternion.AngleAxis(Random.Range(-rangeForThrowDirections, rangeForThrowDirections), transform.forward);
            directionsForThrow[i] = rotation * directionsForThrow[i];

            children[x, y].LeanScale(Vector3.zero, 1f).delay = timeToDisappear + Random.Range(-timeToDisappearScope, timeToDisappearScope);
        }
        LeanTween.delayedCall(timeToDisappear + timeToDisappearScope + 1f, () => {/* ChangeTransparencyInTime(1f, 1f, true); */ Destroy(this.gameObject);});
        
        decayFirstFrame = true;
        duringDecay = true;
    }
    /**
    <summary>Operations done continuesly until destruction of Piece</summary>
    **/
	protected override void DecayContinous()
	{
		base.DecayContinous();
        PrepareSidesVisibilitySeparately();

	}
    public override void ChangeColor(Color color_) {
        color = color_;
        ChangeColor((Color32)color_);
    }
    public void ChangeColor(Color32 color32_) {
        color32 = color32_;
        SetColorsOnMesh(color32);
    }
    public override void RetrieveColor() {
        while (cubeAnimationData.Count != 0) {
            RemoveCubeAnimation(cubeAnimationData.Count - 1);
        }
        SetColorsOnMesh(color32);
    }
    public override void ChangeColorFor(Color color_, float trans, bool[,] arrang) {
        ChangeColorFor((Color32)color_, arrang, 1f, 0.5f, 0f, true);
    }
    public void ChangeColorFor(Color32 color32_, bool[,] arrang, float speed = 0f, float intensityMax = 1f, float intensityMin = 0f, bool toWhite = true) {
        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                if (arrang[i, j] == I) {
                    if (speed > 0f)
                        AddCubeAnimation(children[i, j].GetComponent<PiecePartMeshGenerate>(), color32_, speed, intensityMax, intensityMin/* , toWhite */);
                    else SetColorsOnMesh(children[i, j].GetComponent<PiecePartMeshGenerate>(), color32_);
                }
            }
        }
    }
    public override void ChangeTransparency(float trans, bool abandonChangeInTime = false) {}
    public override void ChangeTransparencyInTime(float destinedTransparency, float time, bool destroyOnFinish = false) {
        if (destroyOnFinish)
        LeanTween.delayedCall(time, () => {
            if (this != null)
                Destroy(this.gameObject);
            });
    }

    /**
    <summary>For PiecePG: Creating meshes for MeshFilter and MeshCollider</summary>
    **/
	protected override void PrepareSidesVisibility()
	{
        if (printDebugMessages)Debug.Log("Setting changes on this piece: " + ++operationsCounter);
        allVertices = new List<Vector3>();
        allTriangles = new List<int>();
        if (uvIncluded) allUVs = new List<Vector2>();
        mesh = new Mesh();
        colliderMesh = new Mesh();

        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                if (children[i, j].activeSelf) {
                    PiecePartMeshGenerate above = null, below = null, onLeft = null, onRight = null;

                    if (i > 0) if (children[i - 1, j].activeSelf) below = children[i - 1, j].GetComponent<PiecePartMeshGenerate>();
                    if (i < variantInt - 1) if (children[i + 1, j].activeSelf) above = children[i + 1, j].GetComponent<PiecePartMeshGenerate>();
                    if (j > 0) if (children[i, j - 1].activeSelf) onLeft = children[i, j - 1].GetComponent<PiecePartMeshGenerate>();
                    if (j < variantInt - 1) if (children[i, j + 1].activeSelf) onRight = children[i, j + 1].GetComponent<PiecePartMeshGenerate>();

                    if (pieceTheme == Themes.DarkElement || pieceTheme == Themes.TicTacToe) {
                        List<float[,]> table = new List<float[,]>();
                        if (pieceTheme == Themes.DarkElement) {
                            table.Add(darkElementHeights);
                        } else if (pieceTheme == Themes.TicTacToe) {
                            table.Add(ticTacToeHeightsCircle); table.Add(ticTacToeHeightsCross);
                        }
                        children[i, j].GetComponent<PiecePartMeshGenerate>().Initialize(below, above, onLeft, onRight, meshPointsInRow, sizeOfSide, table, distanceBetweenCubes);
                    } else {
                        children[i, j].GetComponent<PiecePartMeshGenerate>().Initialize(below, above, onLeft, onRight, meshPointsInRow, sizeOfSide, scopeForZValue, distanceBetweenCubes);
                    }
                }
            }
        }

        GetVerticesFromChildren();
        CalculateTriangles();
        
        CalculateColliderMeshTriangles();

        mesh.vertices = allVertices.ToArray();
        mesh.triangles = allTriangles.ToArray();
        if (uvIncluded) mesh.uv = allUVs.ToArray();
        mesh.colors32 = new Color32[mesh.vertices.Length];
        SetColorsOnMesh(color32);        

        mesh.RecalculateNormals();


        GetComponent<MeshCollider>().sharedMesh = colliderMesh;
        GetComponent<MeshFilter>().mesh = mesh;
	}
    /**
    <summary>Variant of calculating mesh method just for decay animation</summary>
    **/
    protected void PrepareSidesVisibilitySeparately()
	{
        allVertices = new List<Vector3>();
        if (decayFirstFrame){
            allTriangles = new List<int>();
            allUVs = new List<Vector2>();
        }

        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                if (children[i, j].activeSelf) {
                    
                    if (pieceTheme == Themes.DarkElement || pieceTheme == Themes.TicTacToe) {
                        List<float[,]> table = new List<float[,]>();
                        if (pieceTheme == Themes.DarkElement) {
                            table.Add(darkElementHeights);
                        } else if (pieceTheme == Themes.TicTacToe) {
                            table.Add(ticTacToeHeightsCircle); table.Add(ticTacToeHeightsCross);
                        }
                        children[i, j].GetComponent<PiecePartMeshGenerate>().Initialize(null, null, null, null, meshPointsInRow, sizeOfSide, table, distanceBetweenCubes);
                    } else {
                        children[i, j].GetComponent<PiecePartMeshGenerate>().Initialize(null, null, null, null, meshPointsInRow, sizeOfSide, scopeForZValue, distanceBetweenCubes);
                    }
                }
            }
        }

        GetVerticesFromChildren();
        mesh.vertices = allVertices.ToArray();

        if (decayFirstFrame) {
            CalculateTriangles();

            mesh.triangles = allTriangles.ToArray();
            if (uvIncluded) mesh.uv = allUVs.ToArray();
            mesh.colors32 = new Color32[mesh.vertices.Length];
            SetColorsOnMesh(color32);        

            mesh.RecalculateNormals();
            decayFirstFrame = false;
        }

        GetComponent<MeshFilter>().mesh = mesh;
	}
    /**
    <summary>Variant of calculating mesh method with shifted cubes</summary>
    **/
	protected void PrepareSidesVisibilityShifted()
	{
        allVertices = new List<Vector3>();
        if (shiftRenewTriangles) {
            allTriangles = new List<int>();
            allUVs = new List<Vector2>();
            mesh.Clear();
        }

        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                if (children[i, j].activeSelf) {
                    byte shiftedState = shiftedCubes[i, j];
                    bool duringShift = children[i, j].GetComponent<PiecePartMeshGenerate>().duringShift;
                    PiecePartMeshGenerate above = null, below = null, onLeft = null, onRight = null;

                    if (i > 0) if (children[i - 1, j].activeSelf &&
                        shiftedCubes[i - 1, j] == shiftedState && 
                        children[i - 1, j].GetComponent<PiecePartMeshGenerate>().duringShift == duringShift) 
                        below = children[i - 1, j].GetComponent<PiecePartMeshGenerate>();
                    if (i < variantInt - 1) if (children[i + 1, j].activeSelf && 
                        shiftedCubes[i + 1, j] == shiftedState &&
                        children[i + 1, j].GetComponent<PiecePartMeshGenerate>().duringShift == duringShift) 
                        above = children[i + 1, j].GetComponent<PiecePartMeshGenerate>();
                    if (j > 0) if (children[i, j - 1].activeSelf && 
                        shiftedCubes[i, j - 1] == shiftedState &&
                        children[i, j - 1].GetComponent<PiecePartMeshGenerate>().duringShift == duringShift) 
                        onLeft = children[i, j - 1].GetComponent<PiecePartMeshGenerate>();
                    if (j < variantInt - 1) if (children[i, j + 1].activeSelf && 
                        shiftedCubes[i, j + 1] == shiftedState &&
                        children[i, j + 1].GetComponent<PiecePartMeshGenerate>().duringShift == duringShift) 
                        onRight = children[i, j + 1].GetComponent<PiecePartMeshGenerate>();


                    if (pieceTheme == Themes.DarkElement || pieceTheme == Themes.TicTacToe) {
                        List<float[,]> table = new List<float[,]>();
                        if (pieceTheme == Themes.DarkElement) {
                            table.Add(darkElementHeights);
                        } else if (pieceTheme == Themes.TicTacToe) {
                            table.Add(ticTacToeHeightsCircle); table.Add(ticTacToeHeightsCross);
                        }
                        children[i, j].GetComponent<PiecePartMeshGenerate>().Initialize(below, above, onLeft, onRight, meshPointsInRow, sizeOfSide, table, distanceBetweenCubes);
                    } else {
                        children[i, j].GetComponent<PiecePartMeshGenerate>().Initialize(below, above, onLeft, onRight, meshPointsInRow, sizeOfSide, scopeForZValue, distanceBetweenCubes);
                    }
                }
            }
        }

        GetVerticesFromChildren();
        mesh.vertices = allVertices.ToArray();
        if (shiftRenewTriangles) {
            if (printDebugMessages)Debug.Log("renew triangles, piece " + this.name);
            CalculateTriangles();
            if (printDebugMessages)Debug.Log(allTriangles.Count);

            mesh.triangles = allTriangles.ToArray();
            if (uvIncluded) mesh.uv = allUVs.ToArray();
            mesh.colors32 = new Color32[mesh.vertices.Length];
            SetColorsOnMesh(color32);

            mesh.RecalculateNormals();
            shiftRenewTriangles = false;
        }
        GetComponent<MeshFilter>().mesh = mesh;
	}

    private void GetVerticesFromChildren() {
        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                if (children[i, j].activeSelf) {
                    PiecePartMeshGenerate childPP = children[i, j].GetComponent<PiecePartMeshGenerate>();
                    childPP.piecePartMeshData.startingIndex = allVertices.Count;
                    for (int s = 0; s < 6; s++) {
                        if (childPP.GetChildPresanceBySide((Side)s)) {
                            for (int v = 0; v < childPP.GetVerticesBySide((Side)s).Length; v++) {
                                Vector3 temp = transform.InverseTransformPoint(childPP.GetChildBySide((Side)s).transform.TransformPoint(childPP.GetVerticesBySide((Side)s)[v]));
                                allVertices.Add(temp);
                                if (uvIncluded) {
                                    float incrementU = (uvEnd.x - uvStart.x) / (meshPointsInRow - 1);
                                    float incrementV = (uvEnd.y - uvStart.y) / (meshPointsInRow - 1);
                                    Vector2 tempUV = new Vector2(uvStart.x + incrementU * (v % meshPointsInRow), uvStart.y + incrementV * (v / meshPointsInRow));
                                    allUVs.Add(tempUV);
                                }
                            }
                            childPP.piecePartMeshData.Amount((Side)s, childPP.GetVerticesBySide((Side)s).Length);
                        }
                        else {
                            childPP.piecePartMeshData.Amount((Side)s, 0);
                        }
                    }
                }
            }
        }
    }

    private void CalculateTriangles() {
        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                if (children[i, j].activeSelf) {
                    PiecePartMeshGenerate childPP = children[i, j].GetComponent<PiecePartMeshGenerate>();
                    for (int s = 0; s < 6; s++) {
                        if (childPP.GetChildPresanceBySide((Side)s)) {
                            CalculateTriangles(childPP, (Side)s);
                        }
                    }
                }
            }
        }
    }
    private void CalculateTriangles(PiecePartMeshGenerate childPP, Side side_) {


        for (int line = 0; line < meshPointsInRow - 1; line++) {
            for (int i = 0; i < (meshPointsInRow - 1); i++) {
                allTriangles.Add(childPP.piecePartMeshData.StartForSide(side_) + i + (meshPointsInRow * (line + 1)));
                allTriangles.Add(childPP.piecePartMeshData.StartForSide(side_) + i + (meshPointsInRow * line) + 1);
                allTriangles.Add(childPP.piecePartMeshData.StartForSide(side_) + i + (meshPointsInRow * line));

                allTriangles.Add(childPP.piecePartMeshData.StartForSide(side_) + i + (meshPointsInRow * (line + 1)));
                allTriangles.Add(childPP.piecePartMeshData.StartForSide(side_) + i + (meshPointsInRow * (line + 1)) + 1);
                allTriangles.Add(childPP.piecePartMeshData.StartForSide(side_) + i + (meshPointsInRow * line) + 1);
            }
        }

        // set neighbour vertices for vertices arrays
        // ------------------ BOTTOM
        if (side_ == Side.bottom) {
            // up
            CalculateEdgeTriangles(childPP, side_, Dir.U, childPP, Dir.D, Side.front);
            // right
            if (childPP.hasOnRight){
                if (!childPP.cubeOnRight.hasBelow) {
                    CalculateEdgeTriangles(childPP, side_, Dir.Rn, childPP.cubeOnRight, Dir.Ln, Side.bottom);
                    CalculateCornerTriangle(childPP, side_, Corner.Ur, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP, Side.front, Corner.Dr, 
                        childPP.cubeOnRight, Side.bottom, Corner.Ul);
                    CalculateCornerTriangle(childPP, side_, Corner.Dr, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeOnRight, Side.bottom, Corner.Dl, 
                        childPP, Side.back, Corner.Ur);
                }
                else {
                    CalculateEdgeTriangles(childPP, side_, Dir.Rn, childPP.cubeOnRight.cubeBelow, Dir.U, Side.left);
                }
            }
            else {
                CalculateEdgeTriangles(childPP, side_, Dir.Rn, childPP, Dir.D, Side.right);
            }
            // down
            CalculateEdgeTriangles(childPP, side_, Dir.Dn, childPP, Dir.Un, Side.back);
            // left
            if (childPP.hasOnLeft) {
                if (!childPP.cubeOnLeft.hasBelow) {
                    CalculateEdgeTriangles(childPP, side_, Dir.L, childPP.cubeOnLeft, Dir.R, Side.bottom);
                    CalculateCornerTriangle(childPP, side_, Corner.Dl, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP, Side.back, Corner.Ul, 
                        childPP.cubeOnLeft, Side.bottom, Corner.Dr);
                    CalculateCornerTriangle(childPP, side_, Corner.Ul, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP, Side.front, Corner.Dl, 
                        childPP.cubeOnLeft, Side.bottom, Corner.Ur, true);
                }
                else {
                    CalculateEdgeTriangles(childPP, side_, Dir.Ln, childPP.cubeOnLeft.cubeBelow, Dir.U, Side.right);
                }
            }
            else {
                CalculateEdgeTriangles(childPP, side_, Dir.L, childPP, Dir.D, Side.left);
            }
        }

        // ------------------ BACK
        if (side_ == Side.back) {
            // up
            if (!childPP.hasBelow) {
                CalculateEdgeTriangles(childPP, side_, Dir.U, childPP, Dir.D, Side.bottom);
            }
            else {
                CalculateEdgeTriangles(childPP, side_, Dir.U, childPP.cubeBelow, Dir.D, Side.back);
            }
            // right
            if (!childPP.hasOnRight) {
                CalculateEdgeTriangles(childPP, side_, Dir.Rn, childPP, Dir.R, Side.right);
                if (!childPP.hasBelow) {
                    CalculateCornerTriangle(childPP, side_, Corner.Ur, PiecePartMeshGenerate.CornerPresance.CornerType.OneTriangle, 
                        childPP, Side.right, Corner.Dr, 
                        childPP, Side.bottom, Corner.Dr, true);
                } else {
                    CalculateCornerTriangle(childPP, side_, Corner.Ur, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP, Side.right, Corner.Dr, 
                        childPP.cubeBelow, Side.back, Corner.Dr, true);
                } if (!childPP.hasAbove) {
                    CalculateCornerTriangle(childPP, side_, Corner.Dr, PiecePartMeshGenerate.CornerPresance.CornerType.OneTriangle, 
                        childPP, Side.right, Corner.Ur, 
                        childPP, Side.top, Corner.Ur);
                } else {
                    CalculateCornerTriangle(childPP, side_, Corner.Dr, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP, Side.right, Corner.Ur, 
                        childPP.cubeAbove, Side.back, Corner.Ur);
                }
            }
            else {
                CalculateEdgeTriangles(childPP, side_, Dir.Rn, childPP.cubeOnRight, Dir.Ln, Side.back);
                if (!childPP.hasBelow) {
                    CalculateCornerTriangle(childPP, side_, Corner.Ur, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeOnRight, Side.back, Corner.Ul, 
                        childPP, Side.bottom, Corner.Dr, true);
                } else {
                    if (!childPP.cubeBelow.hasOnRight) {
                        CalculateCornerTriangle(childPP, side_, Corner.Ur, PiecePartMeshGenerate.CornerPresance.CornerType.ThreeTriangle, 
                            childPP.cubeOnRight, Side.bottom, Corner.Dl, 
                            childPP.cubeBelow, Side.right, Corner.Ur, true);
                    } else {
                        CalculateCornerTriangle(childPP, side_, Corner.Ur, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                            childPP.cubeOnRight, Side.back, Corner.Ul, 
                            childPP.cubeBelow, Side.back, Corner.Dr, true);
                    }
                } if (!childPP.hasAbove) {
                    CalculateCornerTriangle(childPP, side_, Corner.Dr, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeOnRight, Side.back, Corner.Dl, 
                        childPP, Side.top, Corner.Ur);
                } else {
                    if (!childPP.cubeAbove.hasOnRight) {
                        CalculateCornerTriangle(childPP, side_, Corner.Dr, PiecePartMeshGenerate.CornerPresance.CornerType.ThreeTriangle, 
                            childPP.cubeOnRight, Side.top, Corner.Ul, 
                            childPP.cubeAbove, Side.right, Corner.Dr);
                    } else {
                        CalculateCornerTriangle(childPP, side_, Corner.Dr, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                            childPP.cubeOnRight, Side.back, Corner.Dl, 
                            childPP.cubeAbove, Side.back, Corner.Ur);
                    }
                }
            }
            // down
            if (!childPP.hasAbove) {
                CalculateEdgeTriangles(childPP, side_, Dir.Dn, childPP, Dir.Un, Side.top);
            }
            else {
                CalculateEdgeTriangles(childPP, side_, Dir.Dn, childPP.cubeAbove, Dir.Un, Side.back);
            }
            // left
            if (!childPP.hasOnLeft) {
                CalculateEdgeTriangles(childPP, side_, Dir.L, childPP, Dir.Ln, Side.left);
                if (!childPP.hasBelow) {
                    CalculateCornerTriangle(childPP, side_, Corner.Ul, PiecePartMeshGenerate.CornerPresance.CornerType.OneTriangle, 
                        childPP, Side.left, Corner.Dl, 
                        childPP, Side.bottom, Corner.Dl);
                } else {
                    CalculateCornerTriangle(childPP, side_, Corner.Ul, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP, Side.left, Corner.Dl, 
                        childPP.cubeBelow, Side.back, Corner.Dl);
                } if (!childPP.hasAbove) {
                    CalculateCornerTriangle(childPP, side_, Corner.Dl, PiecePartMeshGenerate.CornerPresance.CornerType.OneTriangle, 
                        childPP, Side.left, Corner.Ul, 
                        childPP, Side.top, Corner.Ul, true);
                } else {
                    CalculateCornerTriangle(childPP, side_, Corner.Dl, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP, Side.left, Corner.Ul, 
                        childPP.cubeAbove, Side.back, Corner.Ul, true);
                }
            }
            else {
                CalculateEdgeTriangles(childPP, side_, Dir.L, childPP.cubeOnLeft, Dir.R, Side.back);
                if (!childPP.hasBelow) {
                    CalculateCornerTriangle(childPP, side_, Corner.Ul, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeOnLeft, Side.back, Corner.Ur, 
                        childPP, Side.bottom, Corner.Dl);
                } else {
                    if (!childPP.cubeBelow.hasOnLeft) {
                        CalculateCornerTriangle(childPP, side_, Corner.Ul, PiecePartMeshGenerate.CornerPresance.CornerType.ThreeTriangle, 
                            childPP.cubeOnLeft, Side.bottom, Corner.Dr, 
                            childPP.cubeBelow, Side.left, Corner.Ul);
                    } else {
                        CalculateCornerTriangle(childPP, side_, Corner.Ul, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                            childPP.cubeOnLeft, Side.back, Corner.Ur, 
                            childPP.cubeBelow, Side.back, Corner.Dl);
                    }
                } if (!childPP.hasAbove) {
                    CalculateCornerTriangle(childPP, side_, Corner.Dl, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeOnLeft, Side.back, Corner.Dr, 
                        childPP, Side.top, Corner.Ul, true);
                } else {
                    if (!childPP.cubeAbove.hasOnLeft) {
                        CalculateCornerTriangle(childPP, side_, Corner.Dl, PiecePartMeshGenerate.CornerPresance.CornerType.ThreeTriangle, 
                            childPP.cubeOnLeft, Side.top, Corner.Ur, 
                            childPP.cubeAbove, Side.left, Corner.Dl, true);
                    } else {
                        CalculateCornerTriangle(childPP, side_, Corner.Dl, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                            childPP.cubeOnLeft, Side.back, Corner.Dr, 
                            childPP.cubeAbove, Side.back, Corner.Ul, true);
                    }
                }
            }
        }

        // ------------------ LEFT
        if (side_ == Side.left) {
            // up
            if (!childPP.hasAbove) {
                CalculateEdgeTriangles(childPP, side_, Dir.U, childPP, Dir.Ln, Side.top);
            }
            else {
                if (!childPP.cubeAbove.hasOnLeft) {
                    CalculateEdgeTriangles(childPP, side_, Dir.U, childPP.cubeAbove, Dir.D, Side.left);
                    CalculateCornerTriangle(childPP, side_, Corner.Ur, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeAbove, Side.left, Corner.Dr, 
                        childPP, Side.front, Corner.Ul);
                    CalculateCornerTriangle(childPP, side_, Corner.Ul, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeAbove, Side.left, Corner.Dl, 
                        childPP, Side.back, Corner.Dl, true);
                }
                else {
                    CalculateEdgeTriangles(childPP, side_, Dir.U, childPP.cubeAbove.cubeOnLeft, Dir.R, Side.bottom);
                }
            }
            // right
            CalculateEdgeTriangles(childPP, side_, Dir.Rn, childPP, Dir.Ln, Side.front);
            // down
            if (!childPP.hasBelow) {
                CalculateEdgeTriangles(childPP, side_, Dir.Dn, childPP, Dir.L, Side.bottom);
            }
            else {
                if (!childPP.cubeBelow.hasOnLeft) {
                    CalculateEdgeTriangles(childPP, side_, Dir.Dn, childPP.cubeBelow, Dir.U, Side.left);
                    CalculateCornerTriangle(childPP, side_, Corner.Dr, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeBelow, Side.left, Corner.Ur, 
                        childPP, Side.front, Corner.Dl, true);
                    CalculateCornerTriangle(childPP, side_, Corner.Dl, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeBelow, Side.left, Corner.Ul, 
                        childPP, Side.back, Corner.Ul);
                }
                else {
                    CalculateEdgeTriangles(childPP, side_, Dir.D, childPP.cubeBelow.cubeOnLeft, Dir.R, Side.top);
                }
            }
            // left
            CalculateEdgeTriangles(childPP, side_, Dir.Ln, childPP, Dir.L, Side.back);
        }

        // ------------------ RIGHT
        if (side_ == Side.right) {
            // up
            if (!childPP.hasAbove) {
                CalculateEdgeTriangles(childPP, side_, Dir.U, childPP, Dir.R, Side.top);
            }
            else {
                if (!childPP.cubeAbove.hasOnRight) {
                    CalculateEdgeTriangles(childPP, side_, Dir.U, childPP.cubeAbove, Dir.D, Side.right);
                    CalculateCornerTriangle(childPP, side_, Corner.Ur, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeAbove, Side.right, Corner.Dr, 
                        childPP, Side.back, Corner.Dr);
                    CalculateCornerTriangle(childPP, side_, Corner.Ul, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeAbove, Side.right, Corner.Dl, 
                        childPP, Side.front, Corner.Ur, true);
                }
                else {
                    CalculateEdgeTriangles(childPP, side_, Dir.U, childPP.cubeAbove.cubeOnRight, Dir.Ln, Side.bottom);
                }
            }
            // right
            CalculateEdgeTriangles(childPP, side_, Dir.R, childPP, Dir.Rn, Side.back);
            // down
            if (!childPP.hasBelow) {
                CalculateEdgeTriangles(childPP, side_, Dir.Dn, childPP, Dir.R, Side.bottom);
            }
            else {
                if (!childPP.cubeBelow.hasOnRight) {
                    CalculateEdgeTriangles(childPP, side_, Dir.Dn, childPP.cubeBelow, Dir.U, Side.right);
                    CalculateCornerTriangle(childPP, side_, Corner.Dr, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeBelow, Side.right, Corner.Ur, 
                        childPP, Side.back, Corner.Ur, true);
                    CalculateCornerTriangle(childPP, side_, Corner.Dl, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeBelow, Side.right, Corner.Ul, 
                        childPP, Side.front, Corner.Dr);
                }
                else {
                    CalculateEdgeTriangles(childPP, side_, Dir.Dn, childPP.cubeBelow.cubeOnRight, Dir.L, Side.top);
                }
            }
            // left
            CalculateEdgeTriangles(childPP, side_, Dir.L, childPP, Dir.R, Side.front);
        }

        // ------------------ FRONT
        if (side_ == Side.front) {
            // up
            if (!childPP.hasAbove) {
                CalculateEdgeTriangles(childPP, side_, Dir.U, childPP, Dir.D, Side.top);
            }
            else {
                CalculateEdgeTriangles(childPP, side_, Dir.U, childPP.cubeAbove, Dir.D, Side.front);
            }
            // right
            if (!childPP.hasOnRight) {
                CalculateEdgeTriangles(childPP, side_, Dir.Rn, childPP, Dir.L, Side.right);
                if (!childPP.hasBelow) {
                    CalculateCornerTriangle(childPP, side_, Corner.Dr, PiecePartMeshGenerate.CornerPresance.CornerType.OneTriangle, 
                        childPP, Side.right, Corner.Dl, 
                        childPP, Side.bottom, Corner.Ur);
                } else {
                    CalculateCornerTriangle(childPP, side_, Corner.Dr, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP, Side.right, Corner.Dl, 
                        childPP.cubeBelow, Side.front, Corner.Ur);
                } if (!childPP.hasAbove) {
                    CalculateCornerTriangle(childPP, side_, Corner.Ur, PiecePartMeshGenerate.CornerPresance.CornerType.OneTriangle, 
                        childPP, Side.right, Corner.Ul, 
                        childPP, Side.top, Corner.Dr, true);
                } else {
                    CalculateCornerTriangle(childPP, side_, Corner.Ur, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP, Side.right, Corner.Ul, 
                        childPP.cubeAbove, Side.front, Corner.Dr, true);
                }
            }
            else {
                CalculateEdgeTriangles(childPP, side_, Dir.Rn, childPP.cubeOnRight, Dir.Ln, Side.front);
                if (!childPP.hasBelow) {
                    CalculateCornerTriangle(childPP, side_, Corner.Dr, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeOnRight, Side.front, Corner.Dl, 
                        childPP, Side.bottom, Corner.Ur);
                } else {
                    if (!childPP.cubeBelow.hasOnRight) {
                        CalculateCornerTriangle(childPP, side_, Corner.Dr, PiecePartMeshGenerate.CornerPresance.CornerType.ThreeTriangle, 
                            childPP.cubeOnRight, Side.bottom, Corner.Ul, 
                            childPP.cubeBelow, Side.right, Corner.Ul);
                    } else {
                        CalculateCornerTriangle(childPP, side_, Corner.Dr, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                            childPP.cubeOnRight, Side.front, Corner.Dl, 
                            childPP.cubeBelow, Side.front, Corner.Ur);
                    }
                } if (!childPP.hasAbove) {
                    CalculateCornerTriangle(childPP, side_, Corner.Ur, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeOnRight, Side.front, Corner.Ul, 
                        childPP, Side.top, Corner.Dr, true);
                } else {
                    if (!childPP.cubeAbove.hasOnRight) {
                        CalculateCornerTriangle(childPP, side_, Corner.Ur, PiecePartMeshGenerate.CornerPresance.CornerType.ThreeTriangle, 
                            childPP.cubeOnRight, Side.top, Corner.Dl, 
                            childPP.cubeAbove, Side.right, Corner.Dl, true);
                    } else {
                        CalculateCornerTriangle(childPP, side_, Corner.Ur, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                            childPP.cubeOnRight, Side.front, Corner.Ul, 
                            childPP.cubeAbove, Side.front, Corner.Dr, true);
                    }
                }
            }
            // down
            if (!childPP.hasBelow) {
                CalculateEdgeTriangles(childPP, side_, Dir.Dn, childPP, Dir.U, Side.bottom);
            }
            else {
                CalculateEdgeTriangles(childPP, side_, Dir.Dn, childPP.cubeBelow, Dir.U, Side.front);
            }
            // left
            if (!childPP.hasOnLeft) {
                CalculateEdgeTriangles(childPP, side_, Dir.L, childPP, Dir.R, Side.left);
                if (!childPP.hasBelow) {
                    CalculateCornerTriangle(childPP, side_, Corner.Dl, PiecePartMeshGenerate.CornerPresance.CornerType.OneTriangle, 
                        childPP, Side.left, Corner.Dr, 
                        childPP, Side.bottom, Corner.Ul, true);
                } else {
                    CalculateCornerTriangle(childPP, side_, Corner.Dl, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP, Side.left, Corner.Dr, 
                        childPP.cubeBelow, Side.front, Corner.Ul, true);
                } if (!childPP.hasAbove) {
                    CalculateCornerTriangle(childPP, side_, Corner.Ul, PiecePartMeshGenerate.CornerPresance.CornerType.OneTriangle, 
                        childPP, Side.left, Corner.Ur, 
                        childPP, Side.top, Corner.Dl);
                } else {
                    CalculateCornerTriangle(childPP, side_, Corner.Ul, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP, Side.left, Corner.Ur, 
                        childPP.cubeAbove, Side.front, Corner.Dl);
                }
            }
            else {
                CalculateEdgeTriangles(childPP, side_, Dir.L, childPP.cubeOnLeft, Dir.R, Side.front);
                if (!childPP.hasBelow) {
                    CalculateCornerTriangle(childPP, side_, Corner.Dl, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeOnLeft, Side.front, Corner.Dr, 
                        childPP, Side.bottom, Corner.Ul, true);
                } else {
                    if (!childPP.cubeBelow.hasOnLeft) {
                        CalculateCornerTriangle(childPP, side_, Corner.Dl, PiecePartMeshGenerate.CornerPresance.CornerType.ThreeTriangle, 
                            childPP.cubeOnLeft, Side.bottom, Corner.Ur, 
                            childPP.cubeBelow, Side.left, Corner.Ur, true);
                    } else {
                        CalculateCornerTriangle(childPP, side_, Corner.Dl, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                            childPP.cubeOnLeft, Side.front, Corner.Dr, 
                            childPP.cubeBelow, Side.front, Corner.Ul, true);
                    }
                } if (!childPP.hasAbove) {
                    CalculateCornerTriangle(childPP, side_, Corner.Ul, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeOnLeft, Side.front, Corner.Ur, 
                        childPP, Side.top, Corner.Dl);
                } else {
                    if (!childPP.cubeAbove.hasOnLeft) {
                        CalculateCornerTriangle(childPP, side_, Corner.Ul, PiecePartMeshGenerate.CornerPresance.CornerType.ThreeTriangle, 
                            childPP.cubeOnLeft, Side.top, Corner.Dr, 
                            childPP.cubeAbove, Side.left, Corner.Dr);
                    } else {
                        CalculateCornerTriangle(childPP, side_, Corner.Ul, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                            childPP.cubeOnLeft, Side.front, Corner.Ur, 
                            childPP.cubeAbove, Side.front, Corner.Dl);
                    }
                }
            }
        }

        // ------------------ TOP
        if (side_ == Side.top) {
            // up
            CalculateEdgeTriangles(childPP, side_, Dir.U, childPP, Dir.D, Side.back);
            //right
            if (!childPP.hasOnRight) {
                CalculateEdgeTriangles(childPP, side_, Dir.Rn, childPP, Dir.U, Side.right);
            }
            else {
                if (!childPP.cubeOnRight.hasAbove) {
                    CalculateEdgeTriangles(childPP, side_, Dir.Rn, childPP.cubeOnRight, Dir.Ln, Side.top);
                    CalculateCornerTriangle(childPP, side_, Corner.Ur, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP, Side.back, Corner.Dr, 
                        childPP.cubeOnRight, Side.top, Corner.Ul);
                    CalculateCornerTriangle(childPP, side_, Corner.Dr, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP.cubeOnRight, Side.top, Corner.Dl, 
                        childPP, Side.front, Corner.Ur);
                }
                else {
                    CalculateEdgeTriangles(childPP, side_, Dir.Rn, childPP.cubeOnRight.cubeAbove, Dir.D, Side.left);
                }
            }
            // down
            CalculateEdgeTriangles(childPP, side_, Dir.Dn, childPP, Dir.U, Side.front);
            // left
            if (!childPP.hasOnLeft) {
                CalculateEdgeTriangles(childPP, side_, Dir.Ln, childPP, Dir.U, Side.left);
            }
            else {
                if (!childPP.cubeOnLeft.hasAbove) {
                    CalculateEdgeTriangles(childPP, side_, Dir.L, childPP.cubeOnLeft, Dir.R, Side.top);
                    CalculateCornerTriangle(childPP, side_, Corner.Dl, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP, Side.front, Corner.Ul, 
                        childPP.cubeOnLeft, Side.top, Corner.Dr);
                    CalculateCornerTriangle(childPP, side_, Corner.Ul, PiecePartMeshGenerate.CornerPresance.CornerType.TwoTriangle, 
                        childPP, Side.back, Corner.Dl, 
                        childPP.cubeOnLeft, Side.top, Corner.Ur, true);
                }
                else {
                    CalculateEdgeTriangles(childPP, side_, Dir.L, childPP.cubeOnLeft.cubeAbove, Dir.D, Side.right);
                }
            }
        }
    }

    private void CalculateEdgeTriangles(PiecePartMeshGenerate mainPP, Side mainSide, Dir mainDirection, PiecePartMeshGenerate desPP, Dir desDirection, Side desSide) {
        PiecePartMeshGenerate.Connections connectionsMain = mainPP.GetComponent<PiecePartMeshGenerate>().GetConnectionsBySide(mainSide);
        PiecePartMeshGenerate.Connections connectionsDes = desPP.GetComponent<PiecePartMeshGenerate>().GetConnectionsBySide(desSide);

        // edge center
        if (!connectionsMain.ReturnByDir(mainDirection)) {
            connectionsMain.SetByDir(mainDirection, true);
            connectionsDes.SetByDir(desDirection);

            for (int i = 0; i < meshPointsInRow - 1; i++) {

                // To separate method
                int mainVerticesI;
                int mainVerticesINext;
                switch (mainDirection) {
                    default:
                    case Dir.D:
                        mainVerticesI = i;
                        mainVerticesINext = i + 1;
                        break;
                    case Dir.Dn:
                        mainVerticesI = meshPointsInRow - 1 - i;
                        mainVerticesINext = meshPointsInRow - 2 - i;
                        break;
                    case Dir.R:
                        mainVerticesI = meshPointsInRow * (i + 1) - 1;
                        mainVerticesINext = meshPointsInRow * (i + 2) - 1;
                        break;
                    case Dir.Rn:
                        mainVerticesI = meshPointsInRow * (meshPointsInRow - i) - 1;
                        mainVerticesINext = meshPointsInRow * (meshPointsInRow - i - 1) - 1;
                        break;
                    case Dir.U:
                        mainVerticesI = meshPointsInRow * meshPointsInRow - meshPointsInRow + i;
                        mainVerticesINext = meshPointsInRow * meshPointsInRow - meshPointsInRow + i + 1;
                        break;
                    case Dir.Un:
                        mainVerticesI = meshPointsInRow * meshPointsInRow - 1 - i;
                        mainVerticesINext = meshPointsInRow * meshPointsInRow - 2 - i;
                        break;
                    case Dir.L:
                        mainVerticesI = meshPointsInRow * i;
                        mainVerticesINext = meshPointsInRow * (i + 1);
                        break;
                    case Dir.Ln:
                        mainVerticesI = meshPointsInRow * meshPointsInRow - (meshPointsInRow * (i + 1));
                        mainVerticesINext = meshPointsInRow * meshPointsInRow - (meshPointsInRow * (i + 2));
                        break;
                }
                int desVerticesI;
                int desVerticesINext;
                switch (desDirection) {
                    default:
                    case Dir.D:
                        desVerticesI = i;
                        desVerticesINext = i + 1;
                        break;
                    case Dir.R:
                        desVerticesI = meshPointsInRow * (i + 1) - 1;
                        desVerticesINext = meshPointsInRow * (i + 2) - 1;
                        break;
                    case Dir.U:
                        desVerticesI = meshPointsInRow * meshPointsInRow - meshPointsInRow + i;
                        desVerticesINext = meshPointsInRow * meshPointsInRow - meshPointsInRow + i + 1;
                        break;
                    case Dir.L:
                        desVerticesI = meshPointsInRow * i;
                        desVerticesINext = meshPointsInRow * (i + 1);
                        break;
                    case Dir.Dn:
                        desVerticesI = meshPointsInRow - 1 - i;
                        desVerticesINext = meshPointsInRow - 2 - i;
                        break;
                    case Dir.Rn:
                        desVerticesI = meshPointsInRow * (meshPointsInRow - i) - 1;
                        desVerticesINext = meshPointsInRow * (meshPointsInRow - i - 1) - 1;
                        break;
                    case Dir.Un:
                        desVerticesI = meshPointsInRow * meshPointsInRow - 1 - i;
                        desVerticesINext = meshPointsInRow * meshPointsInRow - 2 - i;
                        break;
                    case Dir.Ln:
                        desVerticesI = meshPointsInRow * meshPointsInRow - (meshPointsInRow * (i + 1));
                        desVerticesINext = meshPointsInRow * meshPointsInRow - (meshPointsInRow * (i + 2));
                        break;
                }

                allTriangles.Add(desPP.piecePartMeshData.StartForSide(desSide) + desVerticesI);
                allTriangles.Add(mainPP.piecePartMeshData.StartForSide(mainSide) + mainVerticesINext);
                allTriangles.Add(mainPP.piecePartMeshData.StartForSide(mainSide) + mainVerticesI);

                allTriangles.Add(desPP.piecePartMeshData.StartForSide(desSide) + desVerticesI);
                allTriangles.Add(desPP.piecePartMeshData.StartForSide(desSide) + desVerticesINext);
                allTriangles.Add(mainPP.piecePartMeshData.StartForSide(mainSide) + mainVerticesINext);
            }
        }
    }

    private int CornerToIndex(Corner corner) {
        switch (corner) {
            default:
            case Corner.Ul:
                return meshPointsInRow * meshPointsInRow - meshPointsInRow;
            case Corner.Ur:
                return meshPointsInRow * meshPointsInRow - 1;
            case Corner.Dl:
                return 0;
            case Corner.Dr:
                return meshPointsInRow - 1;
        }
    }

    private void CalculateCornerTriangle(
        PiecePartMeshGenerate mainPP, Side mainSide, Corner mainCorner, PiecePartMeshGenerate.CornerPresance.CornerType cornerType,
        PiecePartMeshGenerate desPP1, Side desSide1, Corner desCorner1,
        PiecePartMeshGenerate desPP2, Side desSide2, Corner desCorner2, bool swap = false) 
    {
        if (!mainPP.GetCornerPresanceBySide(mainSide).ReturnPresanceByCorner(mainCorner) || cornerType == PiecePartMeshGenerate.CornerPresance.CornerType.ThreeTriangle) {
            mainPP.GetCornerPresanceBySide(mainSide).SetCornerTypeByCorner(mainCorner, cornerType);

            // if corner isn't made of three triangles or corner connects to different sides on each piece part (desPP1, desPP2) than mainSide
            // only situation when this if expression isn't executed is when corner has 2 other triangles and this is middle one
            if (cornerType != PiecePartMeshGenerate.CornerPresance.CornerType.ThreeTriangle || (desSide1 != mainSide && desSide2 != mainSide)) {
                desPP1.GetCornerPresanceBySide(desSide1).SetCornerTypeByCorner(desCorner1, cornerType);
                desPP2.GetCornerPresanceBySide(desSide2).SetCornerTypeByCorner(desCorner2, cornerType);
            }
            PiecePartMeshGenerate first, second;
            if (swap) {
                second = desPP1;
                first = desPP2;
                Side tempSide = desSide1;
                Corner tempCorner = desCorner1;
                desSide1 = desSide2; desCorner1 = desCorner2;
                desSide2 = tempSide; desCorner2 = tempCorner;
            } else {
                first = desPP1;
                second = desPP2;
            }

            allTriangles.Add(mainPP.piecePartMeshData.StartForSide(mainSide) + CornerToIndex(mainCorner));
            allTriangles.Add(first.piecePartMeshData.StartForSide(desSide1) + CornerToIndex(desCorner1));
            allTriangles.Add(second.piecePartMeshData.StartForSide(desSide2) + CornerToIndex(desCorner2));
        }
    }

    private void CalculateColliderMeshTriangles() {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                if (children[i, j].activeSelf) {
                    PiecePartMeshGenerate childPP = children[i, j].GetComponent<PiecePartMeshGenerate>();
                    // bottom side
                    if (!childPP.hasBelow) {
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftBottomBack, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftBottomFront, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightBottomBack, childPP.transform));

                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftBottomFront, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightBottomFront, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightBottomBack, childPP.transform));
                    }
                    // back side
                    triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftTopBack, childPP.transform));
                    triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftBottomBack, childPP.transform));
                    triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightTopBack, childPP.transform));

                    triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftBottomBack, childPP.transform));
                    triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightBottomBack, childPP.transform));
                    triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightTopBack, childPP.transform));
                    // left side
                    if (!childPP.hasOnLeft) {
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftTopBack, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftBottomFront, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftBottomBack, childPP.transform));

                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftTopBack, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftTopFront, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftBottomFront, childPP.transform));
                    }
                    // right side
                    if (!childPP.hasOnRight) {
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightBottomFront, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightTopFront, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightTopBack, childPP.transform));

                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightBottomFront, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightTopBack, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightBottomBack, childPP.transform));
                    }
                    // front side
                    triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftTopFront, childPP.transform));
                    triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightBottomFront, childPP.transform));
                    triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftBottomFront, childPP.transform));

                    triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftTopFront, childPP.transform));
                    triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightTopFront, childPP.transform));
                    triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightBottomFront, childPP.transform));
                    // top side
                    if (!childPP.hasAbove) {
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftTopFront, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftTopBack, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightTopBack, childPP.transform));

                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfLeftTopFront, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightTopBack, childPP.transform));
                        triangles.Add(CalcColliderMeshHelper(vertices, childPP.boundOfRightTopFront, childPP.transform));
                    }
                }
            }
        }

        colliderMesh.vertices = vertices.ToArray();
        colliderMesh.triangles = triangles.ToArray();
    }

    private int CalcColliderMeshHelper(List<Vector3> vert, Vector3 vec, Transform rel) {
        vec = transform.InverseTransformPoint(rel.TransformPoint(vec));
        int index = vert.IndexOf(vec);
        if (index == -1) {
            index = vert.Count;
            vert.Add(vec);
        }
        return index;
    }

    /**
    <summary>Set color certain vertices</summary>
    **/
    private void SetColorsOnMesh(int startIndex, int len, Color32 color) {
        Color32[] colors = mesh.colors32;
        if (colors.Length != mesh.vertices.Length)
            return;
        for (int a = startIndex; a < startIndex + len; a++) {
            colors[a] = color;
        }
        mesh.colors32 = colors;
    }
    /**
    <summary>Set color on whole mesh</summary>
    **/
    private void SetColorsOnMesh(Color32 color) {
        Mesh mesh = this.mesh;
        Color32[] colors32 = mesh.colors32;
        SetColorsOnMesh(0, mesh.colors32.Length, color);
    }
    /**
    <summary>Set color on one cube</summary>
    **/
    private void SetColorsOnMesh(PiecePartMeshGenerate childPP, Color32 color) {
        for (int s = 0; s < 6; s++) {
            SetColorsOnMesh(childPP, (Side)s, color);
        }
    }
    /**
    <summary>Set color on one side of cube</summary>
    **/
    private void SetColorsOnMesh(PiecePartMeshGenerate childPP, Side side, Color32 color) {
        SetColorsOnMesh(childPP.piecePartMeshData.StartForSide((Side)side), childPP.piecePartMeshData.Amount((Side)side), color);
    }

    private Color32 Color32ByIntensityBlack(Color32 color32_, float intensity) {
        return Color32ByIntensityColor32(color32_, intensity, new Color32(0, 0, 0, 255));
    }
    private Color32 Color32ByIntensityWhite(Color32 color32_, float intensity) {
        return Color32ByIntensityColor32(color32_, intensity, new Color32(255, 255, 255, 255));
    }
    private Color32 Color32ByIntensityColor32(Color32 color32_, float intensity, Color32 refColor) {
        byte r, g, b;
        r = (byte)((refColor.r - color32_.r) * intensity + color32_.r);
        g = (byte)((refColor.g - color32_.g) * intensity + color32_.g);
        b = (byte)((refColor.b - color32_.b) * intensity + color32_.b);
        return new Color32(r, g, b, color32_.a);
    }


    // Cube color animation
    protected class CubeAnimationData {
        public PiecePartMeshGenerate childPP;
        public Color32 color32;
        public float speed;
        public float intensity, intenMax, intenMin;
        // public bool toWhite;
        public bool increaseInten = true;
    }
    protected List<CubeAnimationData> cubeAnimationData = new List<CubeAnimationData>();
    public void AddCubeAnimation(PiecePartMeshGenerate childPP, Color32 color32, float speed, float intensityMax, float intensityMin/* , bool toWhite */) {
        CubeAnimationData anim = new CubeAnimationData();
        anim.childPP = childPP;
        anim.color32 = color32;
        anim.speed = speed;
        anim.intensity = anim.intenMin = intensityMin;
        anim.intenMax = intensityMax;
        // anim.toWhite = toWhite;
        anim.increaseInten = true;
        cubeAnimationData.Add(anim);
    }
    public void RemoveCubeAnimation(int index) {
        if(index < cubeAnimationData.Count) {
            SetColorsOnMesh(cubeAnimationData[index].childPP, color32);
            cubeAnimationData.RemoveAt(index);
        }
    }
    private void AnimateColorFrame() {
        foreach (var anim in cubeAnimationData) {
            SetColorsOnMesh(anim.childPP, Color32ByIntensityColor32(anim.color32, anim.intensity, color32));
            if (anim.increaseInten) {
                anim.intensity += (anim.intenMax - anim.intenMin) * anim.speed * Time.deltaTime;
                if (anim.intensity >= anim.intenMax) {
                    anim.intensity = anim.intenMax;
                    anim.increaseInten = false;
                }
            }
            if (!anim.increaseInten) {
                anim.intensity -= (anim.intenMax - anim.intenMin) * anim.speed * Time.deltaTime;
                if (anim.intensity <= anim.intenMin) {
                    anim.intensity = anim.intenMin;
                    anim.increaseInten = true;
                }
            }

        }

    }

    // Shifting cubes
    /*
    <summary>Shifting cubes away from center of workspace</summary>
    <param name="arrang">Arrangement of cubes which are shifted, null to shift none</param>
    */
    protected void ShiftCubes(bool [,] arrang) {
        shiftedCubes = new byte[variantInt, variantInt];
        if (arrang.GetLength(0) != variantInt) {
            Debug.Log("Different variant from arrang");
            return;
        }
        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                if (setting[i, j] == I && arrang[i, j] == I) {
                    if (shiftedCubes[i, j] == 1) {
                        // children[i, j].GetComponent<PiecePartMeshGenerate>().duringShift = false;
                    } else {
                        children[i, j].GetComponent<PiecePartMeshGenerate>().duringShift = true;
                        shiftedCubes[i, j] = 1;
                    }
                } else if (setting[i, j] == I && arrang[i, j] == O) {
                    if (shiftedCubes[i, j] == 2) {
                        // children[i, j].GetComponent<PiecePartMeshGenerate>().duringShift = false;
                    } else {
                        children[i, j].GetComponent<PiecePartMeshGenerate>().duringShift = true;
                        shiftedCubes[i, j] = 2;
                    }
                } else {
                    shiftedCubes[i, j] = 0;
                }
            }
        }
        shiftRenewTriangles = true;
        return;
    }

    private void ShiftCubesMovement() {
        if (shiftedCubes == null) return;
        bool anyChanges = false;
        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                if (children[i, j].activeSelf && children[i, j].GetComponent<PiecePartMeshGenerate>().duringShift) {
                    if (shiftedCubes[i, j] == 2) {
                        Vector3 pos = children[i, j].transform.localPosition;
                        pos.y += shiftDistance * (Time.deltaTime / shiftTime);
                        if (pos.y >= shiftDistance) {
                            pos.y = shiftDistance;
                            children[i, j].GetComponent<PiecePartMeshGenerate>().duringShift = false;
                            shiftRenewTriangles = true;
                        }
                        children[i, j].transform.localPosition = pos;
                    }
                    else {
                        Vector3 pos = children[i, j].transform.localPosition;
                        pos.y -= shiftDistance * (Time.deltaTime / shiftTime);
                        if (pos.y <= 0) {
                            pos.y = 0;
                            children[i, j].GetComponent<PiecePartMeshGenerate>().duringShift = false;
                            shiftRenewTriangles = true;
                        }
                        children[i, j].transform.localPosition = pos;
                    }
                    anyChanges = true;
                }
            }
        }
        if (anyChanges) {
            // shiftRenewTriangles = true;
            PrepareSidesVisibilityShifted();
        }
    }


    protected override void Update() {
        base.Update();
        AnimateColorFrame();
        ShiftCubesMovement();

        // if (Input.GetKeyDown(KeyCode.S)) {
        //     FindChildren();
        //     ChangeSetting();
        // }
        // if (Input.GetKeyDown(KeyCode.C)) {
        //     RotateClockwise(1);
        // }
    }





    /* private void dispNames() {
        string log = "";
        for (int i = 0; i < variantInt; i++) {
            for (int j = 0; j < variantInt; j++) {
                log += children[i, j].GetComponent<PiecePartMeshGenerate>().vertices.name + " ";
            }
            log += '\n';
        }
        Debug.Log(log);
    } */
}
