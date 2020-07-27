using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;

public class NetWorkClient : MonoBehaviour
{
    public bool isActivate;
    public ServerClient client;
    private TcpClient clientSocket;
    private IPEndPoint clientAddress;
    private IPEndPoint serverAddress;
    // Start is called before the first frame update
    void Start()
    {
        isActivate = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFeature()
    {
        if(isActivate == true)
            return;
        isActivate = true;
        try
        {
            serverAddress = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 6321);
            clientAddress = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 0);
            clientSocket = new TcpClient(clientAddress);
            client = new ServerClient(clientSocket);
            client.tcp.Connect(serverAddress);
            NetworkStream stream = client.tcp.GetStream();
            string message = "Hello Host!!\n";
            byte[] data = Encoding.Default.GetBytes(message);
            Debug.Log(stream.CanWrite);
            stream.Write(data, 0, data.Length); 
        }
        catch(SocketException e)
        {
            Debug.Log("Socket error : " + e.Message);
        }
    }
}
