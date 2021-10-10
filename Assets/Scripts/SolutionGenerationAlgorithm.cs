﻿using UnityEngine;
using static Enums;

public static class SolutionGenerationAlgorithm
{

    private const int a = 0, b = 1;
    /**
    <value>Seed of previously generated solution</value>
    **/
    public static int SeedOfLast {get; set;}
    public static Variant Variant {get; private set;}
    private static int variantInt;

    private static bool[][,] solution = new bool[6][,];

    private class Corner {
        // all 3 sides that share this corner
        public Side[] sidesSharing = new Side[3];
        // where on all 3 sides this corner is
        public Enums.Corner[] sideAppearance = new Enums.Corner[3];
        // can this corner appear each of sides
        public bool[] canAppear = new bool[3];
        // on which side this corner will be
        public int sideAppearing;
    }

    private class Edge {
        public Side[] sidesSharing = new Side[2];
        public Side[] sideAppearings = new Side[2];

        public Edge() {
            int size = 2;
            if (SolutionGenerationAlgorithm.Variant == Variant.x3) size = 1;
            if (SolutionGenerationAlgorithm.Variant == Variant.x4) size = 2;
            sideAppearings = new Side[size];
        }
    }

    public static bool[][,] GetNewSolution(Variant variant_ = Variant.x4) {
        return GetNewSolution(System.Environment.TickCount, variant_);
    }
    public static bool[][,] GetNewSolution(int seed, Variant variant_ = Variant.x4) {
        SeedOfLast = seed;
        Random.InitState(SeedOfLast);
        
        Variant = variant_;
        variantInt = (int)variant_;

        FillCenters();
        FillMiddleEdges();
        FillCorners();

        return solution;
    }
    private static void FillCenters() {
        for (int o = 0; o < 6; o++) {
            solution[o] = new bool[variantInt, variantInt];
            for (int i = 0; i < variantInt; i++) {
                for (int j = 0; j < variantInt; j++) {
                    int l = variantInt - 1;
                    if (i > 0 && i < l && j > 0 && j < l)
                        solution[o][i, j] = I;
                    else
                        solution[o][i, j] = O;
                }
            }
        }
    }
    private static void FillMiddleEdges() {
        Edge[] edges = new Edge[12];
        for (int i = 0; i < 4; i++) {
            edges[i] = new Edge();
            edges[i].sidesSharing[0] = Side.Bottom;
            edges[i].sidesSharing[1] = (Side)(i + 1);
            edges[i + 4] = new Edge();
            edges[i + 4].sidesSharing[0] = Side.Top;
            edges[i + 4].sidesSharing[1] = (Side)(i + 1);
        }
        for (int i = 8; i < 12; i++) edges[i] = new Edge();
        edges[8].sidesSharing[0] = Side.Left;
        edges[8].sidesSharing[1] = Side.Front;
        edges[9].sidesSharing[0] = Side.Front;
        edges[9].sidesSharing[1] = Side.Right;
        edges[10].sidesSharing[0] = Side.Right;
        edges[10].sidesSharing[1] = Side.Back;
        edges[11].sidesSharing[0] = Side.Back;
        edges[11].sidesSharing[1] = Side.Left;

        for (int i = 0; i < 12; i++) {
            for (int j = 0; j < edges[i].sideAppearings.Length; j++)
                edges[i].sideAppearings[j] = edges[i].sidesSharing[Random.Range(0, 2)];
        }

        switch (Variant) {
            case Variant.x4:
                // bottom
                solution[0][0,1] = edges[0].sideAppearings[a] == Side.Bottom;
                solution[0][0,2] = edges[0].sideAppearings[b] == Side.Bottom;
                solution[0][2,0] = edges[1].sideAppearings[a] == Side.Bottom;
                solution[0][1,0] = edges[1].sideAppearings[b] == Side.Bottom;
                solution[0][1,3] = edges[2].sideAppearings[a] == Side.Bottom;
                solution[0][2,3] = edges[2].sideAppearings[b] == Side.Bottom;
                solution[0][3,2] = edges[3].sideAppearings[a] == Side.Bottom;
                solution[0][3,1] = edges[3].sideAppearings[b] == Side.Bottom;
                // back
                solution[1][0,1] = edges[4].sideAppearings[b] == Side.Back;
                solution[1][0,2] = edges[4].sideAppearings[a] == Side.Back;
                solution[1][2,0] = edges[11].sideAppearings[a] == Side.Back;
                solution[1][1,0] = edges[11].sideAppearings[b] == Side.Back;
                solution[1][1,3] = edges[10].sideAppearings[b] == Side.Back;
                solution[1][2,3] = edges[10].sideAppearings[a] == Side.Back;
                solution[1][3,2] = edges[0].sideAppearings[b] == Side.Back;
                solution[1][3,1] = edges[0].sideAppearings[a] == Side.Back;
                // left
                solution[2][0,1] = edges[11].sideAppearings[b] == Side.Left;
                solution[2][0,2] = edges[11].sideAppearings[a] == Side.Left;
                solution[2][2,0] = edges[5].sideAppearings[b] == Side.Left;
                solution[2][1,0] = edges[5].sideAppearings[a] == Side.Left;
                solution[2][1,3] = edges[1].sideAppearings[b] == Side.Left;
                solution[2][2,3] = edges[1].sideAppearings[a] == Side.Left;
                solution[2][3,2] = edges[8].sideAppearings[a] == Side.Left;
                solution[2][3,1] = edges[8].sideAppearings[b] == Side.Left;
                // right
                solution[3][0,1] = edges[10].sideAppearings[a] == Side.Right;
                solution[3][0,2] = edges[10].sideAppearings[b] == Side.Right;
                solution[3][2,0] = edges[2].sideAppearings[b] == Side.Right;
                solution[3][1,0] = edges[2].sideAppearings[a] == Side.Right;
                solution[3][1,3] = edges[6].sideAppearings[b] == Side.Right;
                solution[3][2,3] = edges[6].sideAppearings[a] == Side.Right;
                solution[3][3,2] = edges[9].sideAppearings[b] == Side.Right;
                solution[3][3,1] = edges[9].sideAppearings[a] == Side.Right;
                // front
                solution[4][0,1] = edges[3].sideAppearings[b] == Side.Front;
                solution[4][0,2] = edges[3].sideAppearings[a] == Side.Front;
                solution[4][2,0] = edges[8].sideAppearings[b] == Side.Front;
                solution[4][1,0] = edges[8].sideAppearings[a] == Side.Front;
                solution[4][1,3] = edges[9].sideAppearings[a] == Side.Front;
                solution[4][2,3] = edges[9].sideAppearings[b] == Side.Front;
                solution[4][3,2] = edges[7].sideAppearings[b] == Side.Front;
                solution[4][3,1] = edges[7].sideAppearings[a] == Side.Front;
                // top
                solution[5][0,1] = edges[4].sideAppearings[a] == Side.Top;
                solution[5][0,2] = edges[4].sideAppearings[b] == Side.Top;
                solution[5][2,0] = edges[6].sideAppearings[a] == Side.Top;
                solution[5][1,0] = edges[6].sideAppearings[b] == Side.Top;
                solution[5][1,3] = edges[5].sideAppearings[a] == Side.Top;
                solution[5][2,3] = edges[5].sideAppearings[b] == Side.Top;
                solution[5][3,2] = edges[7].sideAppearings[a] == Side.Top;
                solution[5][3,1] = edges[7].sideAppearings[b] == Side.Top;
                break;
            case Variant.x3:
                // bottom
                solution[0][0,1] = edges[0].sideAppearings[a] == Side.Bottom;
                solution[0][1,0] = edges[1].sideAppearings[a] == Side.Bottom;
                solution[0][1,2] = edges[2].sideAppearings[a] == Side.Bottom;
                solution[0][2,1] = edges[3].sideAppearings[a] == Side.Bottom;
                // back
                solution[1][0,1] = edges[4].sideAppearings[a] == Side.Back;
                solution[1][1,0] = edges[11].sideAppearings[a] == Side.Back;
                solution[1][1,2] = edges[10].sideAppearings[a] == Side.Back;
                solution[1][2,1] = edges[0].sideAppearings[a] == Side.Back;
                // left
                solution[2][0,1] = edges[11].sideAppearings[a] == Side.Left;
                solution[2][1,0] = edges[5].sideAppearings[a] == Side.Left;
                solution[2][1,2] = edges[1].sideAppearings[a] == Side.Left;
                solution[2][2,1] = edges[8].sideAppearings[a] == Side.Left;
                // right
                solution[3][0,1] = edges[10].sideAppearings[a] == Side.Right;
                solution[3][1,0] = edges[2].sideAppearings[a] == Side.Right;
                solution[3][1,2] = edges[6].sideAppearings[a] == Side.Right;
                solution[3][2,1] = edges[9].sideAppearings[a] == Side.Right;
                // front
                solution[4][0,1] = edges[3].sideAppearings[a] == Side.Front;
                solution[4][1,0] = edges[8].sideAppearings[a] == Side.Front;
                solution[4][1,2] = edges[9].sideAppearings[a] == Side.Front;
                solution[4][2,1] = edges[7].sideAppearings[a] == Side.Front;
                // top
                solution[5][0,1] = edges[4].sideAppearings[a] == Side.Top;
                solution[5][1,0] = edges[6].sideAppearings[a] == Side.Top;
                solution[5][1,2] = edges[5].sideAppearings[a] == Side.Top;
                solution[5][2,1] = edges[7].sideAppearings[a] == Side.Top;
                break;

        }
    }
    
    private static void FillCorners() {
        Corner[] corners = new Corner[8];
        for (int i = 0; i < 8; i++) corners[i] = new Corner();
        corners[0].sidesSharing = new Side[] {Side.Bottom, Side.Back, Side.Left};
        corners[0].sideAppearance = new Enums.Corner[] { Enums.Corner.Ul, Enums.Corner.Dl, Enums.Corner.Ur};

        corners[1].sidesSharing = new Side[] {Side.Bottom, Side.Back, Side.Right};
        corners[1].sideAppearance = new Enums.Corner[] { Enums.Corner.Ur, Enums.Corner.Dr, Enums.Corner.Ul};

        corners[2].sidesSharing = new Side[] {Side.Bottom, Side.Front, Side.Right};
        corners[2].sideAppearance = new Enums.Corner[] { Enums.Corner.Dr, Enums.Corner.Ur, Enums.Corner.Dl};

        corners[3].sidesSharing = new Side[] {Side.Bottom, Side.Front, Side.Left};
        corners[3].sideAppearance = new Enums.Corner[] { Enums.Corner.Dl, Enums.Corner.Ul, Enums.Corner.Dr};

        corners[4].sidesSharing = new Side[] {Side.Top, Side.Back, Side.Right};
        corners[4].sideAppearance = new Enums.Corner[] { Enums.Corner.Ul, Enums.Corner.Ur, Enums.Corner.Ur};

        corners[5].sidesSharing = new Side[] {Side.Top, Side.Back, Side.Left};
        corners[5].sideAppearance = new Enums.Corner[] { Enums.Corner.Ur, Enums.Corner.Ul, Enums.Corner.Ul};

        corners[6].sidesSharing = new Side[] {Side.Top, Side.Front, Side.Left};
        corners[6].sideAppearance = new Enums.Corner[] { Enums.Corner.Dr, Enums.Corner.Dl, Enums.Corner.Dl};

        corners[7].sidesSharing = new Side[] {Side.Top, Side.Front, Side.Right};
        corners[7].sideAppearance = new Enums.Corner[] { Enums.Corner.Dl, Enums.Corner.Dr, Enums.Corner.Dr};

        int l = variantInt - 1;

        for (int i = 0; i < 8; i++) {
            for (int s = 0; s < l; s++) {
                switch (Variant) {
                    case Variant.x4:
                        if (corners[i].sideAppearance[s] == Enums.Corner.Ul) {
                            if (solution[(int)corners[i].sidesSharing[s]][0,1] == I ||
								solution[(int)corners[i].sidesSharing[s]][1,0] == I) {
                                corners[i].canAppear[s] = true;
                            } else corners[i].canAppear[s] = false;
                        }
                        if (corners[i].sideAppearance[s] == Enums.Corner.Ur) {
                            if (solution[(int)corners[i].sidesSharing[s]][0,2] == I ||
								solution[(int)corners[i].sidesSharing[s]][1,3] == I) {
                                corners[i].canAppear[s] = true;
                            } else corners[i].canAppear[s] = false;
                        }
                        if (corners[i].sideAppearance[s] == Enums.Corner.Dr) {
                            if (solution[(int)corners[i].sidesSharing[s]][2,3] == I ||
								solution[(int)corners[i].sidesSharing[s]][3,2] == I) {
                                corners[i].canAppear[s] = true;
                            } else corners[i].canAppear[s] = false;
                        }
                        if (corners[i].sideAppearance[s] == Enums.Corner.Dl) {
                            if (solution[(int)corners[i].sidesSharing[s]][3,1] == I ||
								solution[(int)corners[i].sidesSharing[s]][2,0] == I) {
                                corners[i].canAppear[s] = true;
                            } else corners[i].canAppear[s] = false;
                        }
                        break;
                    case Variant.x3:
                        if (corners[i].sideAppearance[s] == Enums.Corner.Ul) {
                            if (solution[(int)corners[i].sidesSharing[s]][1,0] == I ||
								solution[(int)corners[i].sidesSharing[s]][0,1] == I) {
                                corners[i].canAppear[s] = true;
                            } else corners[i].canAppear[s] = false;
                        }
                        if (corners[i].sideAppearance[s] == Enums.Corner.Ur) {
                            if (solution[(int)corners[i].sidesSharing[s]][0,1] == I ||
								solution[(int)corners[i].sidesSharing[s]][1,2] == I) {
                                corners[i].canAppear[s] = true;
                            } else corners[i].canAppear[s] = false;
                        }
                        if (corners[i].sideAppearance[s] == Enums.Corner.Dr) {
                            if (solution[(int)corners[i].sidesSharing[s]][1,2] == I ||
								solution[(int)corners[i].sidesSharing[s]][2,1] == I) {
                                corners[i].canAppear[s] = true;
                            } else corners[i].canAppear[s] = false;
                        }
                        if (corners[i].sideAppearance[s] == Enums.Corner.Dl) {
                            if (solution[(int)corners[i].sidesSharing[s]][2,1] == I ||
								solution[(int)corners[i].sidesSharing[s]][1,0] == I) {
                                corners[i].canAppear[s] = true;
                            } else corners[i].canAppear[s] = false;
                        }
                        break;
                }
            }
        }

        for (int i = 0; i < 8; i++) {
            int countAvailable = 0;
            for (int c = 0; c < 3; c++) {
                if (corners[i].canAppear[c]) {
                    countAvailable++;
                }
            }

            corners[i].sideAppearing = Random.Range(0, countAvailable);
            // przy O I I i wylosowaniu liczby 1 wybierze inna sciane niz zamierzono
            while (corners[i].canAppear[corners[i].sideAppearing] == false) corners[i].sideAppearing++;
            switch (corners[i].sideAppearance[corners[i].sideAppearing]) {
                case Enums.Corner.Ul:
					solution[(int)corners[i].sidesSharing[corners[i].sideAppearing]][0, 0] = I;
                    break;
                case Enums.Corner.Ur:
					solution[(int)corners[i].sidesSharing[corners[i].sideAppearing]][0, l] = I;
                    break;
                case Enums.Corner.Dr:
					solution[(int)corners[i].sidesSharing[corners[i].sideAppearing]][l, l] = I;
                    break;
                case Enums.Corner.Dl:
					solution[(int)corners[i].sidesSharing[corners[i].sideAppearing]][l, 0] = I;
                    break;
            }
        }
    }
}
