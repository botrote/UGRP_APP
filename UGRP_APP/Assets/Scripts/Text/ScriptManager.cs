using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScriptManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int i = 0;
    public string[] script = new string[5];
    public Text info_text;
    void Start()
    {
        info_text = GameObject.Find("Info_Text").GetComponent<Text>();
        script[0] = string.Copy("0");
        script[1] = string.Copy("1");
        script[2] = string.Copy("2");
        script[3] = string.Copy("3");
        script[4] = string.Copy("4");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void next_script(){
        if(i<5){
            info_text.text = script[i];
            i++;
        }
        else {
            SceneManager.LoadScene("startScene");
        }
    }
}
