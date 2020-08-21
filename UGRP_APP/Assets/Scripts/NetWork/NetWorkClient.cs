using System.Collections;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

public class NetWorkClient : MonoBehaviour
{
    public bool isActivate;
    string localIP;
    public ServerClient client;
    private TcpClient clientSocket;
    private IPEndPoint clientAddress;
    private IPEndPoint serverAddress;
    public string inputAddress { get; set; }
    public string inputMessage { get; set; }
    public AudioSerializer audioSerializer;
    public ClientUIManager clientUIManager;

    // Start is called before the first frame update
    void Start()
    {
        isActivate = false;
        localIP = null;
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

        inputAddress = "";
        inputMessage = "";

        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        Debug.Log("Client IP : " + localIP);
        clientUIManager.ShowClientInfo("Client IP : " + localIP);
        KeyInputManager keyInputManager = GameObject.Find("KeyInputManager").GetComponent<KeyInputManager>();
        keyInputManager.EV_escape += EndConnect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FileLoadCoroutine()
    {
        clientUIManager.ShowClientInfo("File is loading... please wait...");
        StartCoroutine(audioSerializer.LoadAudioClipToByte(inputMessage));
        while(audioSerializer.isLoading == true)
            yield return null;
            clientUIManager.ShowClientInfo("File loading complete!");
        StartFeature();
    }

    public void StartFeature()
    {
        try
        {
            NetworkStream stream = client.tcp.GetStream();
            string message = inputMessage;

            byte[] dataBuffer = null;
            byte[] modeBuffer = new byte[1];
            modeBuffer[0] = Convert.ToByte(clientUIManager.transferMode);

            if(clientUIManager.transferMode == false) //transfer mesage
            {
                dataBuffer = Encoding.Default.GetBytes(message);
                clientUIManager.ShowLog("sended your message : " + message);
            }
            else if(clientUIManager.transferMode == true) //transfer file
            {
                dataBuffer = audioSerializer.loadedAudio;
                clientUIManager.ShowLog("sended your file : " + message + ".wav");
            }

            byte[] finalBuffer = new byte[dataBuffer.Length + 1];
            Buffer.BlockCopy(modeBuffer, 0, finalBuffer, 0, 1);
            Buffer.BlockCopy(dataBuffer, 0, finalBuffer, 1, dataBuffer.Length);
            stream.Write(finalBuffer, 0, finalBuffer.Length); 
        }
        catch(Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
            clientUIManager.ShowError(e.Message);
            isActivate = false;
        }
    }

    public void OnExit()
    {
        client.tcp.Close();
    }

    public void OnConnectStart()
    {
        if(isActivate == true)
        {
            clientUIManager.ShowLog("Already connected");
            return;
        }
        isActivate = true;
        try
        {
            serverAddress = new IPEndPoint(IPAddress.Parse(inputAddress), 6321);
            clientAddress = new IPEndPoint(0, 0);
            clientSocket = new TcpClient(clientAddress);
            client = new ServerClient(clientSocket);
            client.tcp.Connect(serverAddress); 
        }
        catch(Exception e)
        {
            Debug.Log("Error : " + e.Message);
            clientUIManager.ShowError(e.Message);
            isActivate = false;
        }
    }

    public void OnFeatureStart()
    {
        if(isActivate == false)
        {
            clientUIManager.ShowLog("Please connect first");
            return;
        }
        if(clientUIManager.transferMode == null)
        {
            clientUIManager.ShowLog("Please choose transfer mode");
            return;
        }
        else if(clientUIManager.transferMode == false) //transfer message
        {
            StartFeature();
        }
        else if(clientUIManager.transferMode == true) //transfer file
        {
            StartCoroutine(FileLoadCoroutine());
        }
    }

    public void EndConnect()
    {
        isActivate = false;
        if(clientSocket != null)
            clientSocket.Close();
    }

}
