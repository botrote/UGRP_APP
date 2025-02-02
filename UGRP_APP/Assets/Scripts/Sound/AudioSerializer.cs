﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class AudioSerializer : MonoBehaviour
{
    public bool isLoading { get; private set; }
    public byte[] loadedAudio { get; private set; }
    int num;
    void Start()
    {
        loadedAudio = null;
        num = 0;
        //StartCoroutine(Test());
    }

    void Update()
    {
        //Debug.Log(isLoading);
    }

    private IEnumerator Test()
    {
        //yield return StartCoroutine(LoadAudioClipToByte("test"));
        yield return new WaitForSeconds(5);
       // StoreByteClip(loadedAudio);
    }

    public IEnumerator LoadAudioClipToByte(string fileName, int n)
    {
        isLoading = true;
        if (!fileName.ToLower().EndsWith(".wav")) 
        {
			fileName += ".wav";
		}
        //wav폴더
        string path;
        if( n ==1 ) path =  Path.Combine(Application.persistentDataPath + "/data/", fileName);
        else path = Path.Combine( "C:/Users/최수아/Documents"+"/UGRP/sync_system/computer/download/", fileName);

        if(File.Exists(path) == false)
        {
            Debug.Log("File " + path + " doesn't exist.");
            yield break;
        }

        WWW tempWWW = new WWW(@"file://" + path);
        
        Debug.Log(path);

        while(!tempWWW.isDone)
                yield return null;

        AudioClip clip = tempWWW.GetAudioClip(false, false);
        float[] soundData = new float[clip.samples * clip.channels];
        clip.GetData(soundData, 0);
        
        /*
         * | sample=4 byte | channel=4byte | filenameLength=4 byte | fileName=n byte | data=m byte |
         * 
         *
         */

        byte[] b_samples =  BitConverter.GetBytes(clip.samples);
        byte[] b_channels = BitConverter.GetBytes(clip.channels);
        byte[] b_fileNameLength = BitConverter.GetBytes(fileName.Length);
        byte[] b_fileName= Encoding.UTF8.GetBytes(fileName);

        loadedAudio = new byte[soundData.Length * 4 + 12 + b_fileName.Length];

        Buffer.BlockCopy(b_samples, 0, loadedAudio, 0, 4);
        Buffer.BlockCopy(b_channels, 0, loadedAudio, 4, 4);
        Buffer.BlockCopy(b_fileNameLength, 0, loadedAudio, 8, 4);
        Buffer.BlockCopy(b_fileName, 0, loadedAudio, 12, b_fileName.Length);
        Buffer.BlockCopy(soundData, 0, loadedAudio, 12+b_fileName.Length, loadedAudio.Length-12-b_fileName.Length);

        Debug.Log(loadedAudio.Length);

        isLoading = false;
    }

    public AudioClip StoreByteClip(byte[] data, int i)
    {
      //  Debug.Log(data.Length);
        byte[] b_samples = new byte[4];
        byte[] b_channels = new byte[4];
        byte[] b_fileNameLength = new byte[4];
    //    byte[] fileName = new byte[];

        Buffer.BlockCopy(data, 0, b_samples, 0, 4);
        Buffer.BlockCopy(data, 4, b_channels, 0, 4);
        Buffer.BlockCopy(data, 8, b_fileNameLength, 0, 4);

        int samples = BitConverter.ToInt32(b_samples, 0);
        int channels = BitConverter.ToInt32(b_channels, 0);
        int fileNameLength = BitConverter.ToInt32(b_fileNameLength, 0);

        byte[] b_fileName = new byte[fileNameLength];
        float[] soundData = new float[((data.Length-12-fileNameLength) / 4) + 1];

        Buffer.BlockCopy(data, 12, b_fileName, 0, fileNameLength);
        //string fileName = Encoding.UTF8.GetString(b_fileName);
        string fileName;
        if(i==2){
            num++;
            fileName = "script_"+string.Format("{0:D7}", num);;
            Debug.Log(fileName);
        }
        else fileName = Encoding.UTF8.GetString(b_fileName);

        Buffer.BlockCopy(data, 12+fileNameLength, soundData, 0, data.Length-12-fileNameLength);

        AudioClip clip = AudioClip.Create(fileName, samples, channels, 44100, false);
        clip.SetData(soundData, 0);
        Debug.Log(samples);
        Debug.Log(channels);
        Debug.Log(soundData.Length);
        //script 저장 폴더
        if(i==2) {
            SavWav.Save(fileName, clip, "C:/Users/최수아/Documents"+"/UGRP/data/");
        }
        return clip;
    }
}
