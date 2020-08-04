using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissileController : MonoBehaviour
{
	public float projectileSpeed = 0f;
	public GameObject missile;
	public int nrOfProjectiles = 1;

	private GameObject currentMissile;
	private List<GameObject> currentMissiles;
	private bool fired = false;

	public void spawnAndShoot(Vector3 dir, int damage){
		this.spawn();
		this.shoot(dir, damage);
	}

	public void spawn(){
		currentMissiles = new List<GameObject>();
		int counter = 0;
		float posModifier = 0;
		for (int i = 0; i < nrOfProjectiles; i++) {
			int spawnDirection = -1;
			if (i == 0 || i % 2 == 0) {
				spawnDirection = 1;
			}
			if (i % 2 == 1) {
				posModifier = 2.5f + i/2 * 2.5f;
				Debug.Log(posModifier);
			}
			Vector3 spawnPosition = new Vector3(transform.position.x + spawnDirection*posModifier, transform.position.y, 0);
			GameObject missile = Instantiate(this.missile, transform.position, Quaternion.identity);
			missile.GetComponent<ProjectileController>().animatedMoveTo = spawnPosition;
			this.currentMissiles.Add(missile);
		}
	}

	public void shoot(Vector3 target, int damage){
		if (this.currentMissiles.Count > 0) {
			this.fired = true;
			for (int i = 0; i < currentMissiles.Count; i++) {
				GameObject missile = currentMissiles[i];
				Vector3 dir = target - missile.transform.position;
				dir.Normalize();
				missile.GetComponent<ProjectileController>().shoot(this.gameObject, dir, this.projectileSpeed, damage);
			}
		}
	}

	public void destroyProjectile(float delay = 0f){

		if (this.currentMissiles != null) {
			for (int i = 0; i < this.currentMissiles.Count; i++) {
				GameObject missile = this.currentMissiles[i];
				if (missile != null) {
					missile.GetComponent<ProjectileController>().destroy(delay);
				}
			}
			this.currentMissiles = null;
			this.fired = false;
		}
	}

	void OnDestroy(){

		if (this.currentMissiles != null) {
			for (int i = 0; i < this.currentMissiles.Count; i++) {
				GameObject missile = this.currentMissiles[i];
				if (missile != null) {
					missile.GetComponent<ProjectileController>().destroy(0.1f);
				}
			}
		}

		// if (this.currentMissile != null) {
		// 	this.currentMissile.GetComponent<ProjectileController>().destroy(0.1f);
		// }
	}

}
