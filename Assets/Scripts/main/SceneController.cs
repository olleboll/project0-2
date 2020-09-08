using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{

	// World scene enum should be the names of the scenes
	// that the player can teleport to.
	public enum WorldScene {
		None = 0,
		dev_objects = 1 << 0,
		boss_1 = 1 << 1,
		SampleScene = 1 << 2,
		abisko = 1 << 3,
		Elyn = 1 << 4,
		Brittania = 1 << 5,
		All = ~0,
	}

	// LocalScene are scenes that are swapped automatically
	// because of position. Like going into a house, or dungeon etc.

	public delegate void SceneLoadDelegate(string _scene);

	private SceneLoadDelegate m_SceneLoadDelegate;
	private bool m_SceneIsLoading;
	private string m_TargetScene;
	private Scene m_OldScene;
	private string m_currentScene;
	private PlayerData playerData;

	private Animator levelLoaderAnimation;

	private void Awake(){
		Debug.Log("Scene controller init");
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void Start(){
		Debug.Log("Scene controller is running start!");
		this.playerData = GetComponent<PlayerData>();

		Debug.Log(SceneManager.GetActiveScene().name);
		string currentSceneName = SceneManager.GetActiveScene().name;
		string scenePrefix = this.playerData.GetWorldLocationPrefix();

		if (!currentSceneName.ToLower().Contains(scenePrefix)) {
			char[] seperator = {'_'};
			string[] prefix = currentSceneName.Split(seperator);
			this.playerData.SetWorldLocationPrefix(prefix[0]+"_");
		}
	}

	public void SwapScene(string newScene, SceneLoadDelegate _sceneLoadDelegate=null, bool _reload=false){
		if (!SceneCanBeLoaded(newScene, _reload)) {
			return;
		}

		string scenePrefix = this.playerData.GetWorldLocationPrefix();

		m_SceneIsLoading = true;
		m_TargetScene = scenePrefix + newScene;
		m_SceneLoadDelegate = _sceneLoadDelegate;
		m_OldScene  = SceneManager.GetActiveScene();
		StartCoroutine("LoadScene");
	}

	private IEnumerator LoadScene(){
		SceneManager.LoadScene(m_TargetScene);
		yield return null;
	}

	private async void OnSceneLoaded(UnityEngine.SceneManagement.Scene _scene, LoadSceneMode _mode) {
		if (m_SceneLoadDelegate != null) {
			try {
				m_SceneLoadDelegate(m_TargetScene);
			} catch (System.Exception) {
				Debug.LogWarning("Could not call delgate for old scene");
			}
		}
		m_SceneIsLoading = false;
	}
	public string currentSceneName {
		get {
			return SceneManager.GetActiveScene().name;
		}
	}

	private bool SceneCanBeLoaded(string _scene, bool _reload){
		if (currentSceneName == _scene) {
			Debug.Log("You are already on "+_scene+" scene");
			return false;
		} else if (m_SceneIsLoading) {
			return false;
		}
		return true;
	}

}
