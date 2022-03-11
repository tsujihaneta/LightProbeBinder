using UnityEditor;
using UnityEngine;

namespace TsujihaTools.LightProbeBinder
{
    [FilePath("ProjectSettings/LightProbeBinder.asset", FilePathAttribute.Location.ProjectFolder)]
    public class LightProbeBinderSettings : ScriptableSingleton<LightProbeBinderSettings>
    {
        [Tooltip("Automatically open Light Probe Binder Window after the lightbake.")]
        public bool autoOpenWindow = false;
        [Tooltip("Automatically update owner's light probe after lightbake.")]
        public bool autoUpdate = false;
        [Tooltip("Disable popup when process is completed.")]
        public bool disableDialog = false;

        public void Save()
        {
            Save(true);
        }
    }
}
