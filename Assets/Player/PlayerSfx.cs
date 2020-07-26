using System.Collections;
using UnityEngine;

public class PlayerSfx : MonoBehaviour
{
	[SerializeField]
	private AudioSource moveSfx;
	[SerializeField]
	private AudioSource dashSfx;

	private static PlayerSfx inst;

	private static bool move = false;
	private static bool dash = false;
	private static Coroutine endDashRoutine;

	// Start is called before the first frame update
	public static void PlayDashSfx()
	{
		dash = true;
		var startTime = Conductor.GetNextNote(Note.Thirtysecond);
		var endTime = startTime + Conductor.noteDurations[Note.Eighth];
		inst.dashSfx.PlayScheduled(startTime);
		inst.dashSfx.SetScheduledEndTime(endTime);
		endDashRoutine = inst.StartCoroutine(inst.WaitForEndDashSfx(endTime));
	}

	private IEnumerator WaitForEndDashSfx(double end)
	{
		yield return new WaitForDspTime(end);
		dash = false;
	}

	public static void StartMoveSfx()
	{

		move = true;
		inst.moveSfx.PlayScheduled(Conductor.GetNextNote(Note.Eighth));
	}

	public static void StopMoveSfx()
	{
		move = false;
		inst.moveSfx.SetScheduledEndTime(Conductor.GetNextNote(Note.Eighth));
	}

	private void Awake()
	{
		Debug.Assert(inst == null);
		inst = this;
	}
}
