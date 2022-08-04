﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_game : MonoBehaviour
{
    // TODO : script plus utilisé, remplacé par 'SelectSceneMenu' => à supprimer

    private string[] levelToLoad= {"SimpleSceneLalaland","SimpleSceneOmen","SimpleSceneSwing"};
    public ImageTab script;
    public PlayerControls controls;


    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player1.Enable();
        controls.Player2.Enable();
    }


    // Update is called once per frame
    void Update()
    {
        controls.Player1.Start.performed += ctx => Launch();
        controls.Player2.Start.performed += ctx => Launch();
    }


    private void Launch()
    {
        Debug.Log("Launch");
        SceneManager.LoadScene(levelToLoad[script.level]);

    }
}
