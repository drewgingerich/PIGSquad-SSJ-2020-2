using UnityEngine;

public class WaitForNoteLength : CustomYieldInstruction
{
	private double targetTime;

	public override bool keepWaiting
	{
		get
		{
			return targetTime > AudioSettings.dspTime;
		}
	}

	public WaitForNoteLength(Note note, int offset = 0)
	{
		targetTime = Conductor.CalcNoteTime(note, offset);
	}
}
