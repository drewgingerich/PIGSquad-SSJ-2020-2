using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
	public int voices = 2;
	public List<AudioClip> clips;
	public Sequence sequence;

	private int step = 0;
	private List<AudioSource> sources;
	private int sourceIndex;

	public void PlayNext(double time)
	{
		for (int lane = 0; lane < sequence.LaneCount; lane++)
		{
			if (sequence.GetValue(lane, step))
			{
				var clip = clips[lane];
				if (clip != null) Play(clip, time);
			}
		}
		step++;
		step %= sequence.StepCount;
	}

	private void Awake()
	{
		sources = new List<AudioSource>();
		for (int i = 0; i < voices; i++)
		{
			var newSrc = gameObject.AddComponent<AudioSource>();
			sources.Add(newSrc);
		}
	}

	private void Play(AudioClip clip, double time)
	{
		var src = sources[sourceIndex];
		src.Stop();
		src.clip = clip;
		src.PlayScheduled(time);
		sourceIndex++;
		sourceIndex %= voices;

	}
}
