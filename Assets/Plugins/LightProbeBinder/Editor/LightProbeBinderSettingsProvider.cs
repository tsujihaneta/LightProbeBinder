using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TsujihaTools.LightProbeBinder
{
    public class LightProbeBinderSettingsProvider : SettingsProvider
    {
        private const string SettingPath = "Project/Light Probe Binder";

        [SettingsProvider]
        public static SettingsProvider CreateSettingProvider()
        {
            return new LightProbeBinderSettingsProvider(SettingPath, SettingsScope.Project, null);
        }

        public LightProbeBinderSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(path, scopes, keywords)
        {
        }

        private Editor _editor;

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            var preferences = LightProbeBinderSettings.instance;

            preferences.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;

            Editor.CreateCachedEditor(preferences, null, ref _editor);
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUI.BeginChangeCheck();
            _editor.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                LightProbeBinderSettings.instance.Save();
            }
        }
    }
}
