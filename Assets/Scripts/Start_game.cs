using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_game : MonoBehaviour
{
    private string[] levelToLoad= {"SimpleSceneLalaland","SimpleSceneOmen","SimpleSceneSwing"};
    public ImageTab script;
    public PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }
    // Update is called once per frame
    void Update()
    {
        controls.Gameplay.Start.performed += ctx => Launch();
    }

    private void Launch()
    {
        Debug.Log("Launch");
        SceneManager.LoadScene(levelToLoad[script.level]);

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
