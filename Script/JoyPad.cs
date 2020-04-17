using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyPad : MonoBehaviour {
    public GameScene manager;
    public SimpleAI player;
	// Use this for initialization
	void Start () {
        manager = GameObject.FindObjectOfType<GameScene>();
        if(player!=null)
        {
            player.LoadPlayerSetting();
        }
	}

    public void SetPlayer(SimpleAI pl)
    {
        if(pl!=null)
        {
            player = pl;
            player.LoadPlayerSetting();
        }
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 touchPos = Vector3.zero;
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            touchPos = Input.touches[0].position;
        }
        if(Input.GetMouseButtonUp(1))
        {
            touchPos = Input.mousePosition;
        }
        if (touchPos != Vector3.zero)
        {
            object ray = Camera.main.ScreenPointToRay(touchPos);
            RaycastHit hit;
            bool isHit = Physics.Raycast((Ray)ray, out hit);
            if (isHit)
            {
                //Debug.Break();
                if (manager != null)
                {
                    manager.GenerateEntriesByPos(hit.point);
                }
                
            }
        }

        KeyBoardEvent();

    }

    public void KeyBoardEvent()
    {
        // keyboard event
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnUpClick();
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            OnUpCancel();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            OnDownClick();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            OnDownCancel();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            OnLeftClick();
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            OnLeftCancel();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            OnRightClick();
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            OnRightCancel();
        }

        if(Input.GetMouseButtonDown(0))
        {
            OnFightClick();
        }
    }


    //Direct control
    public void OnUpClick()
    {
        if (player == null)
            return;
        player.isForward = true;
    }
    public void OnUpCancel()
    {
        if (player == null)
            return;
        player.isForward = false;
    }

    public void OnDownClick()
    {
        if (player == null)
            return;
        player.isBackward = true;
    }
    public void OnDownCancel()
    {
        if (player == null)
            return;
        player.isBackward = false;
    }
    public void OnLeftClick()
    {
        if (player == null)
            return;
        player.isTurnningLeft = true;
    }
    public void OnLeftCancel()
    {
        if (player == null)
            return;
        player.isTurnningLeft = false;
    }
    public void OnRightClick()
    {
        if (player == null)
            return;
        player.isTurnningRight = true;
    }
    public void OnRightCancel()
    {
        if (player == null)
            return;
        player.isTurnningRight = false;
    }

    //battle control
    public void OnFightClick()
    {
        if (player == null)
            return;
        player.DoAttack();
    }

    public void OnDefendClick()
    {
        player.DoDefend();
    }

    public void OnResetClick()
    {
        Debug.Log("reset..");
        if (manager!=null)
        {
            manager.Reset();
        }
    }
}
