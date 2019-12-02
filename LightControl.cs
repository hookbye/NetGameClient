using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour {
    Light light;
    private bool isLightOn=true;
	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(500, 0, 100, 100), new GUIContent("Open light")))
        {
            isLightOn = !isLightOn;
            light.enabled = isLightOn;
        }
    }
}
