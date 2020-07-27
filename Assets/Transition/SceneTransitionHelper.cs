﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHelper : MonoBehaviour
{
	int currentSceneIndex;

	void Awake()
	{
		var currentScene = SceneManager.GetActiveScene();
		currentSceneIndex = currentScene.buildIndex;
	}

	IEnumerator Start()
	{
		yield return SceneTransitionContext.Initialize();
	}

	public void TransitionToScene(int sceneIndex)
	{
		SceneTransitionContext.manager.FadeToScene(currentSceneIndex, sceneIndex);
	}
}