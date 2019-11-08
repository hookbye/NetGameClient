using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//logic 
public class BatteObject : MonoBehaviour {
    public int id = 0;
    public float maxHp = 10f;
    public float hp = 10f;
    public float attack = 2f;
    public float defend = 1f;
    public float searchScope = 50f;
    public float battleScope = 2f;
    public float speed = 1f;
    //default scale is per second
    public float battleScale = 1f;
    //move params
    public Vector3 pos = Vector3.zero;
    public float x = 0;
    public float y = 0;
    public float z = 0;
    public Vector3 direction = Vector3.zero;
    public enum BaseStatus
    {
        NONE,
        IDLE,
        MOVE,
        FIGHT,
        DIE,
    }
    public BaseStatus status=BaseStatus.NONE;
    // Use this for initialization
    void Start () {
		
	}

    //do something here
    public bool IsDie()
    {
        return status == BaseStatus.DIE;
    }

    public void SetStatus(BaseStatus st)
    {
        status = st;
    }

    public void OnHurt(float hurt)
    {
        hp = hp - hurt;
    }

    public void OnHeal(float heal)
    {
        hp = hp + heal;
    }
    //should be override
    public float GetDamage()
    {
        return attack;
    }

    public void ReLive(float rhp=0)
    {
        if (rhp > 0)
        {
            hp = rhp;
        }
        else
        {
            hp = maxHp;
        }
    }

    public void Reset()
    {
        hp = maxHp;
        status = BaseStatus.NONE;
    }
    //actions
    public void UpdatePos(float px,float py,float pz)
    {
        x = px;
        y = py;
        z = pz;
        pos = new Vector3(x, y, z);
    }

    public void UpdatePos(Vector3 ppos)
    {
        pos = ppos;
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
