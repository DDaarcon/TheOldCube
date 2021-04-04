using UnityEngine;
public static class Enums
{
    public const bool I = true, O = false;
    public enum Variant {x3 = 3, x4 = 4};
    public enum Side {bottom = 0, back = 1, left = 2, right = 3, front = 4, top = 5};
    public enum Dir {U, L, D, R, Un, Ln, Dn, Rn};
    public enum IndexStateChange {increase, stay, decrease};
    public enum Corner {Ul, Ur, Dr, Dl};
    public enum RotOrPos {rotation, position};
    public enum Themes {
        BasicStone, FancyStone, Gold, Minecraft, RedShiny, Copper, BlueShiny, DarkElement, TicTacToe
    }

    public static Vector3 DirToVector3(Dir dir_) {
        switch (dir_) {
            default:
            case Dir.U:
            case Dir.Un:
                return Vector3.up;
            case Dir.R:
            case Dir.Rn:
                return Vector3.right;
            case Dir.D:
            case Dir.Dn:
                return Vector3.down;
            case Dir.L:
            case Dir.Ln:
                return Vector3.left;
        }
    }

    public static Dir ReverseDir(Dir dir_) {
        switch (dir_) {
            default:
            case Dir.U:
                return Dir.D;
            case Dir.Un:
                return Dir.Dn;
            case Dir.R:
                return Dir.L;
            case Dir.Rn:
                return Dir.Ln;
            case Dir.D:
                return Dir.U;
            case Dir.Dn:
                return Dir.Un;
            case Dir.L:
                return Dir.R;
            case Dir.Ln:
                return Dir.Rn;
        }
    }

    public static bool IntToBool(int i) {
        switch (i) {
            default:
            case 0:
                return false;
            case 1:
                return true;
        }
    }
    public static int BoolToInt(bool b) {
        return b ? 1 : 0;
    }
}
