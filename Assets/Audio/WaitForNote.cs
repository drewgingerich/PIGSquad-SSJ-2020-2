using UnityEngine;

public class WaitForNote : CustomYieldInstruction
{
	private double targetTime;

	public override bool keepWaiting
	{
		get
		{
			return targetTime > AudioSettings.dspTime;
		}
	}

	public WaitForNote(Note noteType, int offset = 0)
	{
		targetTime = NoteTracker.GetNextNoteTime(noteType, offset);
	}
}
