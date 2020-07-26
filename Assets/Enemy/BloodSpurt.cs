using System.Collections;
using UnityEngine;

public class BloodSpurt : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem bloodSystem;

	public void Run(Vector2 direction)
	{
		float angle = Vector2.SignedAngle(Vector2.right, direction);
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		bloodSystem.Play();
	}

	public void Stop()
	{
		bloodSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
	}

	public void Destroy()
	{
		Destroy(gameObject);
	}
}
