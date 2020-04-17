using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleCamera : MonoBehaviour {
    public GameObject role;
    public float rollX = 0;
    public float rollY = 3;
    public float rollZ = -4;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (role!=null)
        {
            transform.position = role.transform.position + new Vector3(rollX,rollY,rollZ);
            //transform.forward = role.transform.forward;
            transform.LookAt(role.transform);
            
        }
	}
}
