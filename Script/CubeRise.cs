using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRise : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Terrain terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        TerrainUtil.Rise(terrain, new Vector3(0, 4f, 0), 1, 10);
        int[] indexs = new int[10];
        for (int i=0;i<9;i++)
        {
            indexs[i] = i;
        }
        TerrainUtil.Rise(terrain,indexs,1f,2);
    }
	
	// Update is called once per frame
	void Update () {
        //transform.position+transform.up
        
	}
}
