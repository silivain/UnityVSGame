using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_paused : MonoBehaviour
{
    
    public GameObject gamePaused;
    public GameObject countdown;
    public GameObject GOAduio;
    private bool isGamePausedActive = false;
    public PlayerControls controls;


    // Start is called before the first frame update
    void Start()
    {
        gamePaused.SetActive(false);
        
    }
    private void Awake()
    {
        controls = new PlayerControls();
    }

        // Update is called once per frame
        void Update()
    {
        if (!countdown.activeSelf)//si le countdown de départ est terminé, on peux mettre en pause
        {
            if(controls.Gameplay.Start.triggered && isGamePausedActive==false ){ //si on appuie sur la touche G, le menu pause s'active
            gamePaused.SetActive(true);
            isGamePausedActive = true;
            GOAduio.GetComponent<AudioSource>().Pause();
            Debug.Log("into Pause Menu");
            Time.timeScale=0f;
            
            }else if(controls.Gameplay.Start.triggered && isGamePausedActive==true) // si on appuie a nouveau le jeu se relance
            {
                gamePaused.SetActive(false);
                isGamePausedActive = false; 
                Time.timeScale=1f;
                GOAduio.GetComponent<AudioSource>().Play();
            }
        }
        
    }
}
