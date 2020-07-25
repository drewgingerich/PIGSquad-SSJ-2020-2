using UnityEngine;

public class FollowPlayerAndMouse : MonoBehaviour
{
	[SerializeField]
	Transform player;

	[Range(0, 1)]
	[SerializeField]
	private float mouseImportance = 0.5f;

	private Vector2 aimInput;

	void Start()
	{
		PlayerController.controls.Player.Aim.performed += ctx =>
		{
			var screenPoint = ctx.ReadValue<Vector2>();
			aimInput = Camera.main.ScreenToWorldPoint(screenPoint);
		};

	}

	void Update()
	{
		var diff = (Vector3)aimInput - player.position;
		transform.position = player.position + diff * mouseImportance;
	}
}
