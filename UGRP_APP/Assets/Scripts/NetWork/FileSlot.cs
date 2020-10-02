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

    bool isSendingWav;
    int packetNum;
    private AudioSerializer audioSerializer;
    public byte[] txtFileData;
    public byte[] wavFileData;
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        packetNum = -1;
        txtFileData = null;
        wavFileData = null;
        isSendingWav = false;
        audioSerializer = GameObject.Find("AudioSerializer").GetComponent<AudioSerializer>();
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
        DecodeTextFile();
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
        if(wavFileData == null || wavFileData.Length != 1024 * packetNum)
            wavFileData = new byte[1024 * packetNum];
        Buffer.BlockCopy(packet.data, 0, wavFileData, 1024 * packet.thisPacketNum, 1024);
        Debug.Log("packet number " + packet.thisPacketNum + " saved");
        if(packet.thisPacketNum == packet.maxPacketNum - 1)
            DecodeWavFile();
    }

    public IEnumerator UploadWavCoroutine(bool isToHost)
    {
        for(int i = 0; i < packetNum; i++)
        {
            WavPacket tempPacket;
            tempPacket.maxPacketNum = packetNum;
            tempPacket.thisPacketNum = i;
            tempPacket.packetSize = 1024;
            tempPacket.data = new byte[1024];
            Buffer.BlockCopy(wavFileData, 1024 * i, tempPacket.data, 0, 1024);
            Debug.Log("packet number " + i + " finished");
            if(isToHost)
                CmdUploadWavPacket(tempPacket);
            else
                RpcUploadWavPacket(tempPacket);
            yield return null;
        }
    }

    public void EncodeTextFile(string content)
    {
        txtFileData = Encoding.Default.GetBytes(content);
    }

    public void EncodeWavFile(string fileName)
    {
        StartCoroutine(WavEncodingCoroutine(fileName));
    }

    public IEnumerator WavEncodingCoroutine(string fileName)
    {
        StartCoroutine(audioSerializer.LoadAudioClipToByte(fileName));
        while(audioSerializer.isLoading == true)
            yield return null;
        byte[] wavDataTemp = audioSerializer.loadedAudio;
        Debug.Log(wavDataTemp.Length);
        packetNum = (wavDataTemp.Length / 1024) + 1;
        Debug.Log(packetNum);
        wavFileData = new byte[1024 * packetNum];
        wavDataTemp.CopyTo(wavFileData, 0);
    }

    void DecodeTextFile()
    {
        Debug.Log("decode text called");
        string encoded = Encoding.UTF8.GetString(txtFileData);
        GameObject.Find("SendRoutineManager").GetComponent<SendRoutineManager>().OnTextRecieved(this, encoded);
        TextManager.TextWrite(encoded);
    }

    void DecodeWavFile()
    {
        Debug.Log("decode wav called");
        GameObject.Find("AudioSource").GetComponent<AudioSource>().clip = audioSerializer.StoreByteClip(wavFileData);
        File.Delete(Path.Combine(Application.persistentDataPath + "/data/", "result.wav"));
        GameObject.Find("Canvas").GetComponent<SentTxtSceneUIManager>().SetLoadingImageEnabled(false);
    }

}
