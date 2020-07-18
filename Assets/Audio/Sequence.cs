using UnityEngine;

[System.Serializable]
public struct Sequence
{
	public int LaneCount { get { return lanes.Length; } }
	public int LaneLength { get { return lanes[0].Length; } }

	[SerializeField]
	private Lane[] lanes;

	public Sequence(int laneCount, int length)
	{
		lanes = new Lane[length];
		// lanes = new Lane[lanes];
		for (int i = 0; i < laneCount; i++)
		{
			ResetLane(i);
		}
	}

	public Sequence(Lane[] lanes)
	{
		this.lanes = lanes;
	}

	public Sequence Copy()
	{
		return new Sequence(lanes);
	}

	public bool Get(int laneIndex, int stepIndex)
	{
		return lanes[laneIndex].Get(stepIndex);
	}

	public void Set(int laneIndex, int stepIndex, bool value)
	{
		lanes[laneIndex].Set(stepIndex, value);
	}

	public void SetLane(int index, Lane lane)
	{
		Debug.Assert(lane.Length == LaneLength);
		lanes[index] = lane;
	}

	public void ResetLane(int index)
	{
		SetLane(index, new Lane(LaneLength));
	}
}

[System.Serializable]
public struct Lane
{
	public int Length { get; private set; }

	[SerializeField]
	private bool[] steps;

	public Lane(int length)
	{
		this.Length = length;
		steps = new bool[length];
		Clear();
	}

	public Lane(bool[] steps)
	{
		this.Length = steps.Length;
		this.steps = steps;
	}

	public bool Get(int index)
	{
		return steps[index];
	}

	public void Set(int index, bool value)
	{
		steps[index] = value;
	}

	public void Clear()
	{
		for (int i = 0; i < Length; i++)
		{
			steps[i] = false;
		}
	}
}
