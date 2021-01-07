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
    private GameObject loadingPanel;
    private GameObject ratePanel;
    private Button playButton;
    private Button saveButton;
    private Button rateButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton = transform.Find("PlayButton").gameObject.GetComponent<Button>();
        saveButton = transform.Find("SaveButton").gameObject.GetComponent<Button>();
        rateButton = transform.Find("RateButton").gameObject.GetComponent<Button>();
        inputTxtField = transform.Find("InputField").GetComponent<InputField>();
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        loadingPanel = transform.Find("LoadingPanel").gameObject;
        loadingPanel.SetActive(false);
        ratePanel = transform.Find("RatePanel").gameObject;
        ratePanel.SetActive(false);
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
        loadingPanel.SetActive(b);
        playButton.interactable = !b;
        saveButton.interactable = !b;
        rateButton.interactable = !b;
    }

    public void OnPlaybutton()
    {
        audioSource.Play();
    }

    public void OnSaveButton()
    {
        string clipName = DateTimeGetter.getNowString();
        string filepath = Application.persistentDataPath + "/data/";
        Debug.Log(clipName);
        //wav폴더
        //SavWav.Save(clipName, audioSource.clip,  "C:/Users/최수아/Documents"+"/UGRP/sync_system/computer/download/" );
        SavWav.Save(clipName, audioSource.clip, filepath);
       
    }

    public void OnRateButton()
    {
        ratePanel.SetActive(true);
    }

    public void OnRateQuitButton()
    {
        ratePanel.SetActive(false);
    }

    public void OnRateSendButton()
    {
        // Slider slider = ratePanel.transform.Find("Slider").gameObject.GetComponent<Slider>();
        // int rating = (int)(slider.value * 10);
        // if(fileSlot == null)
        //     return;
        // fileSlot.CmdRate(rating);

        Image star = ratePanel.transform.Find("Star").gameObject.GetComponent<Image>();
        Image star_1 = ratePanel.transform.Find("Star (1)").gameObject.GetComponent<Image>();
        Image star_2 = ratePanel.transform.Find("Star (2)").gameObject.GetComponent<Image>();
        Image star_3 = ratePanel.transform.Find("Star (3)").gameObject.GetComponent<Image>();
        Image star_4 = ratePanel.transform.Find("Star (4)").gameObject.GetComponent<Image>();
        Image star_5 = ratePanel.transform.Find("Star (5)").gameObject.GetComponent<Image>();
        Image star_6 = ratePanel.transform.Find("Star (6)").gameObject.GetComponent<Image>();
        Image star_7 = ratePanel.transform.Find("Star (7)").gameObject.GetComponent<Image>();
        Image star_8 = ratePanel.transform.Find("Star (8)").gameObject.GetComponent<Image>();
        Image star_9 = ratePanel.transform.Find("Star (9)").gameObject.GetComponent<Image>();
        Image star_10 = ratePanel.transform.Find("Star (10)").gameObject.GetComponent<Image>();
        int rating;

        if(star_1.IsActive()) rating = 1;
        else if(star_2.IsActive()) rating = 2;
        else if(star_3.IsActive()) rating = 3;
        else if(star_4.IsActive()) rating = 4;
        else if(star_5.IsActive()) rating = 5;
        else if(star_6.IsActive()) rating = 6;
        else if(star_7.IsActive()) rating = 7;
        else if(star_8.IsActive()) rating = 8;
        else if(star_9.IsActive()) rating = 9;
        else if(star_10.IsActive()) rating = 10;
        else rating = 0;
        
        fileSlot.CmdRate(rating);
        
    }

}
