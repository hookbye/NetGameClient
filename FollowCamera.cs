using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
    private GameObject target;
    public float rot = 0;
    public float roll = 30 * Mathf.PI * 2 / 360;
    public float distance = 25;

    public float rotSpeed = 0.2f;
    public float rollSpeed = 0.2f;
    public float maxRoll = 70 * Mathf.PI * 2 / 360;
    public float minRoll = -10 * Mathf.PI * 2 / 360;
    public float disSpeed = 2f;
    public float maxDis = 30 ;
    public float minDis = 5;

    // Use this for initialization
    void Start () {
        target = GameObject.Find("tank");
        //SetTarget();

    }

    void SetTarget()
    {
        if (GameObject.Find("tank") && GameObject.Find("tank").transform.Find("cameraPos"))
            target = GameObject.Find("cameraPos");
        else
            target = GameObject.Find("tank");
    }

    void Rotate()
    {
        float w = Input.GetAxis("Mouse X") * rotSpeed;
        rot -= w;
    }

    void Roll()
    {
        float w = Input.GetAxis("Mouse Y") * rollSpeed;
        roll -= w;
        if (roll < minRoll)
            roll = minRoll;
        if (roll > maxRoll)
            roll = maxRoll;
    }

    void Zoom()
    {
        float w = Input.GetAxis("Mouse ScrollWheel") * disSpeed;
        distance -= w;
        if (distance < minDis)
            distance = minDis;
        if (distance > maxDis)
            distance = maxDis;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (target == null)
            return;
        if (Camera.main == null)
            return;
        Vector3 targetPos = target.transform.position;
        Vector3 cameraPos;
        float d = distance * Mathf.Cos(roll);
        float height = distance * Mathf.Sin(roll);
        cameraPos.x = targetPos.x + d * Mathf.Cos(rot);
        cameraPos.z = targetPos.z + d * Mathf.Sin(rot);
        cameraPos.y = targetPos.y + height;
        Camera.main.transform.position = cameraPos;
        Camera.main.transform.LookAt(target.transform);

        Rotate();
        Roll();
        Zoom();
	}
}
