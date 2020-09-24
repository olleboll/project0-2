using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestController : MonoBehaviour
{
	public bool startActive = false;
	public GameObject successSound;

	public abstract void activateQuest();
	public abstract void completeQuest();
	public abstract void autoCompleteQuest();

	public struct QuestData {
		public bool active;
		public bool complete;
		public bool activated;
	}

	public QuestData state = new QuestData();
}
