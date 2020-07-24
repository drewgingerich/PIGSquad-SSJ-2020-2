﻿using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Metronome : MonoBehaviour
{
	public static event Action<int> OnTick;

	public double bpm = 120f;

	const double secPerMin = 60f;
	const double ticksPerBeat = 8f;

	public static double TickLength { get; private set; }
	public static double NextTick { get; private set; }

	void Awake()
	{
		TickLength = 1 / bpm * secPerMin / ticksPerBeat;
		NextTick = AudioSettings.dspTime + TickLength;
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		double diff = (AudioSettings.dspTime - NextTick);
		var tickCount = (int)(diff / TickLength);
		if (tickCount > 0)
		{
			var remainder = diff % TickLength;
			OnTick?.Invoke(tickCount);
			NextTick = AudioSettings.dspTime - remainder + TickLength;
		}
	}
}