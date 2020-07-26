using System.Collections;
using UnityEngine;

public class DanceFloor : MonoBehaviour
{
	[SerializeField]
	private Note switchTimeNote = Note.Half;
	[SerializeField]
	private GameObject floor1;
	[SerializeField]
	private GameObject floor2;

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
		while (true)
		{
			yield return new WaitForNote(switchTimeNote);
			floor1.SetActive(true);
			floor2.SetActive(false);
			yield return new WaitForNote(switchTimeNote);
			floor1.SetActive(false);
			floor2.SetActive(true);
		}
	}
}
