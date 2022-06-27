﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_game : MonoBehaviour
{
    public PlayerControls controls;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(controls.Gameplay.Start.triggered)
        {
           SceneManager.LoadScene("SimpleArena");
       } 
    }
}
