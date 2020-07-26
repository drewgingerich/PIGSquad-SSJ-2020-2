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
	private float spawnTimeSpacing;
	private float endTime;
	private int afterimageCount;
	private List<Afterimage> afterimages = new List<Afterimage>();

	public void Run(Vector2 direction, float distance, float time, float fraction)
	{
		this.direction = direction;
		spawnTimeSpacing = time / baseAfterimageCount;
		endTime = time * fraction;
		afterimageCount = (int)(baseAfterimageCount * fraction);
		spawnSpacing = distance * fraction / afterimageCount;


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
		var dashTimeRemaining = endTime;

		while (dashTimeRemaining > 0)
		{
			var go = Instantiate(afterImagePrefab, spawnPosition, Quaternion.identity);
			var afterimage = go.GetComponent<Afterimage>();
			afterimages.Add(afterimage);

			spawnPosition += direction * spawnSpacing;
			dashTimeRemaining -= spawnTimeSpacing;

			yield return new WaitForSeconds(spawnTimeSpacing);
		}
	}

	public IEnumerator DestroyAfterimages()
	{
		var spacingNote = Note.Thirtysecond;
		var spacingTime = Conductor.noteDurations[spacingNote];
		var startNote = Note.Quarter;

		var start = Conductor.GetNextNote(startNote);
		var end = start + Conductor.noteDurations[spacingNote] * afterimages.Count;
		PlayerSfx.PlayDashSfx(start, end);

		for (int i = 0; i < afterimages.Count; i++)
		{
			var targetTime = start + i * spacingTime;
			afterimages[i].Run(targetTime);
		}

		yield return new WaitForNote(startNote);

		Destroy(gameObject);
	}
}
