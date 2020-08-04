using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardLifeController : MonoBehaviour
{

	private HealthPoints hp;

	void Start()
	{
		hp = GetComponent<HealthPoints>();
	}

	void Update()
	{
		// if (this.hp.currentHealth <= 0) {
		// 	Destroy(gameObject);
		// }
	}
}
