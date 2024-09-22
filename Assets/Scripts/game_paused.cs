using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class game_paused : MonoBehaviour
{
    public GameObject[] SelectCorners;          // tableau des images de selection de menu
    private int selectIndex = 0;                // indice désignant le menu actuellement sélectionné

    public GameObject gamePaused;               // Menu Pause
    public GameObject countdown;                // compte à rebours de début de scène
    public GameObject GOAudio;                  // AudioSource
    public GameObject SettingsMenu;             // SettingsMenu window
    private SettingsMenu SettingsMenuScript;    // SettingsMenu.cs
    private bool controlBlocked = false;        // vrai si le menu pause a été appelé il y a moins de 'delay' sec
    private float delay = 0.25f;                // délai avant de pouvoir rappeler le menu pause
    public PlayerControls controls;             // inputs
    public GameObject Player1;                  // access to Player1 scripts
    public GameObject Player2;                  // access to Player2 scripts


    /* récupère les inputs
    * règle les inputs sur la phase de jeu
    */
    private void Awake() {
        controls = new PlayerControls();
        controlInGame();
    }


    // désactive le menu pause au démarrage
    private void Start() {
        gamePaused.SetActive(false);
        SelectCorners[selectIndex].SetActive(false);
        selectIndex = 0;
        SelectCorners[selectIndex].SetActive(true);

        if (!SettingsMenu.TryGetComponent<SettingsMenu>(out SettingsMenuScript)) {
            Debug.Log("failed to pull SettingsMenu.cs in game_paused.cs");
        }
    }


    /* met le jeu en pause ou le reprend
    * le countdown de début de niveau doit être terminé
    */
    private void PauseGame() {
        if (!countdown.activeSelf && !controlBlocked) {
            controlBlocked = true;
            StartCoroutine(Delay());

            gamePaused.SetActive(true);
            GOAudio.GetComponent<AudioSource>().Pause();
            Time.timeScale = 0f;

            controlInMenu();                // règle les inputs sur la phase menu
        }
    }


    /* Déplace la sélection vers le haut
    * met à jour l'index
    */
    private void selectUp() {
        if (selectIndex != 0 && gameObject.activeSelf && !controlBlocked) {
            SelectCorners[selectIndex--].SetActive(false);
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* Déplace la sélection vers le bas
    * met à jour l'index
    */
    private void selectDown() {
        if (selectIndex != 2 && gameObject.activeSelf && !controlBlocked) {
            SelectCorners[selectIndex++].SetActive(false);
            SelectCorners[selectIndex].SetActive(true);
        }
    }


    /* bloque les contrôles pdnt 'delay' secs
    * évite les doubles inputs quand les joueurs ont la même touche de pause
    */
    IEnumerator Delay() {
        yield return new WaitForSecondsRealtime(delay);     // /!\ WaitForSeconds marche pas si le temps est suspendu /!\
        controlBlocked = false;
    }


    /* règle les inputs pour la phase de jeu
    * permet de capter l'appel de pause des joueurs
    */
    private void controlInGame() {
        controls.UI.Disable();
        controls.Player1.Enable();
        controls.Player2.Enable();
        controls.Player1.Start.performed += ctx => PauseGame();
        controls.Player2.Start.performed += ctx => PauseGame();

    }


    /* règle les inputs pour la phase dans le menu
    * permet de naviguer dans le menu et sélectionner le bouton voulu
    */
    public void controlInMenu() {
        controls.Player1.Disable();
        controls.Player2.Disable();
        controls.UI.Enable();
        controls.UI.GoUp.performed += ctx => selectUp();
        controls.UI.GoDown.performed += ctx => selectDown();
        controls.UI.Start.performed += ctx => selectScene();
    }


    /* Disable in game inputs :
       - PlayerMovement
       - game_paused
    */
    private void DisableInGameInputs() {
        Player1.GetComponent<PlayerMovement>().controls.Player1.Disable();
        Player1.GetComponent<PlayerMovement>().controls.Player2.Disable();
        Player1.GetComponent<PlayerHealth>().controls.Player1.Disable();
        Player1.GetComponent<PlayerWeapon>().controls.Player1.Disable();
        Player2.GetComponent<PlayerMovement>().controls.Player1.Disable();
        Player2.GetComponent<PlayerMovement>().controls.Player2.Disable();
        Player2.GetComponent<PlayerHealth>().controls.Player2.Disable();
        Player2.GetComponent<PlayerWeapon>().controls.Player2.Disable();
        controls.Player1.Disable();
        controls.Player2.Disable();
    }


    /* charge la scène sélectionnée par le joueur
    */
    public void selectScene() {
        if (gameObject.activeSelf && !controlBlocked) {
            switch(selectIndex) {
                case 0:
                    Time.timeScale = 1f;                    // temps en vitesse normale.
                    gamePaused.SetActive(false);            // on désactive l'écran de pause
                    controlInGame();
                    GOAudio.GetComponent<AudioSource>().Play();
                    break;
                case 1:
                    gamePaused.SetActive(false);
                    controls.UI.Disable();

                    // set the volume slider value in settings to the current volume value
                    float currentVolume;
                    SettingsMenuScript.audioMixer.GetFloat("Master", out currentVolume);
                    SettingsMenuScript.synchroVolume(currentVolume);
                    SettingsMenuScript.Caller("game_paused");
                    SettingsMenu.SetActive(true);         // on affiche l'écran des settings
                    // reactivate PauseMenu inputs if necessary
                    if (!SettingsMenuScript.controls.UI.enabled) {
                        SettingsMenuScript.Awake();
                    }
                    break;
                case 2:
                    Time.timeScale = 1f;                    // temps en vitesse normale.
                    gamePaused.SetActive(false);            // on désactive l'écran de pause
                    DisableInGameInputs();                  // disable ingame inputs
                    controls.UI.Disable();                  // disable PauseMenu inputs
                    SceneManager.LoadScene("MainMenu");
                    break;
            }
        }
    }
}
