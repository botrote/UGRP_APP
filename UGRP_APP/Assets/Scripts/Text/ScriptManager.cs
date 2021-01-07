using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScriptManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int curScriptNum { get; private set; }
    private string[] script;
    private Text info_text;
    private static ScriptManager instance = null;
    public static int maxScriptNum { get { return 5; } }
    void Start()
    {
        instance = this;
        curScriptNum = 0;
        info_text = GameObject.Find("Info_Text").GetComponent<Text>();
        script = new string[maxScriptNum];
        script[0] = string.Copy("0");
        script[1] = string.Copy("1");
        script[2] = string.Copy("2");
        script[3] = string.Copy("3");
        script[4] = string.Copy("4");
        info_text.text = script[0];
    }

    // Update is called once per frame
    public static ScriptManager getInstance()
    {
        return instance;
    }

    public void next_script()
    {
        if(curScriptNum < maxScriptNum)
        {
            curScriptNum++;
            if(curScriptNum >= maxScriptNum)
                return;
            info_text.text = script[curScriptNum];
        }
    }
}
