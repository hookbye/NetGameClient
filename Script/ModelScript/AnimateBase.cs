using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AnimateBase : MonoBehaviour {
    public enum AnimateType
    {
        Player,
        Computer,
    }
    public enum FaceDirect //eight direction
    {
        FRONT,
        FRONT_RIGHT,
        RIGHT,
        BACK_RIGHT,
        BACK,
        BACK_LEFT,
        LEFT,
        FRONT_LEFT,
    }
    public enum Status
    {
        NONE,
        IDLE,
        MOVING,
        ATTACKING,
        FREEZING,
        DIE,
    }
    private FaceDirect faceDir = FaceDirect.FRONT;
    private Status status = Status.NONE;
    public float speed = 10f;
    //auto move
    private Vector3 targetPosition;
    private GameObject targetGameObject;
    private Transform targetTransform;
    //control by joypad-->screenInput
    public bool isLeft { get; set; }
    public bool isRight { get; set; }
    public bool isUp { get; set; }
    public bool isDown { get; set; }
    //control by input-->keyboardInput
    //animation
    public Animator animator;
    private float actDuringTime = 1f;
    private float lastActBeginTime = 0f;

	// Use this for initialization
	void Start () {
		animator = this.GetComponentInChildren<Animator>();
	}

    void DoAction(string act)
    {
        if (animator == null)
            return;
        if (Time.time - lastActBeginTime < actDuringTime)
            return;
        lastActBeginTime = Time.time;
        switch (act)
        {
            case "attack":
                status = Status.ATTACKING;
                actDuringTime = 2f;
                break;
            case "run":
                status = Status.MOVING;
                actDuringTime = -1f;
                break;
            case "idle":
                status = Status.IDLE;
                actDuringTime = -1f;
                break;
            default:
                actDuringTime = 2f;
                break;

        }

        animator.Play(act);
        //animator.CrossFade(act, 0.1f);
    }

    void MoveTo(Vector3 targetPos)
    {
        targetPosition = targetPos;
    }

    void SetFaceDirect(FaceDirect dir)
    {
        faceDir = dir;
        float angle = (int)dir * 45f;
        transform.eulerAngles = new Vector3(0, angle, 0);
    }

    void UpdateDirection()
    {

    }

    bool OnCtrl()
    {
        bool isMove = false;
        float x = Input.GetAxis("Horizontal");
        float angle = 0;
        bool isHorigin = false;
        bool isVetical = false;
        float angleX = 0;
        float angleY = 0;
        if(Input.GetKey(KeyCode.A)||isLeft)
        {
            angleX = 270f;
            isHorigin = true;
        }
        else if (Input.GetKey(KeyCode.D)||isRight)
        {
            angleX = 90f;
            isHorigin = true;
        }
        if (Input.GetKey(KeyCode.W)||isUp)
        {
            angleY = 0f;
            isVetical = true;
        }
        else if (Input.GetKey(KeyCode.S)||isDown)
        {
            angleY = 180f;
            isVetical = true;
        }

        
        if(isHorigin || isVetical)
        {
            isMove = true;
            if(isHorigin && isVetical)
            {
                angle = (angleX == 270 && angleY == 0) ?315:(angleX + angleY) * 0.5f;
            }
            else if (isHorigin)
            {
                angle = angleX;
            }
            else
            {
                angle = angleY;
            }
            
            Debug.Log(angle);
            transform.eulerAngles = new Vector3(0, angle, 0);
            DoAction("run");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            DoAction("attack");
        }
        if (Input.GetKey(KeyCode.J))
        {
            DoAction("play");
        }
        if (Input.GetKey(KeyCode.K))
        {
            DoAction("walk");
        }
        return isMove;
        
    }
	
	// Update is called once per frame
	void Update () {
        bool isMove = OnCtrl();
        if(isMove && status == Status.MOVING)
        {
            transform.position += transform.forward * speed;// * Time.deltaTime;
            DoAction("run");
        }else
        {
            DoAction("idle");
        }
    }
}
