using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using FSP;

public class NetworkUIManager : UIManager
{
    private Text exceptionText;
    private Text clientInfoText;
    private Text logText;
    private InputField inputAddressField;
    private string inputAddress;
    private InputField inputMessageField;
    private string inputMessage;
    private ToggleGroup togglegroup;
    public FileType fileType { get; set; }
    private FileSlot fileSlot;

    // Start is called before the first frame update
    void Start()
    {
        fileSlot = null;
        fileType = FileType.Text;
        exceptionText = GameObject.Find("SocketExceptionText").GetComponent<Text>();
        exceptionText.text = "";
        clientInfoText = GameObject.Find("ClientInfoText").GetComponent<Text>();
        clientInfoText.text = "";
        logText = GameObject.Find("LogText").GetComponent<Text>();
        logText.text = "";
        togglegroup = GameObject.Find("ToggleGroup").GetComponent<ToggleGroup>();
        inputAddressField = GameObject.Find("AddressInputField").GetComponent<InputField>();
        inputMessageField = GameObject.Find("MessageInputField").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInputAddress()
    {
        inputAddress = inputAddressField.text;
        ((Text)inputAddressField.placeholder).text = "address set to " + inputAddress; 
        inputAddressField.text = "";
    }

    public void OnInputMessage()
    {
        inputMessage = inputMessageField.text;

        if(fileType == FileType.Text)
            ((Text)inputMessageField.placeholder).text = "Message set to " + inputMessage; 
        else if(fileType == FileType.Wav)
            ((Text)inputMessageField.placeholder).text = "Filename set to " + inputMessage + ".wav";

        inputMessageField.text = "";
    }

    public void OnInputType()
    {
        if(togglegroup == null)
            return;
        string id = togglegroup.ActiveToggles().FirstOrDefault().name;
        //Debug.Log(id);
        if(id == "File") 
            fileType = FileType.Wav;
        else if (id == "Text") 
            fileType = FileType.Text;    
        //Debug.Log("currentSelection: " + inputType.ToString());
    }   

    public void OnSend()
    {
        if(fileSlot == null)
            fileSlot = GameObject.Find("FileSlot(Clone)").GetComponent<FileSlot>();

        if(fileType == FileType.Text)
            fileSlot.CmdUploadTxt(fileSlot.txtFileData);
        else if(fileType == FileType.Wav)    
            StartCoroutine(fileSlot.UploadWavCoroutine());
    }

    public void OnEnroll()
    {
        if(fileSlot == null)
            fileSlot = GameObject.Find("FileSlot(Clone)").GetComponent<FileSlot>();

        if(fileType == FileType.Text)
            fileSlot.EncodeTextFile(inputMessage);
        else if(fileType == FileType.Wav)    
            fileSlot.EncodeWavFile(inputMessage);
    }

    public void ShowError(string s)
    {
        exceptionText.text = s;
    }

    public void ShowClientInfo(string s)
    {
        clientInfoText.text = s;
    }

    public void ShowLog(string s)
    {
        logText.text = s;
    }
}
