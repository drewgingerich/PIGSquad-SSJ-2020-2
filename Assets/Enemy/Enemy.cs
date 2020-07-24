using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	[SerializeField]
	private float speed = 3;

	private bool rush = true;

	public void Activate()
	{
		StartCoroutine(MoveLoop());
	}

	private IEnumerator MoveLoop()
	{
		yield return new WaitForNote(Note.Quarter);
		while (true)
		{
			yield return StartCoroutine(Move());
			// yield return new WaitForNote(Note.Quarter, 1);
			Debug.Log(NoteTracker.GetNextNote(Note.Quarter));
			Debug.Log(NoteTracker.GetNextNote(Note.Quarter, 1));
			// yield return new WaitForNote(Note.Quarter, 1);
			yield return new WaitForSeconds(1);
			rush = !rush;
		}

	}

	private IEnumerator Move()
	{
		var diff = (Vector2)(PlayerController.playerTransform.position - transform.position);
		var direction = diff.normalized;

		var timer = 0f;
		var targetTime = NoteTracker.noteDurations[Note.Quarter];

		while (timer < targetTime)
		{
			var rotation = rush ? Random.Range(-10, 10) : Random.Range(80, 120);

			timer += Time.deltaTime;
			transform.Translate(direction * speed * Time.deltaTime);
			yield return null;
		}
	}
}
