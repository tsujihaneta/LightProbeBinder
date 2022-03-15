using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TsujihaTools.LightProbeBinder
{
    public class LightProbeBinderProcessor : Editor
    {
        public static readonly string LightprobeAssetName = "Lightprobe";
        public static readonly string LightprobeAssetExtension = ".asset";
        public static readonly string LogPrefix = "[Light Probe Binder] ";

        public static void OnBakeStarted()
        {
        }

        public static void OnBakeCompleted()
		{
            if(LightProbeBinderSettings.instance != null)
			{
				if (LightProbeBinderSettings.instance.autoOpenWindow)
				{
                    LightProbeBinderWindow.Open();
                }

                if (LightProbeBinderSettings.instance.autoUpdate)
                {
                    OnAutoExecute();
                }
            }
        }

        public static void OnAutoExecute()
		{
            var owners = GetOwners();
			if (!owners.Any())
			{
                // No owner exists for the scene.
                return;
			}

            if (owners.Count > 1)
            {
                Debug.LogError(LogPrefix + "[Light Probe Binder] The automatic update process has been stopped. There are multiple owners in the scene.");
                return;
            }

            var target = owners.First();
            if (!target.isActiveAndEnabled)
            {
                Debug.LogError(LogPrefix + "The automatic update process has been stopped. There is no active owner in the scene.");
                return;
            }

            Debug.Log("[Light Probe Binder] Perform automatic update process.");

            ExecuteStore(target.gameObject);
        }

        public static List<LightProbes> GetUnusedProbes()
        {
            string directory = GetSceneDirectoryPath();
            if (string.IsNullOrEmpty(directory))
            {
                return null;
            }

            var assets = GetAllAssets<LightProbes>(directory);
            var owners = GetOwners();

            var result = new List<LightProbes>();
            foreach(var asset in assets)
			{
                bool isUsed = false;

                foreach (var owner in owners)
                {
                    if(owner.Lightprobes == asset)
					{
                        isUsed = true;
                        break;
                    }
                }

				if (!isUsed)
				{
                    result.Add(asset);
				}
            }

            if(!result.Any())
            {
                return null;
            }

            return result;
        }

        public static List<LightProbeRestorer> GetOwners()
		{
            var lightProbeRestorers = FindObjectsOfType<LightProbeRestorer>()
                .Where(obj => obj.gameObject.scene == SceneManager.GetActiveScene())
                .ToList();

            return lightProbeRestorers;
        }

        public static bool ExistAsset<T>(string directory) where T : UnityEngine.Object
        {
            if (!Directory.Exists(directory))
            {
                return false;
            }

            var assets = GetAllAssets<T>(directory);
            if (assets.Any())
			{
                return true;
			}

            return false;
        }

        public static List<T> GetAllAssets<T>(string directory) where T : UnityEngine.Object
        {
            if (!Directory.Exists(directory))
            {
                return null;
            }

            return EnumerateAssets<T>(directory).ToList();
        }

        public static IEnumerable<T> EnumerateAssets<T>(string directory) where T : UnityEngine.Object
        {
            var files = Directory.GetFiles(directory, "*.asset", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (IsAssetType<T>(file))
                {
                    yield return AssetDatabase.LoadAssetAtPath<T>(file);
                }
            }
        }

        public static bool IsAssetType<T>(string file) where T : UnityEngine.Object
        {
            var extension = Path.GetExtension(file);
            if (!string.Equals(extension, LightprobeAssetExtension))
            {
                return false;
            }

            var data = AssetDatabase.LoadAssetAtPath<T>(file);
            if (data == null)
            {
                return false;
            }

            return true;
        }

        public static void ExecuteStore(GameObject root)
        {
            // コンポーネントを生成.
            var restorer = root.GetComponent<LightProbeRestorer>();
            if (restorer == null)
            {
                restorer = root.AddComponent<LightProbeRestorer>();
            }

            // アセットを生成.
            LightProbes lightProbes;
            if (!TryGenerateLightProbeAsset(out lightProbes))
            {
                return;
            }
            restorer.Lightprobes = lightProbes;

            EditorUtility.SetDirty(root);

            var message = "LightProbe data is Stored." + "\n" + root.name;

            if (!LightProbeBinderSettings.instance.disableDialog)
            {
                if (EditorUtility.DisplayDialog("Completed", message, "OK"))
                {

                }
            }

            Debug.Log(LogPrefix + message);
        }

        public static void ExecuteClear(GameObject root)
        {
            var restorer = root.GetComponent<LightProbeRestorer>();
            if (restorer == null)
            {
                return;
            }

            // Assetを削除.
            LightProbes lightProbes = restorer.Lightprobes;
            var assetPath = AssetDatabase.GetAssetPath(lightProbes);
            AssetDatabase.DeleteAsset(assetPath);
            restorer.Lightprobes = null;

            // コンポーネントを削除.
            GameObject.DestroyImmediate(restorer);

            EditorUtility.SetDirty(root);

            var message = "LightProbe data is Cleared." + "\n" + root.name;

            if (!LightProbeBinderSettings.instance.disableDialog)
            {
                if (EditorUtility.DisplayDialog("Completed", message, "OK"))
                {

                }
            }

            Debug.Log(LogPrefix + message);
        }

        private static bool TryGenerateLightProbeAsset(out LightProbes _lightProbes)
        {
            CreateSceneDirectory();

            string assetFilePath = GetLightprobeFilePath();

            if (LightmapSettings.lightProbes == null)
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
            Debug.Log(LogPrefix + "LightProbe file is generated.\n" + assetFilePath);

            _lightProbes = lightProbes;
            return true;
        }

        private static void CreateSceneDirectory()
        {
            string directory = GetSceneDirectoryPath();
            if (string.IsNullOrEmpty(directory))
            {
                return;
            }

            Directory.CreateDirectory(directory);
        }

        public static string GetSceneDirectoryPath()
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene == null)
            {
                return null;
            }

            string directory = Path.Combine(Path.GetDirectoryName(scene.path), Path.GetFileNameWithoutExtension(scene.path));

            return directory;
        }

        public static string GetLightprobeFilePath()
        {
            string directory = GetSceneDirectoryPath();
			if (string.IsNullOrEmpty(directory))
			{
                return null;
			}

            string file = Path.Combine(directory, LightprobeAssetName + LightprobeAssetExtension);

            return file;
        }
    }
}
