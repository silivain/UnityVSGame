using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string[] levelToLoad= {"MainMenu"};
    public PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }
    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name != "MainMenu") {
            controls.Gameplay.Start.performed += ctx => Launch();
        }
    }

    private void Launch()
    {
        SceneManager.LoadScene("MainMenu");

    }
    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
