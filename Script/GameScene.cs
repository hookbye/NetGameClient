using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour {
    public const int MAX_ENTYNUM = 64;
    public const float MAX_BOARDER = 23;
    private List<SimpleAI> entries = new List<SimpleAI>(MAX_ENTYNUM);
    public GameObject footmanPrefab;
    public JoyPad joyPad;
    private int battleIndex=0;
	// Use this for initialization
	void Start () {
        Camera.main.transform.LookAt(GameObject.Find("Cube").transform);
        joyPad = GameObject.Find("JoyPad").GetComponent<JoyPad>();
        if (footmanPrefab != null)
        {
            for (int i = 0; i < 2; i++)
            {
                SimpleAI footAI = GenerateEntries();
                footAI.transform.position = new Vector3((i - 1) *20, 0, 0);
            }
            joyPad.SetPlayer(GenerateEntries());
        }
		
	}

    public SimpleAI FindTarget(SimpleAI seeker)
    {
        SimpleAI target = null;
        Vector3 seekerPos = seeker.transform.position;
        float scope = seeker.searchScope;
        foreach(SimpleAI iter in entries)
        {
            if (iter != seeker && !iter.IsDie())
            {
                Vector3 iterPos = iter.transform.position;
                if (scope < Vector3.Distance(seekerPos, iterPos))
                    return iter;
            }
        }
        return target;
    }
	
    public int GenBattleId()
    {
        battleIndex = battleIndex + 1;
        return battleIndex;
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void Reset()
    {
        foreach (SimpleAI iter in entries)
        {
            iter.Reset();
        }
    }

    public SimpleAI GenerateEntries()
    {
        GameObject footMan = Instantiate(footmanPrefab);//Resources.LoadAssetAtPath<GameObject>("Assets/Footman/Prefabs/Footman_Unlit_Green.prefab");// GameObject.Instantiate("Footman_Unlit_Green.prefab");
        SimpleAI footAI = footMan.AddComponent<SimpleAI>();
        footAI.BindBattleManager(this);
        entries.Add(footAI);
        return footAI;
    }

    public void GenerateRandomEntries()
    {
        float randomX = Random.Range(-MAX_BOARDER, MAX_BOARDER);
        float randomZ = Random.Range(-MAX_BOARDER, MAX_BOARDER);
        SimpleAI footAI = GenerateEntries();
        footAI.transform.position = new Vector3(randomX, 0, randomZ);
        footAI.SetEntyColor(SimpleAI.EntyColor.BLUE);
    }
}
