using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDuration : MonoBehaviour
{
	public float pauseAfter = 0.5f;
	public float destroyAfter = 0.5f;
	void Start()
	{
		StartCoroutine(pausEffect());
		StartCoroutine(cleanUpEffect());
	}

	IEnumerator pausEffect(){
		yield return new WaitForSeconds(pauseAfter);
		// If there is ParticleSystem on the game object.
		// This could be used for audio aswell though?
		if (gameObject != null && gameObject.GetComponent<ParticleSystem>() != null) {
			gameObject.GetComponent<ParticleSystem>().Pause(true);
		}
	}

	IEnumerator cleanUpEffect(){
		yield return new WaitForSeconds(destroyAfter);
		Destroy(gameObject);
	}
}
