using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Net : MonoBehaviour {
    public Socket socket;
    public Text recvText;
    public InputField textInput;
    public InputField ipInput;
    public InputField portInput;
    public string recvStr;
    const int BUFFER_SIZE = 1024;
    public byte[] readBuff = new byte[BUFFER_SIZE];
	// Use this for initialization
	void Start () {
        //socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //socket.Connect("127.0.0.1", 1234);
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes("hello im client");
        //socket.Send(bytes);
        //socket.Close();
	}

    void OnGUI()
    {
        //Debug.Log("On gui...");
    }

    public void Connection()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        string ipStr = ipInput.text;
        int portNum = int.Parse(portInput.text);
        socket.Connect(ipStr!="" ?ipStr: "127.0.0.1", portNum!=0?portNum:1234);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes("hello im client");
        //socket.Send(bytes);
        socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
    }

    private void ReceiveCb(IAsyncResult ar)
    {
        try
        {
            int count = socket.EndReceive(ar);
            string str = System.Text.Encoding.UTF8.GetString(readBuff,0,count);
            //recvStr.Trim();
            if (recvStr.Length > 600)
            {
                Debug.Log("recvStr.Length " + recvStr.Length);
                recvStr = "";
            }
            recvStr += str + "\n";
            socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
        }catch(Exception e)
        {
            recvText.text += "链接断开";
            socket.Close();
        }
    }

    public void Close()
    {
        //socket.Close();
    }

    public void Send()
    {
        string str = textInput.text;
        Debug.Log("发送 ： "+str);
        byte[] bytes = System.Text.Encoding.Default.GetBytes(str);
        try
        {
            socket.Send(bytes);
        }
        catch (Exception e)
        {
            recvText.text += "链接断开"+e.Message;
            socket.Close();
        }
    }
	
	// Update is called once per frame
	void Update () {
        recvText.text = recvStr;
	}
}
