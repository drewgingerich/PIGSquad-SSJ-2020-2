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
public class Conductor : MonoBehaviour
{
	private static readonly Note[] notes = new Note[]
	{
		Note.Whole,
		Note.Half,
		Note.Quarter,
		Note.Eighth,
		Note.Sixteenth,
		Note.Thirtysecond,
	};

	private static Dictionary<Note, double> nextTimes;
	public static Dictionary<Note, double> noteDurations;

	private int tickCount = 0;

	private void Awake()
	{
		MusicStartAnnouncer.OnStart += HandleStart;
	}

	private void HandleStart(double startTime)
	{
		MusicStartAnnouncer.OnStart -= HandleStart;

		Initialize(startTime);

		Metronome.OnTick += HandleTick;
	}

	private void Initialize(double startTime)
	{
		nextTimes = new Dictionary<Note, double>();
		noteDurations = new Dictionary<Note, double>();

		foreach (Note note in notes)
		{
			noteDurations[note] = Metronome.TickLength * (int)note;
		}

		foreach (Note note in notes)
		{
			nextTimes[note] = startTime;
		}
	}

	private void HandleTick(int count)
	{
		tickCount += count;
		tickCount %= 32;

		var nextTickTime = Metronome.NextTick;

		foreach (Note note in notes)
		{
			if (tickCount % (int)note == 0)
			{
				nextTimes[note] = nextTickTime + noteDurations[note];
			}
		}
	}

	public static double GetNextNote(Note note, int offset = 0)
	{
		var time = nextTimes[note];
		// Debug.Log(time);
		return time + noteDurations[note] * offset;
	}
}
