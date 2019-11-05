using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour {
    private List<GameObject> entries = new List<GameObject>(5);
    public GameObject footmanPrefab;
	// Use this for initialization
	void Start () {
        Camera.main.transform.LookAt(GameObject.Find("Cube").transform);
        if(footmanPrefab != null)
        {
            for (int i = 0; i < 1; i++)
            {
                GameObject footMan = Instantiate(footmanPrefab);//Resources.LoadAssetAtPath<GameObject>("Assets/Footman/Prefabs/Footman_Unlit_Green.prefab");// GameObject.Instantiate("Footman_Unlit_Green.prefab");
                footMan.AddComponent<SimpleAI>();
                entries.Add(footMan);
            }
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
