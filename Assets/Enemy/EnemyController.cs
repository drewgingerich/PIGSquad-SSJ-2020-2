using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public List<Enemy> group;

	void Start()
	{
		MusicStartAnnouncer.OnStart += ActivateEnemies;
	}

	void ActivateEnemies(double startTime)
	{
		foreach (Enemy enemy in group)
		{
			enemy.Activate();
		}
	}
}
