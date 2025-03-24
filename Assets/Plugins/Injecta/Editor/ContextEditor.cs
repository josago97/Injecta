#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Injecta.UnityEditor
{/*
    [CustomEditor(typeof(Context), true)]
    public class ContextEditor : Editor
    {
        private static readonly string[] PROPERTY_NAMES =
        {
            "scriptableObjectInstallers",
            "monoInstallers",
            "prefabInstallers"
        };

        private static readonly string[] DISPLAY_NAMES =
        {
            "Scriptable Object Installers",
            "Mono Installers",
            "Prefab Installers"
        };

        private List<ReorderableList> _installersLists;

        protected virtual void OnEnable()
        {
            _installersLists = new List<ReorderableList>();

            string[] names = PROPERTY_NAMES;
            string[] displayNames = DISPLAY_NAMES;

            var infos = names.Select((n, i) => new { Name = n, DisplayName = displayNames[i] });

            foreach (var info in infos)
            {
                SerializedProperty property = serializedObject.FindProperty(info.Name);
                ReorderableList installersList = new ReorderableList(serializedObject, property, true, true, true, true);
                _installersLists.Add(installersList);

                installersList.drawHeaderCallback += rect =>
                {
                    GUI.Label(rect, info.DisplayName);
                };

                installersList.drawElementCallback += (rect, index, active, focused) =>
                {
                    rect.width -= 40;
                    rect.x += 20;
                    EditorGUI.PropertyField(rect, property.GetArrayElementAtIndex(index), GUIContent.none, true);
                };
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            OnGui();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnGui()
        {
            for (int i = 0; i < _installersLists.Count; i++)
            {
                ReorderableList list = _installersLists[i];
                list.DoLayoutList();
                if (i < _installersLists.Count - 1) EditorGUILayout.Space();
            }
        }
    }*/
}
#endif