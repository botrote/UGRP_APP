using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        KeyInputManager keyInputManager = GameObject.Find("KeyInputManager").GetComponent<KeyInputManager>();
        keyInputManager.EV_escape += OnEscape;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEscape()
    {
        if(SceneManager.GetActiveScene().name == "startScene")
            quitApp();
        else if(SceneManager.GetActiveScene().name == "SampleScene0")
            startSceneN();
        else
            startScene0();
    }

    public void startSceneN()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void startScene0()
    {
        SceneManager.LoadScene("ScriptScene");
    }

    public void startScene1()
    {
        SceneManager.LoadScene("SampleScene1");
    }

    public void startScene2()
    {
        SceneManager.LoadScene("SampleScene2");
    }

    public void startScene3()
    {
        SceneManager.LoadScene("SampleScene3");
    }
    public void startScene4()
    {
        SceneManager.LoadScene("SampleScene4");
    }
    public void startSceneHost()
    {
        SceneManager.LoadScene("HostScene");
    }

    public void startSceneClient()
    {
        SceneManager.LoadScene("ClientScene");
    }

    public void startSceneSendTxt()
    {
        SceneManager.LoadScene("SendTxtScene");
    }

    public void startSceneConnect()
    {
        SceneManager.LoadScene("ConnectScene");
    }

    public void quitApp()
    {
        Application.Quit();
    }

}
