using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SendRoutineManager : MonoBehaviour
{
    // Start is called before the first frame update
    string dataPath;
    string fileName;

    void Start()
    {
        //wav폴더
        dataPath =  "C:/Users/최수아/Documents"+"/UGRP/sync_system/computer/download/";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTextRecieved(FileSlot fileSlot, string content, string date)
    {
        fileName=date;
        Debug.Log("OnTextReceived"+fileName);
        StartCoroutine(GetTxt_SendWav_Routine(fileSlot));
    }

    private IEnumerator GetTxt_SendWav_Routine(FileSlot fileSlot)
    {
        //string fileName = TextManager.get_CmdfileName();
        //Debug.Log("fileName");
        while(true)
        {
            if(File.Exists(Path.Combine(dataPath, fileName+".wav")))
                break;
            Debug.Log(Path.Combine(dataPath,  fileName+".wav"));
            yield return null;
        }

        yield return StartCoroutine(fileSlot.WavEncodingCoroutine(fileName));
        yield return StartCoroutine(fileSlot.UploadWavCoroutine(false));
    }
}
