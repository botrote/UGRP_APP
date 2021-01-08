using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SendRoutineManager2 : MonoBehaviour
{
    // Start is called before the first frame update
    string dataPath;
    string fileName;
    void Start()
    {
        //wav폴더
        dataPath =  "C:/Users/최수아/Documents"+"/UGRP/training_end/";
        fileName = "training";

    }

    public void OnScriptFinished(FileSlot fileSlot)
    {
        Debug.Log("OnTextReceived"+fileName);
        StartCoroutine(GetTxt_SendWav_Routine(fileSlot));
    }

    private IEnumerator GetTxt_SendWav_Routine(FileSlot fileSlot)
    {
        while(true)
        {
            if(File.Exists(Path.Combine(dataPath, fileName+".txt")))
                break; 
            Debug.Log(Path.Combine(dataPath,  fileName+".txt"));
            yield return null; 
        }
        byte[] StrByte = Encoding.Default.GetBytes("1");

        fileSlot.RpcUploadTxt(StrByte);

        yield return null;
    }
}
