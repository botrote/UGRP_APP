using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SentTxtSceneUIManager : MonoBehaviour
{
    private FileSlot fileSlot;
    private InputField inputTxtField;
    private string inputText;
    private AudioSource audioSource;
    private Image waitingImage;
    private Slider slider;
    private int rating;
    // Start is called before the first frame update
    void Start()
    {
        inputTxtField = transform.Find("InputField").GetComponent<InputField>();
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        waitingImage = transform.Find("Image").GetComponent<Image>();
        slider = transform.Find("Slider").GetComponent<Slider>();
        waitingImage.enabled = false;
        rating = 0;
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
        string clipName =clipName = System.DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(" ", "").Replace("P", "").Replace("A", "").Replace("M", "_");
        string filepath = Application.persistentDataPath + "/data/";
        Debug.Log(clipName);
        //wav폴더
        //SavWav.Save(clipName, audioSource.clip,  "C:/Users/최수아/Documents"+"/UGRP/sync_system/computer/download/" );
        SavWav.Save(clipName, audioSource.clip, filepath);
       
    }

    public void OnRateButton()
    {
        Debug.Log((int)(slider.value * 10));
        rating = (int)(slider.value * 10);
        if(fileSlot == null)
            return;
        fileSlot.CmdRate(rating);
    }

}
