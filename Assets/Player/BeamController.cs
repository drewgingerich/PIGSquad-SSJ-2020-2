using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour
{
	[SerializeField]
	private LineRenderer beam;

	private ContactFilter2D hitFilter = new ContactFilter2D();
	private RaycastHit2D[] hitBuffer = new RaycastHit2D[1];

	public void Fire(Vector2 direction)
	{
		StartCoroutine(Shoot(direction));
	}

	private IEnumerator Shoot(Vector2 direction)
	{
		var hits = Physics2D.Raycast(transform.position, direction, hitFilter, hitBuffer);
		var endPoint = hits > 0
			? (Vector3)hitBuffer[0].point
			: transform.position + (Vector3)direction * 100;
		var beamPositions = new Vector3[]{
				transform.position,
				endPoint
			};
		beam.SetPositions(beamPositions);
		beam.startWidth = beam.endWidth = 0.05f;
		beam.enabled = true;
		yield return new WaitForNote(Note.Quarter, 1);
		beam.startWidth = beam.endWidth = 0.15f;
		// shootAudioSource.Play();
		yield return new WaitForSeconds(0.1f);
		beam.enabled = false;
		GameObject.Destroy(gameObject);
	}
}
