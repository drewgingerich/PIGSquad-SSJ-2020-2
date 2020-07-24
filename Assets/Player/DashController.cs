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

	public IEnumerator ActivateRoutine(Vector3 start, Vector2 direction, float distance)
	{
		var spawnPosition = start;
		var positionChange = (Vector3)direction * distance / (afterImageCount - 1);
		for (int i = 0; i < afterImageCount; i++)
		{
			CreateAfterimage(spawnPosition);
			spawnPosition += positionChange;
			yield return new WaitForNote(Note.Thirtysecond);
		}
	}

	public void CreateAfterimage(Vector3 position)
	{
		Instantiate(afterImagePrefab, position, Quaternion.identity);
	}
}
