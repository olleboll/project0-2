using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoDialogue : DialogueController
{
	public Dialogue sickleDialogue;
	public Dialogue weedsDialogue;
	public Dialogue postWeedsDialogue;

	public GameObject weedsQuest;
	private int interactions = 0;

	protected override void additionalStart(){
		if (this.playerData.hasSickle) {
			this.currentDialogue = weedsDialogue;
		}
	}

	public override void decideNextDialogue(){
		if (this.interactions > 0) {
			this.currentDialogue = this.sickleDialogue;
		}
		if (this.playerData.hasSickle) {
			this.currentDialogue = this.weedsDialogue;
		}
		if (this.weedsQuest != null && this.weedsQuest.GetComponent<QuestController>().state.complete) {
			this.currentDialogue = this.postWeedsDialogue;
		}
	}

	public override void dialogueComplete(){
		this.interactions++;
		this.decideNextDialogue();
	}
}
