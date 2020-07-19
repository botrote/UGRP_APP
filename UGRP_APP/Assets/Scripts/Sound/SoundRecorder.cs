using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SoundRecorder : MonoBehaviour
{
    AudioSource audio;
    string clipName;
    Text StatusText;
    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        StatusText = GameObject.Find("Canvas").transform.Find("StatusText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecordStart()
    {
        audio.clip = Microphone.Start(Microphone.devices[0], false, 60, 44100);
        StatusText.enabled = true;
        StatusText.text = "Recording..";
    }

    public void RecordEnd()
    {
        Microphone.End(Microphone.devices[0]);
        clipName = System.DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(" ", "").Replace("P", "").Replace("M", "_");
        audio.clip.name = clipName;
        StatusText.enabled = true;
        StatusText.text = "Recording finished";
    }

    public void RecordPlay()
    {
        audio.Play();
    }

    public void RecordSave()
    {
        
        SavWav.Save(clipName, audio.clip);
        StatusText.enabled = true;
        StatusText.text = "ClipName " + clipName + " saved"; 
        
    }
}
