using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Vec
{
    public Vector3 offset = Vector3.zero;
    public Vector3 vec = Vector3.right;
    public Color lineColor = Color.green;
}

[ExecuteInEditMode]
public class VectorUtils : MonoBehaviour {
    public List<Vec> vecList = new List<Vec>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        //Debug.DrawRay(transform.position, forward, Color.green);
        //Debug.Log(vecList);
        for (int i=0;i<vecList.Count;++i)
        {
            //Debug.Log(vecList[i].lineColor);
            ForDebug(transform.position , vecList[i].vec, vecList[i].lineColor);
        }
	}

    public void ForDebug(Vector3 pos,Vector3 direction,Color lineColor)
    {
        float arrowHeadLength = 0.25f;
        float arrowHeadAngle = 20.0f;
        Debug.DrawRay(pos, direction, lineColor);
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);

        Debug.DrawRay(pos + direction, right * arrowHeadLength, lineColor);
        Debug.DrawRay(pos + direction, left * arrowHeadLength, lineColor);

    }
}
