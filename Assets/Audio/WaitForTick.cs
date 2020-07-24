using UnityEngine;

public class WaitForTick : CustomYieldInstruction
{
	private double remainingTicks;

	public override bool keepWaiting
	{
		get
		{
			return remainingTicks > 0;
		}
	}

	public WaitForTick(int tick)
	{
		remainingTicks = tick;
		remainingTicks -= Conductor.tickCount;
		Metronome.OnTick += HandleTick;

	}

	private void HandleTick(int count)
	{
		remainingTicks -= count;
		if (remainingTicks <= 0)
		{
			Metronome.OnTick -= HandleTick;
		}
	}
}
