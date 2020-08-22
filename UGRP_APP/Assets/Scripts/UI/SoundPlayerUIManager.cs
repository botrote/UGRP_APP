using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPlayerUIManager : UIManager
{
    // Start is called before the first frame update
    private InputField input_clipname;
    private SoundPlayerManager sManager;
    void Start()
    {
        input_clipname = GameObject.Find("InputField").GetComponent<InputField>();
        sManager = GameObject.Find("AudioPlayManager").GetComponent<SoundPlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEndEdit()
    {
        ((Text)input_clipname.placeholder).text = "clipname " + input_clipname.text + " enrolled"; 
        sManager.EnrollSound(input_clipname.text);
        input_clipname.text = "";
    }
}
