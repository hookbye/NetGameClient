using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyPad : MonoBehaviour {
    public GameScene manager;
    public AnimateBase player;
	// Use this for initialization
	void Start () {
        manager = GameObject.FindObjectOfType<GameScene>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void OnUpClick()
    {
        if (player == null)
            return;
        player.isUp = true;
    }
    public void OnUpCancel()
    {
        if (player == null)
            return;
        player.isUp = false;
    }

    public void OnDownClick()
    {
        if (player == null)
            return;
        player.isDown = true;
    }
    public void OnDownCancel()
    {
        if (player == null)
            return;
        player.isDown = false;
    }
    public void OnLeftClick()
    {
        if (player == null)
            return;
        player.isLeft = true;
    }
    public void OnLeftCancel()
    {
        if (player == null)
            return;
        player.isLeft = false;
    }
    public void OnRightClick()
    {
        if (player == null)
            return;
        player.isRight = true;
    }
    public void OnRightCancel()
    {
        if (player == null)
            return;
        player.isRight = false;
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
