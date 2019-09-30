using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabCreate : MonoBehaviour {
    public GameObject prefab;
    public int num = 0;
    public int max = 50;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (num <= max)
        {
            float x = Random.Range(-100, 100);
            float y = Random.Range(40, 60);
            float z = Random.Range(-50, 50);
            Vector3 pos = new Vector3(x, y, z);
            Instantiate(prefab, pos, Quaternion.identity);
            num += 1;
        }
        
    }
}
