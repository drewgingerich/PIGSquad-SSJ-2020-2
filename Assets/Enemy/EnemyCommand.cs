using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommand : MonoBehaviour
{

	[SerializeField]
	EnemyGroup group;

	void Awake()
	{
		MusicStartAnnouncer.OnStart += StartOnslaught;
	}

	void StartOnslaught(double startTime)
	{
		MusicStartAnnouncer.OnStart -= StartOnslaught;
		group.Spawn(1);
	}
}
