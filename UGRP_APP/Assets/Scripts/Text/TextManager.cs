using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI; 
public static class TextManager
{ 
    public static void TextWrite(string txt){
        string fileName = System.DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(" ", "").Replace("P", "").Replace("A", "").Replace("M", "_");
        TextSave(fileName, txt);

    }
    private static bool TextSave(string filename, string txt){
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
