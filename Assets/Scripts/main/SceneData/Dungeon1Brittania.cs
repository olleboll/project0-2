using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon1Brittania : MonoBehaviour
{
	private Puzzle1Controller.Puzzle1Data puzzle1Data;
	private Puzzle2Controller.Puzzle2Data puzzle2Data;

// When we load from disk we'll need to some smart stuff to store and parse shit etc

	public Puzzle1Controller.Puzzle1Data GetPuzzle1Data(){
		return this.puzzle1Data;
	}

	public Puzzle2Controller.Puzzle2Data GetPuzzle2Data(){
		return this.puzzle2Data;
	}

	public void SetPuzzle1Data(Puzzle1Controller.Puzzle1Data data){
		this.puzzle1Data = data;
	}

	public void SetPuzzle2Data(Puzzle2Controller.Puzzle2Data data){
		this.puzzle2Data = data;
	}
}
