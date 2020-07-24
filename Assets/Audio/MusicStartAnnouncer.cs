using System;
using System.Collections;
using UnityEngine;

public class MusicStartAnnouncer : MonoBehaviour
{

	public static event Action<double> OnStart;

	public float warmupTime = 0.25f;

	public Metronome metronome;

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(warmupTime);
		var startTime = metronome.NextTick;
		OnStart?.Invoke(startTime);
	}
}
