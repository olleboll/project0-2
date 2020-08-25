using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TorchController : ActionController
{
	private FireLightController light;

	void Start()
	{
		this.light = transform.parent.Find("fire_light").GetComponent<FireLightController>();
	}

	public override void furtherActions(){
		Debug.Log("Toggling light");
		this.light.toggleLight();
	}
}
