using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI; 
public class TextManager : MonoBehaviour
{ 
    private InputField inputText;
    private string text;
    private string textName;
    
    // Start is called before the first frame update
    void Start()
    {
        inputText = GameObject.Find("InputText").GetComponent<InputField>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TextWrite(){
        text = inputText.text;
        textName = System.DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(" ", "").Replace("P", "").Replace("A", "").Replace("M", "_");
        TextSave(textName, text);

    }
    public bool TextSave(string filename, string txt){
        if(!filename.ToLower().EndsWith(".txt")){
            filename += ".txt" ;
        }
	    var filepath = Path.Combine(Application.persistentDataPath + "/data/", filename);
        
        Directory.CreateDirectory(Path.GetDirectoryName(filepath));

        System.IO.File.WriteAllText(filepath, txt);
        
        return true;
      /*  
        SavWav.Save(clipName, audio.clip);
        StatusText.enabled = true;
        StatusText.text = "ClipName " + clipName + " saved"; 
        */
        
    }
}
