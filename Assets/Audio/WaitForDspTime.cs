using UnityEngine;

public class WaitForDspTime : CustomYieldInstruction
{
	private double targetTime;

	public override bool keepWaiting
	{
		get
		{
			return AudioSettings.dspTime < targetTime;
		}
	}

	public WaitForDspTime(double time)
	{
		targetTime = time;
	}

}
