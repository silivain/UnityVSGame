using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_paused : MonoBehaviour
{

    public GameObject gamePaused;               // Menu Pause
    public GameObject countdown;                // compte à rebours de début de scène
    public GameObject GOAudio;                  // AudioSource
    private bool isGamePausedActive = false;    // vrai si le menu Pause est activé
    public PlayerControls controls;             // inputs


    // désactive le menu pause au démarrage
    void Start() {
        gamePaused.SetActive(false);
    }


    /* récupère les inputs
    */
    private void Awake() {
        controls = new PlayerControls();
        controls.Player1.Enable();
        controls.Player2.Enable();
        controls.Player1.Start.performed += ctx => PauseGame();
        controls.Player2.Start.performed += ctx => PauseGame();
    }


    /* met le jeu en pause ou le reprend
    * le countdown de début de niveau doit être terminé
    */
    private void PauseGame() {
        if (!countdown.activeSelf) {
            if(!isGamePausedActive) {
                gamePaused.SetActive(true);
                isGamePausedActive = true;
                GOAudio.GetComponent<AudioSource>().Pause();
                Debug.Log("into Pause Menu");
                Time.timeScale = 0f;
            }else{
                gamePaused.SetActive(false);
                isGamePausedActive = false;
                Time.timeScale = 1f;
                GOAudio.GetComponent<AudioSource>().Play();
                Debug.Log("out Pause Menu");
            }
        }
    }
}
