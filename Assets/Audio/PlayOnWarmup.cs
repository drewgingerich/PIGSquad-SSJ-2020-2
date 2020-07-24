using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayOnWarmup : MonoBehaviour
{
	private AudioSource src;

	private void Start()
	{
		src = GetComponent<AudioSource>();
		MusicStartAnnouncer.OnStart += StartAudio;
	}

	private void StartAudio(double startTime)
	{
		src.PlayScheduled(startTime);
		MusicStartAnnouncer.OnStart -= StartAudio;
	}
}
