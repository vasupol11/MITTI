using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SlamState : AIState {
    private readonly StatePatternAI AI;
    private Vector3 attackTarget;
    private Vector3 moveToTarget;
    public bool isStop, isStun, isEnd;
    private GameObject slamCol;
	public string name{ get;}
    public List<AIState> choice{ get;set; }

    public SlamState(StatePatternAI statePatternAI){
		AI = statePatternAI;
        choice = new List<AIState>();
	}
    public void StartState()
    {
        
    }
    public void StartState(Vector3 attackTarget)
    {
        Debug.Log("Slam Start");
        AI.currentState = AI.slamState;
        isStop = false;
        isStun = false;
        isEnd = false;
        AI.speed = 40f;
        this.attackTarget = attackTarget;
        moveToTarget = this.attackTarget + AI.transform.forward*30f;

        AI.effectManager.DestroyCircleByName(MagicCircleName.SLAM_CIRCLE);
        AI.effectManager.RemoveCircleFromDictByName(MagicCircleName.SLAM_CIRCLE);
        slamCol = AI.effectManager.CreateAndReturnEffectByName(EffectName.SLAM_COLLIDER ,AI.transform.position + AI.transform.forward*1.05f);
        slamCol.transform.SetParent(AI.transform);
    }

    public void UpdateState()
    {
        Slam();
    }

    public void EndState()
    {
        AI.effectManager.DestroyEffectByName(EffectName.SLAM_COLLIDER);
        AI.effectManager.RemoveEffectFromDictByName(EffectName.SLAM_COLLIDER);
        // Destroy(slamCol);
        if(Player.instance.GetComponent<PlayerStat>().isHitSlam == true){
            Player.instance.GetComponent<Rigidbody>().AddForce(AI.transform.forward*10f, ForceMode.Impulse);
        }
        Player.instance.GetComponent<PlayerStat>().isHitSlam = false;
        Player.instance.transform.SetParent(null);
        
        AI.prepareSlamState.StartState();
    }

    public void StateChangeCondition()
    {

    }

    void Slam(){
        if(isStun){
            AI.effectManager.DestroyEffectByName(EffectName.SLAM_COLLIDER);
            AI.effectManager.RemoveEffectFromDictByName(EffectName.SLAM_COLLIDER);
            isEnd = true;
            // Destroy(slamCol);
            // Player.instance.GetComponent<PlayerControl>().isHitSlam = false;
            AI.GetComponent<Rigidbody>().velocity = Vector3.zero;
            AI.stunState.StartState(5f, AI.GetComponent<Rigidbody>().velocity);
        }
        else if(AI.transform.position != moveToTarget && !isStop){
            // Debug.Log("Moveeeee");
            AI.transform.position = Vector3.MoveTowards(AI.transform.position,
                                                    moveToTarget,
                                                    AI.speed * Time.deltaTime);
                                                    // (speed += (speed/1.25f*Time.deltaTime)) * Time.deltaTime);
        }
        else{
            EndState();
        }
    }
}
