using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {
    public enum TankType
    {
        Player,
        Computer
    }

    public TankType tankType = TankType.Player;

    public Transform turret;
    private Transform tracks;
    private Transform wheels;
    
    public List<AxleInfo> axleInfos;

    private float motor = 0;
    public float maxMotorTorque;
    private float brakeTorque = 0;
    public float maxBrakeTorque = 100;
    private float steering = 0;
    public float maxSteeringAngle;

    public float turretRotSpeed = 0.5f;
    public float turretTarget = 0;

    public Transform gun;
    public float maxRoll = 80f;
    public float minRoll = 20f;
    public float gunRollTarget = 0;

    //射击
    public GameObject bullet;
    public Transform fireFrom;
    private float lastShootTime = 0f;
    private float shootDuring=0.5f;

	// Use this for initialization
	void Start () {
        turret = transform.Find("turret");
        //gun = GameObject.Find("paoGuan").transform;
        tracks = GameObject.Find("tracks").transform;
        wheels = transform.Find("wheels");
        //fireFrom = transform.Find("fireFrom").transform;

    }

    public void PlayerCtrl()
    {
        if(tankType != TankType.Player)
        {
            return;
        }
        motor = maxMotorTorque * Input.GetAxis("Vertical");
        steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        turretTarget = Camera.main.transform.eulerAngles.y;
        gunRollTarget = Camera.main.transform.eulerAngles.x;

        if (Input.GetMouseButton(0))
        {
            if (Time.time - lastShootTime > shootDuring)
            {
                lastShootTime = Time.time;
                Shoot();
            }
        }
    }

    public void WheelsRotation(WheelCollider collider)
    {
        if (wheels == null)
            return;
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position,out rotation);
        foreach(Transform wheel in wheels)
        {
            wheel.rotation = rotation;
        }
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
        if (euler.x > 180)
            euler.x -= 360;
        Debug.Log("euler.x" + euler.x+" "+maxRoll+"  "+minRoll);
        if (euler.x > maxRoll)
            euler.x = maxRoll;
        if (euler.x < minRoll)
            euler.x = minRoll;
        Debug.Log("euler.x====" + euler.x);
        gun.localEulerAngles = new Vector3(euler.x,localEuler.y,localEuler.z);
        Debug.Log(gun.localEulerAngles);
    }

    public void TrackMove()
    {
        if (tracks==null)
        {
            Debug.Log("tracks is null===");
            return;
        }
            
        if (wheels == null)
        {
            Debug.Log("jjjjl===");
            return;
        }
            
        float offset = 0f;
        if (wheels.GetChild(0) != null)
            offset = wheels.GetChild(0).localEulerAngles.x /90f;
        foreach (Transform track in tracks)
        {
            MeshRenderer ms = track.gameObject.GetComponent<MeshRenderer>();
            if (ms == null) continue;
            Material mt = ms.material;
            mt.mainTextureOffset = new Vector2(0, offset);
        }
    }

    void Shoot()
    {
        if (bullet == null)
            return;
        GameObject newBullet = GameObject.Instantiate(bullet, fireFrom.position+ fireFrom.forward*5, fireFrom.rotation);
        //Instantiate(bullet, this.transform, true);
        //newBullet.GetComponent<Bullet>().InitialPos(fireFrom.position,fireFrom.rotation);
    }
	
	// Update is called once per frame
	void LateUpdate () {
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
        //float steer = 20;
        //float x = Input.GetAxis("Horizontal");
        //transform.Rotate(0, x * steer * Time.deltaTime, 0);
        ////前进后退
        //float speed = 3f;
        //float y = Input.GetAxis("Vertical");
        //Vector3 s = y * transform.forward * speed * Time.deltaTime;
        //transform.position += s;
        PlayerCtrl();
        foreach(AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            if (true)
            {
                axleInfo.leftWheel.brakeTorque = brakeTorque;
                axleInfo.rightWheel.brakeTorque = brakeTorque;
            }
        }
        if (axleInfos[1]!=null)
        {
            WheelsRotation(axleInfos[1].leftWheel);
        }
        TrackMove();
        turretTarget = Camera.main.transform.eulerAngles.y;
        //TurretRotation();

        

        gunRollTarget = Camera.main.transform.eulerAngles.x;
        GunRoll();
    }
}
