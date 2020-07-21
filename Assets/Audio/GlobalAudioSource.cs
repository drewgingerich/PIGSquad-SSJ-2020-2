using System.Collections.Generic;
using UnityEngine;

public class GlobalAudioSource : MonoBehaviour
{
	public static GlobalAudioSource inst;

	public int voices = 10;

	private List<AudioSource> srcs;
	private int srcIndex = 0;

	private void Awake()
	{
		Debug.Assert(inst == null);
		inst = this;

		srcs = new List<AudioSource>();
		for (int i = 0; i < voices; i++)
		{
			var newSrc = gameObject.AddComponent<AudioSource>();
			srcs.Add(newSrc);
		}
	}

	public void PlayClip(AudioClip clip, double time)
	{
		Debug.Log(srcIndex);
		var src = srcs[srcIndex];
		src.Stop();
		src.clip = clip;
		src.PlayScheduled(time);
		srcIndex++;
		srcIndex %= voices;
	}
}
