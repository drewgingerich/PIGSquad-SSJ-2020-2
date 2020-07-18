using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
	public List<AudioClip> clips;
	public AudioSource[] srcs;
	public Metronome metronome;
	public int ticks = 8;

	private int counter = 0;
	private int srcIndex = 0;
	private int index = 0;

	bool play = false;

	private void Awake()
	{
		metronome.OnTick += HandleTick;
		srcs = GetComponents<AudioSource>();
	}

	private void HandleTick()
	{
		counter++;
		counter %= ticks;
		if (counter == 0)
		{
			play = true;
		}
	}

	private void Update()
	{
		if (play)
		{
			play = false;
			// src.clip = clips[index];
			// index++;
			// index %= clips.Count;
			double scheduledTime = metronome.nextTick + metronome.tickLength;
			srcs[srcIndex].PlayScheduled(scheduledTime);
			srcIndex++;
			srcIndex %= srcs.Length;
			// src.SetScheduledEndTime(scheduledTime + ticks * metronome.tickLength - 0.0005f);
		}
	}
}
