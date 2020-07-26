using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
	public static Transform playerTransform;

	[Header("Move")]
	[SerializeField]
	private float moveSpeed = 2f;

	[Header("Dash")]
	[SerializeField]
	private float dashDistance = 2f;
	[SerializeField]
	private float dashDuration = 0.2f;
	[SerializeField]
	private float dashCameraShake = 0.5f;
	[SerializeField]
	private AfterimageVfx afterimageVfxPrefab;

	[Header("Shoot")]
	[SerializeField]
	private AudioSource shootAudioSource;
	[SerializeField]
	private GameObject beamControllerPrefab;

	[Header("Reload")]
	[SerializeField]
	private AudioMixerSnapshot reloadSnapshot;
	[SerializeField]
	private AudioSource reloadAudioSource;

	[Header("General")]
	[SerializeField]
	private Rigidbody2D rb;
	[SerializeField]
	private AudioMixerSnapshot mainSnapshot;

	public static PlayerInputActions controls;
	private Vector2 moveInput;
	private Vector2 aimInput;
	private bool dashInput = false;
	private bool shootInput = false;
	private bool reloadInput = false;

	private RaycastHit2D[] hitBuffer = new RaycastHit2D[1];

	private bool isShooting = false;
	private bool isReloading = false;
	private float dashTimer;

	private StateMachine fsm = new StateMachine();
	private const int STATE_IDLE = 0;
	private const int STATE_MOVE = 1;
	private const int STATE_DASH = 2;
	private const int STATE_SHOOT = 3;
	private const int STATE_RELOAD = 4;

	#region LIFECYCLE

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		if (controls == null) WireControls();

		playerTransform = transform;

		fsm.AddState(STATE_IDLE, EnterIdleState, UpdateIdleState, null);
		fsm.AddState(STATE_MOVE, EnterMoveState, UpdateMoveState, ExitMoveState);
		fsm.AddState(STATE_DASH, EnterDashState, null, null);
		fsm.AddState(STATE_SHOOT, EnterShootState, null, ExitShootState);
		fsm.AddState(STATE_RELOAD, EnterReloadState, null, ExitReloadState);
	}

	private void OnEnable()
	{
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
		fsm.Update();
	}

	#endregion

	#region STATE_IDLE

	private void EnterIdleState()
	{
		rb.velocity = Vector2.zero;
	}

	private void UpdateIdleState()
	{
		if (shootInput) fsm.ChangeToState(STATE_SHOOT);
		else if (moveInput != Vector2.zero) fsm.ChangeToState(STATE_MOVE);
	}

	#endregion

	#region STATE_MOVE

	private void EnterMoveState()
	{
		PlayerSfx.StartMoveSfx();
	}

	private void UpdateMoveState()
	{
		if (dashInput) fsm.ChangeToState(STATE_DASH);
		// else if (shootInput) fsm.ChangeToState(STATE_SHOOT);
		else if (moveInput == Vector2.zero)
		{
			fsm.ChangeToState(STATE_IDLE);
			return;
		}

		rb.velocity = moveInput * moveSpeed;
	}

	private void ExitMoveState()
	{
		PlayerSfx.StopMoveSfx();
	}

	#endregion

	#region STATE_DASH

	private void EnterDashState()
	{
		dashInput = false;
		StartCoroutine(Dash());
	}

	private IEnumerator Dash()
	{
		Vector2 direction = moveInput;
		var dashSpeed = dashDistance / dashDuration;

		var hits = rb.Cast(direction, hitBuffer, dashDistance);
		var dashFraction = hits > 0 ? hitBuffer[0].distance / dashDistance : 1;

		var vfx = Instantiate(afterimageVfxPrefab, rb.position, Quaternion.identity);
		vfx.Run(direction, dashDistance, dashDuration, dashFraction);

		CameraJuicer.Shake(dashCameraShake);
		rb.velocity = direction * dashSpeed;

		yield return new WaitForSeconds(dashDuration * dashFraction);

		fsm.ChangeToState(STATE_MOVE);
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
