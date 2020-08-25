using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAllSwitchesPuzzleController : MonoBehaviour
{
	public bool solved = false;
	public GameObject spawnOnSolved;
	private TriggerActionController[] switches;
	// Start is called before the first frame update

	public struct PuzzleData {

	}

	void Start()
	{
		Component[] _switches = GetComponentsInChildren(typeof(TriggerActionController));
		this.switches = new TriggerActionController[_switches.Length];
		for (int i = 0; i < _switches.Length; i++) {
			this.switches[i] = _switches[i].gameObject.GetComponent<TriggerActionController>();
		}

		// Should get data from GameDataController
		// Which should store data that should be saved between scenes.
		// So whether this puzzle is solved or not should be saved.
		// Should even store the switched that has been activated etc.

	}

	// Update is called once per frame
	void Update()
	{
		if (this.solved) {
			// Do solved things?
			return;
		}
		bool allActive = false;
		for (int i = 0; i < switches.Length; i++) {
			if (!switches[i].active) {
				return;
			}
		}
		this.solved = true;
		Debug.Log("lol my first puzzle!");

		Instantiate(spawnOnSolved, transform.position, Quaternion.identity);
	}
}
