using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public class NetworkHost : MonoBehaviour
{
    public int port = 6321;
    public Text MessageText;

    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;
    private TcpListener server;
    private bool serverStarted;
    private int count = 0;


    private void Start()
    {
        MessageText = GameObject.Find("NetworkStatusText").GetComponent<Text>();
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            StartListening();
            serverStarted = true;
            Debug.Log("Server has been started on port " + port.ToString());
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }
    }

    private void Update()
    {
        //count++;
        MessageText.text = count + "clients";
        if (!serverStarted)
            return;
        foreach (ServerClient c in clients)
        {
            // Is the client still conncted?
            if (!IsConnected(c.tcp))
            {
                c.tcp.Close();
                clients.Remove(c);
                disconnectList.Add(c);
                continue;
            }
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    String data = reader.ReadLine();
                    if (data != null)
                        onIncomingData(c, data);
                }

            }

            // check for message from the client
        }
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

    private void onIncomingData(ServerClient c, string data)
    {
        Debug.Log(c.clientName + "has sent the following message : " + data);
        MessageText.text = data;
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
}

public class ServerClient
{
    public TcpClient tcp;
    public string clientName;

    public ServerClient(TcpClient clientSocket)
    {
        clientName = "Guest";
        tcp = clientSocket;
    }
}