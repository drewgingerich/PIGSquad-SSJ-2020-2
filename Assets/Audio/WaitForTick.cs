using UnityEngine;

public class WaitForTick : CustomYieldInstruction
{
	private double targetTime;

	public override bool keepWaiting
	{
		get
		{
			return targetTime > AudioSettings.dspTime;
		}
	}

	public WaitForTick(int offset, bool absolute = false)
	{
		targetTime = Conductor.GetNextTick(offset, absolute);
	}
}
