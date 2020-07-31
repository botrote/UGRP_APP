using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //empty here..
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            if(SceneManager.GetActiveScene().name == "startScene")
                quitApp();
            else if(SceneManager.GetActiveScene().name == "SampleScene0")
                startSceneN();
            else
                startScene0();
        }
    }

    public void startSceneN()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void startScene0()
    {
        SceneManager.LoadScene("SampleScene0");
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

    public void startSceneHost()
    {
        SceneManager.LoadScene("HostScene");
    }

    public void startSceneClient()
    {
        SceneManager.LoadScene("ClientScene");
    }

    public void quitApp()
    {
        Application.Quit();
    }

}
