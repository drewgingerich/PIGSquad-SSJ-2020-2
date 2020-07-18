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

	private void FindLength()
	{
		var lanes = prop.FindPropertyRelative("lanes");
		if (lanes.arraySize == 0)
		{
			length = 4;
			return;
		}

		var someLane = lanes.GetArrayElementAtIndex(0);
		var someSteps = someLane.FindPropertyRelative("steps");
		length = someSteps.arraySize;
		if (length == 0) length = 1;
	}

	private void InsertLane(int index = 0)
	{
		var lanes = prop.FindPropertyRelative("lanes");
		lanes.InsertArrayElementAtIndex(index);
	}

	private void RemoveLane()
	{
		var lanes = prop.FindPropertyRelative("lanes");
		if (lanes.arraySize == 1) return;
		lanes.DeleteArrayElementAtIndex(lanes.arraySize - 1);
	}

	private void AddLength()
	{
		var lanes = prop.FindPropertyRelative("lanes");
		for (int i = 0; i < lanes.arraySize; i++)
		{
			var lane = lanes.GetArrayElementAtIndex(i);
			var steps = lane.FindPropertyRelative("steps");

			var insertIndex = steps.arraySize - 1;

			steps.InsertArrayElementAtIndex(0);
		}
	}

	private void RemoveLength()
	{
		var lanes = prop.FindPropertyRelative("lanes");
		for (int i = 0; i < lanes.arraySize; i++)
		{
			var lane = lanes.GetArrayElementAtIndex(i);
			var steps = lane.FindPropertyRelative("steps");

			if (steps.arraySize == 1) continue;

			var index = steps.arraySize - 1;
			steps.DeleteArrayElementAtIndex(index);
		}
	}

	public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
	{
		prop = property;

		FindLength();

		var lanes = property.FindPropertyRelative("lanes");
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
			InsertLane(lanes.arraySize - 1);
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
		var lane = prop.FindPropertyRelative("lanes").GetArrayElementAtIndex(index);
		var steps = lane.FindPropertyRelative("steps");
		GUI.Label(LayoutNextRect(40), index.ToString());
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
