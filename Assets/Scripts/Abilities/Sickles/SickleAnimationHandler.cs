using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SickleAnimationHandler : MonoBehaviour
{
	private Animator anim;

	public enum Anim {
		idle,
		attack_normal,
		attack_spin
	}
	private bool isPlaying = false;
	private Anim currentlyPlaying;
	void Start(){
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

	public void animationComplete(Anim ani){
		this.isPlaying = false;
		this.currentlyPlaying = Anim.idle;
		this.anim.Play(Anim.idle.ToString());
	}

	public void stopAnimation(){
		this.animationComplete(this.currentlyPlaying);
	}

	public bool getIsPlaying(){
		return this.isPlaying;
	}

	public event Action onDealDamage;
	public void dealDamage(){
		onDealDamage();
	}
}
