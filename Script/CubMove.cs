﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0.001f, 0, 0));
	}
}
