using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_game : MonoBehaviour
{
    private string[] levelToLoad= {"SimpleSceneLalaland","SimpleSceneOmen","SimpleSceneSwing"};
    public ImageTab script;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      //  int script =  GameObject.Find("ImageLevel").GetComponents<ImageTab>(); 
       if(Input.GetKeyDown(KeyCode.G)){
           SceneManager.LoadScene(levelToLoad[script.level]);
       } 
    }
}
