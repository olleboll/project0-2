using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FireLightController : MonoBehaviour
{
	public bool lit = true;
	public float intenstiy = 0f;
	public float range = 0f;
	private float _intensity = 0f;
	private Light2D light;
	private float lightStartRadius;
	private float flickerBig;
	private float flickerSmall;
	private bool isGrowing = true;
	private float interpolation = 0f;

	void Start()
	{
		this.light = GetComponent<Light2D>();
		this.lightStartRadius = this.light.pointLightOuterRadius;

		this._intensity = this.intenstiy;
		this.flickerBig = this.lightStartRadius + this.range;
		this.flickerSmall = this.lightStartRadius - this.range;

		this.light.enabled = this.lit;
	}

	void Update()
	{
		float currentRadius = this.light.pointLightOuterRadius;
		float maxRadius = this.flickerBig;
		float minRadius = this.flickerSmall;

		if (currentRadius > (this.flickerBig - 0.1) && this.isGrowing) {
			this.isGrowing = false;
			this.interpolation = 1;
			this.intenstiy = this._intensity;
		} else if (currentRadius < (this.flickerSmall + 0.1) && !this.isGrowing) {
			this.isGrowing = true;
			this.interpolation = 0;
			this.intenstiy = this._intensity;
		}

		if (!this.isGrowing) {
			maxRadius = currentRadius;
			this.interpolation -= this.intenstiy * Time.deltaTime;
		} else {
			minRadius = currentRadius;
			this.interpolation += this.intenstiy * Time.deltaTime;
		}

		this.light.pointLightOuterRadius = Mathf.Lerp(minRadius, maxRadius, this.interpolation);

		if (Random.value < 0.5) {
			this.intenstiy -= 0.1f;
		} else {
			this.intenstiy += 0.1f;
		}
	}

	public void toggleLight(){
		this.lit = !this.lit;
		this.light.enabled = this.lit;
	}
	public void toggleLight(bool shouldbeOn){
		this.lit = shouldbeOn;
		this.light.enabled = this.lit;
	}
}
