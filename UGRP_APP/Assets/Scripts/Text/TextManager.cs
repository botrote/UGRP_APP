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
        string fileName = DateTimeGetter.getNowString();
        TextSave(fileName, txt, Application.persistentDataPath + "/data/");

    }

    public static void CmdTextWrite(string txt, int num, string date){ 
        
        if(num == 1){  //user가 보내는 텍스트
            CmdfileName = date;
            //text 폴더
            TextSave(CmdfileName, txt, "C:/Users/최수아/Documents"+ "/UGRP/sync_system/computer/upload/");
        }   
        else if(num == 2){ //feedback폴더
            TextSave(CmdfileName, txt, "C:/Users/최수아/Documents"+ "/UGRP/feedback_system/computer/upload/");
        }      

    }
    public static string get_CmdfileName(){
        Debug.Log("get file name:"+CmdfileName);
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
