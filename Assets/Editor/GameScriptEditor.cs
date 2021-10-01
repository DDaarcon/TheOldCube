using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.Linq;

namespace EditorGUI.BaseScripts
{

    [CustomEditor(typeof(GameScript))]
    public class GameScriptEditor : Editor
    {
        GameScript Target;
        List<SerializedObject> InformationProperties;
        List<SerializedProperty> Properties;

        void OnEnable()
        {
            Target = target as GameScript;
            InformationProperties = new List<SerializedObject>();

            if (Target.Information == null)
                Target.Information = new GameInfo.GameInformation();

            foreach (var prop in Target.Information.GetType().GetProperties())
            {
                object obj = prop.GetValue(Target.Information);
                UnityEngine.Object unityObject = obj as UnityEngine.Object;
                if (unityObject != null)
                {
                    SerializedObject subTarget = new SerializedObject(unityObject);
                    InformationProperties.Add(subTarget);
                }
            }

            Properties = InformationProperties.SelectMany(x =>
            {
                List<SerializedProperty> insideProp = new List<SerializedProperty>();

                var property = x.GetIterator();
                while (property.NextVisible(true))
                {
                    insideProp.Add(property);
                }
                return insideProp;
            }).ToList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            foreach (var property in Properties ?? new List<SerializedProperty>())
            {
                EditorGUILayout.PropertyField(property);
            }
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}
