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
        script[1] = string.Copy("모모는 있는 힘을 다해 용감하게, 회색 신사가 몸을 숨기고 있는");
        script[2] = string.Copy("깜깜한 어둠과 텅 빈 허공 속으로 뛰어들어갔다.");
        script[3] = string.Copy("곁눈질로 모모를 흘끔대던 남자는 모모의 표정의 변화를 놓치지 않았다.");
        script[0] = string.Copy("하지만 모모는 두려워하지 않기로 마음을 다잡아먹었다.");
        //script[4] = string.Copy("4");
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
