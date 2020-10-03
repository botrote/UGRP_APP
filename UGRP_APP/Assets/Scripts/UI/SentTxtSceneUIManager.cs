using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SentTxtSceneUIManager : MonoBehaviour
{
    private FileSlot fileSlot;
    private InputField inputTxtField;
    private string inputText;
    private AudioSource audioSource;
    private Image waitingImage;
    // Start is called before the first frame update
    void Start()
    {
        inputTxtField = transform.Find("InputField").GetComponent<InputField>();
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        waitingImage = transform.Find("Image").GetComponent<Image>();
        waitingImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEndEdit()
    {
        if(fileSlot == null)
            fileSlot = GameObject.Find("FileSlot(Clone)").GetComponent<FileSlot>();

        inputText = inputTxtField.text;
        inputTxtField.text = "";

        fileSlot.EncodeTextFile(inputText);
        fileSlot.CmdUploadTxt(fileSlot.txtFileData);
        SetLoadingImageEnabled(true);
    }

    public void SetLoadingImageEnabled(bool b)
    {
        waitingImage.enabled = b;
    }

    public void OnPlaybutton()
    {
        audioSource.Play();
    }

    public void OnSaveButton()
    {
        string clipName = System.DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(" ", "").Replace("P", "").Replace("A", "").Replace("M", "_");
        SavWav.Save(clipName, audioSource.clip);
    }

    public void OnRateButton()
    {

    }


}
