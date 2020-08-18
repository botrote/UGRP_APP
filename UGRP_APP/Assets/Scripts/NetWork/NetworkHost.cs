﻿using UnityEngine;
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
    public bool isActivate;
    string localIP;
    public int port = 6321;
    public Text MessageText;
    public Text ClientMessageText;
    public Text SocketExceptionText;
    public Text HostInfoText;

    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;
    private TcpListener server;
    private bool serverStarted;
    private int count = 0;
    public AudioSerializer audioSerializer;


    private void Start()
    {
        MessageText = GameObject.Find("NetworkStatusText").GetComponent<Text>();
        MessageText.text = "";
        ClientMessageText = GameObject.Find("ClientMessageText").GetComponent<Text>();
        ClientMessageText.text = "";
        SocketExceptionText = GameObject.Find("SocketExceptionText").GetComponent<Text>();
        SocketExceptionText.text = "";
        HostInfoText = GameObject.Find("HostInfoText").GetComponent<Text>();
        HostInfoText.text = "";

        isActivate = false;
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
            HostInfoText.text = "Server has been started on address " + server.LocalEndpoint.ToString();
        }
        catch (Exception e)
        {
            isActivate = false;
            Debug.Log("Socket error : " + e.Message);
            SocketExceptionText.text = e.Message;
        }

    }

    private void Update()
    {
        try {
        if(isActivate == false)
            return;
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
                count--;
                continue;
            }
            else
            {
                /*
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    String data = reader.ReadLine();
                    if (data != null)
                        onIncomingData(c, data);
                }
                */

                byte[] data = new byte[5000000];
                byte[] modeBuffer = new byte[1];
                NetworkStream s = c.tcp.GetStream();

                HostInfoText.text = "2";
                if(s.DataAvailable)
                    s.Read(modeBuffer, 0, 1);
                else
                    return;
                
                if(s.DataAvailable)
                
                    s.Read(data, 0, data.Length);
                
                else
                    return;
                
                bool transferMode = Convert.ToBoolean(modeBuffer[0]);
                string encoded = Encoding.UTF8.GetString(data);
                //string encoded = data.ToString();
                
                //Debug.Log("I'm called ");
                 HostInfoText.text = "3";

                if(encoded != null)
                {
                    Debug.Log("transferMode: " + transferMode.ToString());
                    if(transferMode == true)
                    {
                        audioSerializer.StoreByteClip(data);
                        onIncomingData(c, "audio file recieved");
                    }
                    if(transferMode == false)
                    {
                        encoded = encoded.TrimEnd(
                            new char[] {(char)0 }
                        );
                        Debug.Log(encoded.Length);
                        onIncomingData(c, encoded);
                        TextManager.TextWrite(encoded);
                    }
                }

            }

            // check for message from the client
        }
        }
        catch(Exception e){
            SocketExceptionText.text = e.Message;
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
        Debug.Log(c.clientName + " has sent the following message : " + data);
        ClientMessageText.text = data;
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
