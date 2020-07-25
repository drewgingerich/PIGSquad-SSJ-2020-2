using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallPulse : MonoBehaviour
{

	[SerializeField]
	private new Tilemap tilemap;
	[SerializeField]
	private Color color1;
	[SerializeField]
	private Color color2;

	void Start()
	{
		MusicStartAnnouncer.OnStart += StartPulse;

	}

	private void StartPulse(double startTime)
	{
		MusicStartAnnouncer.OnStart -= StartPulse;
		StartCoroutine(PulseRoutine());

	}

	private IEnumerator PulseRoutine()
	{
		var startColor = color1;
		var endColor = color2;
		while (true)
		{
			var targetTime = Conductor.GetNextNote(Note.Half);
			yield return StartCoroutine(ColorLerp(startColor, endColor, targetTime));
			var temp = startColor;
			startColor = endColor;
			endColor = temp;
		}

	}

	private IEnumerator ColorLerp(Color startColor, Color endColor, double targetTime)
	{
		var startTime = AudioSettings.dspTime;
		var totalDiff = targetTime - startTime;
		double progress = 0f;
		while (progress < 1)
		{
			var curDiff = AudioSettings.dspTime - startTime;
			progress = curDiff / totalDiff;
			tilemap.color = Color.Lerp(startColor, endColor, (float)progress);
			yield return null;
		}
	}
}
