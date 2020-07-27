using System.Collections;
using UnityEngine;

public class SceneTransitionHelper : MonoBehaviour
{
	int currentSceneIndex;

	void Awake()
	{
		var currentScene = gameObject.scene;
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
