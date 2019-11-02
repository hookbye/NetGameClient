using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour {
    public enum ActionStatus //
    {
        IDLE,
        IDLE2,
        ATTACK01,
        ATTACK02,
        ATTACK03,
        BATTLEWALKFORWARD,
        BATTLEWALKBACKWARD,
        BATTLEWALKLEFT,
        BATTLEWALKRIGHT,
        DEFEND,
        GETHIT, 
        WALK,
        RUN,
        JUMP,
        TAUNT,
        DIE,
    }
    public ActionStatus status { get; set; }
    enum MoveStatus
    {
        WALK,
        RUN,
        JUMP,
    }
    private MoveStatus moveStatus;
    enum FightStatus
    {
        ATTACK,
        FREEZE,
        BEATTACKED,
    }
    private FightStatus fightStatus1;
    private FightStatus fightStatus2;
    //action param
    private float speed=1f;
    private float jumpSpeed = 2f;


    //StateMachine
    private Animator animator;
    private float lastActBeginTime = 0f;
    private float actDuringTime = 0f;

    public Transform target;

	// Use this for initialization
	void Start () {
        animator = GetComponentInChildren<Animator>();
        animator.Play("jump");
        DoAction("jump");
        DoAction("fly");
        DoAction("idle");
        int randNum = Random.Range(0, 17);
        switch(randNum%4){
            case 0:
                transform.Rotate(new Vector3(0, 0, 0));
                break;
            case 1:
                transform.Rotate(new Vector3(0,90,  0));
                break;
            case 2:
                transform.Rotate(new Vector3(0,180, 0));
                break;
            case 3:
                transform.Rotate(new Vector3(0,270, 0));
                break;
            default:
                transform.Rotate(new Vector3(0,0,0));
                break;
        }
        FindTarget();
    }

    void FindTarget()
    {
        if(target == null)
        {
            target = GameObject.Find("CubeTarget").transform;
        }
        
    }

    public void SetTarget(Transform targetT)
    {
        target = targetT;
    }
	
	// Update is called once per frame
	void Update () {
        if(target != null)
        {
            transform.LookAt(target);
            if (Vector3.Distance(transform.position,target.position) < 0.5f)
            {
                return;
            }
        }
        transform.position = transform.position + transform.forward * speed*Time.deltaTime;
	}

    // state machine
    public void DoAction(string actName,bool forceBreak = false)
    {
        int actHashId = Animator.StringToHash(actName);
        if(actHashId < 0 )
        {
            if(IsActionDone() || forceBreak)
            {
                lastActBeginTime = Time.time;
                animator.SetTrigger(actHashId);
                AnimatorStateInfo stateInfo = animator.GetNextAnimatorStateInfo(0);
                actDuringTime = stateInfo.length;
            }
        }
    }

    public bool IsActionDone()
    {
        return Time.time - lastActBeginTime > actDuringTime;
    }

    bool HandleInput()
    {
        bool hasInput = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoAction("jump");
            hasInput = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            hasInput = true;
        }
        return hasInput;
    }
}
