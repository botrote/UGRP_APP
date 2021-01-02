using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class SocketClient : MonoBehaviour
{
    static Socket sck;
    public ClientUIManager clientUIManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectServer()
    {
        sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 6321);
 
        try
        {
            sck.Connect(localEndPoint);
        }
        catch
        {
            clientUIManager.ShowError("Unable to connect to remote end point!\r\n");
        }
    }

    public void SendMessageToServer()
    {
        string text = "hello world";
        byte[] data = Encoding.UTF8.GetBytes(text);
 
        sck.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(sendStr), sck);
    }

    static void sendStr(IAsyncResult ar) 
    {
        Socket transferSock = (Socket)ar.AsyncState;
        int strLength = transferSock.EndSend(ar);
    }
}
