using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PiecePartMeshGenerate))]
public class PiecePartMeshEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        
        PiecePartMeshGenerate piecePartMesh = (PiecePartMeshGenerate)target;
        if (GUILayout.Button("Print bottom")) {
            piecePartMesh.vertices.DebugPrint(Enums.Side.bottom);
        }
        if (GUILayout.Button("Print back")) {
            piecePartMesh.vertices.DebugPrint(Enums.Side.back);
        }
        if (GUILayout.Button("Print left")) {
            piecePartMesh.vertices.DebugPrint(Enums.Side.left);
        }
        if (GUILayout.Button("Print right")) {
            piecePartMesh.vertices.DebugPrint(Enums.Side.right);
        }
        if (GUILayout.Button("Print front")) {
            piecePartMesh.vertices.DebugPrint(Enums.Side.front);
        }
        if (GUILayout.Button("Print top")) {
            piecePartMesh.vertices.DebugPrint(Enums.Side.top);
        }
    }
}
