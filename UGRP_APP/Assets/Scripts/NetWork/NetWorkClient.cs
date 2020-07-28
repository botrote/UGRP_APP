using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using UnityEngine.UI;

public class NetWorkClient : MonoBehaviour
{
    public bool isActivate;
    string localIP;
    public ServerClient client;
    private TcpClient clientSocket;
    private IPEndPoint clientAddress;
    private IPEndPoint serverAddress;
    public Text SocketExceptionText;
    // Start is called before the first frame update
    void Start()
    {
        isActivate = false;
        localIP = null;
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        SocketExceptionText = GameObject.Find("SocketExceptionText").GetComponent<Text>();
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        Debug.Log("clientIP : " + localIP);
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
            serverAddress = new IPEndPoint(IPAddress.Parse(localIP), 6321);
            clientAddress = new IPEndPoint(IPAddress.Parse(localIP), 0);
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
            SocketExceptionText.text = e.Message;
        }
    }
}
