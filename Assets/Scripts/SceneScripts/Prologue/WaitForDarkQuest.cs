using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForDarkQuest : QuestController
{
	private PrologueData sceneData;


	void Start()
	{
		this.sceneData = Object.FindObjectOfType<PrologueData>();

		this.state = this.sceneData.GetQuestData(this.GetType().Name);
		if (!this.state.activated) {
			this.state = new QuestData();
			this.state.active = this.startActive;
			this.state.complete = false;
		}
	}

	public override void activateQuest(){
		this.state.active = true;
		this.saveState();
	}


	public override void completeQuest(){
		if (this.state.complete) {
			// Let's not complete quest more than once.
			// this.complete should only be set to true in this method!
			return;
		}

		Debug.Log("All torches lit, done!");

		this.state.complete = true;
		this.saveState();
	}

	public override void autoCompleteQuest(){
	}

	private void saveState(){
		this.state.activated = true;
		this.sceneData.SetQuestData(this.GetType().Name, this.state);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (this.state.complete || !this.state.active) {
			return;
		}
		if (col.gameObject.name == "Player") {
			if (this.state.active) {
				this.completeQuest();
			}
		}
	}
}
