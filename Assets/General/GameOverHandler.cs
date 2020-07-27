using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
	[SerializeField]
	GameObject gameOverMenu;

	void Awake()
	{
		PlayerController.OnPlayerDeath += HandleGameOver;
	}

	void HandleGameOver()
	{
		PlayerController.OnPlayerDeath -= HandleGameOver;
		gameOverMenu.SetActive(true);
	}
}
