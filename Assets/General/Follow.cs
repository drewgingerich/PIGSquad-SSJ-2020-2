using UnityEngine;

public class Follow : MonoBehaviour
{
	[Range(0, 100)]
	public float stiffness = 10f;
	public Transform target;
	public Vector3 scale = new Vector3(1, 1, 0);

	const float stiffnessScaler = 0.01f;

	void Update()
	{
		Vector3 diff = target.position - transform.position;
		Vector3 scaledDiff = Vector3.Scale(diff, scale);
		transform.Translate(scaledDiff * stiffness * stiffnessScaler);
	}
}
