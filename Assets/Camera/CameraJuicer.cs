using UnityEngine;
using Cinemachine;

public class CameraJuicer : MonoBehaviour
{
	[SerializeField]
	private CinemachineImpulseSource kicker;
	[SerializeField]
	private CinemachineImpulseSource shaker;
	[SerializeField]
	private CameraBounce bouncer;

	private static CameraJuicer inst;

	private void Awake()
	{
		Debug.Assert(inst == null);
		inst = this;
	}

	public static void Shake(float magnitude = 1f)
	{
		inst.shaker.GenerateImpulse(magnitude);
	}

	public static void Kick(Vector3 vector)
	{
		inst.kicker.GenerateImpulse(vector);
	}

	public static void Bounce(float magnitude)
	{
		inst.bouncer.Bounce(magnitude);
	}
}
