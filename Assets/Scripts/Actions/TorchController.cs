using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TorchController : ActionController
{
	private FireLightController light;

	void Start()
	{
		this.light = GetComponent<FireLightController>();
	}

	public override void furtherActions(){
		Debug.Log("Toggling light");
		Debug.Log(this.isRunningActionAnimation);
		Debug.Log(this.light);
		this.light.toggleLight(!this.isRunningActionAnimation);
	}
}
