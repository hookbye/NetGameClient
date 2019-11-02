using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachine {

	protected float actionDuringTime;
    protected float actionBeginTime = Time.time;
    public void doEvent(Action act)
    {
        if (Time.time - actionBeginTime < 0)
            return;
        actionBeginTime = Time.time;
        act();
    }
    public void doEvent(string actName)
    {

    }
    //protected 
}
