using System.Collections;
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
    public ActionStatus status;
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

    enum AIType
    {
        PATROL,
        BATTLE,
    }
    AIType aiType = AIType.BATTLE;

    private Dictionary<string, ActionStatus> actToStatus = new Dictionary<string, ActionStatus>();
    private Dictionary<string, float> actionLastTimes = new Dictionary<string, float>();
    private Dictionary<ActionStatus, string> statusToAct = new Dictionary<ActionStatus, string>();
    //action param
    private float speed=5f;
    private const float jumpSpeed = 2f;
    private const float walkSpeed = 5f;
    public float steerSpeed = 30f;

    //player control
    public bool isTurnningRight = false;
    public bool isTurnningLeft = false;
    public bool isForward = false;
    public bool isBackward = false;
    public bool isLockFaceforward = false;
    public bool isRun = false;
    public bool isJump = false;

    //
    private float updateDuringCD = 1.0f;
    private float updateStampt = 0f;
    //StateMachine
    private Animator animator;
    private float lastActBeginTime = 0f;
    private float actDuringTime = 0f;

    // interactive with other objects
    public SimpleAI target;
    public GameScene battleManager;

    //battle logic
    private int id = 0;
    private float maxHp=10f;
    private float hp=10f;
    private float attack=2f;
    private float defend = 1f;
    public float searchScope = 50f;
    public float searchCD = 2;
    float lastSearchTime = 0f;
    public int searchNum = 0;
    public const int MaxSearchCount=2;
    public float patrolMaxDis = 10f;
    public float patrlDis = 0f;
    Vector3 nextPos = Vector3.zero;
    Vector3 nextDir = Vector3.zero;
    

    public float battleScope = 2f;

    public float attackCD = 0f;
    public float attackInitCD = 0.9f;
    public float attackBeginTime = 0f;
    
    public enum EntyColor{
        RED,
        BLUE,
    }
    public EntyColor entyColor;
    public void SetEntyColor(EntyColor color)
    {
        entyColor = color;
    }

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
        id = manager.GenBattleId();
    }

    public void LoadPlayerSetting()
    {
        ctrlType = CtrlType.PLAYER;
    }

    // display actions init
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
        BindActionAndStatus("defend", ActionStatus.DEFEND);
        BindActionAndStatus("die", ActionStatus.DIE);
    }

    void BindActionAndStatus(string act,ActionStatus stat)
    {
        actToStatus.Add(act, stat);
        statusToAct.Add(stat,act);
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
            //Debug.LogWarning("real do action.."+IsActionDone()+" "+forceBreak);
            animator.SetTrigger(actHashId);
            AnimatorStateInfo stateInfo = animator.GetNextAnimatorStateInfo(0);
            actDuringTime = stateInfo.length;
            //if (id == 2)
            //    Debug.Log(id+" _"+Time.time+" :"+actName + " " + actDuringTime);
            status = ChangeActionToStatus(actName);
            if(!actionLastTimes.ContainsKey(actName))
            {
                actionLastTimes.Add(actName, actDuringTime);
            }

        }
    }

    public bool IsActionDone()
    {
        return Time.time - lastActBeginTime > actDuringTime;
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

    // battle logic 
    // search enemy
    public bool FindTarget(bool resetSearchNum = false)
    {
        Debug.LogWarning(id + " : search Target -num: " + searchNum + "  force Search" + resetSearchNum + " die:" + IsDie());
        if (resetSearchNum)
        {
            searchNum = 0;
        }
        if (searchNum > MaxSearchCount)
        {
            Patrol();
            return false;
        }
        else
        {
            searchNum += 1;
        }
        if (target == null)
        {
            target = battleManager.FindTarget(this);
            if (target)
            {
                searchNum = 0;
                aiType = AIType.BATTLE;
                return true;
            }
        }
        return false;
    }

    public bool HaveTarget()
    {
        return target && !target.IsDie();
    }

    public void SetTarget(SimpleAI targetT)
    {
        target = targetT;
    }

    public float GetAttack()
    {
        return attack;
    }

    void BeAttacked(SimpleAI attacker)
    {
        if (IsDie())
            return;
        if(target == null)
        {
            target = attacker;
            aiType = AIType.BATTLE;
        }
            
        //Debug.Log("hp " + hp);
        if (hp <= 0)
        {
            DoAction("die", true);
        } else
        {
            if(attacker.IsActionDone())
            {
                float hurt = attacker.GetAttack() - defend;
                hp = hp - hurt;
                //if(id == 2)
                //Debug.Log("hurt Time.."+Time.time);
                
            }
            DoAction("getHit", !IsBeAttacked());
        }
    }

    public void Attack(SimpleAI defender)
    {
        if (defender == null)
            return;
        if (IsBeAttacked() && !IsActionDone())
            return;
        if (IsActionDone() || !IsAttacking())
        {
            if (IsActionDone() && IsAttacking())
            {
                defender.BeAttacked(this);
                attackBeginTime = Time.time;
                DoAction("attack_01");
            }
            //Debug.Log("is Attacking..." + IsAttacking()+" "+status);
            if(!IsAttacking())
                DoAction("attack_01", true);
        }
    }
    //for JoyPad
    public void DoAttack()
    {
        if(!HaveTarget())
        {
            FindTarget();
        }
        if(HaveTarget())
        {
            Attack(target);
        }
        else
        {
            DoAction("attack_01", !IsAttacking());
        }
    }

    public void DoDefend()
    {
        Debug.LogError(status);
        DoAction("defend", status != ActionStatus.DEFEND);
    }

    // judge status
    bool IsAttacking()
    {
        return status == ActionStatus.ATTACK01 || status == ActionStatus.ATTACK02 || status == ActionStatus.ATTACK03;
    }

    bool IsRunning()
    {
        return status == ActionStatus.RUN;
    }

    bool IsBeAttacked()
    {
        return status == ActionStatus.GETHIT || status == ActionStatus.DEFEND;
    }

    public bool IsDie()
    {
        return status == ActionStatus.DIE;
    }

    public void Reset()
    {
        hp = maxHp;
        status = ActionStatus.IDLE;
        DoAction("idle", true);
        FindTarget();
    }

    // AI===
    void Patrol()
    {
        if (ctrlType == CtrlType.PLAYER)return;
        if(!IsDie())
        {
            float randomY = Random.Range(-360, 360);
            transform.Rotate(new Vector3(0, randomY, 0));
            patrlDis = 0f;
            patrolMaxDis = Random.Range(5, GameScene.MAX_BOARDER);
            aiType = AIType.PATROL;
        }
    }
    // move control
    public void MoveVertial(bool isForward = true)
    {

        Vector3 dir = transform.forward;
        float moveDistance = speed * Time.deltaTime;
        if (!isForward)
        {
            dir.x = -dir.x;
            dir.z = -dir.z;
        }
        
        nextPos = transform.position + dir * moveDistance;
        //if (Mathf.Abs(nextPos.x) > GameScene.MAX_BOARDER || Mathf.Abs(nextPos.z) > GameScene.MAX_BOARDER)
        //{
        //    Patrol();
        //}
        nextPos.x = Mathf.Clamp(nextPos.x, -GameScene.MAX_BOARDER, GameScene.MAX_BOARDER);
        nextPos.z = Mathf.Clamp(nextPos.z, -GameScene.MAX_BOARDER, GameScene.MAX_BOARDER);
        transform.position = nextPos;
        DoAction("run", !IsRunning());
    }

    public void Turnning(bool isLeft=true)
    {
        float dir = isLeft ? 1 : -1;
        nextDir = transform.eulerAngles;
        nextDir.y = dir*steerSpeed * Time.deltaTime;
        transform.Rotate(nextDir);
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
        if (isTurnningLeft)
        {
            Turnning();
        }
        if (isTurnningRight)
        {
            Turnning(false);
        }
        if (isForward)
        {
            MoveVertial();
        }
        if(isBackward)
        {
            MoveVertial(false);
        }
        
        return hasInput;
    }
    // Update is called once per frame
    void Update()
    {
        //if (Time.time-updateStampt > updateDuringCD)
        //{
        //    Debug.Log("timer:---"+Time.time);
        //    updateStampt = Time.time;
        //}else
        //{
        //    return;
        //}
           
        if(IsDie())
        {
            return;
        }
        HandleInput();
        if(ctrlType == CtrlType.PLAYER)
        {
            return;
        }
        float moveDistance = speed * Time.deltaTime;
        do
        {
            if (aiType == AIType.PATROL)
            {
                Debug.Log("in patrol....");
                patrlDis += moveDistance;
                if (IsActionDone())
                {
                    DoAction("run");
                }
                if (Time.time - lastSearchTime > searchCD )
                {
                    lastSearchTime = Time.time;
                    if (FindTarget(true))
                        break;
                }
                if (patrlDis > patrolMaxDis)
                {
                    aiType = AIType.BATTLE;
                    FindTarget(true);
                    patrlDis = 0;
                }
                nextPos = transform.position + transform.forward * moveDistance;
                if (Mathf.Abs(nextPos.x) > GameScene.MAX_BOARDER || Mathf.Abs(nextPos.z) > GameScene.MAX_BOARDER)
                {
                    Patrol();
                }
                nextPos.x = Mathf.Clamp(nextPos.x, -GameScene.MAX_BOARDER, GameScene.MAX_BOARDER);
                nextPos.z = Mathf.Clamp(nextPos.z, -GameScene.MAX_BOARDER, GameScene.MAX_BOARDER);
                transform.position = nextPos;
                return;
            }
        } while (false);
        
        do
        {
            if (target != null)
            {
                if (target.IsDie())
                {
                    target = null;
                    FindTarget(true);
                    DoAction("run", true);
                    break;
                }
                transform.LookAt(target.transform);

                if (Vector3.Distance(transform.position, target.transform.position) < battleScope)
                {
                    if (IsActionDone())
                    {
                        if (attackCD == 0) attackCD = attackInitCD;
                        if (Time.time - attackBeginTime > attackCD)
                        {
                            Attack(target.gameObject.GetComponent<SimpleAI>());
                        }
                    }
                    return;
                }
                else
                {
                    nextPos = transform.position + transform.forward * moveDistance;
                    if(Mathf.Abs(nextPos.x) > GameScene.MAX_BOARDER || Mathf.Abs(nextPos.z) > GameScene.MAX_BOARDER)
                    {
                        Patrol();
                    }
                    nextPos.x = Mathf.Clamp(nextPos.x, -GameScene.MAX_BOARDER, GameScene.MAX_BOARDER);
                    nextPos.z = Mathf.Clamp(nextPos.z, -GameScene.MAX_BOARDER, GameScene.MAX_BOARDER);
                    transform.position = nextPos;
                }
            }else
            {
                Patrol();
            }
        } while (false);
        
        
        if (IsActionDone())
        {
            //DoAction("idle");
            if (status == ActionStatus.RUN)
            {
                DoAction("run");
            }

        }

    }
}
