using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI; 
public static class TextManager
{       
    static string CmdfileName;
    public static void TextWrite(string txt){
        string fileName = System.DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(" ", "").Replace("P", "").Replace("A", "").Replace("M", "_");
        TextSave(fileName, txt, Application.persistentDataPath + "/data/");

    }

    public static void CmdTextWrite(string txt, int num, string date){ 
        
        if(num == 1){  //user가 보내는 텍스트
            //CmdfileName = System.DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(" ", "").Replace("P", "").Replace("A", "").Replace("M", "_");
            CmdfileName = date;
            //TextSave(CmdfileName, txt, Application.persistentDataPath + "/text/");
            TextSave(CmdfileName, txt, Application.persistentDataPath+ "/text/");
        }   
        else if(num == 2){ //rating
            TextSave(CmdfileName, txt, Application.persistentDataPath+ "/feedback/");
        }      

    }
    public static string get_CmdfileName(){
        return CmdfileName;
    }

    public static bool TextSave(string filename, string txt, string path){
        if(!filename.ToLower().EndsWith(".txt")){
            filename += ".txt" ;
        }
	    var filepath = Path.Combine(path, filename);
        
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
