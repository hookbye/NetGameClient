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
