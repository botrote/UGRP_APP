using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ClientUIManager : UIManager
{
    private Text exceptionText;
    private Text clientInfoText;
    private Text logText;
    private InputField inputAddressField;
    private InputField inputMessageField;
    public ToggleGroup togglegroup;
    public NetWorkClient client;
    public bool? transferMode { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        transferMode = null;
        exceptionText = GameObject.Find("SocketExceptionText").GetComponent<Text>();
        exceptionText.text = "";
        clientInfoText = GameObject.Find("ClientInfoText").GetComponent<Text>();
        clientInfoText.text = "";
        logText = GameObject.Find("LogText").GetComponent<Text>();
        logText.text = "";
        togglegroup = GameObject.Find("Canvas").transform.Find("ToggleGroup").GetComponent<ToggleGroup>();
        inputAddressField = GameObject.Find("AddressInputField").GetComponent<InputField>();
        inputMessageField = GameObject.Find("MessageInputField").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInputAddress()
    {
        client.inputAddress = inputAddressField.text;
        ((Text)inputAddressField.placeholder).text = "address set to " + client.inputAddress; 
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

        client.inputMessage = inputMessageField.text;

        if(transferMode == false)
            ((Text)inputMessageField.placeholder).text = "Message set to " + client.inputMessage; 
        else if(transferMode == true)
            ((Text)inputMessageField.placeholder).text = "Filename set to " + client.inputMessage + ".wav";

        inputMessageField.text = "";
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
