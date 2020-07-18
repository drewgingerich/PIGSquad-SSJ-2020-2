using UnityEngine;

[CreateAssetMenu()]
public class SequenceAsset : ScriptableObject
{
	[SerializeField]
	private Sequence _sequence;

	public Sequence Sequence
	{
		get
		{
			return _sequence.Copy();
		}
	}

}
