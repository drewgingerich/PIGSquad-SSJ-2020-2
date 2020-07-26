using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterimageVfx : MonoBehaviour
{

	[SerializeField]
	private GameObject afterImagePrefab;
	[SerializeField]
	private Note destroyDelayLength = Note.Eighth;
	[SerializeField]
	private int baseAfterimageCount = 4;

	private Vector2 direction;
	private float spawnSpacing;
	private float spawnTiming;
	private float dashTimeRemaining;
	private int afterImageCount;
	private List<GameObject> afterimages = new List<GameObject>();

	public void Run(Vector2 direction, float distance, float time, float fraction)
	{
		this.direction = direction;
		spawnTiming = time / baseAfterimageCount;
		dashTimeRemaining = time * fraction;
		afterImageCount = (int)(baseAfterimageCount * fraction);
		spawnSpacing = distance * fraction / afterImageCount;

		StartCoroutine(AfterimageRoutine());
	}

	public IEnumerator AfterimageRoutine()
	{
		yield return CreateAfterimages();
		yield return new WaitForNote(Note.Sixteenth, 1);
		yield return DestroyAfterimages();
	}

	public IEnumerator CreateAfterimages()
	{
		var spawnPosition = (Vector2)transform.position;

		while (dashTimeRemaining > 0)
		{
			var go = Instantiate(afterImagePrefab, spawnPosition, Quaternion.identity);
			afterimages.Add(go);

			spawnPosition += direction * spawnSpacing;
			dashTimeRemaining -= spawnTiming;

			yield return new WaitForSeconds(spawnTiming);
		}
	}

	public IEnumerator DestroyAfterimages()
	{
		var noteSpacing = Note.Thirtysecond;

		var start = Conductor.GetNextNote(noteSpacing);
		var end = Conductor.GetNextTick(afterimages.Count * (int)noteSpacing);
		Debug.Log("hi");
		PlayerSfx.PlayDashSfx(start, end);

		yield return new WaitForNote(noteSpacing);
		while (afterimages.Count > 0)
		{
			Destroy(afterimages[0]);
			afterimages.RemoveAt(0);
			// yield return new WaitForNote(noteSpacing);
		}

		Destroy(gameObject);
	}
}
