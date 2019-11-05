﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour {
    public enum ActionStatus //
    {
        NONE,
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
    //object type
    enum CtrlType
    {
        PLAYER,
        COMPUTER,
    }
    private CtrlType ctrlType = CtrlType.COMPUTER;

    private Dictionary<string, ActionStatus> actToStatus = new Dictionary<string, ActionStatus>();
    private Dictionary<ActionStatus,string> statusToAct = new Dictionary<ActionStatus, string>();
    //action param
    private float speed=1f;
    private float jumpSpeed = 2f;


    //StateMachine
    private Animator animator;
    private float lastActBeginTime = 0f;
    private float actDuringTime = 0f;

    // interactive with other objects
    public SimpleAI target;
    public GameScene battleManager;

    //battle logic
    private float maxHp=10f;
    private float hp=10f;
    private float attack=2f;
    private float defend = 1f;
    public float searchScope = 2f;
    public float battleScope = 1f;

	// Use this for initialization
	void Start () {
        animator = GetComponentInChildren<Animator>();
        InitActStatusSwitchMap();
        DoAction("run", true);
        FindTarget();
    }

    public void BindBattleManager(GameScene manager)
    {
        battleManager = manager;
    }

    void InitActStatusSwitchMap()
    {
        BindActionAndStatus("idle", ActionStatus.IDLE);
        BindActionAndStatus("idle_02", ActionStatus.IDLE2);
        BindActionAndStatus("attack_01", ActionStatus.ATTACK01);
        BindActionAndStatus("attact_02", ActionStatus.ATTACK02);
        BindActionAndStatus("attact_03", ActionStatus.ATTACK03);
        BindActionAndStatus("walk", ActionStatus.WALK);
        BindActionAndStatus("jum", ActionStatus.JUMP);
        BindActionAndStatus("run", ActionStatus.RUN);
        BindActionAndStatus("taunt", ActionStatus.TAUNT);
        BindActionAndStatus("die", ActionStatus.DIE);
    }

    void BindActionAndStatus(string act,ActionStatus status)
    {
        actToStatus.Add(act, status);
        statusToAct.Add(status,act);
    }

    void FindTarget()
    {
        if(target == null)
        {
            target = battleManager.FindTarget(this);
        }
        
    }

    public void SetTarget(SimpleAI targetT)
    {
        target = targetT;
    }
	
	

    // state machine
    public void DoAction(string actName,bool forceBreak = false)
    {
        int actHashId = Animator.StringToHash(actName);

        if (status == ActionStatus.DIE)
            return;
        if(IsActionDone() || forceBreak)
        {
            lastActBeginTime = Time.time;
            animator.SetTrigger(actHashId);
            AnimatorStateInfo stateInfo = animator.GetNextAnimatorStateInfo(0);
            actDuringTime = stateInfo.length;
            status = ChangeActionToStatus(actName);
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

    public ActionStatus ChangeActionToStatus(string action)
    {
        if (actToStatus.ContainsKey(action))
        {
            return actToStatus[action];
        }
        return ActionStatus.NONE;
    }

    public string GetStatusAction(ActionStatus status)
    {
        if(statusToAct.ContainsKey(status))
        {
            return statusToAct[status];
        }
        return "";
    }
    public float GetAttack()
    {
        return attack;
    }

    void BeAttacked(SimpleAI attacker)
    {
        float hurt = attacker.GetAttack() - defend;
        hp = hp - hurt;
        Debug.Log("hp:"+hp);
        if(hp < 0)
        {
            DoAction("die",true);
        }
    }

    public bool IsDie()
    {
        return status == ActionStatus.DIE;
    }

    void Attack(SimpleAI defender)
    {
        if (IsBeAttacked() && !IsActionDone())
            return;
        if (IsActionDone() || !IsAttacking())
        {
            if (IsActionDone() && IsAttacking())
            {
                defender.BeAttacked(this);
            }
            DoAction("attack_01", true);
        }
    }

    bool IsAttacking()
    {
        return status == ActionStatus.ATTACK01 || status == ActionStatus.ATTACK02 || status == ActionStatus.ATTACK03;
    }

    bool IsBeAttacked()
    {
        return status == ActionStatus.GETHIT || status == ActionStatus.DEFEND;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        if (target != null && target)
        {
            transform.LookAt(target.transform);

            if (Vector3.Distance(transform.position, target.transform.position) < battleScope)
            {
                Attack(target.gameObject.GetComponent<SimpleAI>());
                return;
            }
        }
        
        if (IsActionDone())
        {
            //DoAction("idle");
            if (status == ActionStatus.RUN)
            {
                DoAction("run");
            }

        }
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
    }
}
