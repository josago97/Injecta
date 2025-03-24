#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Injecta.UnityEditor
{
    public class CreationMenuEditor
    {
        private const int MENU_PRIORITY = 1;

        [MenuItem("Assets/Create/Injecta/Project Context", false, MENU_PRIORITY)]
        public static void CreateProjectContext()
        {
            GameObject[] projectContexts = FindPrefabsWithComponent<ProjectContext>();

            if (projectContexts.Length > 0)
            {
                EditorGUIUtility.PingObject(projectContexts[0]);
                ShowError("A prefab with a ProjectContext component already exists.");

                string a = "'ProjectContext.prefab' must be placed inside a directory named 'Resources'. Please try again by right clicking whithin the Project pane in a valid Resources folder. ";
            }
            else
            {
                GameObject prefab = new GameObject();
                prefab.AddComponent<ProjectContext>();

                string path = $"{GetCurrentAssetFolderPath()}/ProjectContext.prefab";
                PrefabUtility.SaveAsPrefabAsset(prefab, path);
                Object.DestroyImmediate(prefab);
            }
        }

        [MenuItem("Assets/Create/Injecta/Mono Installer", false, MENU_PRIORITY + 1)]
        public static void CreateMonoInstaller()
        {
            CreateScriptInstaller("Mono Installer", "MonoInstaller");
        }

        [MenuItem("Assets/Create/Injecta/Scriptable Object Installer", false, MENU_PRIORITY + 1)]
        public static void CreateScriptableObjectInstaller()
        {
            CreateScriptInstaller("Scriptable Object Installer", "ScriptableObjectInstaller");
        }

        [MenuItem("GameObject/Injecta/Scene Context", false, MENU_PRIORITY)]
        public static void CreateSceneContext()
        {
            SceneContext[] sceneContexts = Object.FindObjectsByType<SceneContext>(default);

            if (sceneContexts.Length > 0)
            {
                EditorGUIUtility.PingObject(sceneContexts[0]);
                EditorUtility.DisplayDialog("Error creating Scene Context",
                    "A GameObject with a SceneContext component already exists in the scene.",
                    "Accept");
            }
            else
            {
                GameObject gameObject = new GameObject("SceneContext", typeof(SceneContext));
            }
        }

        private static string GetCurrentAssetFolderPath()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (Path.HasExtension(path))
            {
                path = Path.GetDirectoryName(path);
            }

            return path;
        }

        private static GameObject[] FindPrefabsWithComponent<T>() where T : Component
        {
            List<GameObject> prefabsWithComponent = new List<GameObject>();
            string[] allPrefabIds = AssetDatabase.FindAssets("t:Prefab");

            foreach (string prefabId in allPrefabIds)
            {
                string path = AssetDatabase.GUIDToAssetPath(prefabId);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab.GetComponentInChildren<T>() != null)
                {
                    prefabsWithComponent.Add(prefab);
                }
            }

            return prefabsWithComponent.ToArray();
        }

        private static void CreateScriptInstaller(string friendlyName, string parentClass)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("using UnityEngine;");
            stringBuilder.AppendLine("using Injecta;");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"public class CLASS_NAME : {parentClass}");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("  public override void InstallBindings()");
            stringBuilder.AppendLine("  {");
            stringBuilder.AppendLine("      ");
            stringBuilder.AppendLine("  }");
            stringBuilder.AppendLine("}");

            string template = stringBuilder.ToString();
            string folderPath = GetCurrentAssetFolderPath();

            CreateScriptFromTemplate(friendlyName, "UntitledInstaller", template, folderPath);
        }

        private static void CreateScriptFromTemplate(string friendlyName, string defaultFileName, string template, string folderPath)
        {
            string absolutePath = EditorUtility.SaveFilePanel(
                $"Choose name for {friendlyName}",
                folderPath,
                defaultFileName + ".cs",
                "cs");

            if (string.IsNullOrWhiteSpace(absolutePath))
            {
                // Dialog was cancelled
                return;
            }

            Debug.Log("aaa " + absolutePath);

            if (!absolutePath.EndsWith(".cs", System.StringComparison.OrdinalIgnoreCase))
            {
                absolutePath += ".cs";
            }

            string className = Path.GetFileNameWithoutExtension(absolutePath);
            File.WriteAllText(absolutePath, template.Replace("CLASS_NAME", className));

            AssetDatabase.Refresh();

            /*
            var assetPath = ZenUnityEditorUtil.ConvertFullAbsolutePathToAssetPath(absolutePath);

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(assetPath);*/

        }

        private static void ShowError(string message)
        {
            EditorUtility.DisplayDialog("Error", message, "Ok");
        }
    }
}
#endif