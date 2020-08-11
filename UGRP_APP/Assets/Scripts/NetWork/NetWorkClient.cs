using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using UnityEngine.UI;
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
    public InputField inputAddressField;
    private InputField inputMessageField;
    private string inputAddress;
    private string inputMessage;
    
    public ToggleGroup togglegroup;
    private bool? inputType;
    public AudioSerializer audioSerializer;
    // Start is called before the first frame update
    void Start()
    {
        togglegroup = GameObject.Find("Canvas").transform.Find("ToggleGroup").GetComponent<ToggleGroup>();

        isActivate = false;
        localIP = null;
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
            //byte[] data = Encoding.Default.GetBytes(message);
            byte[] data = audioSerializer.loadedAudio;
            Debug.Log(stream.CanWrite);
            //Debug.Log(data == null);
            stream.Write(data, 0, data.Length); 
        }
        catch(SocketException e)
        {
            Debug.Log("Socket error : " + e.Message);
            SocketExceptionText.text = e.Message;
            isActivate = false;
        }
        /*
        catch(System.SystemException e)
        {
            Debug.Log("Format error : " + e.Message);
            SocketExceptionText.text = e.Message;
            isActivate = false;
        }
        */
    }

    public void OnExit()
    {
        client.tcp.Close();
    }

    public void OnFeatureStart()
    {
        StartCoroutine(FileLoadCoroutine());
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
    public Toggle currentSelection{
        get{
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
