using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchQuest : QuestController
{
	private PrologueData sceneData;
	// this quest has a bunch of torches to light
	private List<GameObject> torches;
	private bool activated = false;
	private bool ready = false;
	void Start()
	{
		this.sceneData = Object.FindObjectOfType<PrologueData>();
		this.state = this.sceneData.GetQuestData(this.GetType().Name);
		if (!this.state.activated) {
			this.state = new QuestData();
			this.state.active = this.startActive;
			this.state.complete = false;
		}

		this.torches = new List<GameObject>();
		foreach (Transform child in transform) {
			torches.Add(child.gameObject);
			child.gameObject.SetActiveRecursively(this.state.active);
		}

		this.state.activated = false;
	}

	public override void activateQuest(){
		if (this.state.active) {
			return;
		}
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

		Debug.Log("All torches lit, done!");

		this.state.complete = true;
		this.saveState();
	}

	public override void autoCompleteQuest(){
		foreach (Transform child in transform) {
			child.gameObject.SetActiveRecursively(true);
		}
		this.completeQuest();
	}

	private void saveState(){
		this.state.activated = true;
		this.sceneData.SetQuestData(this.GetType().Name, this.state);
	}

	IEnumerator waitToBeReady(){
		yield return new WaitForSeconds(0.2f);
		this.ready = true;
	}

	void Update()
	{
		if (!this.activated) {
			if (this.state.active && !this.state.complete) {
				foreach (Transform child in transform) {
					child.Find("fire_light").GetComponent<TorchController>().setActionAnimation(true);
				}
				this.activated = true;
				StartCoroutine(waitToBeReady());
				this.saveState();
			}
			if (this.state.complete) {
				StartCoroutine(waitToBeReady());
				this.autoCompleteQuest();
			}
			return;
		}
		if (this.state.complete || !this.state.active || !this.ready) {
			return;
		}
		bool allLit = true;
		foreach (GameObject torch in this.torches) {
			if (!torch.transform.Find("fire_light").GetComponent<FireLightController>().lit) {
				allLit = false;
			}
		}
		if (allLit) {
			Instantiate(this.successSound, transform.position, Quaternion.identity);
			this.completeQuest();
		}
	}
}
