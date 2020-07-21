using UnityEngine;

public class MuteSourceDriver : MonoBehaviour
{
	[SerializeField]
	private AudioSource source;
	[SerializeField]
	private Metronome metronome;


	public void Play()
	{
		source.PlayScheduled(metronome.NextTick);
	}

	public void Stop()
	{
		source.SetScheduledEndTime(metronome.NextTick);
	}
}
