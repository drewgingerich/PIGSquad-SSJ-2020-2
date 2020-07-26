using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterimage : MonoBehaviour
{
	[SerializeField]
	private Transform ring;
	[SerializeField]
	private float startScale = 2;
	[SerializeField]
	private float endScale = 1;

	private double startTime;
	private double currentTime;
	private double targetDiff;

	public void Run(double targetTime)
	{
		startTime = currentTime = AudioSettings.dspTime;
		targetDiff = targetTime - currentTime;
		StartCoroutine(VfxRoutine());
	}

	private IEnumerator VfxRoutine()
	{
		double diff = 0;
		while (diff < targetDiff)
		{
			diff = currentTime - startTime;
			var progress = (float)(diff / targetDiff);
			float scale = Mathf.Lerp(startScale, endScale, progress);
			ring.localScale = Vector3.one * scale;

			currentTime = AudioSettings.dspTime;

			yield return null;
		}
		Destroy(gameObject);
	}
}
