using System.Collections;
using UnityEngine;

public class AfterimageVfx : MonoBehaviour
{

	[SerializeField]
	private GameObject afterImagePrefab;
	[SerializeField]
	private int afterImageCount = 4;
	[SerializeField]
	private Note destroyDelay = Note.Eighth;

	private Vector2 direction;
	private float distance;
	private float time;

	public void CreateAfterimages(Vector2 direction, float distance, float time)
	{
		this.direction = direction;
		this.distance = distance;
		this.time = time;
		StartCoroutine(AfterimageRoutine());
	}

	public IEnumerator AfterimageRoutine()
	{
		yield return CreateAfterimages();
		yield return new WaitForNote(destroyDelay);
		yield return DestroyAfterimages();
	}

	public IEnumerator CreateAfterimages()
	{
		var spawnPosition = (Vector2)transform.position;
		var spacing = distance / (afterImageCount - 1);
		// Making afterimage spawn a bit ahead looks cooler!
		// spawnPosition += direction * spacing;
		var pause = (float)Conductor.noteDurations[Note.Thirtysecond];
		for (int i = 0; i < afterImageCount; i++)
		{
			Instantiate(afterImagePrefab, spawnPosition, Quaternion.identity);
			spawnPosition += direction * spacing;
			yield return new WaitForSeconds(pause);
		}
	}

	public IEnumerator DestroyAfterimages()
	{
		PlayerSfx.PlayDashSfx();
		yield return new WaitForNote(Note.Thirtysecond);
	}
}
