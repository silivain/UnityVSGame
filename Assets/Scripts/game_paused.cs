using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_paused : MonoBehaviour
{

    public GameObject gamePaused;               // Menu Pause
    public GameObject countdown;                // compte à rebours de début de scène
    public GameObject GOAudio;                  // AudioSource
    private bool isGamePausedActive = false;    // vrai si le menu Pause est activé
    private bool controlBlocked = false;        // vrai si le menu pause a été appelé il y a moins de 'delay' sec
    private float delay = 0.25f;                // délai avant de pouvoir rappeler le menu pause
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
        if (!countdown.activeSelf && !controlBlocked) {
            controlBlocked = true;
            StartCoroutine(Delay());

            if(!isGamePausedActive) {
                gamePaused.SetActive(true);
                isGamePausedActive = true;
                GOAudio.GetComponent<AudioSource>().Pause();
                Time.timeScale = 0f;
            }else{
                gamePaused.SetActive(false);
                isGamePausedActive = false;
                Time.timeScale = 1f;
                GOAudio.GetComponent<AudioSource>().Play();
            }
        }
    }


    /* bloque les contrôles pdnt 'delay' secs
    * évite les doubles inputs quand les joueurs ont la même touche de pause
    */
    IEnumerator Delay() {
        yield return new WaitForSecondsRealtime(delay);     // /!\ WaitForSeconds marche pas si le temps est suspendu /!\
        controlBlocked = false;
    }
}
