using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Metronome : MonoBehaviour
{
	public static event Action<int> OnTick;

	public double bpm = 120f;

	const double secPerMin = 60f;
	const double ticksPerBeat = 8f;

	public static double TickLength { get; private set; }
	public static double LastTick { get; private set; }
	public static double NextTick { get; private set; }
	public static long TickCount { get; private set; }

	void Awake()
	{
		TickLength = 1 / bpm * secPerMin / ticksPerBeat;
		NextTick = AudioSettings.dspTime + TickLength;
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		double diff = (AudioSettings.dspTime - NextTick);
		var tickChange = (int)(diff / TickLength) + 1;
		if (tickChange > 0)
		{
			TickCount += tickChange;
			var remainder = diff % TickLength;
			NextTick = AudioSettings.dspTime - remainder + TickLength;
			LastTick = NextTick - TickLength;
			OnTick?.Invoke(tickChange);
		}
	}
}