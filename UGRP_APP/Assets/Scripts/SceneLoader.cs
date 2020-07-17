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
        //empty here..
    }

    public void startSceneN()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }
    public void startScene0()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene0");
    }

    public void startScene1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene1");
    }

    public void startScene2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene2");
    }

    public void startScene3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene3");
    }

    public void quitApp()
    {
        Application.Quit();
    }

}
