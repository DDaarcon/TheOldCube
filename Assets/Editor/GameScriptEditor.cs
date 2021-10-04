using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.Linq;

namespace EditorGUI.BaseScripts
{

    [CustomEditor(typeof(GameScript))]
    public class GameScriptEditor : Editor
    {
        List<SerializedProperty> Properties;

        void OnEnable()
        {
            /*Properties = new List<SerializedProperty>();
            SerializedProperty environment = serializedObject.FindProperty("EnvironmentInformation");
            environment.Next(true);

            while (environment.Next(false))
            {
                Properties.Add(environment);
            }*/

        }
            
        public override void OnInspectorGUI()
        {
            //serializedObject.Update();

            //foreach (var property in Properties ?? new List<SerializedProperty>())
            //{
            //    EditorGUILayout.PropertyField(property);
            //}
            //serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}
