using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeedsQuest : QuestController
{
	private PrologueData sceneData;
	// this quest has a bunch of weeds to kill
	private List<GameObject> weeds;
	void Start()
	{
		this.sceneData = Object.FindObjectOfType<PrologueData>();
		this.state = this.sceneData.GetQuestData(this.GetType().Name);
		if (!this.state.activated) {
			this.state = new QuestData();
			this.state.active = this.startActive;
			this.state.complete = false;
		}

		this.weeds = new List<GameObject>();
		foreach (Transform child in transform) {
			weeds.Add(child.gameObject);
		}

		if (this.state.active) {
			this.activateQuest();
		}

		if (this.state.complete) {
			this.autoCompleteQuest();
		}
	}

	public override void activateQuest(){
		foreach (Transform child in transform) {
			child.gameObject.SetActiveRecursively(true);
		}
		this.state.active = true;
	}

	public override void completeQuest(){
		if (this.state.complete) {
			// Let's not complete quest more than once.
			// this.complete should only be set to true in this method!
			return;
		}
		Debug.Log("All weeds killed, done!");
		this.state.complete = true;
		this.saveState();
	}

	public override void autoCompleteQuest(){
		foreach (Transform child in transform) {
			child.gameObject.SetActiveRecursively(true);
		}
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}
		this.state.complete = true;
	}

	private void saveState(){
		this.state.activated = true;
		this.sceneData.SetQuestData(this.GetType().Name, this.state);
	}

	void Update()
	{
		if (this.state.complete || !this.state.active) {
			return;
		}
		this.weeds = new List<GameObject>();
		foreach (Transform child in transform) {
			weeds.Add(child.gameObject);
		}
		if (this.weeds.Count == 0) {
			Instantiate(this.successSound, transform.position, Quaternion.identity);
			this.completeQuest();
		}
	}
}
