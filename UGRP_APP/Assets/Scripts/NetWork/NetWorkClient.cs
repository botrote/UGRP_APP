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
    private bool? inputType;
    public AudioSerializer audioSerializer;
    public bool transferMode;
    // Start is called before the first frame update
    void Start()
    {
        togglegroup = GameObject.Find("Canvas").transform.Find("ToggleGroup").GetComponent<ToggleGroup>();

        isActivate = false;
        localIP = null;
        transferMode = false;
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

        inputAddress = "";
        inputMessage = "";

        SocketExceptionText = GameObject.Find("SocketExceptionText").GetComponent<Text>();
        ClinetInfoText = GameObject.Find("ClientInfoText").GetComponent<Text>();
        LogText = GameObject.Find("LogText").GetComponent<Text>();
        SocketExceptionText.text = "";
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
        StartCoroutine(audioSerializer.LoadAudioClipToByte("test"));
        while(audioSerializer.isLoading == true)
            yield return null;
        LogText.text = "File loading complete!";
        StartFeature();
    }

    public void StartFeature()
    {
        if(isActivate == true)
            return;
        isActivate = true;
        try
        {
            serverAddress = new IPEndPoint(IPAddress.Parse(inputAddress), 6321);
            clientAddress = new IPEndPoint(IPAddress.Parse(localIP), 0);
            clientSocket = new TcpClient(clientAddress);
            client = new ServerClient(clientSocket);
            client.tcp.Connect(serverAddress);
            NetworkStream stream = client.tcp.GetStream();
            string message = inputMessage;

            byte[] data = null;
            byte[] modeBuffer = new byte[1];
            modeBuffer[0] = Convert.ToByte(transferMode);
            stream.Write(modeBuffer, 0, 1);

            if(transferMode == false) //transfer mesage
            {
                data = Encoding.Default.GetBytes(message);
                LogText.text = "sended your message : " + message;
            }
            else if(transferMode == true)
            {
                data = audioSerializer.loadedAudio;
                LogText.text = "sended your file : " + "test.wav";
            }

            stream.Write(data, 0, data.Length); 
        }
        catch(SocketException e)
        {
            Debug.Log("Socket error : " + e.Message);
            SocketExceptionText.text = e.Message;
            isActivate = false;
        }
        catch(System.SystemException e)
        {
            Debug.Log("Format error : " + e.Message);
            SocketExceptionText.text = e.Message;
            isActivate = false;
        }
        
    }

    public void OnExit()
    {
        client.tcp.Close();
    }

    public void OnFeatureStart()
    {
        if(transferMode == null)
        {
            LogText.text = "please choose transfer mode";
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
        Debug.Log("End Called : " + inputAddress);
    }

    public void OnInputMessage()
    {
        inputMessage = inputMessageField.text;
        ((Text)inputMessageField.placeholder).text = "Message set to " + inputMessage; 
        inputMessageField.text = "";
        Debug.Log("End Called : " + inputMessage);
    }

    public void EndConnect()
    {
        isActivate = false;
        if(clientSocket != null)
            clientSocket.Close();
    }
    public Toggle currentSelection
    {
        get
        {
            return togglegroup.ActiveToggles().FirstOrDefault();
        }
    }
    public void OnInputType()
    {
        
        string id = currentSelection.name;
        //Debug.Log(id);
        if(id == "File") inputType = true;
        else if (id == "Text") inputType = false;
        else inputType = null;
        
        //Debug.Log("currentSelection: " + inputType.ToString());
    }    

}
