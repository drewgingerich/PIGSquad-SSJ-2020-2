using System;
using System.Collections;
using UnityEngine;

public class WaitForWarmup : MonoBehaviour
{

	public static event Action<double> OnWarmedUp;

	public float warmupTime = 1f;

	public Metronome metronome;

	// Start is called before the first frame update
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(warmupTime);
		var startTime = metronome.NextTick + metronome.TickLength * 3;
		OnWarmedUp?.Invoke(startTime);
	}
}
