using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectSceneUIManager : MonoBehaviour
{
    private InputField inputHostAddrField;

    // Start is called before the first frame update
    void Start()
    {
        inputHostAddrField = transform.Find("AddressInputField").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnterHostAddr()
    {
        UGRPNetworkManager.getInstance().networkAddress = inputHostAddrField.text;
        inputHostAddrField.text = "";
    }

    public void OnConnect()
    {
        UGRPNetworkManager.getInstance().networkPort = 7777;
        UGRPNetworkManager.getInstance().StartClient();
    }

    
}
