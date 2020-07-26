using System;
using System.Collections;
using UnityEngine;

public class MusicStartAnnouncer : MonoBehaviour
{
	public static event Action<double> OnStartEarly;
	public static event Action<double> OnStart;
	public static event Action<double> OnStartLate;

	public float warmupTime = 0.25f;

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(warmupTime);
		var startTime = Metronome.NextTick;
		OnStartEarly?.Invoke(startTime);
		OnStart?.Invoke(startTime);
		OnStartLate?.Invoke(startTime);
	}
}
