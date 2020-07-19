using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Sequence))]
public class SequenceDrawer : PropertyDrawer
{
	const int rowHeight = 25;
	const int smallWidth = 25;
	const int medWidth = 50;

	Rect pos;

	float x;
	float y;

	SerializedProperty prop;
	int length;
	// SerializedProperty lanes;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		var lanes = property.FindPropertyRelative("lanes");
		int rows = lanes.arraySize + 1;
		float height = rows * (rowHeight + 5);
		return height;
	}

	private void LayoutInitialize(Rect pos)
	{
		this.pos = pos;
		x = pos.x;
		y = pos.y;
	}

	private Rect LayoutNextRect(int width)
	{
		var rect = new Rect(x, y, width, rowHeight);
		x += width;
		return rect;
	}

	private void LayoutNextRow()
	{
		x = pos.x;
		y += rowHeight + 5;
	}

	private SerializedProperty GetLanesProp()
	{
		return prop.FindPropertyRelative("lanes");
	}

	private SerializedProperty GetStepsProp(int laneIndex)
	{
		var lanes = GetLanesProp();
		var lane = lanes.GetArrayElementAtIndex(laneIndex);
		return lane.FindPropertyRelative("steps");
	}

	private void FindLength()
	{
		length = 1;
		var lanes = GetLanesProp();
		if (lanes.arraySize == 0) return;
		length = GetStepsProp(0).arraySize;
		if (length == 0) length = 1;
	}

	private void InsertLane()
	{
		var lanes = prop.FindPropertyRelative("lanes");
		lanes.InsertArrayElementAtIndex(lanes.arraySize);
	}

	private void RemoveLane()
	{
		var lanes = GetLanesProp();
		if (lanes.arraySize == 1) return;
		lanes.DeleteArrayElementAtIndex(lanes.arraySize - 1);
	}

	private void AddLength()
	{
		var lanes = GetLanesProp();
		for (int i = 0; i < lanes.arraySize; i++)
		{
			var steps = GetStepsProp(i);
			steps.InsertArrayElementAtIndex(steps.arraySize);
		}
	}

	private void RemoveLength()
	{
		var lanes = GetLanesProp();
		for (int i = 0; i < lanes.arraySize; i++)
		{
			var steps = GetStepsProp(i);
			if (steps.arraySize == 1) continue;
			steps.DeleteArrayElementAtIndex(steps.arraySize - 1);
		}
	}

	public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
	{
		prop = property;

		FindLength();

		var lanes = GetLanesProp();
		if (lanes.arraySize == 0)
		{
			InsertLane();
		}

		LayoutInitialize(pos);

		GUI.Label(LayoutNextRect(40), "Lanes");
		if (GUI.Button(LayoutNextRect(25), "-"))
		{
			RemoveLane();
		}
		if (GUI.Button(LayoutNextRect(25), "+"))
		{
			InsertLane();
		}

		LayoutNextRect(25);

		GUI.Label(LayoutNextRect(45), "Length");
		if (GUI.Button(LayoutNextRect(25), "-"))
		{
			RemoveLength();
		}
		if (GUI.Button(LayoutNextRect(25), "+"))
		{
			AddLength();
		}

		for (int i = 0; i < lanes.arraySize; i++)
		{
			LayoutNextRow();
			DrawLane(i);
		}
	}

	private void DrawLane(int index)
	{
		GUI.Label(LayoutNextRect(40), index.ToString());


		var steps = GetStepsProp(index);
		for (int i = 0; i < steps.arraySize; i++)
		{
			var step = steps.GetArrayElementAtIndex(i);
			var on = DrawToggle(step);
			step.boolValue = on;
		}
	}

	private bool DrawToggle(SerializedProperty step)
	{
		return EditorGUI.Toggle(LayoutNextRect(25), step.boolValue);
	}
}
