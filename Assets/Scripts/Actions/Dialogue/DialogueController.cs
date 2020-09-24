using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
	public Dialogue greeting;
	public float distance = 2;

	protected Dialogue currentDialogue;
	protected bool onGoingDialogue = false;
	protected DialogueManager dialogueManager;
	protected PlayerInputController input;
	protected GameObject player;
	protected PlayerData playerData;
	void Start()
	{
		this.dialogueManager = Object.FindObjectOfType<DialogueManager>();
		this.input = Object.FindObjectOfType<PlayerInputController>();
		this.playerData = Object.FindObjectOfType<PlayerData>();
		this.player = GameObject.Find("Player");
		this.currentDialogue = this.greeting;
		this.additionalStart();
	}
	protected virtual void additionalStart(){
	}

	private void Update(){

		float distanceToPlayer = Vector3.Distance(transform.position, this.player.transform.position);

		if (this.input.actionNow && distanceToPlayer < this.distance) {
			if (this.onGoingDialogue) {
				this.onGoingDialogue = this.dialogueManager.displayNextSentence();
				if (!this.onGoingDialogue) {
					this.dialogueComplete();
				}
			} else {
				this.startDialogue();
				this.onGoingDialogue = true;
			}
		} else if (this.onGoingDialogue && distanceToPlayer > this.distance) {
			this.dialogueManager.endDialogue();
			this.onGoingDialogue = false;
		}
	}

	public void startDialogue(){
		this.decideNextDialogue();
		this.dialogueManager.startDialogue(this.currentDialogue);
	}
	public virtual void decideNextDialogue(){
	}
	public virtual void dialogueComplete(){
	}
}
