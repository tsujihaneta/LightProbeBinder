using UnityEditor;
using UnityEngine;

namespace TsujihaTools.LightProbeBinder
{
    [InitializeOnLoad]
    class LightProbeBinderInitializer
    {
        static LightProbeBinderInitializer()
        {
            Lightmapping.bakeStarted -= OnBakeStarted;
            Lightmapping.bakeStarted += OnBakeStarted;
            Lightmapping.bakeCompleted -= OnBakeCompleted;
            Lightmapping.bakeCompleted += OnBakeCompleted;
        }

        static void OnBakeCompleted()
        {
            LightProbeBinderProcessor.OnBakeCompleted();
        }

        static void OnBakeStarted()
        {
            LightProbeBinderProcessor.OnBakeStarted();
        }
    }
}
