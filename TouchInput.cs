using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {
    public Camera camera;
    private Vector2 m_screenPos = new Vector2();
    private Vector3 lastTouchPos;
    private bool isTouchBegin = false;
    private bool isTouchEnd = true;
	// Use this for initialization
	void Start () {
        camera = Camera.main;
        Input.multiTouchEnabled = true;
    }

    void OnMouseInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isTouchBegin = true;
            lastTouchPos = Input.mousePosition;
            Debug.Log("donee..e.e");
        }
        if(Input.GetMouseButtonUp(0))
        {
            isTouchBegin = false;
        }
        if(isTouchBegin)
        {
            Vector3 touchVec = Input.mousePosition;
            Vector3 deltPos = touchVec - lastTouchPos;
            Vector3 cameraPos = camera.transform.position+deltPos*0.001f;
            
            camera.transform.Translate(cameraPos);
        }

    }

    void OnMobileInput()
    {
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                m_screenPos = Input.touches[0].position;
                Debug.Log(m_screenPos);
            }
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                Vector3 touchVec = Input.touches[0].deltaPosition;
                camera.transform.Translate(new Vector3(touchVec.x * Time.deltaTime, touchVec.y * Time.deltaTime, 0));
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.touchCount <= 0)
        //{
        //    Debug.Log(Input.touchCount);
        //    return;
        //}
        //Debug.Log(Application.platform);
        //if(Application.platform == RuntimePlatform.WindowsEditor)
        //{
        //    OnMouseInput();
        //}
        //else
        //{
        //    OnMobileInput();
        //}
       
        
	}
}
