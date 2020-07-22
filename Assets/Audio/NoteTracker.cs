using System.Collections.Generic;
using UnityEngine;

public class NoteTracker : MonoBehaviour
{
	[SerializeField]
	private Metronome metronome;

	private static Dictionary<Note, double> nextNotes;
	private static Dictionary<Note, double> noteLengths;

	private int tickCount;

	private void Awake()
	{
		WaitForWarmup.OnWarmedUp += HandleWarmedUp;

		noteLengths[Note.Whole] = metronome.TickLength * 32;
		noteLengths[Note.Half] = metronome.TickLength * 16;
		noteLengths[Note.Quarter] = metronome.TickLength * 8;
		noteLengths[Note.Eighth] = metronome.TickLength * 4;
		noteLengths[Note.Sixteenth] = metronome.TickLength * 2;
		noteLengths[Note.Thirtysecond] = metronome.TickLength;
	}

	private void HandleWarmedUp(double startTime)
	{
		tickCount = -1;

		WaitForWarmup.OnWarmedUp -= HandleWarmedUp;

		metronome.OnTick += HandleTick;
	}

	private void HandleTick()
	{
		tickCount++;
		tickCount %= 32;

		var nextTickTime = metronome.NextTick;

		if (tickCount % 32 == 0) nextNotes[Note.Whole] = nextTickTime;
		if (tickCount % 16 == 0) nextNotes[Note.Half] = nextTickTime;
		if (tickCount % 8 == 0) nextNotes[Note.Quarter] = nextTickTime;
		if (tickCount % 4 == 0) nextNotes[Note.Eighth] = nextTickTime;
		if (tickCount % 2 == 0) nextNotes[Note.Sixteenth] = nextTickTime;
		nextNotes[Note.Thirtysecond] = nextTickTime;
	}

	public static double GetNextNoteTime(Note note, int offset = 0)
	{
		var time = nextNotes[note];
		return time + noteLengths[note] * offset;
	}

}
