using UnityEngine;
using Cinemachine;

public class CamJuice : MonoBehaviour
{
	[SerializeField]
	private CinemachineImpulseSource kickSource;
	[SerializeField]
	private CinemachineImpulseSource shakeSource;
	[SerializeField]
	private CinemachineVirtualCamera virtualCam;

	private static CamJuice inst;

	private void Awake()
	{
		Debug.Assert(inst == null);
		inst = this;
	}

	private void _Shake()
	{
		shakeSource.GenerateImpulse(1f);
	}

	private void _Kick(Vector3 direction)
	{
		kickSource.GenerateImpulse(direction * -1);
	}

	private void _Bounce()
	{

	}

	public static void Shake()
	{
		inst._Shake();
	}

	public static void Kick(Vector3 direction)
	{
		inst._Kick(direction);
	}

	public static void Bounce()
	{
		inst._Bounce();
	}

}
