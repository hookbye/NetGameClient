using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour {
    public GameObject monster;
    public Animator anim;
	// Use this for initialization
	void Start () {
        monster = GameObject.Find("Armoring_cannibal@skin");
        anim =  monster.GetComponent<Animator>();
        
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.B))
        {
            anim.Play("attack");
        }else if(Input.GetKey(KeyCode.Space))
        {
            anim.Play("run");
        }
	}
}
