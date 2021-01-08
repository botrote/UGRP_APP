using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SoundRecorder : MonoBehaviour
{
    AudioSource audio;
    public string clipName;
    Text StatusText;
    // Start is called before the first frame update

    float recordStartTime;
    float recordEndTime;
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecordStart()
    {
        recordStartTime = Time.time;
        audio.clip = Microphone.Start(Microphone.devices[0], false, 60, 44100);
        // StatusText.enabled = true;
        // StatusText.text = "Recording..";
    }

    public void RecordEnd()
    {
        recordEndTime = Time.time;
        AudioClip recordedClip = audio.clip;
        int position = Microphone.GetPosition(Microphone.devices[0]);
        float[] soundData = new float[recordedClip.samples * recordedClip.channels];
        recordedClip.GetData (soundData, 0);
        float[] newData = new float[position * recordedClip.channels];
 

        for (int i = 0; i < newData.Length; i++) 
        {
            newData[i] = soundData[i];
        }

        Debug.Log("clip length : " + newData.Length);

        AudioClip newClip = AudioClip.Create(recordedClip.name, position, recordedClip.channels, recordedClip.frequency, false);
        newClip.SetData(newData, 0);

        AudioClip.Destroy(audio.clip);
        audio.clip = newClip;

        clipName = DateTimeGetter.getNowString();
        audio.clip.name = clipName;
        // StatusText.enabled = true;
        // StatusText.text = "Recording finished";

        RecordSave();
    }

    public void RecordPlay()
    {
        audio.Play();
    }

    public void RecordSave()
    {
        
        SavWav.Save(clipName, audio.clip, Application.persistentDataPath + "/data/");
        // StatusText.enabled = true;
        // StatusText.text = "ClipName " + clipName + " saved"; 
        
    }
}
