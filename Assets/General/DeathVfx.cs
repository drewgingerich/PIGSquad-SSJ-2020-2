using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeathVfx : MonoBehaviour
{
	[SerializeField]
	Color deathColor;
	[SerializeField]
	Transform playerTransform;
	[SerializeField]
	new SpriteRenderer renderer;
	[SerializeField]
	ParticleSystem debrisSystem;

	public void Run()
	{
		StartCoroutine(DeathRoutine());
	}

	public IEnumerator DeathRoutine()
	{
		yield return Quake();
		debrisSystem.Play();
		renderer.color = deathColor;
		CameraJuicer.Shake(1);
		yield return new WaitForNote(Note.Eighth);
		debrisSystem.Stop();
		yield return new WaitForNote(Note.Eighth);
		// renderer.enabled = false;
	}

	public IEnumerator Quake()
	{
		var amplitude = 0.03f;
		var someFrequencyOne = 61f;
		var someFrequencyTwo = 59f;

		var targetTime = Conductor.GetNextNote(Note.Quarter, 2) - AudioSettings.dspTime;
		var timer = 0f;
		while (timer < targetTime)
		{
			var x = amplitude * Mathf.Sin(timer * someFrequencyOne);
			var y = amplitude * Mathf.Sin(timer * someFrequencyTwo);
			Vector2 shift = new Vector2(x, y);
			playerTransform.Translate(shift);
			timer += Time.deltaTime;
			yield return null;
		}
	}
}
