using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Metronome : MonoBehaviour
{
	public event Action OnTick;

	public double bpm = 120f;

	const double secPerMin = 60f;
	const double ticksPerBeat = 8f;

	public double TickLength { get; private set; }
	public double NextTick { get; private set; }

	void Awake()
	{
		TickLength = 1 / bpm * secPerMin / ticksPerBeat;
		NextTick = AudioSettings.dspTime + TickLength;
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		double diff = (AudioSettings.dspTime - NextTick);
		while (diff > TickLength)
		{
			diff -= TickLength;
			OnTick?.Invoke();
		}
		NextTick = AudioSettings.dspTime - diff + TickLength;
	}
}