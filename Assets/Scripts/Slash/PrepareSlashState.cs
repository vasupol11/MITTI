﻿using UnityEngine;
using System.Collections;

public class PrepareSlashState : AIState {
	private readonly StatePatternAI AI;
	public string name{ get;}
	private Quaternion targetRotation;
	private float timetoSlash;
	private float slashDegrees;

	public PrepareSlashState(StatePatternAI statePatternAI){
		AI = statePatternAI;
		name = "PrepareSlashState";
	}

	public void StartState(){
		AI.currentState = AI.prepareSlashState;
//		AI.GetComponent<Rigidbody>().isKinematic = true;
//		targetRotation =  Quaternion.AngleAxis(Random.Range(0f, 360f), AI.transform.right);
		timetoSlash = 0f;
		slashDegrees = Random.Range (0f, 180f);
		Debug.Log (slashDegrees);
	}

	public void UpdateState(){
//		AI.transform.rotation= Quaternion.Lerp (AI.transform.rotation, targetRotation , Time.deltaTime); 
		AI.transform.Rotate (0f, 0f, 1f * slashDegrees*2 * Time.deltaTime);
		if(-Mathf.Round(AI.transform.eulerAngles.z)+ Mathf.Round(slashDegrees) <10f) { 
//			AI.transform.Rotate(new Vector3(0,0,-180)); 
			StateChangeCondition ();
		}
//		if (timetoSlash >2f) {
//			StateChangeCondition ();
//		}
		timetoSlash += Time.deltaTime;
//		Debug.Log (timetoSlash);
	}

	public void EndState(){
//		AI.GetComponent<Rigidbody>().isKinematic = false;
	}

	public void StateChangeCondition(){
		AI.slashState.StartState ();
	}
}