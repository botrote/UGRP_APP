using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class SendTxtSceneNetworkManager : NetworkManager
{
    // Start is called before the first frame update
    private static SendTxtSceneNetworkManager instance = null; 
    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public static SendTxtSceneNetworkManager getInstance()
    {
        return instance;
    }

}
