using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Linq;

namespace TsujihaTools.LightProbeBinder
{
    public class LightprobeBinderWindow : EditorWindow
    {
        public static readonly string LightprobeAssetName = "Lightprobe.asset";

        private static GameObject owner = null;
        private static LightProbes lightProbeDataAsset = null;
        private static bool isWarningMultipleLoader = false;
        private static bool isOpenStatus = true;
        private static bool isOpenSceneSettings = true;


        [MenuItem("Tool/Rendering/LightprobeBinder")]
        private static void Open()
        {
            GetWindow<LightprobeBinderWindow>("LightprobeBinder");

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
            SetLightProbeDataAsset();
            SetOwnerAuto();
        }

        private void OnGUI()
        {
            minSize = new Vector2(400, 160);

            isOpenStatus = CustomUI.Foldout(isOpenStatus, "Status");
            if (isOpenStatus)
            {
                EditorGUILayout.LabelField("Active Scene : " + SceneManager.GetActiveScene().name);

                EditorGUI.BeginDisabledGroup(true);
                LightProbes activeProbeDataAsset = EditorGUILayout.ObjectField(new GUIContent("Active Light Probe"), LightmapSettings.lightProbes, typeof(LightProbes), true) as LightProbes;
                EditorGUI.EndDisabledGroup();

                if (LightmapSettings.lightProbes == null)
                {
                    var lightProbeRestorers = FindObjectsOfType<LightProbeGroup>()
                        .Where(obj => obj.gameObject.scene == SceneManager.GetActiveScene())
                        .ToList();

                    string warningMessage = "LightmapSettings.lightProbes does not exist in the scene.";
                    warningMessage += "\nPlease";
                    if (!lightProbeRestorers.Any())
                    {
                        warningMessage += " add LightProbeGroup and";
                    }
                    warningMessage += " generate lighting.";

                    EditorGUILayout.HelpBox(warningMessage, MessageType.Warning);
                }
            }

            var splitterRect = EditorGUILayout.GetControlRect(false, GUILayout.Height(1));
            splitterRect.x = 0;
            splitterRect.width = position.width;
            EditorGUI.DrawRect(splitterRect, Color.gray);

            isOpenSceneSettings = CustomUI.Foldout(isOpenSceneSettings, "SceneSettings");
            if (isOpenSceneSettings)
            {
                owner = EditorGUILayout.ObjectField(new GUIContent("Owner Object", "When this object is activated, it replaces the LightProbes in the scene."), owner, typeof(GameObject), true) as GameObject;

                var lightProbeRestorers = FindObjectsOfType<LightProbeRestorer>()
                    .Where(obj => obj.gameObject.scene == SceneManager.GetActiveScene())
                    .ToList();

                if (lightProbeRestorers.Count > 1)
                {
                    string warningMessage = "There are " + lightProbeRestorers.Count + " LightProbeRestorer components in the scene.";
                    foreach (var item in lightProbeRestorers)
                    {
                        warningMessage += "\n";
                        warningMessage += item.gameObject.name;
                    }

                    EditorGUILayout.HelpBox(warningMessage, MessageType.Warning);

                    isWarningMultipleLoader = true;
                }
                else
                {
                    if (isWarningMultipleLoader)
                    {
                        SetOwnerAuto();
                        isWarningMultipleLoader = false;
                    }
                }

                EditorGUI.BeginDisabledGroup(true);
                lightProbeDataAsset = EditorGUILayout.ObjectField(new GUIContent("Light Probe"), lightProbeDataAsset, typeof(LightProbes), true) as LightProbes;
                EditorGUI.EndDisabledGroup();
            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("Store Probe", "Generate LightProbe asset and add LightProbeRestorer component to Owner Object."), GUILayout.Width(150), GUILayout.Height(20)))
            {
                OnClickStoreButton();
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void OnClickStoreButton()
		{
            ExecuteStore();
        }

        private static void ExecuteStore()
        {
            LightProbes lightProbes;
            if (!TryGenerateLightProbeAsset(out lightProbes))
            {
                return;
            }

            lightProbeDataAsset = lightProbes;

            if (owner == null)
            {
                return;
            }

            var lightProbeRestorer = owner.GetComponent<LightProbeRestorer>();
            if (lightProbeRestorer == null)
            {
                lightProbeRestorer = owner.AddComponent<LightProbeRestorer>();
            }

            lightProbeRestorer.Lightprobes = lightProbes;
        }

        private static void SetLightProbeDataAsset()
        {
            string assetFilePath = GetLightprobePath();
            lightProbeDataAsset = AssetDatabase.LoadAssetAtPath(assetFilePath, typeof(LightProbes)) as LightProbes;
        }

        private static void SetOwnerAuto()
        {
            var lightProbeRestorers = FindObjectsOfType<LightProbeRestorer>()
                .Where(obj => obj.gameObject.scene == SceneManager.GetActiveScene())
                .ToList();

            if (lightProbeRestorers.Any())
            {
                owner = lightProbeRestorers.FirstOrDefault().gameObject;
                return;
            }

            Scene scene = SceneManager.GetActiveScene();
            GameObject[] rootObjects = scene.GetRootGameObjects();
            if (rootObjects.Any())
            {
                owner = rootObjects.FirstOrDefault();
                return;
            }
        }

        private static bool TryGenerateLightProbeAsset(out LightProbes _lightProbes)
        {
            CreateSceneDirectory();

            string assetFilePath = GetLightprobePath();

            if(LightmapSettings.lightProbes == null)
			{
                _lightProbes = null;
                return false;
			}

            LightProbes lightProbes = GameObject.Instantiate(LightmapSettings.lightProbes);
            if (lightProbes == null)
            {
                _lightProbes = null;
                return false;
            }

            AssetDatabase.CreateAsset(lightProbes, assetFilePath);
            Debug.Log("LightProbe file generated.\n" + assetFilePath);

            _lightProbes = lightProbes;
            return true;
        }

        private static void CreateSceneDirectory()
        {
            Scene scene = SceneManager.GetActiveScene();
            string path = Path.Combine(Path.GetDirectoryName(scene.path), Path.GetFileNameWithoutExtension(scene.name));
            Directory.CreateDirectory(path);
        }

        private static string GetLightprobePath()
        {
            Scene scene = SceneManager.GetActiveScene();
            if(scene == null)
			{
                return null;
			}

            string path = Path.Combine(Path.GetDirectoryName(scene.path), Path.GetFileNameWithoutExtension(scene.path), LightprobeAssetName);

            return path;
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
    }
}
