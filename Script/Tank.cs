using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {
    public Transform turret;
    public float turretRotSpeed = 0.5f;
    public float turretTarget = 0;

    public Transform gun;
    public float maxRoll = 80;
    public float minRoll = 20f;
    public float gunRollTarget = 0;

	// Use this for initialization
	void Start () {
        turret = transform.Find("turret");
        gun = GameObject.Find("paoGuan").transform;
	}

    public void TurretRotation()
    {
        if (Camera.main == null)
            return;
        if (turret == null)
            return;
        float angle = turret.eulerAngles.y - turretTarget;
        if (angle < 0) angle += 360;
        if (angle > turretRotSpeed && angle < 180)
            turret.Rotate(0f, -turretRotSpeed, 0f);
        else if (angle > 180 && angle < 360 - turretRotSpeed)
            turret.Rotate(0f, turretRotSpeed, 0f);
    }

    public void GunRoll()
    {
        if (Camera.main == null)
            return;
        if (gun == null)
        {
            Debug.Log("no gun...");
            return;
        }
            
        Vector3 worldEuler = gun.eulerAngles;
        Vector3 localEuler = gun.localEulerAngles;
        worldEuler.x = gunRollTarget;
        gun.eulerAngles = worldEuler;
        Vector3 euler = gun.localEulerAngles;
        //if (euler.x > 180)
        //    euler.x -= 360;

        if (euler.x > maxRoll)
            euler.x = maxRoll;
        if (euler.x < minRoll)
            euler.x = minRoll;
        gun.localEulerAngles = new Vector3(euler.x,localEuler.y,localEuler.z);
    }
	
	// Update is called once per frame
	void Update () {
        //float speed = 1.0f;
        ////上
        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    transform.eulerAngles = new Vector3(0, 0, 0);
        //    transform.position += transform.forward * speed;
        //}
        ////下
        //else if(Input.GetKey(KeyCode.DownArrow))
        //{
        //    transform.eulerAngles = new Vector3(0, 180, 0);
        //    transform.position += transform.forward * speed;
        //}
        ////左
        //else if(Input.GetKey(KeyCode.LeftArrow))
        //{
        //    transform.eulerAngles = new Vector3(0, 270, 0);
        //    transform.position += transform.forward * speed;
        //}
        ////右
        //else if(Input.GetKey(KeyCode.RightArrow))
        //{
        //    transform.eulerAngles = new Vector3(0, 90, 0);
        //    transform.position += transform.forward * speed;
        //}
        float steer = 20;
        float x = Input.GetAxis("Horizontal");
        transform.Rotate(0, x * steer * Time.deltaTime, 0);
        //前进后退
        float speed = 3f;
        float y = Input.GetAxis("Vertical");
        Vector3 s = y * transform.forward * speed * Time.deltaTime;
        transform.position += s;

        turretTarget = Camera.main.transform.eulerAngles.y;
        TurretRotation();

        gunRollTarget = Camera.main.transform.eulerAngles.x;
        GunRoll();
    }
}
