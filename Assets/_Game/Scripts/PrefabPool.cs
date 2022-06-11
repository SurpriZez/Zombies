using Mirror;
using UnityEngine;

public class PrefabPool : NetworkBehaviour
{
	public GameObject _enemyPrefab;
	private GameObject enemy;

	public override void OnStartServer()
	{
		enemy = Instantiate(_enemyPrefab);
		NetworkServer.Spawn(enemy);
	}

	public Vector3 GetSpawnPosition()
	{
		return transform.position;
	}
}