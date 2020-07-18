using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerManager : MonoBehaviour
{
    AudioSource audioPlayer;
    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnrollSound(string filename)
    {
        AudioClip audioFile = Resources.Load("sound/" + filename) as AudioClip;
        audioPlayer.clip = audioFile;
    }
}
