using UnityEngine;
using System.Collections.Generic;
using static Enums;

public class PiecePartMeshGenerate : MonoBehaviour
{

    // Important about vertices: higher z value is closer to the center vertex will be

    public class Connections {
        public bool withUpper, upperOnThis;
        public bool withRight, rightOnThis;
        public bool withLower, lowerOnThis;
        public bool withLeft, leftOnThis;

        public void SetByDir(Dir direction, bool onThis = false) {
            switch (direction) {
                case Dir.U:
                case Dir.Un:
                    withUpper = true;
                    if (onThis) upperOnThis = true;
                    break;
                case Dir.R:
                case Dir.Rn:
                    withRight = true;
                    if (onThis) rightOnThis = true;
                    break;
                case Dir.D:
                case Dir.Dn:
                    withLower = true;
                    if (onThis) lowerOnThis = true;
                    break;
                case Dir.L:
                case Dir.Ln:
                    withLeft = true;
                    if (onThis) leftOnThis = true;
                    break;
                default:
                    break;
            }
        }

        public bool ReturnByDir(Dir direction, bool onThis = false) {
            switch (direction) {
                case Dir.U:
                case Dir.Un:
                    if (onThis) return upperOnThis;
                    return withUpper;
                case Dir.R:
                case Dir.Rn:
                    if (onThis) return rightOnThis;
                    return withRight;
                case Dir.D:
                case Dir.Dn:
                    if (onThis) return lowerOnThis;
                    return withLower;
                case Dir.L:
                case Dir.Ln:
                    if (onThis) return leftOnThis;
                    return withLeft;
                default:
                    return false;
            }
        }
        public bool[] ReturnInArray(bool onThis = false) {
            return onThis ? new bool[] {upperOnThis, rightOnThis, lowerOnThis, leftOnThis} : new bool[] {withUpper, withRight, withLower, withLeft};
        }
    }
    public class PiecePartMeshData {
        public int startingIndex;
        public int amountForBottom;
        public int amountForBack;
        public int amountForLeft;
        public int amountForRight;
        public int amountForFront;
        public int amountForTop;

        public int Amount(Side side_) {
            switch (side_) {
                default:
                case Side.bottom:
                    return amountForBottom;
                case Side.back:
                    return amountForBack;
                case Side.left:
                    return amountForLeft;
                case Side.right:
                    return amountForRight;
                case Side.front:
                    return amountForFront;
                case Side.top:
                    return amountForTop;
            }
        }
        public void Amount(Side side_, int value) {
            switch (side_) {
                default:
                case Side.bottom:
                    amountForBottom = value;
                    break;
                case Side.back:
                    amountForBack = value;
                    break;
                case Side.left:
                    amountForLeft = value;
                    break;
                case Side.right:
                    amountForRight = value;
                    break;
                case Side.front:
                    amountForFront = value;
                    break;
                case Side.top:
                    amountForTop = value;
                    break;
            }
        }

        public int StartForSide(Side side_) {
            int start = startingIndex;
            switch (side_) {
                case Side.top:
                    start += amountForFront;
                    goto case Side.front;
                case Side.front:
                    start += amountForRight;
                    goto case Side.right;
                case Side.right:
                    start += amountForLeft;
                    goto case Side.left;
                case Side.left:
                    start += amountForBack;
                    goto case Side.back;
                case Side.back:
                    start += amountForBottom;
                    goto case Side.bottom;
                case Side.bottom:
                default:
                    break;
            }
            return start;
        }
    }
    public class CornerPresance {
        // CornerType
        public enum CornerType {ThreeTriangle, TwoTriangle, OneTriangle};

        public CornerType upperLeft = CornerType.OneTriangle;
        public bool upperLeftCalc = false;
        public CornerType upperRight = CornerType.OneTriangle;
        public bool upperRightCalc = false;
        public CornerType lowerRight = CornerType.OneTriangle;
        public bool lowerRightCalc = false;
        public CornerType lowerLeft = CornerType.OneTriangle;
        public bool lowerLeftCalc = false;

        public bool ReturnPresanceByCorner(Corner corner) {
            switch (corner) {
                default:
                case Corner.Ul:
                    return upperLeftCalc;
                case Corner.Ur:
                    return upperRightCalc;
                case Corner.Dr:
                    return lowerRightCalc;
                case Corner.Dl:
                    return lowerLeftCalc;
            }
        }
        public CornerType ReturnCornerTypeByCorner(Corner corner) {
            switch (corner) {
                default:
                case Corner.Ul:
                    return upperLeft;
                case Corner.Ur:
                    return upperRight;
                case Corner.Dr:
                    return lowerRight;
                case Corner.Dl:
                    return lowerLeft;
            }
        }
        public bool[] ReturnPresancesByDir(Dir dir) {
            switch (dir) {
                default:
                case Dir.U:
                    return new bool[] {upperLeftCalc, upperRightCalc};
                case Dir.R:
                    return new bool[] {upperRightCalc, lowerRightCalc};
                case Dir.D:
                    return new bool[] {lowerRightCalc, lowerLeftCalc};
                case Dir.L:
                    return new bool[] {lowerLeftCalc,  upperLeftCalc};
            }
        }
        public CornerType[] ReturnCornerTypesByDir(Dir dir) {
            switch (dir) {
                default:
                case Dir.U:
                    return new CornerType[] {upperLeft, upperRight};
                case Dir.R:
                    return new CornerType[] {upperRight, lowerRight};
                case Dir.D:
                    return new CornerType[] {lowerRight, lowerLeft};
                case Dir.L:
                    return new CornerType[] {lowerLeft, lowerRight};
            }
        }
        public void SetCornerTypeByCorner(Corner corner, CornerType cornerType) {
            switch (corner) {
                default:
                case Corner.Ul:
                    upperLeft = cornerType;
                    upperLeftCalc = true;
                    return;
                case Corner.Ur:
                    upperRight = cornerType;
                    upperRightCalc = true;
                    return;
                case Corner.Dr:
                    lowerRight = cornerType;
                    lowerRightCalc = true;
                    return;
                case Corner.Dl:
                    lowerLeft = cornerType;
                    lowerLeftCalc = true;
                    return;
            }
        }
        public void SetCornerTypesByDir(Dir dir, CornerType cornerType) {
            switch (dir) {
                default:
                case Dir.U:
                    SetCornerTypeByCorner(Corner.Ul, cornerType);
                    SetCornerTypeByCorner(Corner.Ur, cornerType);
                    break;
                case Dir.R:
                    SetCornerTypeByCorner(Corner.Ur, cornerType);
                    SetCornerTypeByCorner(Corner.Dr, cornerType);
                    break;
                case Dir.D:
                    SetCornerTypeByCorner(Corner.Dr, cornerType);
                    SetCornerTypeByCorner(Corner.Dl, cornerType);
                    break;
                case Dir.L:
                    SetCornerTypeByCorner(Corner.Dl, cornerType);
                    SetCornerTypeByCorner(Corner.Ul, cornerType);
                    break;
            }
        }
    }
    public class Vertices {
        public string name;
        public short meshPointsInRow;
        public Vector3[] bottom;
        public Vector3[] back;
        public Vector3[] left;
        public Vector3[] right;
        public Vector3[] front;
        public Vector3[] top;
        public Vector3[] GetBySide(Side side_) {
            switch (side_) {
                case Side.bottom:
                    return bottom;
                case Side.back:
                    return back;
                case Side.left:
                    return left;
                case Side.right:
                    return right;
                case Side.front:
                    return front;
                case Side.top:
                    return top;
                default:
                    return null;
            }
        }
        public void SetBySide(Side side_, Vector3[] verticesPerSide) {
            switch (side_) {
                case Side.bottom:
                    bottom = (Vector3[])verticesPerSide.Clone();
                    break;
                case Side.back:
                    back = (Vector3[])verticesPerSide.Clone();
                    break;
                case Side.left:
                    left = (Vector3[])verticesPerSide.Clone();
                    break;
                case Side.right:
                    right = (Vector3[])verticesPerSide.Clone();
                    break;
                case Side.front:
                    front = (Vector3[])verticesPerSide.Clone();
                    break;
                case Side.top:
                    top = (Vector3[])verticesPerSide.Clone();
                    break;
                default:
                    break;
            }
        }
        public void Set(Vertices vertices) {
            name = vertices.name;
            meshPointsInRow = vertices.meshPointsInRow;
            SetBySide(Side.bottom, vertices.bottom);
            SetBySide(Side.back, vertices.back);
            SetBySide(Side.left, vertices.left);
            SetBySide(Side.right, vertices.right);
            SetBySide(Side.front, vertices.front);
            SetBySide(Side.top, vertices.top);
        }
        private void RotateClockwiseBySide(Side side_, int times) {
            Vector3[] vertices = GetBySide(side_);
            times %= 4;
            // Debug.Log(side_);
            
            // string debLog = "";
            // for (int i = 0; i < meshPointsInRow; i ++) {
            //     for (int j = 0; j < meshPointsInRow; j++) {
            //         debLog += vertices[i2dto1d(i, j)].ToString();
            //         debLog += " ";
            //     }
            //     debLog += '\n';
            // }
            // Debug.Log(debLog);
            
            if (times % 2 == 1) {
                for (int a = 0; a < meshPointsInRow / 2; a++) { // loop for each perimeter (square of vertices discarding the fill)
                    for (int b = 0; b < meshPointsInRow - 1 - a; b++) { // loop for each element on side of perimeter except of last one
                        float temp = vertices[i2dto1d(a + b, a)].z;
                        // x increasing with a and b, y increasing with a
                        vertices[i2dto1d(a + b, a)].z = vertices[i2dto1d(meshPointsInRow - 1 - a, a + b)].z;
                        // x decreasing with a, y increasing with a and b
                        vertices[i2dto1d(meshPointsInRow - 1 - a, a + b)].z = vertices[i2dto1d(meshPointsInRow - 1 - a - b, meshPointsInRow - 1 - a)].z;
                        // x decreasing with a and b, y decreasing with a
                        vertices[i2dto1d(meshPointsInRow - 1 - a - b, meshPointsInRow - 1 - a)].z = vertices[i2dto1d(a, meshPointsInRow - 1 - a - b)].z;
                        // x increasing with a, y decreasing with a and b
                        vertices[i2dto1d(a, meshPointsInRow - 1 - a - b)].z = temp;
                    }
                }
                times--;
            }
            if (times == 2) {
                for (int a = 0; a < meshPointsInRow / 2; a++) { // loop for each perimeter
                    for (int b = 0; b < meshPointsInRow - 1 - a; b++) { // loop for each element on side of perimeter except of last one
                        float temp = vertices[i2dto1d(a + b, a)].z;
                        // x increasing with a and b, y increasing with a
                        vertices[i2dto1d(a + b, a)].z = vertices[i2dto1d(meshPointsInRow - 1 - a - b, meshPointsInRow - 1 - a)].z;
                        // x decreasing with a and b, y decreasing with a
                        vertices[i2dto1d(meshPointsInRow - 1 - a - b, meshPointsInRow - 1 - a)].z = temp;
                        temp = vertices[i2dto1d(meshPointsInRow - 1 - a, a + b)].z;
                        // x decreasing with a, y increasing with a and b
                        vertices[i2dto1d(meshPointsInRow - 1 - a, a + b)].z = vertices[i2dto1d(a, meshPointsInRow - 1 - a - b)].z;
                        // x increasing with a, y decreasing with a and b
                        vertices[i2dto1d(a, meshPointsInRow - 1 - a - b)].z = temp;
                    }
                }
            }
            // debLog = "";
            // for (int i = 0; i < meshPointsInRow; i ++) {
            //     for (int j = 0; j < meshPointsInRow; j++) {
            //         debLog += vertices[i2dto1d(i, j)].ToString();
            //         debLog += " ";
            //     }
            //     debLog += '\n';
            // }
            // Debug.Log(debLog);
        }
        private int i2dto1d(int i, int j) {
            return i * meshPointsInRow + j;
        }
        public Vertices GetCopy() {
            Vertices rtn = new Vertices();
            rtn.Set(this);
            return rtn;
        }
        public Vertices RotateClockwiseByCube(int times) {
            times %= 4;
            if (times % 2 == 1) {
                RotateClockwiseBySide(Side.front, 1);
                RotateClockwiseBySide(Side.right, 1);
                Vector3[] rightBackup = (Vector3[])right.Clone();
                RotateClockwiseBySide(Side.bottom, 1);
                right = bottom;
                RotateClockwiseBySide(Side.left, 1);
                bottom = left;
                RotateClockwiseBySide(Side.top, 1);
                left = top;
                top = rightBackup;
                RotateClockwiseBySide(Side.back, 3);
                times--;
            }
            if (times == 2) {
                RotateClockwiseBySide(Side.front, 2);
                RotateClockwiseBySide(Side.back, 2);
                RotateClockwiseBySide(Side.right, 2);
                Vector3[] temp = (Vector3[])right.Clone();
                RotateClockwiseBySide(Side.left, 2);
                right = left;
                left = temp;
                RotateClockwiseBySide(Side.top, 2);
                temp = (Vector3[])top.Clone();
                RotateClockwiseBySide(Side.bottom, 2);
                top = bottom;
                bottom = temp;
            }

            return this;
        }
        public void DebugPrint(Side side_) {
            Vector3[] vertices = GetBySide(side_);
            string log = "";
            for (int i = 0; i < meshPointsInRow; i ++) {
                for (int j = 0; j < meshPointsInRow; j++) {
                    log += vertices[i2dto1d(i, j)].z;
                    log += " ";
                }
                log += '\n';
            }
            Debug.Log(log);
        }
    }


    public int meshPointsInRow;
    public float sizeOfSide;
    public float scopeForZValue;
    public bool duringShift;
    private float distanceBetweenCubes;
    private bool sameData = false;
    private bool ownVerticesCalculated = false;

    public PiecePartMeshGenerate cubeBelow {get; private set;} = null;
    public PiecePartMeshGenerate cubeAbove {get; private set;} = null;
    public PiecePartMeshGenerate cubeOnLeft {get; private set;} = null;
    public PiecePartMeshGenerate cubeOnRight {get; private set;} = null;

    public Connections connectionsBottom {get; private set;}
    public Connections connectionsBack {get; private set;}
    public Connections connectionsLeft {get; private set;}
    public Connections connectionsRight {get; private set;}
    public Connections connectionsFront {get; private set;}
    public Connections connectionsTop {get; private set;}
    public Connections GetConnectionsBySide(Side side_) {
        switch (side_) {
            case Side.bottom:
                return connectionsBottom;
            case Side.back:
                return connectionsBack;
            case Side.left:
                return connectionsLeft;
            case Side.right:
                return connectionsRight;
            case Side.front:
                return connectionsFront;
            case Side.top:
                return connectionsTop;
            default:
                return null;
        }
    }

    public PiecePartMeshData piecePartMeshData {get; set;} = new PiecePartMeshData();

    public CornerPresance cornerPresanceBottom {get; private set;}
    public CornerPresance cornerPresanceBack {get; private set;}
    public CornerPresance cornerPresanceLeft {get; private set;}
    public CornerPresance cornerPresanceRight {get; private set;}
    public CornerPresance cornerPresanceFront {get; private set;}
    public CornerPresance cornerPresanceTop {get; private set;}
    public CornerPresance GetCornerPresanceBySide(Side side_) {
        switch (side_) {
            case Side.bottom:
                return cornerPresanceBottom;
            case Side.back:
                return cornerPresanceBack;
            case Side.left:
                return cornerPresanceLeft;
            case Side.right:
                return cornerPresanceRight;
            case Side.front:
                return cornerPresanceFront;
            case Side.top:
                return cornerPresanceTop;
            default:
                return null;
        }
    }

    public bool hasBelow {get; private set;} = false;
    public bool hasAbove {get; private set;} = false;
    public bool hasOnLeft {get; private set;} = false;
    public bool hasOnRight {get; private set;} = false;

    public int activeMeshes {get; private set;} = 2;

    private Transform ChildBottom = null, ChildBack, ChildLeft = null, ChildRight = null, ChildFront, ChildTop = null;
    public Transform GetChildBySide(Side side_) {
        switch (side_) {
            case Side.bottom:
                return ChildBottom.transform;
            case Side.back:
                return ChildBack.transform;
            case Side.left:
                return ChildLeft.transform;
            case Side.right:
                return ChildRight.transform;
            case Side.front:
                return ChildFront.transform;
            case Side.top:
                return ChildTop.transform;
            default:
                return null;
        }
    }
    public bool GetChildPresanceBySide(Side side_) {
        switch (side_) {
            case Side.bottom:
                return ChildBottom != null;
            case Side.back:
                return ChildBack != null;
            case Side.left:
                return ChildLeft != null;
            case Side.right:
                return ChildRight != null;
            case Side.front:
                return ChildFront != null;
            case Side.top:
                return ChildTop != null;
            default:
                return false;
        }
    }

    // public Vector3[] verticesBottom {get; private set;}
    // public Vector3[] verticesBack {get; private set;}
    // public Vector3[] verticesLeft {get; private set;}
    // public Vector3[] verticesRight {get; private set;}
    // public Vector3[] verticesFront {get; private set;}
    // public Vector3[] verticesTop {get; private set;}
    // public Vector3[] GetVerticesBySide(Side side_) {
    //     switch (side_) {
    //         case Side.bottom:
    //             return verticesBottom;
    //         case Side.back:
    //             return verticesBack;
    //         case Side.left:
    //             return verticesLeft;
    //         case Side.right:
    //             return verticesRight;
    //         case Side.front:
    //             return verticesFront;
    //         case Side.top:
    //             return verticesTop;
    //         default:
    //             return null;
    //     }
    // }
    // public void SetVerticesBySide(Side side_, Vector3[] vertices) {
    //     switch (side_) {
    //         case Side.bottom:
    //             verticesBottom = vertices;
    //             break;
    //         case Side.back:
    //             verticesBack = vertices;
    //             break;
    //         case Side.left:
    //             verticesLeft = vertices;
    //             break;
    //         case Side.right:
    //             verticesRight = vertices;
    //             break;
    //         case Side.front:
    //             verticesFront = vertices;
    //             break;
    //         case Side.top:
    //             verticesTop = vertices;
    //             break;
    //         default:
    //             break;
    //     }
    // }
    // public void RotateVerticesArrayClockwiseBySide(Side side_, int times) {
    //     Vector3[] vertices = GetVerticesBySide(side_);
    //     times %= 4;
    //     int l = vertices.Length - 1;
    //     if (times % 2 == 1) {
    //         for (int i = 0; i < l; i++) {
    //             Vector3 temp = vertices[(0) * l + i];
    //             vertices[(0) * l + i] = vertices[(i) * l + l];
    //             vertices[(i) * l + l] = vertices[(l) * l + l - i];
    //             vertices[(l) * l + l - i] = vertices[(l - i) * l + 0];
    //             vertices[(l - i) * l + 0] = temp;
    //         }
    //         times--;
    //     }
    //     if (times == 2) {
    //         for (int i = 0; i < l; i++) {
    //             Vector3 temp = vertices[0 * l + i];
    //             vertices[0 * l + i] = vertices[l * l + l - i];
    //             vertices[l * l + l - i] = temp;
    //             temp = vertices[i * l + l];
    //             vertices[i * l + l] = vertices[(l - i) * l + 0];
    //             vertices[(l - i) * l + 0] = temp;
    //         }
    //     }
    //     SetVerticesBySide(side_, vertices);
    // }
    public Vertices vertices {get; private set;}

    public Vector3[] GetVerticesBySide(Side side_) {
        return vertices.GetBySide(side_);
    }
    public void SetVerticesBySide(Side side_, Vector3[] verticesPerSide) {
        vertices.SetBySide(side_, verticesPerSide);
    }

    public Vector3 boundOfLeftBottomBack {get; private set;}
    public Vector3 boundOfLeftTopBack {get; private set;}
    public Vector3 boundOfRightBottomBack {get; private set;}
    public Vector3 boundOfRightTopBack {get; private set;}
    public Vector3 boundOfLeftBottomFront {get; private set;}
    public Vector3 boundOfLeftTopFront {get; private set;}
    public Vector3 boundOfRightBottomFront {get; private set;}
    public Vector3 boundOfRightTopFront {get; private set;}

    /**
    <summary>Assigning children transforms to Child[Side] variables</summary>
    **/
    private void FindChildren() {
        if (!hasBelow) ChildBottom = transform.GetChild(0);
        else ChildBottom = null;
        ChildBack = transform.GetChild(1);
        if (!hasOnLeft) ChildLeft = transform.GetChild(2);
        else ChildLeft = null;
        if (!hasOnRight) ChildRight = transform.GetChild(3);
        else ChildRight = null;
        ChildFront = transform.GetChild(4);
        if (!hasAbove) ChildTop = transform.GetChild(5);
        else ChildTop = null;
    }
    
    /**
    <summary>Only method to do all actions on cube (clear data, calculate vertices, find children's transforms)</summary>
    <param name="ppBelow">Cube below this (on bottom side)</param>
    <param name="ppAbove">Cube above this (on top side)</param>
    <param name="ppLeft">Cube on left of this (on left side)</param>
    <param name="ppRight">Cube on right of this (on right side)</param>
    <param name="meshPointsInRow_">Number of vertices of one side in one direction (all vertices of side are this value squared)</param>
    <param name="sizeOfSide_">Size of side in one direction</param>
    <param name="scopeForZValue_">Scope from which Z value will be chosen randomly</param>
    **/
    public void Initialize(
        PiecePartMeshGenerate ppBelow, PiecePartMeshGenerate ppAbove,
        PiecePartMeshGenerate ppLeft, PiecePartMeshGenerate ppRight,
        int meshPointsInRow_, float sizeOfSide_, float scopeForZValue_, float distanceBetweenCubes_) 
    {
        if (meshPointsInRow != meshPointsInRow_ || sizeOfSide != sizeOfSide_ || scopeForZValue != scopeForZValue_) {
            meshPointsInRow = meshPointsInRow_;
            sizeOfSide = sizeOfSide_;
            scopeForZValue = scopeForZValue_;
            sameData = false;
        } else sameData = true;
        if (distanceBetweenCubes != distanceBetweenCubes_) {
            distanceBetweenCubes = distanceBetweenCubes_;
            SetBounds();
        }

        PrepareDataContainers();
        SetNeighbourPresance(ppBelow, ppAbove, ppLeft, ppRight);

        if (!ownVerticesCalculated || !sameData)
            CalculateOwnVertices();
        FindChildren();
    }
    public void Initialize(
        PiecePartMeshGenerate ppBelow, PiecePartMeshGenerate ppAbove,
        PiecePartMeshGenerate ppLeft, PiecePartMeshGenerate ppRight,
        int meshPointsInRow_, float sizeOfSide_, List<float[,]> heightTables, float distanceBetweenCubes_
    ) {
        meshPointsInRow_ = heightTables[0].GetLength(0);

        if (meshPointsInRow != meshPointsInRow_ || sizeOfSide != sizeOfSide_) {
            meshPointsInRow = meshPointsInRow_;
            sizeOfSide = sizeOfSide_;
            sameData = false;
        } else sameData = true;
        if (distanceBetweenCubes != distanceBetweenCubes_) {
            distanceBetweenCubes = distanceBetweenCubes_;
            SetBounds();
        }


        PrepareDataContainers();
        SetNeighbourPresance(ppBelow, ppAbove, ppLeft, ppRight);

        if (!ownVerticesCalculated || !sameData)
            CalculateOwnVertices(heightTables);
        FindChildren();
    }

    /**
    <summary>Clear data for data containers</summary>
    **/
    private void PrepareDataContainers() {
        connectionsBottom = new Connections();
        connectionsBack = new Connections();
        connectionsLeft = new Connections();
        connectionsRight = new Connections();
        connectionsFront = new Connections();
        connectionsTop = new Connections();

        cornerPresanceBottom = new CornerPresance();
        cornerPresanceBack = new CornerPresance();
        cornerPresanceLeft = new CornerPresance();
        cornerPresanceRight = new CornerPresance();
        cornerPresanceFront = new CornerPresance();
        cornerPresanceTop = new CornerPresance();
    }


    public void CalculateOwnVertices()
    {
        InitializeVerticesArrays();

        for (int i = 0; i < meshPointsInRow * meshPointsInRow; i++) {
            int iX = i % meshPointsInRow;
            int iY = i / meshPointsInRow;
            float x = sizeOfSide / (meshPointsInRow - 1) * iX - sizeOfSide / 2f;
            float y = sizeOfSide / (meshPointsInRow - 1) * iY - sizeOfSide / 2f;

            
            Vector3 tempPos = new Vector3(x, y, 0f);
            tempPos.z = Random.Range(-scopeForZValue, scopeForZValue);
            vertices.bottom[i] = (tempPos);

            tempPos.z = Random.Range(-scopeForZValue, scopeForZValue);
            vertices.back[i] = (tempPos);
            
            tempPos.z = Random.Range(-scopeForZValue, scopeForZValue);
            vertices.left[i] = (tempPos);

            tempPos.z = Random.Range(-scopeForZValue, scopeForZValue);
            vertices.right[i] = (tempPos);

            tempPos.z = Random.Range(-scopeForZValue, scopeForZValue);
            vertices.front[i] = (tempPos);

            tempPos.z = Random.Range(-scopeForZValue, scopeForZValue);
            vertices.top[i] = (tempPos);
        }
        ownVerticesCalculated = true;
    }
    private void CalculateOwnVertices(List<float[,]> heightTables) {
        InitializeVerticesArrays();
        int[] tableForSide = new int[6];
        for (int i = 0; i < 6; i++) {
            tableForSide[i] = Random.Range(0, heightTables.Count);
        }

        for (int i = 0; i < meshPointsInRow * meshPointsInRow; i++) {
            int iX = i % meshPointsInRow;
            int iY = i / meshPointsInRow;
            float x = sizeOfSide / (meshPointsInRow - 1) * iX - sizeOfSide / 2f;
            float y = sizeOfSide / (meshPointsInRow - 1) * iY - sizeOfSide / 2f;

            
            Vector3 tempPos = new Vector3(x, y, 0f);
            tempPos.z = heightTables[tableForSide[0]][iX, iY];
            vertices.bottom[i] = (tempPos);

            tempPos.z = heightTables[tableForSide[1]][iX, iY];
            vertices.back[i] = (tempPos);
            
            tempPos.z = heightTables[tableForSide[2]][iX, iY];
            vertices.left[i] = (tempPos);

            tempPos.z = heightTables[tableForSide[3]][iX, iY];
            vertices.right[i] = (tempPos);

            tempPos.z = heightTables[tableForSide[4]][iX, iY];
            vertices.front[i] = (tempPos);

            tempPos.z = heightTables[tableForSide[5]][iX, iY];
            vertices.top[i] = (tempPos);
        }
        ownVerticesCalculated = true;
    }


    private void SetNeighbourPresance(
        PiecePartMeshGenerate ppBelow = null, PiecePartMeshGenerate ppAbove = null,
        PiecePartMeshGenerate ppLeft = null, PiecePartMeshGenerate ppRight = null)
    {
        activeMeshes = 2;

        if (ppBelow == null) {
            hasBelow = false; activeMeshes++;
            cubeBelow = null;
        }
        else {
            hasBelow = true;
            cubeBelow = ppBelow;
        }

        if (ppAbove == null) {
            hasAbove = false; activeMeshes++;
            cubeAbove = null;
        }
        else {
            hasAbove = true;
            cubeAbove = ppAbove;
        }

        if (ppLeft == null) {
            hasOnLeft = false; activeMeshes++;
            cubeOnLeft = null;
        }
        else {
            hasOnLeft = true;
            cubeOnLeft = ppLeft;
        }

        if (ppRight == null) {
            hasOnRight = false; activeMeshes++;
            cubeOnRight = null;
        }
        else {
            hasOnRight = true;
            cubeOnRight = ppRight;
        }
    }

    private void InitializeVerticesArrays() {
        int pSquare = meshPointsInRow * meshPointsInRow;

        vertices = new Vertices();
        vertices.meshPointsInRow = (short)meshPointsInRow;
        vertices.bottom = new Vector3[pSquare];
        vertices.back = new Vector3[pSquare];
        vertices.left = new Vector3[pSquare];
        vertices.right = new Vector3[pSquare];
        vertices.front = new Vector3[pSquare];
        vertices.top = new Vector3[pSquare];
    }
    
    private void SetBounds() {
        float d = distanceBetweenCubes / 2f;
        boundOfLeftBottomBack =     new Vector3(-d, -d, d);
        boundOfLeftTopBack =        new Vector3(-d, d, d);
        boundOfRightBottomBack =    new Vector3(d, -d, d);
        boundOfRightTopBack =       new Vector3(d, d, d);
        boundOfLeftBottomFront =    new Vector3(-d, -d, -d);
        boundOfLeftTopFront =       new Vector3(-d, d, -d);
        boundOfRightBottomFront =   new Vector3(d, -d, -d);
        boundOfRightTopFront =      new Vector3(d, d, -d);
    }

    private void Awake() {
    }



    public void rename() {
        vertices.name = this.name;
    }
}