using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	[SerializeField]
	private float speed = 3;

	[SerializeField]
	private GameObject bloodPrefab;
	[SerializeField]
	private HitDetector hitDetector;

	public void Activate()
	{
		hitDetector.OnHit += HandleHit;
		StartCoroutine(MoveLoop());
	}

	private void HandleHit(Hit type, Vector2 direction)
	{
		switch (type)
		{
			case Hit.Tag:
				Tag();
				break;
			case Hit.Shot:
				Shot(direction);
				break;
		}

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
		StartCoroutine(Quake());
	}

	public void Shot(Vector2 direction)
	{
		StopAllCoroutines();

		var bloodObject = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
		var bloodSpurt = bloodObject.GetComponent<BloodSpurt>();
		bloodSpurt.Activate(direction);

		StartCoroutine(Die());
	}

	public IEnumerator Quake()
	{
		var amplitude = 0.03f;
		var someFrequencyOne = 61f;
		var someFrequencyTwo = 59f;

		float timer = 0;
		while (true)
		{
			var x = amplitude * Mathf.Sin(timer * someFrequencyOne);
			var y = amplitude * Mathf.Sin(timer * someFrequencyTwo);
			Vector2 shift = new Vector2(x, y);
			transform.Translate(shift);
			timer += Time.deltaTime;
			yield return null;
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

	private IEnumerator Die()
	{
		yield return new WaitForNote(Note.Eighth);
		hitDetector.OnHit -= HandleHit;
		Destroy(gameObject);
	}
}
