using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using FSP;
using System.Text;
using System;

public class CustomNetworkManager : NetworkManager
{

    public string inputAddress{get; set;}
    public string inputMessage{get; set;}
    public AudioSerializer audioSerializer;
    public NetworkUIManager networkUIManager;
    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler(CustomMsgType.FileMsgType, OnCustomMessageServer);
    }

    private void OnCustomMessageServer(NetworkMessage fileMsg)
    {
        Debug.Log("Message Received");
        FileMessage fMessage = fileMsg.ReadMessage<FileMessage>();
        Debug.Log(fMessage.fileName);
        Debug.Log(fMessage.fileType);
        if(fMessage.fileType == FileType.Wav)
            audioSerializer.StoreByteClip(fMessage.contents);
        else
            Debug.Log(fMessage.fracNum);//TextManager.TextWrite(Encoding.UTF8.GetString(fMessage.contents));

    }

    private void OnCustomMessageClient(NetworkMessage fileMsg)
    {
        FileMessage fMessage = fileMsg.ReadMessage<FileMessage>();
    }

    public void SendFileToServer(FileType type, string fileName)
    {
        if(type == FileType.Text)
        {
            FileMessage message = new FileMessage();
            message.fileType = FileType.Text;
            message.fileName = "asd12323";
            message.contents = Encoding.Default.GetBytes(inputMessage);
            message.maxFrac = 1;
            message.fracNum = 1;
            client.Send(CustomMsgType.FileMsgType, message);
        }
        else
        {
            StartCoroutine(LoadWavCoroutine(fileName));
        }
    }

    private IEnumerator LoadWavCoroutine(string fileName)
    {
        networkUIManager.ShowClientInfo("File is loading... please wait...");
        StartCoroutine(audioSerializer.LoadAudioClipToByte(fileName));
        while(audioSerializer.isLoading == true)
            yield return null;
        networkUIManager.ShowClientInfo("File loading complete!");
        int size = audioSerializer.loadedAudio.Length;
        int fracNum = (size / 1024) + 1;
        Debug.Log(size.ToString());
        Debug.Log(fracNum.ToString());
        FileMessage[] message = new FileMessage[fracNum];

        for(int i = 0; i < message.Length; i++)
        {
            message[i] = new FileMessage();
            message[i].contents = new byte[1024];
            Debug.Log(i);
            Buffer.BlockCopy(audioSerializer.loadedAudio, 1024 * i, message[i].contents, 0, 1024);
            message[i].fileType = FileType.Wav;
            message[i].fileName = fileName;
            message[i].maxFrac = fracNum;
            message[i].fracNum = i + 1;
        }
        
        for(int i = 0; i < message.Length; i++)
        {
            client.Send(CustomMsgType.FileMsgType, message[i]);
            yield return null;
        }
    }

    private void SendFileToClient(FileType type, string fileName)
    {
        
    }

}
