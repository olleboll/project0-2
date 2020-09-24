using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{


	private Queue<string> sentences;


	private Text nameField;
	private Text sentenceField;
	private GameObject dialogueWindow;
	private IEnumerator typer;
	void Start()
	{
		sentences = new Queue<string>();
		SceneManager.sceneLoaded += onSceneLoad;
		this.onSceneStart();
	}

	void onSceneLoad(Scene scene, LoadSceneMode mode){
		this.onSceneStart();
	}

	void onSceneStart(){
		this.dialogueWindow = GameObject.Find("UI").transform.Find("Dialogue").gameObject;
		this.nameField = this.dialogueWindow.transform.Find("title").GetComponent<Text>();
		this.sentenceField = this.dialogueWindow.transform.Find("sentence").GetComponent<Text>();
		this.dialogueWindow.SetActive(false);

	}

	public void startDialogue(Dialogue dialogue){
		Debug.Log("starting conversation");
		Debug.Log(dialogue.characterName);
		this.dialogueWindow.SetActive(true);
		this.nameField.text = dialogue.characterName;
		this.sentences.Clear();

		for (int i = 0; i < dialogue.sentences.Length; i++) {
			string sentence = dialogue.sentences[i];
			sentences.Enqueue(sentence);
		}
		this.displayNextSentence();
	}

	public bool displayNextSentence(){
		if (sentences.Count == 0) {
			endDialogue();
			return false;
		}

		string sentence = sentences.Dequeue();
		Debug.Log(sentence);
		if (this.typer != null) {
			StopCoroutine(this.typer);
		}
		this.typer = typeSentence(sentence);
		StartCoroutine(this.typer);
		return true;
	}

	IEnumerator typeSentence(string sentence){
		this.sentenceField.text = "";
		foreach (char letter in sentence.ToCharArray()) {
			this.sentenceField.text += letter;
			yield return new WaitForSeconds(0.02f);
		}
	}

	public void endDialogue(){
		Debug.Log("End");
		this.dialogueWindow.SetActive(false);
	}

}
