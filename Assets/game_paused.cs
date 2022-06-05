using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_paused : MonoBehaviour
{
    
    public GameObject gamePaused;
    public GameObject countdown;
    private bool isGamePausedActive = false;


    // Start is called before the first frame update
    void Start()
    {
        gamePaused.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!countdown.activeSelf)//si le countdown de départ est terminé, on peux mettre en pause
        {
            if(Input.GetKeyDown("g") && isGamePausedActive==false ){ //si on appuie sur la touche G, le menu pause s'active
            gamePaused.SetActive(true);
            isGamePausedActive = true;
            Time.timeScale=0f;
            
            }else if(Input.GetKeyDown("g") && isGamePausedActive==true) // si on appuie a nouveau le jeu se relance
            {
                gamePaused.SetActive(false);
                isGamePausedActive = false; 
                Time.timeScale=1f;
            }
        }
        
    }
}
