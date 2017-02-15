﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEffectManager : MonoBehaviour {

	//For enable/disable effects
	public GameObject[] magicCircles;
	private GameObject[] tempCircles;
	//----------------------------------------//
	// 0 = stomp circle
	// 1 = dig circle
	public GameObject[] skillEffects;
	private GameObject[] tempEffects;
	//----------------------------------------//
	// 0 = dirt blast

	// Use this for initialization
	void Awake () {
		// Debug.Log("Starteffectman");
		tempCircles = new GameObject[magicCircles.Length];
		tempEffects = new GameObject[skillEffects.Length];
	}

//-----------SkillEffect-----------------------------------
	public void CreateStompCircle(Vector3 effectPos) {
		tempCircles[0] = ((GameObject)Instantiate(magicCircles[0], effectPos, Quaternion.identity));
	}
	public void DestroyStompCircle() {
		Destroy(tempCircles[0]);
		tempCircles[0] = null;
	}

	public void CreateDigStrikeCircle(Vector3 effectPos) {
		tempCircles[1] = (GameObject)Instantiate(magicCircles[1], effectPos, Quaternion.identity);
	}
	public void DestroyDigStrikeCircle() {
		Destroy(tempCircles[1]);
		tempCircles[1] = null;
	}

	public void CreateDirtBlast(Vector3 effectPos) {
		tempEffects[0] = (GameObject)Instantiate(skillEffects[0], effectPos, Quaternion.identity);
	}
	public void DestroyDirtBlast() {
		Destroy(tempEffects[0]);
		tempEffects[0] = null;
	}

	public void CreateRockSpikeSummoner(Vector3 effectPos) {
		tempEffects[1] = (GameObject)Instantiate(skillEffects[1], effectPos, Quaternion.identity);
	}
	public void DestroyRockSpikeSummoner() {
		Destroy(tempEffects[1]);
		tempEffects[1] = null;
	}
//---------------------------------------------------------
}