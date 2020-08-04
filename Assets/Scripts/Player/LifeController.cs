using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeController : MonoBehaviour
{

	private HealthPoints hp;

	void Start()
	{
		hp = GetComponent<HealthPoints>();
	}

	void Update()
	{
		if (this.hp.currentHealth <= 0) {
			this.hp.currentHealth = 1000;
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
