using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActionController : MonoBehaviour
{
	public bool active = false;
	public bool canToggle = true;
	public bool hasBeenSwitched = false;

	private Animator anim;
	void Start(){
		this.anim = GetComponent<Animator>();
	}
	public void triggerAction(GameObject hitter, GameObject hitWith){
		if (!canToggle && hasBeenSwitched) {
			return;
		}
		this.active = !this.active;
		this.anim.SetBool("active", this.active);
		this.hasBeenSwitched = true;
	}

	public void forceDeactivate(){
		this.active = false;
		this.anim.SetBool("active", this.active);
		if (!this.canToggle) {
			this.hasBeenSwitched = false;
		}
	}

	public void forceTrigger(){
		this.active = !this.active;
		this.anim.SetBool("active", this.active);
		this.hasBeenSwitched = true;
	}

}
