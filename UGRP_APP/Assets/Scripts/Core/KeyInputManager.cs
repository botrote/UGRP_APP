﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputManager : MonoBehaviour
{
    public delegate void DL_escape();
    public event DL_escape EV_escape;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            EV_escape();
    }
}
