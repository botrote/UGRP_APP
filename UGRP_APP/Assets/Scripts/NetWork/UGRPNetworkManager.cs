using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UGRPNetworkManager : NetworkManager
{
    // Start is called before the first frame update
    private static UGRPNetworkManager instance = null; 
    private SceneLoader sceneLoader;
    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public static UGRPNetworkManager getInstance()
    {
        return instance;
    }

    void Start()
    {
        sceneLoader = GameObject.Find("SceneManager").GetComponent<SceneLoader>();
        StartCoroutine(checkNetworkStateRoutine());
    }

    void Update()
    {
        
    }
    
    private void watchNetworkState()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if(this.IsClientConnected() && sceneName == "ConnectScene")
        {
            sceneLoader.startSceneN();
        }
        else if(!this.IsClientConnected() && sceneName != "ConnectScene")
        {
            sceneLoader.startSceneConnect();
        }
        
    }

    private IEnumerator checkNetworkStateRoutine()
    {
        while(true)
        {
            watchNetworkState();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
