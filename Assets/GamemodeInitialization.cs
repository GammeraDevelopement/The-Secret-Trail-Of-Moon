﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeInitialization : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Gamemode", 0);
    }

    public void ChangeGamemode(int value) {

        PlayerPrefs.SetInt("Gamemode", value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
