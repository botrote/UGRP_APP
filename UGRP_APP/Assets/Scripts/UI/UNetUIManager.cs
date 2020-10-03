using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UNetUIManager : NetworkManager
{
    private InputField inputAddressField;
    private Text StatusText;
    private FileSlot fileSlot;
    SoundRecorder s;

    void Start()
    {
        fileSlot = null;
        networkAddress = "192.168.219.117";
        s = GameObject.Find("RecordManager").GetComponent<SoundRecorder>();
        // OpenServer();
        ConnectClientToServer();
    }

    public void OnInputAddress()
    {
        networkAddress = inputAddressField.text;
        ((Text)inputAddressField.placeholder).text = "address set to " + networkAddress; 
        inputAddressField.text = "";
    }


    public void OpenServer()
    {
        networkPort = 7777;

        StartServer();  
    }

    public void OpenHost()
    {
        networkPort = 7777;

        StartHost();
    }

    public void ConnectClientToServer()
    {
        networkPort = 7777;

        StartClient();
    }
    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        Debug.Log("[Client]Start Client");
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        
        Debug.Log("[Client]Connect Server Sucess.");
        StatusText.text = "[Client]Connect Server Success.";
    }
    public void OnEndRecord()
    {
        if(fileSlot == null)
            fileSlot = GameObject.Find("FileSlot(Clone)").GetComponent<FileSlot>();
        fileSlot.EncodeWavFile(s.clipName);
    }
    public void OnSendToHost()
    {
        if(fileSlot == null)
            fileSlot = GameObject.Find("FileSlot(Clone)").GetComponent<FileSlot>();
        StartCoroutine(fileSlot.UploadWavCoroutine(true));
    }
}