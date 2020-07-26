using System.Collections;
using UnityEngine;

public class PlayerSfx : MonoBehaviour
{
	[SerializeField]
	private AudioSource moveSfx;
	[SerializeField]
	private AudioSource dashSfx;

	private static PlayerSfx inst;

	private static bool playingMoveSfx = false;

	private static bool playingDashSfx = false;
	private static Coroutine endDashRoutine;

	public static void PlayDashSfx(double start, double end)
	{
		if (endDashRoutine != null) inst.StopCoroutine(endDashRoutine);
		playingDashSfx = true;

		inst.dashSfx.PlayScheduled(start);
		inst.dashSfx.SetScheduledEndTime(end);

		endDashRoutine = inst.StartCoroutine(inst.WaitForEndDashSfx(end));
	}

	private IEnumerator WaitForEndDashSfx(double end)
	{
		yield return new WaitForDspTime(end);
		playingDashSfx = false;
	}

	public static void StartMoveSfx()
	{
		if (!playingMoveSfx)
		{
			inst.moveSfx.PlayScheduled(Conductor.GetNextNote(Note.Eighth));
			playingMoveSfx = true;
		}
	}

	public static void StopMoveSfx()
	{
		playingMoveSfx = false;
		inst.moveSfx.SetScheduledEndTime(Conductor.GetNextNote(Note.Eighth));
	}

	private void Awake()
	{
		Debug.Assert(inst == null);
		inst = this;
	}
}
