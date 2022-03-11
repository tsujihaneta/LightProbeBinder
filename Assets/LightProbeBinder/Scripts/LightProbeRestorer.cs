using UnityEngine;
using System.Collections;

namespace TsujihaTools.LightProbeBinder
{
	[ExecuteAlways]
	public class LightProbeRestorer : MonoBehaviour
	{
		[SerializeField] private LightProbes lightprobes = null;
		public LightProbes Lightprobes {
			get {
				return lightprobes;
			}
			set {
				lightprobes = value;
			}
		}

		[SerializeField] private LightProbeMergeType mergeType = LightProbeMergeType.Overwrite;
		public LightProbeMergeType MergeType {
			get {
				return mergeType;
			}
			set {
				mergeType = value;
			}
		}

		void OnEnable()
		{
			if (lightprobes == null)
			{
				return;
			}

			Merge();
		}

		void Update()
		{
			if (LightmapSettings.lightProbes == null)
			{
				Merge();
			}
		}

		void OnDisable()
		{
			if (lightprobes == null)
			{
				return;
			}

			Purge();
		}

		public void Merge()
		{
			switch (MergeType)
			{
				case LightProbeMergeType.Overwrite:
					LightmapSettings.lightProbes = lightprobes;
					LightProbes.TetrahedralizeAsync();
					break;
				case LightProbeMergeType.Additive:
					LightProbes.TetrahedralizeAsync();
					break;
			}
		}

		public void Purge()
		{
			switch (MergeType)
			{
				case LightProbeMergeType.Overwrite:
					LightmapSettings.lightProbes = null;
					LightProbes.TetrahedralizeAsync();
					break;
				case LightProbeMergeType.Additive:
					LightProbes.TetrahedralizeAsync();
					break;
			}
		}
	}
}
