using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SendRoutineManager : MonoBehaviour
{
    // Start is called before the first frame update
    string dataPath;

    void Start()
    {
        dataPath = Application.persistentDataPath + "/data/";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTextRecieved(FileSlot fileSlot, string content)
    {
        StartCoroutine(GetTxt_SendWav_Routine(fileSlot));
    }

    private IEnumerator GetTxt_SendWav_Routine(FileSlot fileSlot)
    {
        while(true)
        {
            if(File.Exists(Path.Combine(dataPath, "result.wav")))
                break;
            Debug.Log(File.Exists(Path.Combine(dataPath, "result.wav")));
            yield return null;
        }

        yield return StartCoroutine(fileSlot.WavEncodingCoroutine("result"));
        yield return StartCoroutine(fileSlot.UploadWavCoroutine(false));
    }
}
