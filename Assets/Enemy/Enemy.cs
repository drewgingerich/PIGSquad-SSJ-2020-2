using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
	public event System.Action<Enemy> OnDie;

	[SerializeField]
	private float health = 2;
	[SerializeField]
	private float speed = 3;

	[SerializeField]
	private GameObject bloodPrefab;
	[SerializeField]
	private HitDetector hitDetector;

	private Rigidbody2D rb;
	private BloodSpurt bloodSpurtVfx;
	private bool dead = false;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void Activate()
	{
		hitDetector.OnHit += HandleHit;
		StartCoroutine(MoveLoop());
	}

	private void HandleHit(Hit type, Vector2 direction)
	{
		StopAllCoroutines();
		Debug.Log("Here");
		StartCoroutine(TakeHit(direction));
	}

	private IEnumerator MoveLoop()
	{
		while (true)
		{
			yield return StartCoroutine(Rush());
			yield return StartCoroutine(Flank());
		}
	}

	private IEnumerator TakeHit(Vector2 direction)
	{
		health -= 1;
		if (health == 0) dead = true;

		rb.velocity = Vector2.zero;

		yield return new WaitForNote(Note.Eighth);

		var bloodObject = Instantiate(bloodPrefab, transform);
		bloodSpurtVfx = bloodObject.GetComponent<BloodSpurt>();
		bloodSpurtVfx.Run(direction);

		if (dead)
		{
			StartCoroutine(Die());
		}
		else
		{
			StartCoroutine(Stumble());
		}
	}

	private IEnumerator Stumble()
	{
		yield return new WaitForNote(Note.Sixteenth);
		bloodSpurtVfx.Stop();
		bloodSpurtVfx.Destroy();
		StartCoroutine(MoveLoop());
	}

	private IEnumerator Die()
	{
		yield return new WaitForNote(Note.Quarter, 3);
		bloodSpurtVfx.Stop();
		bloodSpurtVfx.Destroy();
		OnDie?.Invoke(this);
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
		yield return StartCoroutine(Move(direction * speed * 1.5f, targetTime));

	}

	private IEnumerator Flank()
	{
		var diff = (Vector2)(PlayerController.playerTransform.position - transform.position);
		var rotation = Random.Range(80, 120);
		var direction = Quaternion.AngleAxis(rotation, Vector3.back) * diff.normalized;
		var targetTime = Conductor.GetNextNote(Note.Quarter);
		yield return StartCoroutine(Move(direction * speed, targetTime));

	}

	private IEnumerator Move(Vector2 velocity, double targetTime)
	{
		while (AudioSettings.dspTime < targetTime)
		{
			rb.velocity = velocity;
			yield return null;
		}
	}
}
