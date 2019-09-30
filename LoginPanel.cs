using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : MonoBehaviour {
    public string userName = "";
    public string passWord = "";
	// Use this for initialization
	void Start () {
		
	}
	void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 200, 120), "登录框 ");
        GUI.Label(new Rect(20, 40, 50, 30), "用户名 ");
        userName = GUI.TextField(new Rect(70, 40, 120, 20), userName);
        GUI.Label(new Rect(20, 70, 50, 30), "密  码 ");
        passWord = GUI.TextField(new Rect(70, 70, 120, 20), passWord);
        if (GUI.Button(new Rect(70,100,50,25),"登  录"))
        {
            if(userName == "hook" && passWord == "111111")
            {
                Debug.Log("登录成功");
            }else
            {
                Debug.Log("账号密码错误");
            }
            //Application.LoadLevel("totalAsset");

            UnityEngine.SceneManagement.SceneManager.LoadScene("tankTest");
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("tankTest");
    }
    // Update is called once per frame
    void Update () {
		
	}
}
