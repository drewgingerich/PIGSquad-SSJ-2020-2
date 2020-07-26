using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Afterimage : MonoBehaviour
{
	private double targetTime;

	public void Run(double targetTime)
	{
		this.targetTime = targetTime;
		StartCoroutine(VfxRoutine());
	}

	private IEnumerator VfxRoutine()
	{
		yield return new WaitForDspTime(targetTime);
		Destroy(gameObject);
	}
}
