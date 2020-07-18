using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Text platformText;
    void Awake()
    {
        platformText = gameObject.transform.Find("PlatformText").GetComponent<Text>();
        if(Application.platform == RuntimePlatform.Android)
            platformText.text = "running on Android..";
        else if(Application.platform == RuntimePlatform.WindowsPlayer)
            platformText.text = "running on Windows..";
        else
            platformText.text = "running on Unknown platform..";

        int count = -3;
        if(Application.platform == RuntimePlatform.Android)
        {
            foreach(Transform child in transform)
            {
                if(child.gameObject.GetComponent<Button>() != null)
                {
                    count++;
                    RectTransform curRect = child.gameObject.GetComponent<RectTransform>();
                    curRect.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                    curRect.anchoredPosition = new Vector2(0, 0);
                    curRect.anchoredPosition += new Vector2(0, (-100 * count));
                }
                Debug.Log(child.name);
            }
            count = 4;
            platformText.gameObject.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, (-100 * count));
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
