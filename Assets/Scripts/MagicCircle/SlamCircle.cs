﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamCircle : MonoBehaviour {
	private float health;
	[HideInInspector] public bool isBreak;
	private GameObject circle;
	private Color hitColor, normalColor;
	private Collider circleCollider;
	public GameObject slamBreak;
	public AudioSource crackSound;
	// Use this for initialization
	void Start () {
		health = 100;
		isBreak = false;

		circle = this.transform.GetChild(0).gameObject;
		circleCollider = this.GetComponent<Collider>();
		circleCollider.enabled = false;

		normalColor = circle.GetComponent<Renderer>().material.GetColor("_TintColor");
		hitColor = new Color(normalColor.r, normalColor.g / 4f, normalColor.b / 4f, 1f);	
	}
	
	// Update is called once per frame
	void Update () {
		circleCollider.enabled = true;
		if(StatePatternAI.instance.health <= 0){
			Destroy(this.gameObject);
		}
		// if(circle.GetComponent<SelfRotate>().isOkColor){
		// 	circleCollider.enabled = true;
		// }

		if(health <= 0f) {
			isBreak = true;
			slamBreak.SetActive(true);
			slamBreak.transform.SetParent(null);
			circle.SetActive(false);
			this.transform.SetParent(null);
			health = 100f;
		}
		else{
			CircleColorRecover();
			this.transform.LookAt(StatePatternAI.instance.player.transform);
		}
	}

	void OnTriggerEnter(Collider col){
		// Debug.Log("Hit Collider"+col);
		if(col.tag.Equals("playersword") || col.tag.Equals("projectile") && health > 0){
			// Debug.Log("HitSlamCircle");
			crackSound.Play();
			circle.GetComponent<Renderer>().material.SetColor("_TintColor", hitColor);
			health -= 20f;
		}
	}

	void CircleColorRecover(){
		Color nowColor = circle.GetComponent<Renderer>().material.GetColor("_TintColor");
		if(nowColor.g < normalColor.g){
			nowColor.g += Time.deltaTime*0.5f;
			nowColor.b += Time.deltaTime*0.5f;
			circle.GetComponent<Renderer>().material.SetColor("_TintColor", nowColor);
		}
	}
}
