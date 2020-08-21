using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostUIManager : UIManager
{
    public Text StatusText;
    public Text ClientMessageText;
    public Text ExceptionText;
    public Text FileMessageText;
    public Text HostInfoText;

    void Start()
    {
        StatusText = GameObject.Find("NetworkStatusText").GetComponent<Text>();
        StatusText.text = "";
        ClientMessageText = GameObject.Find("ClientMessageText").GetComponent<Text>();
        ClientMessageText.text = "";
        ExceptionText = GameObject.Find("SocketExceptionText").GetComponent<Text>();
        ExceptionText.text = "";
        HostInfoText = GameObject.Find("HostInfoText").GetComponent<Text>();
        HostInfoText.text = "";
        FileMessageText = GameObject.Find("FileMessageText").GetComponent<Text>();
        FileMessageText.text = "";
    }

    void Update()
    {
        
    }

    public void ShowHostInfo(string s)
    {
        HostInfoText.text = s;
    }

    public void ShowClientMessage(string s)
    {
        ClientMessageText.text = s;
    }

    public void ShowStatus(string s)
    {
        StatusText.text = s;
    }

    public void ShowFileStatus(string s)
    {
        FileMessageText.text = s;
    }

    public void ShowError(string s)
    {
        ExceptionText.text = s;
    }
}
