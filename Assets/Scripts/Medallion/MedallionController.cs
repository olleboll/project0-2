using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedallionController : MonoBehaviour
{
	private Animator animator;
	private PlayerData playerData;
	private SceneController sceneController;
	private PlayerInputController input;
	private PausController pausController;

	private bool isTeleporting = false;
	private bool isRenderingMedallion = false;
	private string willSwapToWorld = "";

	void Start()
	{
		this.animator = GetComponent<Animator>();

		this.input = Object.FindObjectOfType<PlayerInputController>();
		this.playerData = Object.FindObjectOfType<PlayerData>();
		this.sceneController = Object.FindObjectOfType<SceneController>();
		this.pausController = Object.FindObjectOfType<PausController>();

		dontRenderMedallion();
	}

	void Update(){
		// This should not be done on each update. Probably just trigger the UI
		// to show if this.input.teleport is true and then do something else.
		// Like different states here maybe?
		if (this.isTeleporting) {
			return;
		}
		if (this.input.teleport) {
			// omg do teleport if we are in a zone!
			List<string> options = playerData.GetTeleportOptions();
			// Should show some ui here instead. And more logic for this.
			// Maybe move this to somwhere else? Like a UI controller or something?
			if (options.Count > 0) {
				renderMedallion();
			} else if (this.isRenderingMedallion) {
				dontRenderMedallion();
			}
		} else if (this.isRenderingMedallion) {
			dontRenderMedallion();
		}
	}

	private void renderMedallion() {
		Debug.Log("Should render medallion");
		if (this.isRenderingMedallion) {
			// Do some selection checking with gamepad?
			// Or can unity do that for us?
		} else {
			this.pausController.EnableSlowmotion(0.1f);
			List<string> options = playerData.GetTeleportOptions();
			for (int i = 0; i < options.Count; i++) {
				Transform child = transform.Find("Worlds").Find(options[i]);
				if (child == null) {
					continue;
				}
				GameObject button = child.gameObject;
				button.SetActive(true);
				button.transform.Find("Text").gameObject.SetActive(true);
			}
			this.isRenderingMedallion = true;
		}
	}

	public void initializeSwap(string pickedWorld){
		this.willSwapToWorld = pickedWorld;
		this.animator.SetTrigger("start");
		this.isTeleporting = true;
		dontRenderMedallion();
	}

	private void dontRenderMedallion() {
		Transform[] allChildren = transform.Find("Worlds").GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
			Debug.Log(child);
			if (child.name != "Worlds") {
				child.gameObject.SetActive(false);
			}
		}
		this.isRenderingMedallion = false;
		this.pausController.DisableSlowmotion();
	}


	// this is called when the fade animation is near complete.
	// This works great now.. We'll see how it works when the scenes are bigger :O
	// Hello from the past if you are trying to fix that? That would be aweseome though
	// So kudos to you for making so much content! Vi är fan bäst! Fortsätt kriga, vi vill detta!
	public void SwapScene(){
		this.sceneController.SwapScene(this.willSwapToWorld);
		this.isTeleporting = false;
	}

}
