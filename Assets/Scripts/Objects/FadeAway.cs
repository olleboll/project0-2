using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
	public float speed = 1f;

	private bool shouldFade = false;
	private SpriteRenderer renderer;

	void Start()
	{
		this.renderer = GetComponent<SpriteRenderer>();
	}

	public void fadeAway(){
		this.shouldFade = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (!this.shouldFade) {
			return;
		}
		if (this.renderer.color.a > 0) {
			Color newColor = this.renderer.color;
			newColor.a -= this.speed * Time.deltaTime;
			this.renderer.color = newColor;
		} else {
			Destroy(gameObject);
		}
	}
}
