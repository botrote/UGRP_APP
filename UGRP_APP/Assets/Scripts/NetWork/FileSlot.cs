﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.IO;

public class FileSlot : NetworkBehaviour
{
    public struct WavPacket
    {
        public int maxPacketNum;
        public int thisPacketNum;
        public int packetSize;
        public byte[] data; 
    }
    int i;
    int packetNum;
    public byte[] txtFileData;
    public byte[] wavFileData;
    public AudioClip clip;
    private SceneLoader sceneLoader;
    public bool isEncodingWav { get; private set; }
    public bool isSendingWav { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        i=0;
        packetNum = -1;
        txtFileData = null;
        wavFileData = null;
        
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command] 
    public void CmdUploadTxt(byte[] data)
    {
        Debug.Log("cmd text called");
        txtFileData = data;
        CmdDecodeTextFile();
    }

    [Command]
    public void CmdRate(int rating)
    {
        TextManager.CmdTextWrite(rating.ToString(), 2, "rate");
    }

    [ClientRpc]
    public void RpcUploadTxt(byte[] data)
    {
        Debug.Log("rpc text called");
        txtFileData = data;
        DecodeTextFile();
    }

    [ClientRpc]
    public void RpcUploadWavPacket(WavPacket packet)
    {
        packetNum = packet.maxPacketNum;
        if(wavFileData == null || wavFileData.Length != 1024 * packetNum)
            wavFileData = new byte[1024 * packetNum];
        Buffer.BlockCopy(packet.data, 0, wavFileData, 1024 * packet.thisPacketNum, 1024);
        Debug.Log("packet number " + packet.thisPacketNum + " saved");
        if(packet.thisPacketNum == packet.maxPacketNum - 1)
            DecodeWavFile();
    }

    [Command]
    public void CmdUploadWavPacket(WavPacket packet)
    {
        packetNum = packet.maxPacketNum;
        Debug.Log("2");
        if(wavFileData == null || wavFileData.Length != 1024 * packetNum)
            wavFileData = new byte[1024 * packetNum];
        Buffer.BlockCopy(packet.data, 0, wavFileData, 1024 * packet.thisPacketNum, 1024);
        Debug.Log("packet number " + packet.thisPacketNum + " saved");
        Debug.Log("3");
        if(packet.thisPacketNum == packet.maxPacketNum - 1)
            DecodeWavFile2();
    }

    public IEnumerator UploadWavCoroutine(bool isToHost)
    {
        Debug.Log(isToHost);
        isSendingWav = true;
        for(int i = 0; i < packetNum; i++)
        {
            WavPacket tempPacket;
            tempPacket.maxPacketNum = packetNum;
            tempPacket.thisPacketNum = i; 
            tempPacket.packetSize = 1024;
            tempPacket.data = new byte[1024];
            Buffer.BlockCopy(wavFileData, 1024 * i, tempPacket.data, 0, 1024);
            Debug.Log("packet number " + i + " finished");
            if(isToHost){
                //Debug.Log("Hello");
                CmdUploadWavPacket(tempPacket);
            }
            else
                RpcUploadWavPacket(tempPacket);
            yield return null;
        }
        isSendingWav = false;
    }

    public void EncodeTextFile(string content)
    {
        txtFileData = Encoding.Default.GetBytes(content);
    }

    public void EncodeWavFile(string fileName)
    {
        StartCoroutine(WavEncodingCoroutine(fileName, 1));
    }

    public IEnumerator WavEncodingCoroutine(string fileName, int n)
    {
        isEncodingWav = true;
        AudioSerializer audioSerializer = GameObject.Find("AudioSerializer").GetComponent<AudioSerializer>();
        StartCoroutine(audioSerializer.LoadAudioClipToByte(fileName, n));
        while(audioSerializer.isLoading == true)
            yield return null;
        byte[] wavDataTemp = audioSerializer.loadedAudio;
        Debug.Log(wavDataTemp.Length);
        packetNum = (wavDataTemp.Length / 1024) + 1;
        Debug.Log(packetNum);
        wavFileData = new byte[1024 * packetNum];
        wavDataTemp.CopyTo(wavFileData, 0);
        isEncodingWav = false;
    }
     void DecodeTextFile()
    {
        Debug.Log("decode text!!!!");
        sceneLoader = GameObject.Find("SceneManager").GetComponent<SceneLoader>();
        sceneLoader.startScene2();
    }
    void CmdDecodeTextFile()
    {
        Debug.Log("decode text called");
        string encoded = Encoding.UTF8.GetString(txtFileData);
        string date = DateTimeGetter.getNowString();
        GameObject.Find("SendRoutineManager").GetComponent<SendRoutineManager>().OnTextRecieved(this, encoded, date);
        TextManager.CmdTextWrite(encoded, 1, date);
    }

    void DecodeWavFile()
    {
        AudioSerializer audioSerializer = GameObject.Find("AudioSerializer").GetComponent<AudioSerializer>();
        Debug.Log("decode wav called");
        GameObject.Find("AudioSource").GetComponent<AudioSource>().clip = audioSerializer.StoreByteClip(wavFileData, 1);
        //File.Delete(Path.Combine(Application.persistentDataPath + "/data/", "result.wav"));
        GameObject.Find("Canvas").GetComponent<SentTxtSceneUIManager>().SetLoadingImageEnabled(false);
    }
    void DecodeWavFile2()
    {
        i++;
        AudioSerializer audioSerializer = GameObject.Find("AudioSerializer").GetComponent<AudioSerializer>();
        Debug.Log("decode wav2 called");
        audioSerializer.StoreByteClip(wavFileData, 2);
        if(i==5){
            GameObject.Find("SendRoutineManager2").GetComponent<SendRoutineManager2>().OnScriptFinished(this);
            i=0;
        }

        //File.Delete(Path.Combine(Application.persistentDataPath + "/data/", "result.wav"));
    }
}
