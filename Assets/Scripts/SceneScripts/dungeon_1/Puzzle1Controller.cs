using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1Controller : MonoBehaviour
{
	public bool solved = false;
	public GameObject[] switchObjects;
	public GameObject blocker;
	private TriggerActionController[] switches;
	private FadeAway blockerController;

	private Dungeon1Brittania dungeonData;

	public struct Puzzle1Data {
		public bool solved;
	}

	void Start(){
		this.blockerController = this.blocker.GetComponent<FadeAway>();
		this.switches = new TriggerActionController[this.switchObjects.Length];
		for (int i = 0; i < switchObjects.Length; i++) {
			this.switches[i] = this.switchObjects[i].GetComponent<TriggerActionController>();
		}

		this.dungeonData = Object.FindObjectOfType<Dungeon1Brittania>();
		if (this.dungeonData.GetPuzzle1Data().solved) {
			this.onSolve();
		}
	}

	void Update()
	{
		if (this.solved) {
			return;
		}
		bool allActive = false;
		for (int i = 0; i < this.switches.Length; i++) {
			if (!switches[i].active) {
				return;
			}
		}
		this.onSolve();
	}

	private void onSolve(){
		this.solved = true;
		this.blockerController.fadeAway();
		for (int i = 0; i < this.switches.Length; i++) {
			if (!switches[i].active) {
				switches[i].forceTrigger();
			}
		}

		Puzzle1Data data = new Puzzle1Data();
		data.solved = true;
		this.dungeonData.SetPuzzle1Data(data);
	}

}
