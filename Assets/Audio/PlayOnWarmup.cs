using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayOnWarmup : MonoBehaviour
{
	private AudioSource src;

	private void Start()
	{
		src = GetComponent<AudioSource>();
		WaitForWarmup.OnWarmedUp += StartAudio;
	}

	private void StartAudio(double startTime)
	{
		src.PlayScheduled(startTime);
		WaitForWarmup.OnWarmedUp -= StartAudio;
	}
}
