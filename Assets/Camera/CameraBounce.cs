using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBounce : MonoBehaviour
{
	[SerializeField]
	private float returnTime;
	[SerializeField]
	private CinemachineVirtualCamera cam;

	private float baseOrthoSize;
	private float currentOrthoSize;

	private void Awake()
	{
		baseOrthoSize = currentOrthoSize = cam.m_Lens.OrthographicSize;
	}

	public void Bounce(float magnitude)
	{
		currentOrthoSize += magnitude;
		StopAllCoroutines();
		StartCoroutine(ReturnRoutine());
	}

	private IEnumerator ReturnRoutine()
	{
		var diff = baseOrthoSize - currentOrthoSize;
		var rate = diff / returnTime;
		while (currentOrthoSize > baseOrthoSize)
		{
			currentOrthoSize += rate * Time.deltaTime;
			cam.m_Lens.OrthographicSize = currentOrthoSize;
			yield return null;
		}
	}
}
