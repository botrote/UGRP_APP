using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UNetUIManager : MonoBehaviour
{
    private FileSlot fileSlot;
    SoundRecorder s;
    int i;
    void Start()
    {
        fileSlot = GameObject.Find("FileSlot(Clone)").GetComponent<FileSlot>();
        s = GameObject.Find("RecordManager").GetComponent<SoundRecorder>();
    }

    public void OnEndRecord()
    {
        fileSlot.EncodeWavFile(s.clipName);
    }

    public void OnSendToHost()
    {
        if(fileSlot == null)
            fileSlot = GameObject.Find("FileSlot(Clone)").GetComponent<FileSlot>();
        Debug.Log("[Client]OnSendToHost.");
        StartCoroutine(fileSlot.UploadWavCoroutine(true));
        Debug.Log("[Client]OnSendToHost Finished.");
    }

}