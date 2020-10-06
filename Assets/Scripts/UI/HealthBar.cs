using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Slider slider;
	private PlayerData playerData;
	// Start is called before the first frame update
	void Start()
	{
		this.playerData = Object.FindObjectOfType<PlayerData>();
	}

	// Update is called once per frame
	void Update()
	{
		slider.value = (float)this.playerData.playerHealth / (float)this.playerData.playerMaxHealth;
	}
}
