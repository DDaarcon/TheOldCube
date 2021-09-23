using UnityEngine;

using GameInfo.GameInfoInternals;

namespace GameExtensions.Editor
{
    public static class EditorExtensions
    {
        public static void AbortPlacing(this EditorEnvironmentInfo editorInfo)
        {
            editorInfo.DuringPlacing = false;
            Object.Destroy(editorInfo.PlacedPiece.Object);
        }
    }
}
