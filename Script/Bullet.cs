using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 1000f;
    public float initTime;
    public float maxLife = 2.0f;
    public GameObject explode;
	// Use this for initialization
	void Start () {
        initTime = Time.time;
	}

    public void InitialPos(Vector3 pos,Quaternion rotation)
    {
        transform.position = pos;
        transform.rotation = rotation;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time - initTime > maxLife)
            Destroy(this.gameObject);
        transform.position += transform.forward * speed * Time.deltaTime;
	}

    void OnCollisionEnter()
    {
        Debug.Log("on collisionenter....");
        Destroy(this.gameObject);
    }
}
