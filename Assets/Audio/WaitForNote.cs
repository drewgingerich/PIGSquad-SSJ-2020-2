using UnityEngine;

public class WaitForNote : CustomYieldInstruction
{
	private Note noteType;
	private double targetTime;
	private int offset;

	public override bool keepWaiting
	{
		get
		{
			return targetTime < AudioSettings.dspTime;
		}
	}

	public WaitForNote(Note noteType, int offset = 0)
	{
		this.noteType = noteType;
		targetTime = NoteTracker.GetNextNoteTime(noteType, offset);
	}
}
