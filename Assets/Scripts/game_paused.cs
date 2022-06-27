using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_paused : MonoBehaviour
{
    
    public GameObject gamePaused;
    public GameObject countdown;
    public GameObject GOAudio;
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
        controls.Gameplay.Start.performed += ctx => Pause();
    }

    private void Pause()
    {
        if (!countdown.activeSelf)//si le countdown de départ est terminé, on peux mettre en pause
        {
            if(Input.GetKeyDown("g") && isGamePausedActive==false ){ //si on appuie sur la touche G, le menu pause s'active
            gamePaused.SetActive(true);
            isGamePausedActive = true;
            GOAudio.GetComponent<AudioSource>().Pause();
            Debug.Log("into Pause Menu");
            Time.timeScale=0f;
            
            }else if(Input.GetKeyDown("g") && isGamePausedActive==true) // si on appuie a nouveau le jeu se relance
            {
                gamePaused.SetActive(false);
                isGamePausedActive = false;
                Time.timeScale = 1f;
                GOAudio.GetComponent<AudioSource>().Play();
            }
        }
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
