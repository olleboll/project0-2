using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionController : MonoBehaviour
{
	public bool useInput = true;
	public bool isRunningActionAnimation;

	private Animator anim;
	private PlayerInputController input;
	private bool actionAble = false;
	private bool actionCooldown = false;

	void Awake()
	{
		this.input = Object.FindObjectOfType<PlayerInputController>();
		this.anim = transform.parent.gameObject.GetComponent<Animator>();
		Debug.Log(this.input);
		Debug.Log(this.anim);
	}

	void Start(){
		this.anim.SetBool("actionAnimation", this.isRunningActionAnimation);
	}

	// Update is called once per frame
	public virtual void Update()
	{
		if (this.anim.GetBool("actionAnimation") != this.isRunningActionAnimation) {
			this.anim.SetBool("actionAnimation", this.isRunningActionAnimation);
		}

		if (this.useInput && this.input.action && this.actionAble && !this.actionCooldown) {
			this.actionCooldown = true;
			StartCoroutine(offCooldown());
			this.toggleAction();
		}
	}

	IEnumerator offCooldown(){
		yield return new WaitForSeconds(1);
		this.actionCooldown = false;
	}

	// this is originally for a torch
	public virtual void toggleAction(){
		if (this.anim == null) {
			Debug.Log("No animator assigned. Override toggleAction to get rid of this warning.");
			return;
		}

		Debug.Log("TOGGLED");

		this.isRunningActionAnimation = !this.isRunningActionAnimation;
		this.anim.SetBool("actionAnimation", this.isRunningActionAnimation);
		this.furtherActions();
	}

	public virtual void furtherActions(){
	}

	void OnTriggerEnter2D(Collider2D other){
		this.actionAble = true;
		Debug.Log("Is colliding");
	}

	void OnTriggerExit2D(Collider2D other){
		this.actionAble = false;
	}
}
