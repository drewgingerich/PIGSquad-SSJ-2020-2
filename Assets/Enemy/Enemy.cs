using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	[SerializeField]
	float speed = 3;

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
			yield return new WaitForNote(Note.Quarter);
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
			timer += Time.deltaTime;
			transform.Translate(direction * speed * Time.deltaTime);
			yield return null;
		}
	}
}
