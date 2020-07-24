using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	[SerializeField]
	private float speed = 3;

	public void Activate()
	{
		StartCoroutine(MoveLoop());
	}

	private IEnumerator MoveLoop()
	{
		while (true)
		{
			yield return StartCoroutine(Rush());
			yield return StartCoroutine(Flank());
		}

	}

	public void Tag()
	{
		StopAllCoroutines();
	}

	public IEnumerator Quake()
	{
		float timer = 0;
		while (true)
		{
			var x = 0.1f * Mathf.Sin(timer * 5.2f);
			var y = 0.1f * Mathf.Sin(timer * 5.5f);
			Vector2 shift = new Vector2(x, y);
			transform.Translate(shift);
		}
	}

	private IEnumerator Rush()
	{
		var diff = (Vector2)(PlayerController.playerTransform.position - transform.position);
		var rotation = Random.Range(-10, 10);
		var direction = Quaternion.AngleAxis(rotation, Vector3.back) * diff.normalized;
		var targetTime = Conductor.GetNextNote(Note.Quarter);
		yield return StartCoroutine(Move(direction, speed * 1.5f, targetTime));

	}

	private IEnumerator Flank()
	{
		var diff = (Vector2)(PlayerController.playerTransform.position - transform.position);
		var rotation = Random.Range(80, 120);
		var direction = Quaternion.AngleAxis(rotation, Vector3.back) * diff.normalized;
		var targetTime = Conductor.GetNextNote(Note.Quarter);
		yield return StartCoroutine(Move(direction, speed, targetTime));

	}

	private IEnumerator Move(Vector2 direction, float speed, double targetTime)
	{
		while (AudioSettings.dspTime < targetTime)
		{
			transform.Translate(direction * speed * Time.deltaTime);
			yield return null;
		}
	}
}
