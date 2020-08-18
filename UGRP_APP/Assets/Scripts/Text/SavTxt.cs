using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class SavTxt{
    // Start is called before the first frame update
    public static bool Save(string filename, string txt){
        if(!filename.ToLower().EndsWith(".txt")){
            filename += ".txt" ;
        }
	    var filepath = Path.Combine(Application.persistentDataPath + "/data/", filename);
        
        Directory.CreateDirectory(Path.GetDirectoryName(filepath));

        System.IO.File.WriteAllText(filepath, txt);
        
        return true;
    }
}
