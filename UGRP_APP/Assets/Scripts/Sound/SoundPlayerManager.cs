using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
        /*
        AudioClip audioFile = Resources.Load("sound/" + filename) as AudioClip;
        audioPlayer.clip = audioFile;
        */
        StartCoroutine(EnrollSound_C(filename));
        
    }

    private IEnumerator EnrollSound_C(string filename)
    {
        if (!filename.ToLower().EndsWith(".wav")) 
        {
			filename += ".wav";
		}
        string path = Path.Combine(Application.persistentDataPath + "/data/", filename);

        WWW tempWWW = new WWW(@"file://" + path);
        
        Debug.Log(path);

        while(!tempWWW.isDone)
            yield return null;
        
        audioPlayer.clip = tempWWW.GetAudioClip(false, false);

        yield return null;
    }
}
