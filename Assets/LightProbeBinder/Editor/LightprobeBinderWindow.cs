using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

namespace TsujihaTools.LightProbeBinder
{
    public class LightProbeBinderWindow : EditorWindow
    {
        private static readonly Vector2 WindowMinSize = new Vector2(500, 400);

        private static bool isOpenProjectSettings = true;
        private static bool isOpenStatusMonitor = true;
        private static bool isOpenBindingSettings = true;
        private static bool isOpenOwnerManagement = true;
        private static bool isOpenOthers = true;

        public static GameObject root = null;

        [MenuItem("Tools/Rendering/Light Probe Binder")]
        public static void Open()
        {
            var window = GetWindow<LightProbeBinderWindow>("Light Probe Binder");

            Init();
        }

        private static void Init()
		{
            ResetSettings();

            EditorSceneManager.sceneOpened -= OnSceneLoaded;
            EditorSceneManager.sceneOpened += OnSceneLoaded;

            EditorSceneManager.activeSceneChanged -= OnActiveSceneChanged;
            EditorSceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private static void OnSceneLoaded(Scene scene, OpenSceneMode mode)
        {
            ResetSettings();
        }

        private static void OnActiveSceneChanged(Scene current, Scene next)
        {
            ResetSettings();
        }

        private static void ResetSettings()
        {
        }

        private void OnGUI()
        {
            // ウィンドウサイズ.
            minSize = WindowMinSize;

            // プロジェクト設定.
            isOpenProjectSettings = CustomUI.Foldout(isOpenProjectSettings, "Project Settings");
            if (isOpenProjectSettings)
            {
                if (GUILayout.Button(new GUIContent("Open Project Settings", "Open Project Settings / Light Probe Binder"), GUILayout.Width(150), GUILayout.Height(20)))
                {
                    SettingsService.OpenProjectSettings("Project/Light Probe Binder");
                }
            }

            CustomUI.HorizontalLine(position.width);

            // ステータス.
            isOpenStatusMonitor = CustomUI.Foldout(isOpenStatusMonitor, "Status Monitor");
            if (isOpenStatusMonitor)
            {
                EditorGUILayout.LabelField("Active Scene : " + SceneManager.GetActiveScene().name);

                EditorGUI.BeginDisabledGroup(true);
                LightProbes activeProbeDataAsset = EditorGUILayout.ObjectField(new GUIContent("Active Light Probe"), LightmapSettings.lightProbes, typeof(LightProbes), true) as LightProbes;
                EditorGUI.EndDisabledGroup();

                if (LightmapSettings.lightProbes == null)
                {
                    var lightProbeGroups = FindObjectsOfType<LightProbeGroup>()
                        .Where(obj => obj.gameObject.scene == SceneManager.GetActiveScene())
                        .ToList();

                    string warningMessage = "LightProbes does not exist in the scene.";
                    warningMessage += "\nPlease";

                    var directory = LightProbeBinderProcessor.GetSceneDirectoryPath();
                    if (!LightProbeBinderProcessor.ExistAsset<LightingDataAsset>(directory))
                    {

                        if (!lightProbeGroups.Any())
                        {
                            warningMessage += " add LightProbeGroup to scene and";
                        }
                        warningMessage += " generate lighting.";
                    }
                    else
                    {
                        warningMessage += " reload scene.";
                    }

                    EditorGUILayout.HelpBox(warningMessage, MessageType.Warning);
                }
            }

            CustomUI.HorizontalLine(position.width);

            var owners = LightProbeBinderProcessor.GetOwners();

            isOpenBindingSettings = CustomUI.Foldout(isOpenBindingSettings, "Binding Settings");
            if (isOpenBindingSettings)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUI.BeginDisabledGroup(owners.Any());
                root = EditorGUILayout.ObjectField(new GUIContent("Root Object", "When this object is activated, it replaces the LightProbes in the scene."), root, typeof(GameObject), true) as GameObject;
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(owners.Any() || root == null || LightmapSettings.lightProbes == null);
                if (GUILayout.Button(new GUIContent("Subscribe", "Add to Owner Object."), GUILayout.Width(150)))
                {
                    OnClickSubscribeButton(root);
                    root = null;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.EndDisabledGroup();

                if (LightmapSettings.lightProbes == null)
                {
                    string warningMessage = "LightProbes does not exist in the scene.\nPlease check the status monitor.";
                    EditorGUILayout.HelpBox(warningMessage, MessageType.Warning);
                }

                if (owners.Any())
                {
                    string errorMessage = "The owner of this scene is already registered.";
                    EditorGUILayout.HelpBox(errorMessage, MessageType.Info);
                }
            }

            CustomUI.HorizontalLine(position.width);

            isOpenOwnerManagement = CustomUI.Foldout(isOpenOwnerManagement, "Owner Management");
            if (isOpenOwnerManagement)
            {
                foreach (var owner in owners)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUI.BeginDisabledGroup(true);
                    GameObject ownerDisp = EditorGUILayout.ObjectField(owner.gameObject, typeof(GameObject), true) as GameObject;
                    EditorGUILayout.Space(5);
                    LightProbes lightProbeDataAsset = EditorGUILayout.ObjectField(owner.Lightprobes, typeof(LightProbes), true) as LightProbes;
                    EditorGUI.EndDisabledGroup();

                    owner.MergeType = (LightProbeMergeType)EditorGUILayout.EnumPopup(owner.MergeType);

                    EditorGUI.BeginDisabledGroup(lightProbeDataAsset == null);
                    if (GUILayout.Button(new GUIContent("Activate Probe", "Use generated probes.")))
                    {
                        OnClickActivateButton(owner);
                    }
                    EditorGUI.EndDisabledGroup();

                    if (GUILayout.Button(new GUIContent("Unsubscribe", "Remove from Owner Object."), GUILayout.Width(150)))
                    {
                        OnClickUnsubscribeButton(owner.gameObject);
                    }

                    EditorGUILayout.EndHorizontal();

                    if (lightProbeDataAsset == null)
                    {
                        var errorMessage = owner.gameObject.name + " does not have a light probe.\nPlease configure in Inspector or unsubscribe.";
                        EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                    }
                }

                if (owners.Count > 1)
                {
                    string errorMessage = "There are " + owners.Count + " owners in the scene. There must be only one.";
                    EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                }
            }

            isOpenOthers = CustomUI.Foldout(isOpenOthers, "Others");
            if (isOpenOthers)
            {
                var unusedProbes = LightProbeBinderProcessor.GetUnusedProbes();
                if(unusedProbes != null)
				{
                    foreach (var probe in unusedProbes)
					{
                        EditorGUILayout.BeginHorizontal();

                        EditorGUI.BeginDisabledGroup(true);
                        GameObject unusedProbeDisp = EditorGUILayout.ObjectField(probe, typeof(GameObject), true) as GameObject;
                        EditorGUI.EndDisabledGroup();

                        if (GUILayout.Button(new GUIContent("Delete", "Delete file : " + probe), GUILayout.Width(100)))
                        {
                            var probePath = AssetDatabase.GetAssetPath(probe);
                            AssetDatabase.DeleteAsset(probePath);
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    if (unusedProbes.Count > 1)
                    {
                        string warningMessage = "Unused light probes found.";
                        EditorGUILayout.HelpBox(warningMessage, MessageType.Warning);
                    }
                }
            }
        }

        private static void OnClickActivateButton(LightProbeRestorer restorer)
        {
            LightmapSettings.lightProbes = restorer.Lightprobes;
        }

        private static void OnClickSubscribeButton(GameObject target)
        {
            LightProbeBinderProcessor.ExecuteStore(target);
        }

        private static void OnClickUnsubscribeButton(GameObject target)
        {
            LightProbeBinderProcessor.ExecuteClear(target);
        }
    }

    public static class CustomUI
    {
        public static bool Foldout(bool display, string title)
        {
            var style = new GUIStyle();
            style.font = new GUIStyle(EditorStyles.label).font;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 12;
            style.contentOffset = new Vector2(18.0f, 1.0f);

            var rect = GUILayoutUtility.GetRect(16f, 22f, style);
            GUI.Box(rect, title, style);

            var e = Event.current;

            var toggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
            if (e.type == EventType.Repaint)
            {
                EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
            }

            if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
            {
                display = !display;
                e.Use();
            }

            return display;
        }

        public static void HorizontalLine(float width)
		{
            var splitterRect = EditorGUILayout.GetControlRect(false, GUILayout.Height(1));
            splitterRect.x = 0;
            splitterRect.width = width;
            EditorGUI.DrawRect(splitterRect, Color.gray);
        }
    }
}
