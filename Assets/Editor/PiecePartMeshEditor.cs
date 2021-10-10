using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PiecePartMeshGenerate))]
public class PiecePartMeshEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        
        PiecePartMeshGenerate piecePartMesh = (PiecePartMeshGenerate)target;
        if (GUILayout.Button("Print bottom")) {
            piecePartMesh.vertices.DebugPrint(Enums.Side.Bottom);
        }
        if (GUILayout.Button("Print back")) {
            piecePartMesh.vertices.DebugPrint(Enums.Side.Back);
        }
        if (GUILayout.Button("Print left")) {
            piecePartMesh.vertices.DebugPrint(Enums.Side.Left);
        }
        if (GUILayout.Button("Print right")) {
            piecePartMesh.vertices.DebugPrint(Enums.Side.Right);
        }
        if (GUILayout.Button("Print front")) {
            piecePartMesh.vertices.DebugPrint(Enums.Side.Front);
        }
        if (GUILayout.Button("Print top")) {
            piecePartMesh.vertices.DebugPrint(Enums.Side.Top);
        }
    }
}
