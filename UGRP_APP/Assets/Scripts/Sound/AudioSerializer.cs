using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System;

public class AudioSerializer : MonoBehaviour
{
    public bool isLoading { get; private set; }
    public byte[] loadedAudio { get; private set; }
    public AudioSource audioSource;

    void Start()
    {

    }

    public IEnumerator LoadAudioClipToByte(string fileName)
    {
        isLoading = true;
        if (!fileName.ToLower().EndsWith(".wav")) 
        {
			fileName += ".wav";
		}
        string path = Path.Combine(Application.persistentDataPath + "/data/", fileName);

        WWW tempWWW = new WWW(@"file://" + path);
        
        Debug.Log(path);

        while(!tempWWW.isDone)
                yield return null;
        
        AudioClip clip = tempWWW.GetAudioClip(false, false);
        audioSource.clip = clip;
        float[] soundData = new float[clip.samples * clip.channels];
        clip.GetData(soundData, 0);

        loadedAudio = new byte[soundData.Length * 4];
        Buffer.BlockCopy(soundData, 0, loadedAudio, 0, loadedAudio.Length);
        isLoading = false;
    }

    public AudioClip StoreByteClip(string fileName, byte[] data, int samples, int channels)
    {
        float[] soundData = new float[data.Length / 4];
        Buffer.BlockCopy(data, 0, soundData, 0, data.Length);
        AudioClip clip = AudioClip.Create(fileName, samples, channels, 44100, false);
        clip.SetData(soundData, 0);
        return clip;
    }
}
