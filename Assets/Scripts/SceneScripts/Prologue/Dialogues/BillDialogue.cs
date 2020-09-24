using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillDialogue : DialogueController
{
	public Dialogue torchesDialogue;
	public Dialogue postTorchesDialogue;
	public Dialogue wolvesDialogue;
	public Dialogue postWolvesDialogue;

	// Must have completed the weeds to trigger torches quest
	public GameObject weedsQuest;
	public GameObject torchQuest;
	public GameObject wolvesQuest;
	public GameObject waitForDarkQuest;

	public override void decideNextDialogue(){

		if (this.weedsQuest != null && this.weedsQuest.GetComponent<QuestController>().state.complete) {
			this.currentDialogue = this.torchesDialogue;
			if (!this.torchQuest.GetComponent<QuestController>().state.active) {
				this.torchQuest.GetComponent<QuestController>().activateQuest();
			}
		}

		if (this.torchQuest != null && this.torchQuest.GetComponent<QuestController>().state.complete) {
			this.currentDialogue = this.postTorchesDialogue;
			if (!this.waitForDarkQuest.GetComponent<QuestController>().state.active) {
				this.waitForDarkQuest.GetComponent<QuestController>().activateQuest();
			}
		}

		if (this.waitForDarkQuest != null && this.waitForDarkQuest.GetComponent<QuestController>().state.complete) {
			this.currentDialogue = this.wolvesDialogue;
			if (!this.wolvesQuest.GetComponent<QuestController>().state.active) {
				this.wolvesQuest.GetComponent<QuestController>().activateQuest();
			}
		}

		if (this.wolvesQuest != null && this.wolvesQuest.GetComponent<QuestController>().state.complete) {
			this.currentDialogue = this.postWolvesDialogue;
		}
	}

	public override void dialogueComplete(){
		this.decideNextDialogue();
	}
}
