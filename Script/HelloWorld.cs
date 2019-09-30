using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloWorld : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), "Hello world!");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
