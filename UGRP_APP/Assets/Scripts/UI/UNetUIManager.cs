using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UNetUIManager : MonoBehaviour
{
    private FileSlot fileSlot;
    private SoundRecorder s;
    private Button recordStartButton;
    private Button recordEndButton;
    private Button recordPlayButton;
    private Button nextButton;
    private GameObject LoadingPanel;


    void Start()
    {
        fileSlot = GameObject.Find("FileSlot(Clone)").GetComponent<FileSlot>();
        s = GameObject.Find("RecordManager").GetComponent<SoundRecorder>();
        recordStartButton = transform.Find("RecordStartButton").gameObject.GetComponent<Button>();
        recordEndButton = transform.Find("RecordEndButton").gameObject.GetComponent<Button>();
        recordPlayButton = transform.Find("RecordPlayButton").gameObject.GetComponent<Button>();
        nextButton = transform.Find("NextButton").gameObject.GetComponent<Button>();
        LoadingPanel = transform.Find("LoadingPanel").gameObject;
    }

    void LateUpdate()
    {
        if(fileSlot.isEncodingWav || fileSlot.isSendingWav)
        {
            recordStartButton.interactable = false;
            recordEndButton.interactable = false;
            recordPlayButton.interactable = false;
            nextButton.interactable = false;
            LoadingPanel.SetActive(true);
        }
        else
        {
            recordStartButton.interactable = true;
            recordEndButton.interactable = true;
            recordPlayButton.interactable = true;
            nextButton.interactable = true;
            LoadingPanel.SetActive(false);
            if(ScriptManager.getInstance().curScriptNum >= ScriptManager.maxScriptNum)
                GameObject.Find("SceneManager").GetComponent<SceneLoader>().startLoadScene();
        }
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