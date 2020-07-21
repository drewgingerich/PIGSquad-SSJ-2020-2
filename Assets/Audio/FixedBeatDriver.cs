using System;
using UnityEngine;

public class FixedBeatDriver : MonoBehaviour
{
	public int tickLag = 4;

	[Header("References")]
	public Metronome metronome;
	public Sequencer sequencer;

	private int tickCount = 0;
	private int playahead = 4;
	private bool playNext = true;

	private void Start()
	{
		metronome.OnTick += HandleTick;
	}

	private void Update()
	{
		if (playNext)
		{
			sequencer.PlayNext(metronome.NextTick + metronome.TickLength * playahead);
			playNext = false;
		}
	}

	private void HandleTick()
	{
		playNext = tickCount == 0;
		tickCount++;
		tickCount %= tickLag;
	}
}
