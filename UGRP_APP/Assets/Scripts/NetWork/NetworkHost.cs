using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.IO;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class NetworkHost : MonoBehaviour
{
    private bool isActivate;
    private bool isHandlingFile;
    string localIP;
    public int port = 6321;
    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;
    private TcpListener server;
    private bool serverStarted;
    private int count = 0;
    public AudioSerializer audioSerializer;
    public HostUIManager HostUIManager;


    private void Start()
    {
        isActivate = false;
        isHandlingFile = false;
        localIP = null;
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        Debug.Log("hostIP : " + localIP);
        KeyInputManager keyInputManager = GameObject.Find("KeyInputManager").GetComponent<KeyInputManager>();
        keyInputManager.EV_escape += EndServer;
    }

    public void StartFeature()
    {
        if(isActivate == true)
            return;
        isActivate = true;
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();
        server = null;

        try
        {
            IPEndPoint localAddress = new IPEndPoint(0, port);
            server = new TcpListener(localAddress);
            //server = new TcpListener(IPAddress.Parse(localIP), port);
            server.Start();
            StartListening();
            serverStarted = true;
            Debug.Log("Server has been started on address " + server.LocalEndpoint.ToString());
            HostUIManager.ShowHostInfo("Server has been started on address " + localIP.ToString());
        }
        catch (Exception e)
        {
            isActivate = false;
            Debug.Log("Socket error : " + e.Message);
            HostUIManager.ShowError(e.Message);
        }
    }

    private void Update()
    {
        SetFileMessage();
        if(isActivate == false || isHandlingFile == true)
            return;
        HostUIManager.ShowStatus(count + "clients");
        if (!serverStarted)
            return;
        foreach (ServerClient c in clients)
        {
            if (!IsConnected(c.tcp))
            {
                c.tcp.Close();
                clients.Remove(c);
                disconnectList.Add(c);
                count--;
                continue;
            }
            else
            {
                NetworkStream s = c.tcp.GetStream();
                StartCoroutine(HandlingFile(s));
            }
        }
    }

    private IEnumerator HandlingFile(NetworkStream s)
    {
        if(s.DataAvailable == false)
            yield break;

        isHandlingFile = true;
        yield return new WaitForSeconds(1.5f);
        byte[] data = new byte[5000000];
        byte[] modeBuffer = new byte[1];

        if(s.DataAvailable)
        {
            s.Read(modeBuffer, 0, 1);
        }
        else
        {
            isHandlingFile = false;
            yield break;
        }
            
        if(s.DataAvailable)
        {
            s.Read(data, 0, data.Length);  
        }        
        else
        {
            isHandlingFile = false;
            yield break;
        }
                
        bool transferMode = Convert.ToBoolean(modeBuffer[0]);
        string encoded = Encoding.UTF8.GetString(data);

        if(encoded != null)
        {
            Debug.Log("transferMode: " + transferMode.ToString());
            if(transferMode == true)
            {
                audioSerializer.StoreByteClip(data);
                onIncomingData("audio file recieved");
            }
            if(transferMode == false)
            {
                encoded = encoded.TrimEnd(new char[] {(char)0});
                Debug.Log(encoded.Length);
                onIncomingData(encoded);
                TextManager.TextWrite(encoded);
            }
        }
        isHandlingFile = false;
    }

    private void SetFileMessage()
    {
        if(isHandlingFile)
            HostUIManager.ShowFileStatus("File loading... please wait..");
        else
            HostUIManager.ShowFileStatus("");
    }

    private bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return true;
                }
                return true;
            }
            else return false;
        }
        catch
        {
            return false;
        }
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;
        clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
        StartListening();
        count++;
        Debug.Log("Client Connected");

        //* Send a message to everyone, say someone has connected
        //Broadcast(clients[clients.Count - 1].clientName + "has connectred", clients);
    }

    private void onIncomingData(string data)
    {
        Debug.Log("client has sent the following message : " + data);
        HostUIManager.ShowClientMessage(data);
    }

    private void Broadcast(string data, List<ServerClient> cl)
    {
        foreach (ServerClient c in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                Debug.Log("Write : error " + e.Message + " to client" + c.clientName);
            }
        }
    }

    public void EndServer()
    {
        isActivate = false;
        if(server != null)
            server.Stop();
    }

}
