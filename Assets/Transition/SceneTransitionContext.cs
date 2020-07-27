using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTransitionContext
{
	static int transitionSceneIndex = 1;
	public static SceneTransitionManager manager { get; set; }

	public static IEnumerator Initialize()
	{
		if (manager == null)
		{
			yield return SceneManager.LoadSceneAsync(transitionSceneIndex, LoadSceneMode.Additive);
		}
	}
}
