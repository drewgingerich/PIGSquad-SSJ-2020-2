using System.Collections;
using UnityEngine;

public class DashController : MonoBehaviour
{

	[SerializeField]
	private GameObject afterImagePrefab;
	[SerializeField]
	private int afterImageCount = 2;

	public void Activate(Vector2 direction, float distance)
	{
		StartCoroutine(ActivateRoutine(transform.position, direction, distance));
	}

	public IEnumerator ActivateRoutine(Vector3 start, Vector3 direction, float distance)
	{
		var spawnPosition = start;
		var afterImageSpacing = distance / (afterImageCount - 1);
		// Making afterimage spawn a bit ahead looks cooler!
		spawnPosition += direction * afterImageSpacing * 0.4f;
		for (int i = 0; i < afterImageCount; i++)
		{
			CreateAfterimage(spawnPosition);
			spawnPosition += direction * afterImageSpacing;
			yield return new WaitForNote(Note.Thirtysecond);
		}
	}

	public void CreateAfterimage(Vector3 position)
	{
		Instantiate(afterImagePrefab, position, Quaternion.identity);
	}
}
