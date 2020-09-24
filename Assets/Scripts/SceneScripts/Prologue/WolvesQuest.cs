using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolvesQuest : QuestController
{
	private PrologueData sceneData;
	// this quest has a bunch of wolves to kill
	private List<GameObject> wolves;
	void Start()
	{
		this.sceneData = Object.FindObjectOfType<PrologueData>();
		this.state = this.sceneData.GetQuestData(this.GetType().Name);
		if (!this.state.activated) {
			this.state = new QuestData();
			this.state.active = this.startActive;
			this.state.complete = false;
		}

		this.wolves = new List<GameObject>();
		foreach (Transform child in transform) {
			wolves.Add(child.gameObject);
			child.gameObject.SetActiveRecursively(this.state.active);
		}

		if (this.state.active) {
			this.activateQuest();
		}

		if (this.state.complete) {
			this.autoCompleteQuest();
		}
	}

	public override void activateQuest(){
		if (this.state.active) {
			return;
		}
		foreach (Transform child in transform) {
			child.gameObject.SetActiveRecursively(true);
		}
		this.state.active = true;
		this.saveState();
	}

	public override void completeQuest(){
		if (this.state.complete) {
			// Let's not complete quest more than once.
			// this.complete should only be set to true in this method!
			return;
		}
		Debug.Log("All wolves killed, done!");
		this.state.complete = true;
	}

	public override void autoCompleteQuest(){
		foreach (Transform child in transform) {
			child.gameObject.SetActiveRecursively(false);
		}
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}
		this.completeQuest();
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
		this.wolves = new List<GameObject>();
		foreach (Transform child in transform) {
			wolves.Add(child.gameObject);
		}
		if (this.wolves.Count == 0) {
			Instantiate(this.successSound, transform.position, Quaternion.identity);
			this.completeQuest();
		}
	}
}
