using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sequence
{
	public int LaneCount { get { return lanes.Length; } }
	public int StepCount { get { return lanes[0].Length; } }

	[SerializeField]
	private Lane[] lanes;

	public Sequence(int numLanes = 3, int numSteps = 4)
	{
		lanes = new Lane[numLanes];
		// lanes = new Lane[lanes];
		for (int i = 0; i < lanes.Length; i++)
		{
			lanes[i] = new Lane(numSteps);
		}
	}

	private Sequence(Lane[] lanes)
	{
		var numLanes = lanes.Length;
		var numSteps = lanes[0].Length;

		this.lanes = new Lane[numLanes];

		for (int i = 0; i < numLanes; i++)
		{
			this.lanes[i] = new Lane(numLanes);
			for (int j = 0; j < numSteps; j++)
			{
				this.lanes[i][j] = lanes[i][j];
			}
		}
	}

	public bool this[int lane, int step]
	{
		get { return lanes[lane][step]; }
	}

	public bool GetValue(int laneIndex, int stepIndex)
	{
		return lanes[laneIndex][stepIndex];
	}

	public Sequence Copy()
	{
		return new Sequence(lanes);
	}

	[System.Serializable]
	private struct Lane
	{
		[SerializeField]
		private bool[] steps;

		public Lane(int numSteps)
		{
			steps = new bool[numSteps];
		}

		public int Length { get { return steps.Length; } }

		public bool this[int i]
		{
			get { return steps[i]; }
			set { steps[i] = value; }
		}
	}
}
