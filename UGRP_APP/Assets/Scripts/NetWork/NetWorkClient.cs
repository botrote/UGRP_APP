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
    private Text ClinetInfoText;
    public InputField inputAddressField;
    private InputField inputMessageField;
    private string inputAddress;
    private string inputMessage;
    // Start is called before the first frame update
    void Start()
    {
        isActivate = false;
        localIP = null;
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

        inputAddress = "";
        inputMessage = "";

        SocketExceptionText = GameObject.Find("SocketExceptionText").GetComponent<Text>();
        ClinetInfoText = GameObject.Find("ClientInfoText").GetComponent<Text>();
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
            byte[] data = Encoding.Default.GetBytes(message);
            Debug.Log(stream.CanWrite);
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

    public void OnInputAddress()
    {
        inputAddress = inputAddressField.text;
        ((Text)inputAddressField.placeholder).text = "address set to " + inputAddress; 
        inputAddressField.text = "";
        Debug.Log("End Called : " + inputAddress);
    }

    public void OnInputMessage()
    {
        inputMessage = inputMessageField.text + "\n";
        ((Text)inputMessageField.placeholder).text = "Message set to " + inputMessage; 
        inputMessageField.text = "";
        Debug.Log("End Called : " + inputMessage);
    }
}
