using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWave
{
	public int numberOfEnemies = 1;

	public GameObject enemy;
	public GameObject area;
	public List<GameObject> enemies;

	public void spawnEnemies(){
		for (int i = 0; i < numberOfEnemies; i++) {
			Bounds bounds = area.GetComponent<SpriteRenderer>().bounds;
			Vector3 pos = new Vector3(
				Random.Range(bounds.min.x, bounds.max.x),
				Random.Range(bounds.min.y, bounds.max.y),
				0
				);
			enemies.Add(GameObject.Instantiate(enemy, Vector3.zero, Quaternion.identity));
		}
	}
}
