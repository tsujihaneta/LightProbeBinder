using UnityEngine;
using System.Collections;

namespace TsujihaTools.LightProbeBinder
{
	[ExecuteAlways]
	public class LightProbeRestorer : MonoBehaviour
	{
		[SerializeField] private LightProbes lightprobes;
		public LightProbes Lightprobes {
			get {
				return lightprobes;
			}
			set {
				lightprobes = value;
			}
		}

		void OnEnable()
		{
			if (lightprobes == null)
			{
				return;
			}

			LightmapSettings.lightProbes = lightprobes;
		}

		void Update()
		{
			if (LightmapSettings.lightProbes == null)
			{
				LightmapSettings.lightProbes = lightprobes;
			}
		}

		void OnDisable()
		{
			if (lightprobes == null)
			{
				return;
			}

			LightmapSettings.lightProbes = null;
		}
	}
}
