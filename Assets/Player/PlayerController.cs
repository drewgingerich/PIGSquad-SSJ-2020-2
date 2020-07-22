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
	private NoteTracker noteTracker;
	[SerializeField]
	private LineRenderer beam;
	[SerializeField]
	private MuteSourceDriver moveAudioSource;
	[SerializeField]
	private MuteSourceDriver dashAudioSource;
	[SerializeField]
	private MuteSourceDriver shootAudioSource;

	private StateMachine fsm = new StateMachine();

	private PlayerInputActions controls;
	private Vector2 velocity = Vector2.zero;
	private Vector2 moveInput;
	private Vector2 aimInput;
	private bool dashInput = false;
	private bool shootInput = false;

	private bool isDashing = false;
	private bool isShooting = false;
	private float dashTimer;

	private ContactFilter2D hitFilter = new ContactFilter2D();
	private RaycastHit2D[] hitBuffer = new RaycastHit2D[1];

	private const int STATE_IDLE = 0;
	private const int STATE_MOVE = 1;
	private const int STATE_DASH = 2;
	private const int STATE_SHOOT = 3;
	private const int STATE_RELOAD = 4;

	#region LIFECYCLE

	private void Awake()
	{
		fsm.AddState(STATE_IDLE, EnterIdleState, UpdateIdleState, null);
		fsm.AddState(STATE_MOVE, EnterMoveState, UpdateMoveState, null);
		fsm.AddState(STATE_DASH, EnterDashState, UpdateDashState, ExitDashState);
		fsm.AddState(STATE_SHOOT, EnterShootState, null, ExitShootState);
	}

	private void OnEnable()
	{
		if (controls == null) WireControls();

		controls.Player.Enable();

		fsm.ChangeToState(STATE_IDLE);
	}

	private void WireControls()
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

	private void OnDisable()
	{
		controls.Player.Disable();
	}

	private void Update()
	{
		if (shootInput && !isShooting)
		{
			fsm.ChangeToState(STATE_SHOOT);
		}
		else if (dashInput && !isDashing)
		{
			fsm.ChangeToState(STATE_DASH);
		}
		fsm.Update();
	}

	#endregion

	#region STATE_IDLE

	private void EnterIdleState()
	{
		velocity = Vector2.zero;
	}

	private void UpdateIdleState()
	{
		if (moveInput != Vector2.zero) fsm.ChangeToState(STATE_MOVE);
	}

	#endregion

	#region STATE_MOVE

	private void EnterMoveState()
	{
		moveAudioSource.Play();
	}

	private void UpdateMoveState()
	{
		bool stopped = Vector2.Distance(velocity, Vector2.zero) < 0.05f;
		bool noInput = moveInput == Vector2.zero;
		if (stopped && noInput) fsm.ChangeToState(STATE_IDLE);

		Vector2 moveDiff = moveInput - velocity;
		velocity += moveDiff * Time.deltaTime * mass;
		transform.Translate(velocity * speed);
	}

	private void ExitMoveState()
	{
		moveAudioSource.Stop();
	}

	#endregion

	#region STATE_DASH

	private void EnterDashState()
	{
		dashAudioSource.Play();
		dashInput = false;
		isDashing = true;
		dashTimer = 0f;
	}

	private void UpdateDashState()
	{
		Vector2 direction = velocity.normalized;
		dashTimer += Time.deltaTime;
		if (dashTimer >= dashDuration) fsm.ChangeToState(STATE_IDLE);

		transform.Translate(direction * dashSpeed * Time.deltaTime);
	}

	private void ExitDashState()
	{
		isDashing = false;
		dashAudioSource.Stop();
	}

	#endregion

	#region STATE_SHOOT

	private void EnterShootState()
	{
		shootInput = false;
		isShooting = true;
		StartCoroutine(
			Shoot(() => fsm.ChangeToState(STATE_MOVE))
		);
	}

	private IEnumerator Shoot(Action onEnd)
	{
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

		onEnd?.Invoke();
	}

	private void ExitShootState()
	{
		isShooting = false;
	}

	#endregion
}
