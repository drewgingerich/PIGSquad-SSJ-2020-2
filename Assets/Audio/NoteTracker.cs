using System.Collections.Generic;
using UnityEngine;

public enum Note
{
	Whole = 32,
	Half = 16,
	Quarter = 8,
	Eighth = 4,
	Sixteenth = 2,
	Thirtysecond = 1,
}
public class NoteTracker : MonoBehaviour
{
	[SerializeField]
	private Metronome metronome;

	private static readonly Note[] notes = new Note[]
	{
		Note.Whole,
		Note.Half,
		Note.Quarter,
		Note.Eighth,
		Note.Sixteenth,
		Note.Thirtysecond,
	};
	private static readonly Dictionary<Note, int> noteTickLengths = new Dictionary<Note, int>
	{
		{Note.Whole , 32},
		{Note.Half , 16},
		{Note.Quarter , 8},
		{Note.Eighth , 4},
		{Note.Sixteenth , 2},
		{Note.Thirtysecond, 1}
	};

	private static Dictionary<Note, double> nextTimes;
	public static Dictionary<Note, double> noteDurations;

	private int tickCount = 0;

	private void Awake()
	{
		MusicStartAnnouncer.OnStart += HandleWarmedUp;
	}

	private void Initialize(double startTime)
	{
		nextTimes = new Dictionary<Note, double>();
		noteDurations = new Dictionary<Note, double>();

		foreach (Note note in notes)
		{
			noteDurations[note] = metronome.TickLength * noteTickLengths[note];
		}

		foreach (Note note in notes)
		{
			nextTimes[note] = startTime;
		}
	}

	private void HandleWarmedUp(double startTime)
	{
		MusicStartAnnouncer.OnStart -= HandleWarmedUp;

		Initialize(startTime);

		metronome.OnTick += HandleTick;
	}

	private void HandleTick()
	{
		tickCount++;
		tickCount %= 32;

		var nextTickTime = metronome.NextTick;

		foreach (Note note in notes)
		{
			if (tickCount % noteTickLengths[note] == 0)
			{
				nextTimes[note] = nextTickTime + noteDurations[note];
			}
		}
	}

	public static double GetNextNoteTime(Note note, int offset = 0)
	{
		var time = nextTimes[note];
		return time + noteDurations[note] * offset;
	}

}
