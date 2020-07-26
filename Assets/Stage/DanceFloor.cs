using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class DanceFloor : MonoBehaviour
{
	[SerializeField]
	private Note switchTimeNote = Note.Half;
	[SerializeField]
	private Tilemap floor1;
	[SerializeField]
	private Tilemap floor2;
	[SerializeField]
	private Color color1;
	[SerializeField]
	private Color color2;

	private void Start()
	{
		MusicStartAnnouncer.OnStart += StartDiscoTime;
	}

	private void StartDiscoTime(double startTime)
	{
		MusicStartAnnouncer.OnStart -= StartDiscoTime;
		StartCoroutine(DiscoTimeRoutine());
	}

	private IEnumerator DiscoTimeRoutine()
	{
		var floor1Color = color1;
		var floor2Color = color2;

		while (true)
		{
			var targetTime = Conductor.GetNextNote(switchTimeNote);
			var duration = (float)(targetTime - AudioSettings.dspTime);

			DOTween.To(() => floor1.color, c => floor1.color = c, floor1Color, duration).SetEase(Ease.InExpo);
			DOTween.To(() => floor2.color, c => floor2.color = c, floor2Color, duration).SetEase(Ease.InExpo);
			yield return new WaitForDspTime(targetTime);

			var temp = floor1Color;
			floor1Color = floor2Color;
			floor2Color = temp;
		}
	}
}
