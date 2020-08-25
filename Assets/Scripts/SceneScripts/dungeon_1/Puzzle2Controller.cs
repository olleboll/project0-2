using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle2Controller : MonoBehaviour
{
	public bool solved = false;
	public GameObject switch1;
	public GameObject switch2;
	public GameObject switch3;
	public GameObject blocker;
	private TriggerActionController _switch1;
	private TriggerActionController _switch2;
	private TriggerActionController _switch3;
	private FadeAway blockerController;

	private Dungeon1Brittania dungeonData;

	public struct Puzzle2Data {
		public bool solved;
	}

	void Start(){
		this.blockerController = this.blocker.GetComponent<FadeAway>();
		this._switch1 = this.switch1.GetComponent<TriggerActionController>();
		this._switch2 = this.switch2.GetComponent<TriggerActionController>();
		this._switch3 = this.switch3.GetComponent<TriggerActionController>();

		this.dungeonData = Object.FindObjectOfType<Dungeon1Brittania>();
		if (this.dungeonData.GetPuzzle2Data().solved) {
			this.onSolve();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (this.solved) {
			return;
		}

		if (this._switch1.active && this._switch2.active && this._switch3.active) {
			this.onSolve();
			return;
		}

		if ((!this._switch1.active && (this._switch2.active || this._switch3.active)) || (!this._switch2.active && this._switch3.active)) {
			this._switch1.forceDeactivate();
			this._switch2.forceDeactivate();
			this._switch3.forceDeactivate();
			return;
		}
	}

	private void onSolve(){
		this.solved = true;
		this.blockerController.fadeAway();

		if (!this._switch1.active) {
			this._switch1.forceTrigger();
		}
		if (!this._switch2.active) {
			this._switch2.forceTrigger();
		}
		if (!this._switch3.active) {
			this._switch3.forceTrigger();
		}

		Puzzle2Data data = new Puzzle2Data();
		data.solved = true;
		this.dungeonData.SetPuzzle2Data(data);
	}
}
