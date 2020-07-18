using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SoundBank : ScriptableObject
{
	[SerializeField]
	private List<AudioClip> clips;

	public int Lanes { get { return clips.Count; } }

	public AudioClip GetClip(int index)
	{
		return clips[index];
	}
}
