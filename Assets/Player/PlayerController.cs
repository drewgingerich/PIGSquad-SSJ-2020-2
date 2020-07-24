using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
	public static Transform playerTransform;

	[Header("Tweaks")]
	[SerializeField]
	private float speed = 2f;
	[SerializeField]
	private float mass = 5f;
	[SerializeField]
	private float dashSpeed = 2f;

	[Header("Refs")]
	[SerializeField]
	private AudioMixerSnapshot mainSnapshot;
	[SerializeField]
	private AudioMixerSnapshot reloadSnapshot;

	[SerializeField]
	private AudioSource moveAudioSource;
	[SerializeField]
	private AudioSource dashAudioSource;
	[SerializeField]
	private AudioSource shootAudioSource;
	[SerializeField]
	private AudioSource reloadAudioSource;

	[SerializeField]
	private GameObject beamControllerPrefab;
	[SerializeField]
	private DashController dashController;

	private StateMachine fsm = new StateMachine();

	private PlayerInputActions controls;
	private Vector2 velocity = Vector2.zero;
	private Vector2 moveInput;
	private Vector2 aimInput;
	private bool dashInput = false;
	private bool shootInput = false;
	private bool reloadInput = false;

	private bool isDashing = false;
	private bool isShooting = false;
	private bool isReloading = false;
	private float dashTimer;

	private const int STATE_IDLE = 0;
	private const int STATE_MOVE = 1;
	private const int STATE_DASH = 2;
	private const int STATE_SHOOT = 3;
	private const int STATE_RELOAD = 4;

	#region LIFECYCLE

	private void Awake()
	{
		playerTransform = transform;

		fsm.AddState(STATE_IDLE, EnterIdleState, UpdateIdleState, null);
		fsm.AddState(STATE_MOVE, EnterMoveState, UpdateMoveState, ExitMoveState);
		fsm.AddState(STATE_DASH, EnterDashState, null, ExitDashState);
		fsm.AddState(STATE_SHOOT, EnterShootState, null, ExitShootState);
		fsm.AddState(STATE_RELOAD, EnterReloadState, null, ExitReloadState);
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
		controls.Player.Reload.performed += ctx => reloadInput = true;
	}

	private void OnDisable()
	{
		controls.Player.Disable();
	}

	private void Update()
	{
		if (dashInput && !isDashing && !isReloading)
		{
			fsm.ChangeToState(STATE_DASH);
		}
		else if (reloadInput && !isReloading)
		{
			fsm.ChangeToState(STATE_RELOAD);
		}
		else if (shootInput && !isShooting)
		{
			fsm.ChangeToState(STATE_SHOOT);
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
		moveAudioSource.PlayScheduled(Conductor.GetNextNote(Note.Eighth));
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
		moveAudioSource.SetScheduledEndTime(Conductor.GetNextNote(Note.Eighth));
	}

	#endregion

	#region STATE_DASH

	private void EnterDashState()
	{
		dashInput = false;
		isDashing = true;
		StartCoroutine(Dash());
	}

	private IEnumerator Dash()
	{
		Vector2 direction = velocity.normalized;
		var dashTime = (float)Conductor.noteDurations[Note.Sixteenth];
		var distance = dashTime * dashSpeed;

		dashController.Activate(direction, distance);

		var nextThirtysecond = Conductor.GetNextNote(Note.Thirtysecond);
		dashAudioSource.PlayScheduled(nextThirtysecond);

		float timer = 0f;
		while (timer < dashTime)
		{
			timer += Time.deltaTime;
			transform.Translate(direction * dashSpeed * Time.deltaTime);
			yield return null;
		}

		nextThirtysecond = Conductor.GetNextNote(Note.Thirtysecond);
		dashAudioSource.SetScheduledEndTime(nextThirtysecond);

		fsm.ChangeToState(STATE_MOVE);
	}

	private void ExitDashState()
	{
		isDashing = false;
	}

	#endregion

	#region STATE_SHOOT

	private void EnterShootState()
	{
		shootInput = false;
		isShooting = true;
		var go = Instantiate(beamControllerPrefab, transform.position, Quaternion.identity);
		var beamController = go.GetComponent<BeamController>();
		Vector2 direction = (aimInput - (Vector2)transform.position).normalized;
		beamController.Fire(direction, shootAudioSource);
		fsm.ChangeToState(STATE_MOVE);
	}

	private void ExitShootState()
	{
		isShooting = false;
	}

	#endregion

	#region STATE_RELOAD

	private void EnterReloadState()
	{
		reloadInput = false;
		isReloading = true;
		StartCoroutine(Reload());
	}

	private IEnumerator Reload()
	{
		yield return new WaitForNote(Note.Half);
		reloadSnapshot.TransitionTo((float)Conductor.noteDurations[Note.Sixteenth]);
		reloadAudioSource.PlayScheduled(Conductor.GetNextNote(Note.Eighth));
		yield return new WaitForNote(Note.Half);
		yield return new WaitForNote(Note.Quarter);
		yield return new WaitForNote(Note.Eighth);
		// yield return new WaitForSeconds((float)NoteTracker.noteDurations[Note.Whole]);



		mainSnapshot.TransitionTo((float)Conductor.noteDurations[Note.Sixteenth]);
		// yield return new WaitForNote(Note.Eighth);

		fsm.ChangeToState(STATE_MOVE);
	}

	private void ExitReloadState()
	{
		isReloading = false;
	}

	#endregion
}
