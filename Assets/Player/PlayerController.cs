using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Tweaks")]
	[SerializeField]
	private float speed = 2f;
	[SerializeField]
	private float mass = 5f;
	[SerializeField]
	private float dashSpeed = 2f;
	[SerializeField]
	private float dashDuration = 2f;

	[Header("Refs")]
	[SerializeField]
	private LineRenderer beam;
	[SerializeField]
	private MuteSourceDriver moveAudioSource;
	[SerializeField]
	private MuteSourceDriver dashAudioSource;
	[SerializeField]
	private MuteSourceDriver shootAudioSource;

	private PlayerInputActions controls;
	private Vector2 velocity = Vector2.zero;
	private Vector2 moveInput;
	private Vector2 aimInput;
	private bool dashInput = false;
	private bool shootInput = false;

	private bool isDashing = false;
	private bool isShooting = false;

	private ContactFilter2D hitFilter = new ContactFilter2D();
	private RaycastHit2D[] hitBuffer = new RaycastHit2D[1];

	private void OnEnable()
	{
		if (controls == null)
		{
			controls = new PlayerInputActions();
			controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
			controls.Player.Dash.performed += ctx => dashInput = true;
			controls.Player.Aim.performed += ctx =>
			{
				var screenPoint = ctx.ReadValue<Vector2>();
				aimInput = Camera.main.ScreenToWorldPoint(screenPoint);
			};
			controls.Player.Shoot.performed += ctx => shootInput = true;
		}
		controls.Player.Enable();

		StopAllCoroutines();
		StartCoroutine(MoveState());
	}

	private void Update()
	{
		if (shootInput && !isShooting)
		{
			moveAudioSource.Stop();
			dashAudioSource.Stop();
			isDashing = false;
			StopAllCoroutines();
			StartCoroutine(ShootState());
		}
		if (dashInput && !isDashing)
		{
			isShooting = false;
			moveAudioSource.Stop();
			StopAllCoroutines();
			StartCoroutine(DashState());
		}
	}

	private void OnDisable()
	{
		controls.Player.Disable();
	}

	private IEnumerator IdleState()
	{
		velocity = Vector2.zero;
		while (true)
		{
			if (moveInput != Vector2.zero)
			{
				StartCoroutine(MoveState());
				break;
			}
			yield return null;
		}
	}

	private IEnumerator MoveState()
	{
		moveAudioSource.Play();
		while (true)
		{
			bool stopped = Vector2.Distance(velocity, Vector2.zero) < 0.05f;
			bool noInput = moveInput == Vector2.zero;
			if (stopped && noInput)
			{
				moveAudioSource.Stop();
				StartCoroutine(IdleState());
				break;
			}

			Vector2 moveDiff = moveInput - velocity;
			velocity += moveDiff * Time.deltaTime * mass;
			transform.Translate(velocity * speed);
			yield return null;
		}
	}

	private IEnumerator DashState()
	{
		dashAudioSource.Play();
		dashInput = false;
		isDashing = true;
		Vector2 direction = velocity.normalized;
		float timer = 0f;
		while (true)
		{
			timer += Time.deltaTime;
			if (timer >= dashDuration)
			{
				isDashing = false;
				dashAudioSource.Stop();
				StartCoroutine(MoveState());
				break;
			}
			transform.Translate(direction * dashSpeed * Time.deltaTime);
			yield return null;
		}
	}

	private IEnumerator ShootState()
	{
		shootInput = false;
		isShooting = true;
		Vector2 direction = (aimInput - (Vector2)transform.position).normalized;
		var hits = Physics2D.Raycast(transform.position, direction, hitFilter, hitBuffer);
		var endPoint = hits > 0
			? (Vector3)hitBuffer[0].point
			: transform.position + (Vector3)direction * 100;
		var beamPositions = new Vector3[]{
				transform.position,
				endPoint
			};
		beam.SetPositions(beamPositions);
		beam.enabled = true;
		shootAudioSource.Play();
		yield return new WaitForSeconds(0.25f);
		beam.enabled = false;
		isShooting = false;
		StartCoroutine(IdleState());
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
		beam.enabled = true;
		yield return new WaitForSeconds(0.25f);
		beam.enabled = false;
	}
}
