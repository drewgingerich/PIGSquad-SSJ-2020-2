using System;
using UnityEngine;

public enum Hit
{
	Tag,
	Shot,
	Bump,
}

public class HitDetector : MonoBehaviour
{
	public event Action<Hit, Vector2> OnHit;

	public void Hit(Hit type, Vector2 direction)
	{
		OnHit?.Invoke(type, direction);
	}

	// private void OnCollisionEnter2D(Collision2D other) {
	//     other.gameObject.layer
	// }
}
