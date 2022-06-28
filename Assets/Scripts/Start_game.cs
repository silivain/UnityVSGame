using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_game : MonoBehaviour
{
    private string[] levelToLoad= {"SimpleSceneLalaland","SimpleSceneOmen","SimpleSceneSwing"};
    public ImageTab script;
    public PlayerControls controls;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Start.performed += ctx => Launch();
    }
    // Update is called once per frame
    void Update()
    {
        
        //  int script =  GameObject.Find("ImageLevel").GetComponents<ImageTab>(); 

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
