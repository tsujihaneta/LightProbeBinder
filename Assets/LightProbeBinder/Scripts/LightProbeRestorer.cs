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
			//LightProbes.TetrahedralizeAsync();
		}

		void Update()
		{
			if (LightmapSettings.lightProbes == null)
			{
				LightmapSettings.lightProbes = lightprobes;
				//LightProbes.TetrahedralizeAsync();
			}
		}

		void OnDisable()
		{
			if (lightprobes == null)
			{
				return;
			}

			LightmapSettings.lightProbes = null;
			//LightProbes.TetrahedralizeAsync();
		}
	}
}
