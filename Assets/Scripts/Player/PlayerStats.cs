using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : HealthPoints
{
	// Start is called before the first frame update
	private PlayerData playerData;

	void Start(){
		this.playerData = Object.FindObjectOfType<PlayerData>();
		// need to load player health here.
		// For now skip.. everything should be loaded from playerData
		// But that's a problem for future me
		this.playerData.playerHealth = this.currentHealth;
		this.playerData.playerMaxHealth = this.currentHealth;
	}

	public override void takeDamage(int damage){
		Debug.Log(this.name + " Took damage!!");
		this.currentHealth -= damage;
		this.playerData.playerHealth = this.currentHealth;

		if (this.currentHealth <= 0) {
			if (deathEffect) {
				Instantiate(deathEffect, transform.position, Quaternion.identity);
			}
			Destroy(gameObject);
			return;
		}
	}

	public void setPlayerMaxHealth(int hp){
		this.maxHealth = hp;
		this.playerData.playerMaxHealth = hp;
	}
}
