using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

[ExecuteAlways]
public class AssignTracks : MonoBehaviour
{
	[SerializeField] string key = string.Empty;

	void Start()
	{
		GameObject playableDirectorObj = GameObject.Find("PlayableDirector");
		if(playableDirectorObj != null)
		{
			PlayableDirector director = FindObjectOfType<PlayableDirector>();
			if(director != null)
			{
				var binding = director.playableAsset.outputs.First(c => c.streamName == key);
				director.SetGenericBinding(binding.sourceObject, gameObject);
			}
		}
	}
}
