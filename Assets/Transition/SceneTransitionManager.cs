using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
	[SerializeField]
	Animator fadeAnimator;

	void Awake()
	{
		SceneTransitionContext.manager = this;
	}

	public void FadeToScene(int currentSceneIndex, int nextSceneIndex)
	{
		StartCoroutine(FadeRoutine(currentSceneIndex, nextSceneIndex));
	}

	IEnumerator FadeRoutine(int currentSceneIndex, int nextSceneIndex)
	{
		fadeAnimator.SetTrigger("fadeIn");
		yield return new WaitForSeconds(1f);

		yield return SceneManager.UnloadSceneAsync(currentSceneIndex);
		yield return SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);

		var nextScene = SceneManager.GetSceneByBuildIndex(nextSceneIndex);
		SceneManager.SetActiveScene(nextScene);

		fadeAnimator.SetTrigger("fadeOut");
	}
}
