using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
	public string name {
		get; private set;
	}
	public string description {
		get; private set;
	}

	private State(string name, string description){
		this.name = name;
		this.description = description;
	}

	public static State idle = new State("idle", "Not doing anything");
}
