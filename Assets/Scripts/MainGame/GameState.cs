﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameState : MonoBehaviour {
	private SceneController sceneCon = new SceneController();
	private Color black = Color.black;
	private Color white = Color.white;


	public Material skyboxChaos;
	public Material skyboxNorm;
	public GameObject normalLight, chaosLight, outerEnvi, tutorEnvi, tutorAI;
	public bool tutorialState, AIOpen, afterAIOpen, mainGame, end, isFallingPlay, isNearFallPlay, isDestroyAI, isPlayEarth, isWaitDie;
	public PlaySound fallingWindSoundPlayer, nearFloorSoundPlayer, earthQuakeSoundPlayer;
	public GameObject dieSound;
	public GameObject sceneDestroyer, sceneProps;
	public GameObject playerTransFilter;
	public GameObject dieBG, dieText;
	// Use this for initialization
	private static GameState _instance;
		public static GameState instance
		{
			get
			{
				if ( _instance == null )
				{
					_instance = FindObjectOfType<GameState>();
				}
				return _instance;
			}
		}
	void Start () {
		Debug.Log("Game Start");

		tutorialState = true;
		AIOpen = false;
		afterAIOpen = false;
		mainGame = false;
		end = false;
		isFallingPlay = false;
		isNearFallPlay = false;
		isDestroyAI = false;
		isPlayEarth = false;
		isWaitDie = false;
		Physics.IgnoreCollision(StatePatternAI.instance.body.GetComponent<Collider>(), sceneDestroyer.GetComponent<Collider>());
		Physics.IgnoreLayerCollision(10, 9);

		black.a = 0;
		white.a = 0;
	}
	void OnEnable(){
		_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		MainGameState();
	}
	void MainGameState(){
		if(tutorialState) {
			TutorialState();
		}
		if(AIOpen) {
			AIOpening();
		}
		if(afterAIOpen) {
			AfterAIOpening();
		}

		if(mainGame) {
			RotateSkyBox();
			Debug.Log(Player.instance.GetComponent<PlayerStat>().health);
			if(StatePatternAI.instance.health <= 0){
				Debug.Log("Purify");
				mainGame = false;
				end = true;
			}
			if(Player.instance.GetComponent<PlayerStat>().health <= 0){
				Debug.Log("You Die");
				StatePatternAI.instance.stopState.StartState();
				// mainGame = false;
				if (!isWaitDie) {
					dieBG.SetActive(true);
					isWaitDie = true;
					dieSound.SetActive(true);
					StartCoroutine(WaitDie());
				}
				black.a += 0.005f;
				white.a += 0.005f;

				dieBG.GetComponent<Image>().color = black;
				dieText.GetComponent<Image>().color = white;
			}
		}

		if(end) {
			Debug.Log("End");
			if(!isDestroyAI){
				Player.instance.GetComponent<Rigidbody>().useGravity = false;
				Player.instance.GetComponent<Rigidbody>().isKinematic = true;
				RenderSettings.skybox = skyboxNorm;
				normalLight.SetActive(true);
				chaosLight.SetActive(false);
				Destroy(StatePatternAI.instance.gameObject);
				outerEnvi.SetActive(true);
				isDestroyAI = true;
			}

			Player.instance.transform.position = Player.instance.transform.position + Vector3.up * 0.1f;
		}
	}

	IEnumerator WaitDie(){
		yield return new WaitForSeconds(10f);
		sceneCon.ChangeScene(SceneController.MAIN_MENU);
	}

	void TutorialState(){
		if(tutorAI.GetComponent<TutorialAI>().isEndTutor && !isFallingPlay){
			Destroy(tutorEnvi.gameObject);
			fallingWindSoundPlayer.Play();
			isFallingPlay = true;
		}
		if(Player.instance.transform.position.y <= 150f && !isNearFallPlay){
			nearFloorSoundPlayer.Play();
			fallingWindSoundPlayer.isStartFadeOut = true;
			isNearFallPlay = true;
		}
		if(Player.instance.GetComponent<PlayerControl>().isOnFloor == true){
			if(!isPlayEarth){
				earthQuakeSoundPlayer.Play();
				isPlayEarth = true;
			}
			Player.instance.GetComponent<Rigidbody>().isKinematic = true;
			
			tutorialState = false;
			AIOpen = true;

			outerEnvi.SetActive(false);
			Destroy(tutorAI);
			
			StatePatternAI.instance.stopState.EndState();
			StatePatternAI.instance.openingState.StartState();
		}
	}

	void AIOpening(){
		if(StatePatternAI.instance.transform.position.y > 4.4f){
			sceneDestroyer.SetActive(true);
		}

		if(StatePatternAI.instance.currentState != StatePatternAI.instance.openingState){
			AIOpen = false;

			normalLight.SetActive(false);
			chaosLight.SetActive(true);
			RenderSettings.skybox = skyboxChaos;
			RenderSettings.ambientMode = AmbientMode.Skybox;
			RenderSettings.ambientIntensity = 5f;
			skyboxChaos.SetFloat("_Exposure", 8f);
			Debug.Log("force");
			sceneDestroyer.GetComponent<SceneDestroyer>().force = 10f;
			sceneDestroyer.GetComponent<SceneDestroyer>().upwardsModifier = 5f;
			
			afterAIOpen = true;
		}
	}

	void AfterAIOpening(){
		if(skyboxChaos.GetFloat("_Exposure") > 1f){
			skyboxChaos.SetFloat("_Exposure", skyboxChaos.GetFloat("_Exposure") - 0.1f);
			RenderSettings.ambientIntensity -= 0.055f;
		}
		else{
			afterAIOpen = false;
			mainGame = true;
			Destroy(sceneDestroyer);
			Destroy(sceneProps);
			earthQuakeSoundPlayer.Stop();
		}
	}

	void RotateSkyBox(){
		if(skyboxChaos.GetFloat("_Rotation") >= 360f){
			skyboxChaos.SetFloat("_Rotation", 0f);
		}
		else{
			skyboxChaos.SetFloat("_Rotation", skyboxChaos.GetFloat("_Rotation") + 0.01f);
		}
	}


}
