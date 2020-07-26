using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AfterImage : MonoBehaviour
{
	private Vector2 direction;
	private float speed;
	private Note duration;

	public void Go(Vector2 direction, float speed, Note duration)
	{
		this.direction = direction;
		this.speed = speed;
		this.duration = duration;
	}

	// public IEnumerator GoRoutine()
	// {
	// 	dashController.Activate(direction, dashDistance);
	// 	rb.velocity = direction * dashSpeed;
	// 	dashAudioSource.SetScheduledEndTime(Conductor.CalcNoteTime(dashNoteType));
	// 	yield return new WaitForNoteLength(dashNoteType);

	// 	rb.velocity = direction * moveSpeed;
	// 	fsm.ChangeToState(STATE_MOVE);
	// 	yield return null;
	// }

	private void Awake()
	{
		Destroy(gameObject, 0.25f);
	}
}
