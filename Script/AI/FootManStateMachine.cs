using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootManStateMachine : BaseStateMachine {
    public Animator animator;
	public void InitStateMachine(Animator animatorP)
    {
        animator = animatorP;
    }

    public void DoWalk()
    {
        animator.Play("walk");
    }

    public void DoIdle()
    {
        animator.Play("idle");
    }
}
