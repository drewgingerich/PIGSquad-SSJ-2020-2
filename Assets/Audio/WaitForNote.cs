﻿using UnityEngine;

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

	public WaitForNote(Note note, int offset = 0)
	{
		targetTime = Conductor.GetNextNote(note, offset);
	}
}
