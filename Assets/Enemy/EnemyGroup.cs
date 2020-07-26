using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
	public event System.Action<EnemyGroup> OnGroupDepleted;

	[SerializeField]
	GameObject enemyPrefab;
	[SerializeField]
	float spawnRadius = 5f;

	List<Enemy> group = new List<Enemy>();


	public void Spawn(int count)
	{
		var basePoint = SpawnPointAllocator.GetSpawnPoint();
		for (int i = 0; i < count; i++)
		{
			var spawnPoint = basePoint + Random.insideUnitCircle * spawnRadius;
			var go = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
			var enemy = go.GetComponent<Enemy>();
			enemy.OnDie += HandleEnemyDeath;
			enemy.Activate();
			group.Add(enemy);
		}
	}

	void HandleEnemyDeath(Enemy enemy)
	{
		group.Remove(enemy);
		enemy.OnDie -= HandleEnemyDeath;
		Destroy(enemy.gameObject);

		Spawn(1);

		if (group.Count == 0)
		{
			OnGroupDepleted?.Invoke(this);
		}
	}
}
