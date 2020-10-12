using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public List<EnemyWave> spawns;
	private Queue<EnemyWave> waves = new Queue<EnemyWave>();
	private EnemyWave currentWave;
	private bool loadingNextWave = false;

	void Start(){
		for (int i = 0; i < this.spawns.Count; i++) {
			EnemyWave wave = spawns[i];
			this.waves.Enqueue(wave);
		}
	}
	void Update(){
		if (this.waves.Count == 0 && this.currentWave.enemies.Count == 0 && !this.loadingNextWave) {
			Debug.Log("Done!!");
			return;
		}
		if ((this.currentWave == null || this.currentWave.enemies.Count == 0) && !this.loadingNextWave) {
			StartCoroutine(startNextWave());
		} else if (this.currentWave != null) {
			this.currentWave.enemies = this.currentWave.enemies.FindAll(f => f != null);
		}
	}

	IEnumerator startNextWave(){
		this.loadingNextWave = true;
		// Do some success things?
		yield return new WaitForSeconds(3);
		this.currentWave = this.waves.Dequeue();
		this.currentWave.spawnEnemies();
		this.loadingNextWave = false;
	}
}
