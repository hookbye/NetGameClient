using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestYeild : MonoBehaviour {
    private static IEnumerable Download() {
        Debug.Log("first do someting");
        yield return 1;
        if(Random.Range(1,2)==1)
        {
            Debug.Log("222");
            yield return "hello";
        }
        Debug.Log("333");
        yield return "continue";
    }
	// Use this for initialization
	void Start () {
        int count = 0;
		while(count < 3)
        {
            count += 1;
            Download();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
