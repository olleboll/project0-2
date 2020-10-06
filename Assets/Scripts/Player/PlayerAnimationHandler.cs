using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
	public enum Anim {
		idle,
		spin
	}
	private Animator anim;
	private Anim currentlyPlaying;
	public bool isPlaying = false;

	void Start()
	{
		this.anim = GetComponent<Animator>();
	}

	public void play(Anim ani){
		Debug.Log("Gonna play: " +ani);
		this.currentlyPlaying = ani;
		this.isPlaying = true;
		StartCoroutine(startAnimationNextFrame(ani));
	}

	IEnumerator startAnimationNextFrame(Anim ani){
		yield return null;
		this.anim.Play(ani.ToString());
	}

	public void animationComplete(){
		Debug.Log("Animatoin complete");
		this.isPlaying = false;
		this.anim.Play(Anim.idle.ToString());
	}

	public void updateAnimation(Vector3 dir, float speed){
		if (!this.isPlaying) {
			this.anim.SetFloat("dirX", dir.x);
			this.anim.SetFloat("dirY", dir.y);
			this.anim.SetFloat("speed", speed);
		}
	}
}
