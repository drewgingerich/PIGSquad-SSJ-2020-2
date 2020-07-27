using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuedPlayer : MonoBehaviour
{
	[SerializeField]
	Note quantization;
	[SerializeField]
	AudioSource src;

	bool queued = false;

	public void Play()
	{
		if (queued) return;
		StartCoroutine(PlayRoutine());
	}

	IEnumerator PlayRoutine()
	{
		var playTime = Conductor.GetNextNote(quantization);
		src.PlayScheduled(playTime);
		yield return new WaitForDspTime(playTime);
	}
}
