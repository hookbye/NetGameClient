using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour {
    private List<GameObject> entries = new List<GameObject>(5);
    public GameObject footmanPrefab;
    public JoyPad joyPad;
	// Use this for initialization
	void Start () {
        Camera.main.transform.LookAt(GameObject.Find("Cube").transform);
        if(footmanPrefab != null)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject footMan = Instantiate(footmanPrefab);//Resources.LoadAssetAtPath<GameObject>("Assets/Footman/Prefabs/Footman_Unlit_Green.prefab");// GameObject.Instantiate("Footman_Unlit_Green.prefab");
                SimpleAI footAI = footMan.AddComponent<SimpleAI>();
                footAI.BindBattleManager(this);
                footMan.transform.position = new Vector3((i - 1) *20, 0, 0);
                entries.Add(footMan);
            }
        }
		
	}

    public SimpleAI FindTarget(SimpleAI seeker)
    {
        SimpleAI target = null;
        Vector3 seekerPos = seeker.transform.position;
        float scope = seeker.searchScope;
        foreach(GameObject iter in entries)
        {
            SimpleAI ai = iter.GetComponent<SimpleAI>();
            if (iter != seeker.gameObject && !ai.IsDie())
            {
                Vector3 iterPos = iter.transform.position;
                if (scope > Vector3.Distance(seekerPos, iterPos))
                    return ai;
            }
        }
        return target;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
