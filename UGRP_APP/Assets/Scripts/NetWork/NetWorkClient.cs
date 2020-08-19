using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using UnityEngine.UI;
using System;
using System.Linq;

public class NetWorkClient : MonoBehaviour
{
    public bool isActivate;
    string localIP;
    public ServerClient client;
    private TcpClient clientSocket;
    private IPEndPoint clientAddress;
    private IPEndPoint serverAddress;
    public Text SocketExceptionText;
    private Text ClinetInfoText;
    private Text LogText;
    private InputField inputAddressField;
    private InputField inputMessageField;
    private string inputAddress;
    private string inputMessage;
    
    public ToggleGroup togglegroup;
    private bool? transferMode;
    public AudioSerializer audioSerializer;

    // Start is called before the first frame update
    void Start()
    {
        togglegroup = GameObject.Find("Canvas").transform.Find("ToggleGroup").GetComponent<ToggleGroup>();

        isActivate = false;
        localIP = null;
        transferMode = null;
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

        inputAddress = "";
        inputMessage = "";

        SocketExceptionText = GameObject.Find("SocketExceptionText").GetComponent<Text>();
        ClinetInfoText = GameObject.Find("ClientInfoText").GetComponent<Text>();
        LogText = GameObject.Find("LogText").GetComponent<Text>();
        SocketExceptionText.text = "";
        LogText.text = "";
        ClinetInfoText.text = "";
        inputAddressField = GameObject.Find("AddressInputField").GetComponent<InputField>();
        inputMessageField = GameObject.Find("MessageInputField").GetComponent<InputField>();

        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        Debug.Log("Client IP : " + localIP);
        ClinetInfoText.text = "Client IP : " + localIP;
        KeyInputManager keyInputManager = GameObject.Find("KeyInputManager").GetComponent<KeyInputManager>();
        keyInputManager.EV_escape += EndConnect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FileLoadCoroutine()
    {
        LogText.text = "File is loading... please wait...";
        StartCoroutine(audioSerializer.LoadAudioClipToByte(inputMessage));
        while(audioSerializer.isLoading == true)
            yield return null;
        LogText.text = "File loading complete!";
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
            modeBuffer[0] = Convert.ToByte(transferMode);

            if(transferMode == false) //transfer mesage
            {
                dataBuffer = Encoding.Default.GetBytes(message);
                LogText.text = "sended your message : " + message;
            }
            else if(transferMode == true) //transfer file
            {
                dataBuffer = audioSerializer.loadedAudio;
                LogText.text = "sended your file : " + message + ".wav";
            }

            byte[] finalBuffer = new byte[dataBuffer.Length + 1];
            Buffer.BlockCopy(modeBuffer, 0, finalBuffer, 0, 1);
            Buffer.BlockCopy(dataBuffer, 0, finalBuffer, 1, dataBuffer.Length);
            stream.Write(finalBuffer, 0, finalBuffer.Length); 
        }
        catch(Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
            SocketExceptionText.text = e.Message;
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
            LogText.text = "Already connected";
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
            SocketExceptionText.text = e.Message;
            isActivate = false;
        }
    }

    public void OnFeatureStart()
    {
        if(isActivate == false)
        {
            LogText.text = "Please connect first";
            return;
        }
        if(transferMode == null)
        {
            LogText.text = "Please choose transfer mode";
            return;
        }
        else if(transferMode == false) //transfer message
        {
            StartFeature();
        }
        else if(transferMode == true) //transfer file
        {
            StartCoroutine(FileLoadCoroutine());
        }
    }

    public void OnInputAddress()
    {
        inputAddress = inputAddressField.text;
        ((Text)inputAddressField.placeholder).text = "address set to " + inputAddress; 
        inputAddressField.text = "";
    }

    public void OnInputMessage()
    {
        if(transferMode == null)
        {
            ((Text)inputMessageField.placeholder).text = "Please select transfer mode";
            inputMessageField.text = "";
            return;
        }

        inputMessage = inputMessageField.text;

        if(transferMode == false)
            ((Text)inputMessageField.placeholder).text = "Message set to " + inputMessage; 
        else if(transferMode == true)
            ((Text)inputMessageField.placeholder).text = "Filename set to " + inputMessage + ".wav";

        inputMessageField.text = "";
    }

    public void EndConnect()
    {
        isActivate = false;
        if(clientSocket != null)
            clientSocket.Close();
    }

    public void OnInputType()
    {
        
        string id = togglegroup.ActiveToggles().FirstOrDefault().name;
        //Debug.Log(id);
        if(id == "File") 
            transferMode = true;
        else if (id == "Text") 
            transferMode = false;
        else 
            transferMode = null;
        
        //Debug.Log("currentSelection: " + inputType.ToString());
    }    

}
