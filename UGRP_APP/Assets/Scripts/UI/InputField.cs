using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Input_SoundSelect : MonoBehaviour
{
    // Start is called before the first frame update
    Text inputText;
    SoundPlayerManager sManager;
    void Start()
    {
        inputText = transform.Find("Text").gameObject.GetComponent<Text>();
        sManager = GameObject.Find("AudioPlayManager").GetComponent<SoundPlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEndEdit()
    {
        sManager.EnrollSound(inputText.text);
    }
}
