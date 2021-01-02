using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class SocketHost : MonoBehaviour
{
    public HostUIManager HostUIManager;
    static byte[] Buffer { get; set; }
    Socket server;
    Socket client;
    public enum HostStatus{Off, AcceptingClient, Connected, AcceptingFile}
    public HostStatus curStatus;

    void Start()
    {
        curStatus = HostStatus.Off;
        string localIP = null;
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        HostUIManager.ShowHostInfo(localIP);
    }

    // Update is called once per frame
    void Update()
    {
        switch(curStatus)
        {
            case HostStatus.Off:  
            case HostStatus.AcceptingClient:
            case HostStatus.AcceptingFile:
                return;
            case HostStatus.Connected:
                Read();
                break;    
        }
    }

    public void OpenServer()
    {
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        server.Bind(new IPEndPoint(IPAddress.Any, 6321));
        server.Listen(100);
        server.BeginAccept(new AsyncCallback(SocketConnectCallback), server);
        curStatus = HostStatus.AcceptingClient;
        HostUIManager.ShowStatus("AcceptingClient");
    }

    void SocketConnectCallback(IAsyncResult ar)
    {
        Socket iServer = (Socket)ar.AsyncState;
        curStatus = HostStatus.AcceptingClient;
        client = iServer.EndAccept(ar);
        curStatus = HostStatus.Connected;
        Debug.Log("I'm connected");
        HostUIManager.ShowStatus("Client Accepted");
    }

    void Read()
    {
        curStatus = HostStatus.AcceptingFile;
        Buffer = new byte[1024];
        server.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallBack), server);
        Debug.Log("read called");
    }

    void ReadCallBack(IAsyncResult ar)
    {
        Socket iServer = (Socket)ar.AsyncState;
        int bytesRead = iServer.EndReceive(ar);
        Debug.Log("read complete");
        byte[] formatted = new byte[bytesRead];

        for (int i = 0; i < bytesRead; ++i)
        {
            formatted[i] = Buffer[i];
        }
 
        string strdata = Encoding.UTF8.GetString(formatted);
        HostUIManager.ShowClientMessage(strdata);

        curStatus = HostStatus.Connected;
    }
}
