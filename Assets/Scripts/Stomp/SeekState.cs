using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : AIState {
    private readonly StatePatternAI enemy;
    private float seekTime;
    private Vector3 target;
    private float timer;
    public float speed;
    public float frequency;
    private Vector3 memPlayerPos;
    private float timerBeforeTarget;
	public string name{ get; }

    public SeekState(StatePatternAI statePatternAI){
		enemy = statePatternAI;
	}

    public void StartState()
    {
        Debug.Log("Seek Start");
        enemy.currentState = enemy.seekState;
        speed = 10f;
        frequency = 3f;
        seekTime = Random.Range(3f, 5f);
        timer = 0;
        timerBeforeTarget = 0;
		target = enemy.player.transform.position;
        memPlayerPos = target;
        enemy.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void UpdateState()
    {
        Seek();
    }

    public void EndState()
    {
        Debug.Log("Seek End");
        // enemy.animator.SetBool("isSeek", false);
        enemy.stompState.StartState();
    }

    public void StateChangeCondition()
    {

    }

    void Seek(){ 
        //Use animation instead of Look and Targeting method
        // Look();
        if(timer <= seekTime){
            timer += Time.deltaTime;
            Targeting();
        }
        else{
            EndState();
        }
    }

    void Look(){
        Vector3 lookPos = new Vector3(enemy.player.transform.position.x,
                                        enemy.player.transform.position.y + 8.5f,
                                        enemy.player.transform.position.z);
        enemy.transform.LookAt(lookPos);
    }

    void Targeting(){
        enemy.effectManager.PlaySeekAnim();
        target = new Vector3(enemy.player.transform.position.x,
                                    enemy.player.transform.position.y + 10f,
                                    enemy.player.transform.position.z);
        // target.position = new Vector3(enemy.player.transform.position.x - (Mathf.Sin(Time.time * frequency*2)),
        //                             enemy.player.transform.position.y + (10f + Mathf.Sin(Time.time * frequency)),
        //                             enemy.player.transform.position.z + (Mathf.Cos(Time.time * frequency*2)));
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target, speed * Time.deltaTime);
    }

    void Wait(float sec){
        
    }
}
