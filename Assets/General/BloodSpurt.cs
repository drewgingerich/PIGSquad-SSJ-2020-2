using System.Collections;
using UnityEngine;

public class BloodSpurt : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem bloodSystem;

	public void Activate(Vector2 direction)
	{
		float angle = Vector2.SignedAngle(Vector2.right, direction);
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		StartCoroutine(ActivateRoutine());
	}

	private IEnumerator ActivateRoutine()
	{
		bloodSystem.Play();
		yield return new WaitForNote(Note.Sixteenth);
		bloodSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		yield return new WaitForNote(Note.Whole);
		Destroy(gameObject);
	}
}
