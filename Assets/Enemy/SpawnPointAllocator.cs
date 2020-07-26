using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPointAllocator : MonoBehaviour
{
	[SerializeField]
	List<Vector2> spawnPoints;

	private List<Vector2> usedSpawnPoints = new List<Vector2>();

	private static SpawnPointAllocator inst;

	void Awake()
	{
		Debug.Assert(inst == null);
		inst = this;

	}

	public static Vector2 GetSpawnPoint()
	{
		if (inst.spawnPoints.Count < 2)
		{
			var shuffledPoints = new List<Vector2>();
			inst.spawnPoints = shuffledPoints.Concat(inst.spawnPoints)
				.Concat(inst.usedSpawnPoints)
				.ToList();
		}

		var index = Random.Range(0, inst.spawnPoints.Count - 1);
		var spawnPoint = inst.spawnPoints[index];

		inst.spawnPoints.RemoveAt(index);
		inst.usedSpawnPoints.Add(spawnPoint);

		return spawnPoint;
	}

	void OnDrawGizmosSelected()
	{
		foreach (var pnt in spawnPoints)
		{
			Gizmos.DrawSphere(pnt, 1);
		}
	}
}
