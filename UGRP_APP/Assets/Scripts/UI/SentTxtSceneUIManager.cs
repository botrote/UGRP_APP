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
    private GameObject loadingPanel;
    private GameObject ratePanel;
    // Start is called before the first frame update
    void Start()
    {
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
        ratePanel.SetActive(true);
    }

    public void OnRateQuitButton()
    {
        ratePanel.SetActive(false);
        ratePanel.transform.Find("Slider").gameObject.GetComponent<Slider>().value = 0;
    }

    public void OnRateSendButton()
    {
        Slider slider = ratePanel.transform.Find("Slider").gameObject.GetComponent<Slider>();
        int rating = (int)(slider.value * 10);
        if(fileSlot == null)
            return;
        fileSlot.CmdRate(rating);
    }

}
